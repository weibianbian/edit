using RailShootGame;
using System;
using System.Collections.Generic;
using System.Linq;
using Core.Timer;

namespace GameplayAbilitySystem
{
    public class FInheritedTagContainer
    {
        public GameplayTagContainer CombinedTags;
        public GameplayTagContainer Added;
        public GameplayTagContainer Removed;
    }
    public class FGameplayTagQuery
    {


    }

    public class FGameplayEffectQuery
    {
        public FGameplayTagQuery OwningTagQuery;
        public FGameplayTagQuery EffectTagQuery;
        public FGameplayTagQuery SourceTagQuery;
        public bool Matches(GameplayEffectSpec Spec)
        {
            if (Spec == null)
            {
                return false;
            }
            return true;
        }

    }

    public class ActiveGameplayEffectsContainer : List<ActiveGameplayEffect>
    {
        AbilitySystemComponent Owner;
        public Dictionary<GameplayAttribute, OnGameplayAttributeValueChange> AttributeValueChangeDelegates;
        public List<GameplayEffect> ApplicationImmunityQueryEffects;
        public ActiveGameplayEffectsContainer()
        {
            ApplicationImmunityQueryEffects = new List<GameplayEffect>();
        }
        //这是在属性和ActiveGameplayEffects上执行GameplayEffect的主函数
        public void ExecuteActiveEffectsFrom(GameplayEffectSpec Spec)
        {
            GameplayEffectSpec SpecToUse = Spec;

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

            }

        }
        public bool InternalExecuteMod(GameplayEffectSpec Spec, FGameplayModifierEvaluatedData ModEvalData)
        {
            bool bExecuted = false;
            return bExecuted;
        }
        public ActiveGameplayEffect ApplyGameplayEffectSpec(GameplayEffectSpec Spec, ref bool bFoundExistingStackableGE)
        {
            bFoundExistingStackableGE = false;
            ActiveGameplayEffect AppliedActiveGE = null;
            ActiveGameplayEffect ExistingStackableGE = FindStackableActiveGameplayEffect(Spec);

            bool bSetDuration = true;
            bool bSetPeriod = true;
            int StartingStackCount = 0;
            int NewStackCount = 0;

            if (ExistingStackableGE != null)
            {
                bFoundExistingStackableGE = true;
                GameplayEffectSpec ExistingSpec = ExistingStackableGE.Spec;
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
                ActiveGameplayEffectHandle NewHandle = ActiveGameplayEffectHandle.GenerateNewHandle(Owner);
                AppliedActiveGE = new ActiveGameplayEffect()
                {
                    Handle = NewHandle,
                    Spec = Spec,
                };
                GameplayEffectSpec AppliedEffectSpec = AppliedActiveGE.Spec;
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
                        TimerManager TimerManager = new TimerManager();
                        TimerManager.SetTimer(ref AppliedActiveGE.DurationHandle, TimerDelegate<AbilitySystemComponent, ActiveGameplayEffectHandle>.Create((@owner, @handle) =>
                        {
                            @owner.CheckDurationExpired(@handle);
                        }, Owner, AppliedActiveGE.Handle), FinalDuration, false);
                    }
                }
                return new ActiveGameplayEffect();
            }
            return null;
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
        public void CheckDuration(ActiveGameplayEffectHandle Handle)
        {
            for (int ActiveGEIdx = 0; ActiveGEIdx < this.Count; ++ActiveGEIdx)
            {
                ActiveGameplayEffect Effect = this[ActiveGEIdx];
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
        public bool HasApplicationImmunityToSpec(GameplayEffectSpec SpecToApply, ActiveGameplayEffect OutGEThatProvidedImmunity)
        {
            for (int i = 0; i < ApplicationImmunityQueryEffects.Count; i++)
            {
                GameplayEffect EffectDef = ApplicationImmunityQueryEffects[i];
            }
            return false;
        }
        public ActiveGameplayEffect FindStackableActiveGameplayEffect(GameplayEffectSpec Spec)
        {
            ActiveGameplayEffect StackableGE = null;
            GameplayEffect GEDef = Spec.Def;
            EGameplayEffectStackingType StackingType = GEDef.StackingType;
            if ((StackingType != EGameplayEffectStackingType.None) && (GEDef.DurationPolicy != EGameplayEffectDurationType.Instant))
            {
                AbilitySystemComponent SourceASC = Spec.GetContext().GetInstigatorAbilitySystemComponent();
                for (int i = 0; i < this.Count; i++)
                {
                    ActiveGameplayEffect ActiveEffect = this[i];
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
        public bool HandleActiveGameplayEffectStackOverflow(ActiveGameplayEffect ActiveStackableGE, GameplayEffectSpec OldSpec, GameplayEffectSpec OverflowingSpec)
        {
            GameplayEffect StackedGE = OldSpec.Def;
            bool bAllowOverflowApplication = !(StackedGE.bDenyOverflowApplication);

            if (!bAllowOverflowApplication && StackedGE.bClearStackOnOverflow)
            {
                //Owner.RemoveActiveGameplayEffect(ActiveStackableGE.Handle);
            }
            return bAllowOverflowApplication;
        }
        public void AttemptRemoveActiveEffectsOnEffectApplication(GameplayEffectSpec InSpec, ActiveGameplayEffectHandle InHandle)
        {
            if (InSpec.Def != null)
            {
                FGameplayEffectQuery ClearQuery = new FGameplayEffectQuery();
                for (int i = 0; i < InSpec.Def.RemoveGameplayEffectsWithTags.CombinedTags.Count(); i++)
                {

                }

            }
        }
    }

}

