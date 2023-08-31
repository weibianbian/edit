using Core.Timer;
using RailShootGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace GameplayAbilitySystem
{
    public class FActiveGameplayEffectsContainer : List<FActiveGameplayEffect>
    {
        AbilitySystemComponent Owner;
        public Dictionary<GameplayAttribute, OnGameplayAttributeValueChange> AttributeValueChangeDelegates;
        public Dictionary<GameplayAttribute, FAggregator> AttributeAggregatorMap;
        public List<GameplayEffect> ApplicationImmunityQueryEffects;
        public List<FActiveGameplayEffect> GameplayEffects_Internal;
        public FActiveGameplayEffectsContainer()
        {
            ApplicationImmunityQueryEffects = new List<GameplayEffect>();
            AttributeAggregatorMap = new Dictionary<GameplayAttribute, FAggregator>();
            GameplayEffects_Internal = new List<FActiveGameplayEffect>();
        }
        public void RegisterWithOwner(AbilitySystemComponent InOwner)
        {
            Owner = InOwner;
        }
        //这是在属性和ActiveGameplayEffects上执行GameplayEffect的主函数
        public void ExecuteActiveEffectsFrom(FGameplayEffectSpec Spec)
        {
            FGameplayEffectSpec SpecToUse = Spec;

            SpecToUse.CalculateModifierMagnitudes();
            //这将修改属性的基值
            bool ModifierSuccessfullyExecuted = false;

            for (int ModIdx = 0; ModIdx < SpecToUse.Modifiers.Count(); ++ModIdx)
            {
                GameplayModifierInfo ModDef = SpecToUse.Def.Modifiers[ModIdx];
                FGameplayModifierEvaluatedData EvalData = new FGameplayModifierEvaluatedData()
                {
                    Attribute = ModDef.Attribute,
                    ModifierOp = ModDef.ModifierOp,
                    Magnitude = SpecToUse.GetModifierMagnitude(ModIdx, true),
                };
                ModifierSuccessfullyExecuted |= InternalExecuteMod(SpecToUse, EvalData);
            }
            //调用GameplayCue事件
            bool bHasModifiers = SpecToUse.Modifiers.Count > 0;
            bool bHasExecutions = false;
            bool bHasModifiersOrExecutions = bHasModifiers || bHasExecutions;
            bool InvokeGameplayCueExecute = false;
            //InvokeGameplayCueExecute=(!bHasModifiersOrExecutions) || !Spec.Def.bRequireModifierSuccessToTriggerCues;

            if (bHasModifiersOrExecutions && ModifierSuccessfullyExecuted)
            {
                InvokeGameplayCueExecute = true;
            }
            if (InvokeGameplayCueExecute && SpecToUse.Def.GameplayCues.Count > 0)
            {
                UAbilitySystemGlobals.Get().GetGameplayCueManager().InvokeGameplayCueExecuted_FromSpec(Owner, SpecToUse);
            }

        }
        public bool InternalExecuteMod(FGameplayEffectSpec Spec, FGameplayModifierEvaluatedData ModEvalData)
        {
            bool bExecuted = false;
            AttributeSet AttributeSet = null;
            Type AttributeSetClass = ModEvalData.Attribute.AttributeOwner;
            if (AttributeSetClass != null && AttributeSetClass.IsSubclassOf(typeof(AttributeSet)))
            {
                AttributeSet = Owner.GetAttributeSubobject(AttributeSetClass);
            }
            if (AttributeSet != null)
            {
                FGameplayEffectModCallbackData ExecuteData = new FGameplayEffectModCallbackData(Spec, ModEvalData, Owner);
                if (AttributeSet.PreGameplayEffectExecute(ExecuteData))
                {
                    ApplyModToAttribute(ModEvalData.Attribute, ModEvalData.ModifierOp, ModEvalData.Magnitude, ExecuteData);

                }
            }
            return bExecuted;
        }
        public void ApplyModToAttribute(GameplayAttribute Attribute, EGameplayModOp ModifierOp, float ModifierMagnitude, FGameplayEffectModCallbackData ModData)
        {
            float CurrentBase = GetAttributeBaseValue(Attribute);
            float NewBase = FAggregator.StaticExecModOnBaseValue(CurrentBase, ModifierOp, ModifierMagnitude);
            SetAttributeBaseValue(Attribute, NewBase);
        }
        public float GetAttributeBaseValue(GameplayAttribute Attribute)
        {
            float BaseValue = 0.0f;

            if (Owner != null)
            {
                AttributeSet AttributeSet = null;
                Type AttributeSetClass = Attribute.AttributeOwner;
                if (AttributeSetClass != null && AttributeSetClass.IsSubclassOf(typeof(AttributeSet)))
                {
                    AttributeSet = Owner.GetAttributeSubobject(AttributeSetClass);
                }
                if (AttributeSet == null)
                {
                    return BaseValue;
                }
                FieldInfo fi = Attribute.GetUProperty();
                GameplayAttributeData DataPtr = (GameplayAttributeData)fi.GetValue(AttributeSet);
                BaseValue = DataPtr.GetBaseValue();
            }
            return BaseValue;
        }
        public void SetAttributeBaseValue(GameplayAttribute Attribute, float NewBaseValue)
        {
            AttributeSet Set = null;
            Type AttributeSetClass = Attribute.AttributeOwner;
            if (AttributeSetClass != null && AttributeSetClass.IsSubclassOf(typeof(AttributeSet)))
            {
                Set = Owner.GetAttributeSubobject(AttributeSetClass);
            }
            if (Set == null)
            {
                return;
            }
            float OldBaseValue = 0.0f;
            bool bBaseValueSet = false;
            Set.PreAttributeBaseChange(Attribute, NewBaseValue);

            if (AttributeAggregatorMap.TryGetValue(Attribute, out FAggregator Aggregator))
            {
                OldBaseValue = Aggregator.GetBaseValue();
                Aggregator.SetBaseValue(NewBaseValue);
                bBaseValueSet = true;
            }
            else
            {
                OldBaseValue = Owner.GetNumericAttribute(Attribute);
                InternalUpdateNumericalAttribute(Attribute, NewBaseValue, null);
                bBaseValueSet = true;
            }
            if (bBaseValueSet)
            {
                Set.PostAttributeBaseChange(Attribute, OldBaseValue, NewBaseValue);
            }
        }
        public void InternalUpdateNumericalAttribute(GameplayAttribute Attribute, float NewValue, FGameplayEffectModCallbackData ModData, bool bFromRecursiveCall = false)
        {
            float OldValue = Owner.GetNumericAttribute(Attribute);
            Owner.SetNumericAttribute_Internal(Attribute, NewValue);
        }
        public FActiveGameplayEffect ApplyGameplayEffectSpec(FGameplayEffectSpec Spec, ref bool bFoundExistingStackableGE)
        {
            bFoundExistingStackableGE = false;
            FActiveGameplayEffect AppliedActiveGE = null;
            FActiveGameplayEffect ExistingStackableGE = FindStackableActiveGameplayEffect(Spec);

            bool bSetDuration = true;
            bool bSetPeriod = true;
            int StartingStackCount = 0;
            int NewStackCount = 0;

            if (ExistingStackableGE != null)
            {
                bFoundExistingStackableGE = true;
                FGameplayEffectSpec ExistingSpec = ExistingStackableGE.Spec;
                StartingStackCount = ExistingSpec.StackCount;

                if (ExistingSpec.StackCount == ExistingSpec.Def.StackLimitCount)
                {
                    if (!HandleActiveGameplayEffectStackOverflow(ExistingStackableGE, ExistingSpec, Spec))
                    {
                        return null;
                    }
                }
                NewStackCount = ExistingSpec.StackCount + Spec.StackCount;
                if (ExistingSpec.Def.StackLimitCount > 0)
                {
                    NewStackCount = Math.Min(NewStackCount, ExistingSpec.Def.StackLimitCount);
                }
                ExistingStackableGE.Spec = Spec;
                ExistingStackableGE.Spec.StackCount = NewStackCount;
            }
            else
            {
                FActiveGameplayEffectHandle NewHandle = FActiveGameplayEffectHandle.GenerateNewHandle(Owner);
                AppliedActiveGE = new FActiveGameplayEffect()
                {
                    Handle = NewHandle,
                    Spec = Spec,
                };
                FGameplayEffectSpec AppliedEffectSpec = AppliedActiveGE.Spec;
                //float DefCalcDuration = 0.0f;
                if (AppliedEffectSpec.AttemptCalculateDurationFromDef(out float DefCalcDuration))
                {
                    AppliedEffectSpec.SetDuration(DefCalcDuration, false);
                }
                float DurationBaseValue = AppliedEffectSpec.GetDuration();
                if (DurationBaseValue > 0)
                {
                    float FinalDuration = AppliedEffectSpec.CalculateModifiedDuration();
                    if (FinalDuration <= 0.0f)
                    {
                        FinalDuration = 0.1f;
                    }
                    //AppliedEffectSpec.SetDuration(FinalDuration, true);
                    if (Owner != null && bSetDuration)
                    {
                        TimerManager TimerManager = Owner.GetWorld().GetTimerManager();
                        TimerManager.SetTimer(ref AppliedActiveGE.DurationHandle, TimerDelegate<AbilitySystemComponent, FActiveGameplayEffectHandle>.Create((@owner, @handle) =>
                        {
                            @owner.CheckDurationExpired(@handle);
                        }, Owner, AppliedActiveGE.Handle), FinalDuration, false);
                    }
                }
                if (bSetDuration && Owner != null && AppliedEffectSpec.GetPeriod() > 0)
                {
                    TimerManager TimerManager = Owner.GetWorld().GetTimerManager();
                    ITimerDelegate Delegate = TimerDelegate<AbilitySystemComponent, FActiveGameplayEffectHandle>.Create((@owner, @handle) =>
                    {
                        @owner.ExecutePeriodicEffect(@handle);
                    }, Owner, AppliedActiveGE.Handle);

                    TimerManager.SetTimer(ref AppliedActiveGE.DurationHandle, Delegate, AppliedEffectSpec.GetPeriod(), false);
                }
            }
            if (ExistingStackableGE != null)
            {

            }
            else
            {
                InternalOnActiveGameplayEffectAdded(AppliedActiveGE);
            }
            return AppliedActiveGE;
        }
        public void InternalOnActiveGameplayEffectAdded(FActiveGameplayEffect Effect)
        {
            GameplayEffect EffectDef = Effect.Spec.Def;

            Effect.CheckOngoingTagRequirements();
        }
        public OnGameplayAttributeValueChange GetGameplayAttributeValueChangeDelegate(GameplayAttribute Attribute)
        {
            if (!AttributeValueChangeDelegates.TryGetValue(Attribute, out OnGameplayAttributeValueChange value))
            {
                value = new OnGameplayAttributeValueChange();
                AttributeValueChangeDelegates.Add(Attribute, value);
            }
            return value;
        }
        public void CheckDuration(FActiveGameplayEffectHandle Handle)
        {
            for (int ActiveGEIdx = 0; ActiveGEIdx < this.Count; ++ActiveGEIdx)
            {
                FActiveGameplayEffect Effect = this[ActiveGEIdx];
                if (Effect.Handle == Handle)
                {
                    if (Effect.IsPendingRemove)
                    {
                        break;
                    }
                }
                TimerManager TimerManager = null;
                float Duration = Effect.GetDuration();
                //float CurrentTime = GetWorldTime();
            }
        }
        public bool HasApplicationImmunityToSpec(FGameplayEffectSpec SpecToApply, FActiveGameplayEffect OutGEThatProvidedImmunity)
        {
            for (int i = 0; i < ApplicationImmunityQueryEffects.Count; i++)
            {
                GameplayEffect EffectDef = ApplicationImmunityQueryEffects[i];
            }
            return false;
        }
        public FActiveGameplayEffect FindStackableActiveGameplayEffect(FGameplayEffectSpec Spec)
        {
            FActiveGameplayEffect StackableGE = null;
            GameplayEffect GEDef = Spec.Def;
            EGameplayEffectStackingType StackingType = GEDef.StackingType;
            if ((StackingType != EGameplayEffectStackingType.None) && (GEDef.DurationPolicy != EGameplayEffectDurationType.Instant))
            {
                AbilitySystemComponent SourceASC = Spec.GetContext().GetInstigatorAbilitySystemComponent();
                for (int i = 0; i < this.Count; i++)
                {
                    FActiveGameplayEffect ActiveEffect = this[i];
                    if (ActiveEffect.Spec.Def == Spec.Def &&
                        ((StackingType == EGameplayEffectStackingType.AggregateByTarget) || (SourceASC != null && (SourceASC == ActiveEffect.Spec.GetContext().GetInstigatorAbilitySystemComponent()))))
                    {
                        StackableGE = ActiveEffect;
                        break;
                    }
                }
            }
            return StackableGE;
        }
        public bool HandleActiveGameplayEffectStackOverflow(FActiveGameplayEffect ActiveStackableGE, FGameplayEffectSpec OldSpec, FGameplayEffectSpec OverflowingSpec)
        {
            GameplayEffect StackedGE = OldSpec.Def;
            bool bAllowOverflowApplication = !(StackedGE.bDenyOverflowApplication);

            if (!bAllowOverflowApplication && StackedGE.bClearStackOnOverflow)
            {
                //Owner.RemoveActiveGameplayEffect(ActiveStackableGE.Handle);
            }
            return bAllowOverflowApplication;
        }
        public void AttemptRemoveActiveEffectsOnEffectApplication(FGameplayEffectSpec InSpec, FActiveGameplayEffectHandle InHandle)
        {
            if (InSpec.Def != null)
            {
                FGameplayEffectQuery ClearQuery = new FGameplayEffectQuery();
                for (int i = 0; i < InSpec.Def.RemoveGameplayEffectsWithTags.CombinedTags.Count(); i++)
                {

                }

            }
        }
        public bool InternalRemoveActiveGameplayEffect(int Idx, int StacksToRemove, bool bPrematureRemoval)
        {
            FActiveGameplayEffect Effect = GetActiveGameplayEffect(Idx);
            FGameplayEffectRemovalInfo GameplayEffectRemovalInfo = new FGameplayEffectRemovalInfo()
            {
                StackCount = Effect.Spec.StackCount,
                bPrematureRemoval = bPrematureRemoval,
                EffectContext = Effect.Spec.GetEffectContext()
            };
            if (StacksToRemove > 0 && Effect.Spec.StackCount > StacksToRemove)
            {
                // This won't be a full remove, only a change in StackCount.
                int StartingStackCount = Effect.Spec.StackCount;
                Effect.Spec.StackCount -= StacksToRemove;
                //OnStackCountChange(Effect, StartingStackCount, Effect.Spec.StackCount);
                return false;
            }

            return false;
        }
        public bool RemoveActiveGameplayEffect(FActiveGameplayEffectHandle Handle, int StacksToRemove)
        {
            int NumGameplayEffects = GetNumGameplayEffects();
            for (int ActiveGEIdx = 0; ActiveGEIdx < NumGameplayEffects; ActiveGEIdx++)
            {
                FActiveGameplayEffect Effect = GetActiveGameplayEffect(ActiveGEIdx);
                if (Effect.Handle == Handle && Effect.IsPendingRemove == false)
                {
                    InternalRemoveActiveGameplayEffect(ActiveGEIdx, StacksToRemove, true);
                    return true;
                }
            }
            return false;
        }
        public int GetNumGameplayEffects()
        {
            return 0;
        }
        public FActiveGameplayEffect GetActiveGameplayEffect(int idx)
        {
            if (idx < GameplayEffects_Internal.Count)
            {
                return GameplayEffects_Internal[idx];
            }
            return null;
        }
        public void ExecutePeriodicGameplayEffect(FActiveGameplayEffectHandle Handle)
        {

        }
    }
    public class FGameplayEffectRemovalInfo
    {
        /** 当玩法效果的持续时间没有过期时是正确的，这意味着玩法效果被强行移除 */
        public bool bPrematureRemoval = false;

        /** 这个游戏效果在被移除之前的堆栈数量。 */
        public int StackCount = 0;

        /** 演员这个游戏效果是有针对性的。 */
        public GameplayEffectContextHandle EffectContext;

    }

}

