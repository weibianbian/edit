using Core.Timer;
using RailShootGame;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Reflection;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

namespace GameplayAbilitySystem
{
    public class FActiveGameplayEffectsContainer
    {
        UAbilitySystemComponent Owner;
        public Dictionary<FGameplayAttribute, OnGameplayAttributeValueChange> AttributeValueChangeDelegates;
        public Dictionary<FGameplayAttribute, FAggregator> AttributeAggregatorMap;
        //具有免疫查询的活跃GEs。这是一个加速列表，以避免频繁地搜索Active GameplayEffect列表。(我们只在免疫过程中搜索活性GE)
        public List<UGameplayEffect> ApplicationImmunityQueryEffects;
        public List<FActiveGameplayEffect> GameplayEffects_Internal;
        public FGameplayTagCountContainer ApplicationImmunityGameplayTagCountContainer;
        public FActiveGameplayEffectsContainer()
        {
            ApplicationImmunityQueryEffects = new List<UGameplayEffect>();
            AttributeAggregatorMap = new Dictionary<FGameplayAttribute, FAggregator>();
            GameplayEffects_Internal = new List<FActiveGameplayEffect>();
        }
        public void RegisterWithOwner(UAbilitySystemComponent InOwner)
        {
            Owner = InOwner;
        }
        //这是在属性和ActiveGameplayEffects上执行GameplayEffect的主函数
        public void ExecuteActiveEffectsFrom(FGameplayEffectSpec Spec)
        {
            FGameplayEffectSpec SpecToUse = Spec;

            SpecToUse.CalculateModifierMagnitudes();
            // ------------------------------------------------------
            //	Modifiers
            //		这将修改属性的基值
            // ------------------------------------------------------
            bool ModifierSuccessfullyExecuted = false;

            for (int ModIdx = 0; ModIdx < SpecToUse.Modifiers.Count(); ++ModIdx)
            {
                FGameplayModifierInfo ModDef = SpecToUse.Def.Modifiers[ModIdx];
                FGameplayModifierEvaluatedData EvalData = new FGameplayModifierEvaluatedData()
                {
                    Attribute = ModDef.Attribute,
                    ModifierOp = ModDef.ModifierOp,
                    Magnitude = SpecToUse.GetModifierMagnitude(ModIdx, true),
                };
                ModifierSuccessfullyExecuted |= InternalExecuteMod(SpecToUse, EvalData);
            }
            // ------------------------------------------------------
            //	Executions
            //		这将运行自定义代码来“做事情”。
            // ------------------------------------------------------
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
            UAttributeSet AttributeSet = null;
            Type AttributeSetClass = ModEvalData.Attribute.AttributeOwner;
            if (AttributeSetClass != null && AttributeSetClass.IsSubclassOf(typeof(UAttributeSet)))
            {
                AttributeSet = Owner.GetAttributeSubobject(AttributeSetClass);
            }
            if (AttributeSet != null)
            {
                FGameplayEffectModCallbackData ExecuteData = new FGameplayEffectModCallbackData(Spec, ModEvalData, Owner);
                //这应该适用于“游戏范围”规则。比如将生命值限制在最大生命值，或者每增加一点力量就增加3点生命值等等
                //PreAttributeModify可以返回false来“抛出”这个修改。
                if (AttributeSet.PreGameplayEffectExecute(ExecuteData))
                {
                    float OldValueOfProperty = Owner.GetNumericAttribute(ModEvalData.Attribute);

                    ApplyModToAttribute(ModEvalData.Attribute, ModEvalData.ModifierOp, ModEvalData.Magnitude, ExecuteData);
                    FGameplayEffectModifiedAttribute ModifiedAttribute = Spec.GetModifiedAttribute(ModEvalData.Attribute);
                    if (ModifiedAttribute == null)
                    {
                        //如果我们还没有创建一个修改过的属性持有人，那就创建它
                        ModifiedAttribute = Spec.AddModifiedAttribute(ModEvalData.Attribute);
                    }
                    ModifiedAttribute.TotalMagnitude += ModEvalData.Magnitude;
                    //这应该适用于“游戏范围”规则。比如将生命值限制在最大生命值，或者每增加一点力量就增加3点生命值等等
                    AttributeSet.PostGameplayEffectExecute(ExecuteData);

                    bExecuted = true;
                }
            }
            return bExecuted;
        }
        //实际上将给定的mod应用于属性
        public void ApplyModToAttribute(FGameplayAttribute Attribute, EGameplayModOp ModifierOp, float ModifierMagnitude, FGameplayEffectModCallbackData ModData)
        {
            float CurrentBase = GetAttributeBaseValue(Attribute);
            float NewBase = FAggregator.StaticExecModOnBaseValue(CurrentBase, ModifierOp, ModifierMagnitude);
            SetAttributeBaseValue(Attribute, NewBase);
        }
        public float GetAttributeBaseValue(FGameplayAttribute Attribute)
        {
            float BaseValue = 0.0f;

            if (Owner != null)
            {
                UAttributeSet AttributeSet = null;
                Type AttributeSetClass = Attribute.AttributeOwner;
                if (AttributeSetClass != null && AttributeSetClass.IsSubclassOf(typeof(UAttributeSet)))
                {
                    AttributeSet = Owner.GetAttributeSubobject(AttributeSetClass);
                }
                if (AttributeSet == null)
                {
                    return BaseValue;
                }
                FieldInfo fi = Attribute.GetUProperty();
                FGameplayAttributeData DataPtr = (FGameplayAttributeData)fi.GetValue(AttributeSet);
                BaseValue = DataPtr.GetBaseValue();
            }
            return BaseValue;
        }
        public void SetAttributeBaseValue(FGameplayAttribute Attribute, float NewBaseValue)
        {
            UAttributeSet Set = null;
            Type AttributeSetClass = Attribute.AttributeOwner;
            if (AttributeSetClass != null && AttributeSetClass.IsSubclassOf(typeof(UAttributeSet)))
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

            FieldInfo fi = Attribute.GetUProperty();
            FGameplayAttributeData DataPtr = (FGameplayAttributeData)fi.GetValue(Set);

            OldBaseValue = DataPtr.GetBaseValue();
            DataPtr.SetBaseValue(NewBaseValue);
            bBaseValueSet = true;

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
        private void InternalUpdateNumericalAttribute(FGameplayAttribute Attribute, float NewValue, FGameplayEffectModCallbackData ModData, bool bFromRecursiveCall = false)
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

                //如何一次应用多个栈?如果我们触发一个可以拒绝应用程序的溢出怎么办?
                //我们仍然想应用那些没有让我们崩溃的栈，但我们也想调用HandleActiveGameplayEffectStackOverflow。
                //暂时:只有在已经达到限制时才调用HandleActiveGameplayEffectStackOverflow。否则，我们只需将堆栈限制设置为最大。
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

                UGameplayEffect GEDef = ExistingSpec.Def;
                if (GEDef.StackDurationRefreshPolicy == EGameplayEffectStackingDurationPolicy.NeverRefresh)
                {
                    bSetDuration = false;
                }
                else
                {
                    RestartActiveGameplayEffectDuration(ExistingStackableGE);
                }
                if (GEDef.StackPeriodResetPolicy == EGameplayEffectStackingPeriodPolicy.NeverReset)
                {
                    bSetPeriod = false;
                }
            }
            else
            {
                FActiveGameplayEffectHandle NewHandle = FActiveGameplayEffectHandle.GenerateNewHandle(Owner);
                AppliedActiveGE = new FActiveGameplayEffect()
                {
                    Handle = NewHandle,
                    Spec = Spec,
                };
                GameplayEffects_Internal.Add(AppliedActiveGE);

                //UAbilitySystemGlobals.Get().SetCurrentAppliedGE(AppliedActiveGE.Spec);

                FGameplayEffectSpec AppliedEffectSpec = AppliedActiveGE.Spec;
                //确保收集了目标的标签，以便我们可以正确地过滤无限效果
                //AppliedEffectSpec.CapturedTargetTags.GetActorTags().Reset();
                //Owner->GetOwnedGameplayTags(AppliedEffectSpec.CapturedTargetTags.GetActorTags());
                //计算所有修改器的大小。有些可能需要在以后根据属性更改等进行更新，但这些应该进行更新
                //通过委托回调完成。
                //AppliedEffectSpec.CaptureAttributeDataFromTarget(Owner);
                //AppliedEffectSpec.CalculateModifierMagnitudes();

                //构建ModifiedAttribute列表，以便GCs可以在非周期的情况下获得量级信息
                //注意:有一天可能不想调用gameplay cues，除非正在进行的标签要求得到满足(将需要将此移动到那里)
                bool HasModifiedAttributes = AppliedEffectSpec.ModifiedAttributes.Count > 0;
                bool HasDurationAndNoPeriod = AppliedEffectSpec.Def.DurationPolicy == EGameplayEffectDurationType.HasDuration && AppliedEffectSpec.GetPeriod() == 0;
                bool HasPeriodAndNoDuration = AppliedEffectSpec.Def.DurationPolicy == EGameplayEffectDurationType.Instant && AppliedEffectSpec.GetPeriod() > 0;
                bool ShouldBuildModifiedAttributeList = !HasModifiedAttributes && (HasDurationAndNoPeriod || HasPeriodAndNoDuration);
                if (ShouldBuildModifiedAttributeList)
                {
                    int ModifierIndex = -1;
                    for (int i = 0; i < AppliedEffectSpec.Def.Modifiers.Count; i++)
                    {
                        FGameplayModifierInfo Mod = AppliedEffectSpec.Def.Modifiers[i];
                        ++ModifierIndex;
                        //从已计算的数量级中取数量级
                        float Magnitude = 0.0f;
                        FModifierSpec ModSpec = AppliedEffectSpec.Modifiers[ModifierIndex];
                        Magnitude = ModSpec.GetEvaluatedMagnitude();
                        ////如果ModifiedAttribute列表不存在，则添加到列表中
                        FGameplayEffectModifiedAttribute ModifiedAttribute = AppliedEffectSpec.GetModifiedAttribute(Mod.Attribute);
                        if (ModifiedAttribute != null)
                        {
                            ModifiedAttribute = AppliedEffectSpec.AddModifiedAttribute(Mod.Attribute);
                        }
                        ModifiedAttribute.TotalMagnitude += Magnitude;
                    }
                }
                //重新计算持续时间，因为它可能依赖于目标捕获的属性
                if (AppliedEffectSpec.AttemptCalculateDurationFromDef(out float DefCalcDuration))
                {
                    AppliedEffectSpec.SetDuration(DefCalcDuration, false);
                }
                else if (AppliedEffectSpec.Def.DurationMagnitude.GetMagnitudeCalculationType() == EGameplayEffectMagnitudeCalculation.SetByCaller)
                {
                    AppliedEffectSpec.Def.DurationMagnitude.AttemptCalculateMagnitude(AppliedEffectSpec, out AppliedEffectSpec.Duration);
                }
                float DurationBaseValue = AppliedEffectSpec.GetDuration();
                if (DurationBaseValue > 0)
                {
                    float FinalDuration = AppliedEffectSpec.CalculateModifiedDuration();
                    //我们无法将自己修改为即时或无限持续的效果
                    if (FinalDuration <= 0.0f)
                    {
                        FinalDuration = 0.1f;
                    }
                    AppliedEffectSpec.SetDuration(FinalDuration, true);
                    //用定时器管理器注册持续时间的回调函数
                    if (Owner != null && bSetDuration)
                    {
                        FTimerManager TimerManager = Owner.GetWorld().GetTimerManager();
                        TimerManager.SetTimer(ref AppliedActiveGE.DurationHandle, TimerDelegate<UAbilitySystemComponent, FActiveGameplayEffectHandle>.Create((@owner, @handle) =>
                        {
                            @owner.CheckDurationExpired(@handle);
                        }, Owner, AppliedActiveGE.Handle), FinalDuration, false);
                    }
                }
                if (bSetDuration && Owner != null && AppliedEffectSpec.GetPeriod() > 0)
                {
                    FTimerManager TimerManager = Owner.GetWorld().GetTimerManager();
                    ITimerDelegate Delegate = TimerDelegate<UAbilitySystemComponent, FActiveGameplayEffectHandle>.Create((@owner, @handle) =>
                    {
                        @owner.ExecutePeriodicEffect(@handle);
                    }, Owner, AppliedActiveGE.Handle);
                    //计时器管理器在第一个tick检查活动列表后将事情从挂起列表移动到活动列表，因此我们需要在这里执行
                    if (AppliedEffectSpec.Def.bExecutePeriodicEffectOnApplication)
                    {
                        TimerManager.SetTimerForNextTick(Delegate);
                    }
                    TimerManager.SetTimer(ref AppliedActiveGE.PeriodHandle, Delegate, AppliedEffectSpec.GetPeriod(), true);
                }
            }
            // @注意@todo:这是目前假设(可能是错误的)堆叠GE的抑制状态不会改变
            //作为堆叠的结果。实际上，它可以在具有不同动态授予标记集的复杂情况下使用。
            if (ExistingStackableGE != null)
            {
                OnStackCountChange(ExistingStackableGE, StartingStackCount, NewStackCount);
            }
            else
            {
                InternalOnActiveGameplayEffectAdded(AppliedActiveGE);
            }
            return AppliedActiveGE;
        }
        public void OnStackCountChange(FActiveGameplayEffect ActiveEffect, int OldStackCount, int NewStackCount)
        {
            if (OldStackCount != NewStackCount)
            {
                //只有当堆栈计数实际改变时才更新属性。
                UpdateAllAggregatorModMagnitudes(ActiveEffect);
            }
        }
        public void UpdateAllAggregatorModMagnitudes(FActiveGameplayEffect ActiveEffect)
        {
            //我们不应该为周期性效果这样做，因为它们的mod在属性聚合器上不是持久化的
            if (ActiveEffect.Spec.GetPeriod() > 0)
            {
                return;
            }
            ////我们不需要更新抑制效果
            if (ActiveEffect.bIsInhibited)
            {
                return;
            }
            FGameplayEffectSpec Spec = ActiveEffect.Spec;
            if (Spec.Def == null)
            {
                UnityEngine.Debug.LogError($"UpdateAllAggregatorModMagnitudes called with no UGameplayEffect def.");
                return;
            }
            HashSet<FGameplayAttribute> AttributesToUpdate = new HashSet<FGameplayAttribute>();
            for (int ModIdx = 0; ModIdx < Spec.Modifiers.Count; ++ModIdx)
            {
                FGameplayModifierInfo ModDef = Spec.Def.Modifiers[ModIdx];
                AttributesToUpdate.Add(ModDef.Attribute);
            }
            UpdateAggregatorModMagnitudes(AttributesToUpdate, ActiveEffect);
        }
        void UpdateAggregatorModMagnitudes(HashSet<FGameplayAttribute> AttributesToUpdate, FActiveGameplayEffect ActiveEffect)
        {
            FGameplayEffectSpec Spec = ActiveEffect.Spec;
            foreach (FGameplayAttribute Attribute in AttributesToUpdate)
            {
                // skip over any modifiers for attributes that we don't have
                if (Owner == null || Owner.HasAttributeSetForAttribute(Attribute) == false)
                {
                    continue;
                }

                FAggregator Aggregator = FindOrCreateAttributeAggregator(Attribute);
                // Update the aggregator Mods.
                //Aggregator.UpdateAggregatorMod(ActiveEffect.Handle, Attribute, Spec, ActiveEffect.PredictionKey.WasLocallyGenerated(), ActiveEffect.Handle);
            }
        }
        public bool CanApplyAttributeModifiers(UGameplayEffect GameplayEffect, float Level, FGameplayEffectContextHandle EffectContext)
        {
            FGameplayEffectSpec Spec = new FGameplayEffectSpec(GameplayEffect, EffectContext, Level);

            Spec.CalculateModifierMagnitudes();

            for (int ModIdx = 0; ModIdx < Spec.Modifiers.Count; ++ModIdx)
            {
                FGameplayModifierInfo ModDef = Spec.Def.Modifiers[ModIdx];
                FModifierSpec ModSpec = Spec.Modifiers[ModIdx];

                // It only makes sense to check additive operators
                if (ModDef.ModifierOp == EGameplayModOp.Additive)
                {
                    UAttributeSet Set = Owner.GetAttributeSubobject(ModDef.Attribute.AttributeOwner);
                    float CurrentValue = ModDef.Attribute.GetNumericValueChecked(Set);
                    float CostValue = ModSpec.GetEvaluatedMagnitude();

                    if (CurrentValue + CostValue < 0.0f)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        FAggregator FindOrCreateAttributeAggregator(FGameplayAttribute Attribute)
        {
            if (AttributeAggregatorMap.TryGetValue(Attribute, out FAggregator RefPtr))
            {
                return RefPtr;
            }

            // 为此属性创建一个新的聚合器。
            float CurrentBaseValueOfProperty = Owner.GetNumericAttributeBase(Attribute);

            FAggregator NewAttributeAggregator = new FAggregator(CurrentBaseValueOfProperty);
            UAttributeSet AttributeSet = null;
            Type AttributeSetClass = Attribute.AttributeOwner;
            //if (Attribute.IsSystemAttribute() == false)
            {
                // 回调，以防集合想做什么
                UAttributeSet Set = Owner.GetAttributeSubobject(AttributeSetClass);
                //Set.OnAttributeAggregatorCreated(Attribute, NewAttributeAggregator);
            }
            AttributeAggregatorMap.Add(Attribute, (NewAttributeAggregator));
            return NewAttributeAggregator;
        }
        //在客户端和服务器端添加新的ActiveGameplayEffect时调用*/
        public void InternalOnActiveGameplayEffectAdded(FActiveGameplayEffect Effect)
        {
            UGameplayEffect EffectDef = Effect.Spec.Def;

            //将我们正在进行的标记需求添加到依赖关系图中。我们将在下面检查这些标签。

            //如有必要，添加任何可能影响效果的外部依赖项

            //检查我们是否应该打开(这将是我们第一次打开)
            FGameplayTagContainer OwnerTags = new FGameplayTagContainer();
            Owner.GetOwnedGameplayTags(OwnerTags);
            Effect.CheckOngoingTagRequirements(OwnerTags, this);
        }
        public OnGameplayAttributeValueChange GetGameplayAttributeValueChangeDelegate(FGameplayAttribute Attribute)
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
            for (int ActiveGEIdx = 0; ActiveGEIdx < GameplayEffects_Internal.Count; ++ActiveGEIdx)
            {
                FActiveGameplayEffect Effect = GameplayEffects_Internal[ActiveGEIdx];
                if (Effect.Handle == Handle)
                {
                    if (Effect.IsPendingRemove)
                    {
                        break;
                    }
                }
                FTimerManager TimerManager = Owner.GetWorld().GetTimerManager();
                float Duration = Effect.GetDuration();
                //float CurrentTime = GetWorldTime();
                int StacksToRemove = -2;
                bool RefreshStartTime = false;
                bool RefreshDurationTimer = false;
                bool CheckForFinalPeriodicExec = false;
                if (Duration > 0)
                {
                    switch (Effect.Spec.Def.StackExpirationPolicy)
                    {
                        case EGameplayEffectStackingExpirationPolicy.ClearEntireStack:
                            StacksToRemove = -1; // Remove all stacks
                            CheckForFinalPeriodicExec = true;
                            break;

                        case EGameplayEffectStackingExpirationPolicy.RemoveSingleStackAndRefreshDuration:
                            StacksToRemove = 1;
                            CheckForFinalPeriodicExec = (Effect.Spec.StackCount == 1);
                            RefreshStartTime = true;
                            RefreshDurationTimer = true;
                            break;
                        case EGameplayEffectStackingExpirationPolicy.RefreshDuration:
                            RefreshStartTime = true;
                            RefreshDurationTimer = true;
                            break;
                    }
                }
                else
                {
                    //效果尚未完成，只需刷新其持续时间定时器
                    RefreshDurationTimer = true;
                }
                if (CheckForFinalPeriodicExec)
                {
                    //这个游戏效果已经达到了它的持续时间。检查它是否需要在删除前最后一次执行。
                    if (Effect.PeriodHandle.IsValid() && TimerManager.TimerExists(Effect.PeriodHandle))
                    {
                        float PeriodTimeRemaining = TimerManager.GetTimerRemaining(Effect.PeriodHandle);
                        if (PeriodTimeRemaining <= (1.0E-4F) && !Effect.bIsInhibited)
                        {
                            InternalExecutePeriodicGameplayEffect(Effect);

                            //在InternalExecutePeriodicGameplayEffect中调用ExecuteActiveEffectsFrom会导致这个效果被显式地移除
                            //(例如，它可以杀死所有者并导致通过死亡来消除效果)。
                            //在这种情况下，我们需要提前退出，而不是继续下面的调用InternalRemoveActiveGameplayEffect
                            if (Effect.IsPendingRemove)
                            {
                                break;
                            }
                            // 强制清除周期性刻度，因为此效果将被删除
                            TimerManager.ClearTimer(Effect.PeriodHandle);
                        }
                    }
                }
                if (StacksToRemove >= -1)
                {
                    InternalRemoveActiveGameplayEffect(ActiveGEIdx, StacksToRemove, false);
                }
                if (RefreshStartTime)
                {
                    RestartActiveGameplayEffectDuration(Effect);
                }
            }
        }
        public void InternalExecutePeriodicGameplayEffect(FActiveGameplayEffect ActiveEffect)
        {
            if (!ActiveEffect.bIsInhibited)
            {
                for (int i = 0; i < ActiveEffect.Spec.Def.Modifiers.Count; i++)
                {
                    FGameplayModifierInfo Modifier = ActiveEffect.Spec.Def.Modifiers[i];
                    float Magnitude = 0.0f;
                    Modifier.ModifierMagnitude.AttemptCalculateMagnitude(ActiveEffect.Spec, out Magnitude);
                    //UnityEngine.Debug.Log($"{Modifier.Attribute}: {Modifier.ModifierOp}  {Magnitude}");
                }
                // 每次定时执行前清除修改后的属性
                ActiveEffect.Spec.ModifiedAttributes.Clear();

                // Execute
                ExecuteActiveEffectsFrom(ActiveEffect.Spec);
                // 为正在执行的周期性效果调用委托
                UAbilitySystemComponent SourceASC = ActiveEffect.Spec.GetContext().GetInstigatorAbilitySystemComponent();
                //Owner.OnPeriodicGameplayEffectExecuteOnSelf(SourceASC, ActiveEffect.Spec, ActiveEffect.Handle);
                if (SourceASC != null)
                {
                    //SourceASC.OnPeriodicGameplayEffectExecuteOnTarget(Owner, ActiveEffect.Spec, ActiveEffect.Handle);
                }
            }
        }
        public bool HasApplicationImmunityToSpec(FGameplayEffectSpec SpecToApply, FActiveGameplayEffect OutGEThatProvidedImmunity)
        {
            FGameplayTagContainer AggregatedSourceTags = SpecToApply.CapturedSourceTags.GetAggregatedTags();
            if (AggregatedSourceTags == null)
            {
                return false;
            }
            for (int i = 0; i < ApplicationImmunityQueryEffects.Count; i++)
            {
                UGameplayEffect EffectDef = ApplicationImmunityQueryEffects[i];
                if (EffectDef.GrantedApplicationImmunityQuery.Matches(SpecToApply))
                {
                    for (int n = 0; n < GameplayEffects_Internal.Count; n++)
                    {
                        FActiveGameplayEffect Effect = GameplayEffects_Internal[n];
                        if (Effect.Spec.Def == EffectDef)
                        {
                            OutGEThatProvidedImmunity = Effect;
                            return true;
                        }
                    }
                    break;
                };
            }
            if (!AggregatedSourceTags.HasAny(ApplicationImmunityGameplayTagCountContainer.GetExplicitGameplayTags()))
            {
                return false;
            }
            for (int n = 0; n < GameplayEffects_Internal.Count; n++)
            {
                FActiveGameplayEffect Effect = GameplayEffects_Internal[n];
                if (Effect.Spec.Def.GrantedApplicationImmunityTags.IsEmpty() == false && Effect.Spec.Def.GrantedApplicationImmunityTags.RequirementsMet(AggregatedSourceTags))
                {
                    OutGEThatProvidedImmunity = Effect;
                    return true;
                }
            }
            return false;
        }
        public FActiveGameplayEffect FindStackableActiveGameplayEffect(FGameplayEffectSpec Spec)
        {
            FActiveGameplayEffect StackableGE = null;
            UGameplayEffect GEDef = Spec.Def;
            EGameplayEffectStackingType StackingType = GEDef.StackingType;
            if ((StackingType != EGameplayEffectStackingType.None) && (GEDef.DurationPolicy != EGameplayEffectDurationType.Instant))
            {
                //迭代GameplayEffects以查看是否找到匹配的结果。请注意，我们可以在map中缓存一个句柄，但我们仍然会这样做
                //在GameplayEffects中进行线性搜索以找到实际的FActiveGameplayEffect(因为GameplayEffects数组不稳定)。
                //如果这成为分析器的一个慢点，map仍然可以作为早期退出以避免不必要的扫描。
                UAbilitySystemComponent SourceASC = Spec.GetContext().GetInstigatorAbilitySystemComponent();
                for (int i = 0; i < GameplayEffects_Internal.Count; i++)
                {
                    FActiveGameplayEffect ActiveEffect = GameplayEffects_Internal[i];
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
            UGameplayEffect StackedGE = OldSpec.Def;
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
                for (int i = 0; i < InSpec.Def.RemoveGameplayEffectsWithTags.CombinedTags.Num(); i++)
                {

                }

            }
        }
        public void RestartActiveGameplayEffectDuration(FActiveGameplayEffect ActiveGameplayEffect)
        {
        }
        public bool InternalRemoveActiveGameplayEffect(int Idx, int StacksToRemove, bool bPrematureRemoval)
        {
            if (Idx < GetNumGameplayEffects())
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
                    // 这不会是一个完全的删除，只是改变了StackCount.
                    int StartingStackCount = Effect.Spec.StackCount;
                    Effect.Spec.StackCount -= StacksToRemove;
                    OnStackCountChange(Effect, StartingStackCount, Effect.Spec.StackCount);
                    return false;
                }
                // 调用移除GameplayCue事件
                bool ShouldInvokeGameplayCueEvent = true;
                ShouldInvokeGameplayCueEvent &= !Effect.bIsInhibited;
                // 标记待移除的效果，并移除该效果的所有副作用
                InternalOnActiveGameplayEffectRemoved(Effect, ShouldInvokeGameplayCueEvent, GameplayEffectRemovalInfo);

                if (Effect.DurationHandle.IsValid())
                {
                    Owner.GetWorld().GetTimerManager().ClearTimer(Effect.DurationHandle);
                }
                if (Effect.PeriodHandle.IsValid())
                {
                    Owner.GetWorld().GetTimerManager().ClearTimer(Effect.PeriodHandle);
                }
                //从全局映射中删除此句柄
                Effect.Handle.RemoveFromGlobalMap();
                //如有必要，尝试使用过期效果
                InternalApplyExpirationEffects(Effect.Spec, bPrematureRemoval);
                bool ModifiedArray = false;
                GameplayEffects_Internal.RemoveAt(Idx);
                ModifiedArray = true;
                return ModifiedArray;
            }
            return false;
        }
        public void InternalApplyExpirationEffects(FGameplayEffectSpec ExpiringSpec, bool bPrematureRemoval)
        {
            //不要预测过期的影响
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
        //由客户端和服务器调用:无论效果是在本地删除还是由于复制而删除，都必须进行清理
        public void InternalOnActiveGameplayEffectRemoved(FActiveGameplayEffect Effect, bool bInvokeGameplayCueEvents, FGameplayEffectRemovalInfo GameplayEffectRemovalInfo)
        {

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
        public FActiveGameplayEffect GetActiveGameplayEffect(FActiveGameplayEffectHandle Handle)
        {
            for (int i = 0; i < GameplayEffects_Internal.Count; i++)
            {
                FActiveGameplayEffect Effect = GameplayEffects_Internal[i];
                if (Effect.Handle == Handle)
                {
                    return Effect;
                }
            }
            return null;
        }
        public void ExecutePeriodicGameplayEffect(FActiveGameplayEffectHandle Handle)
        {
            FActiveGameplayEffect ActiveEffect = GetActiveGameplayEffect(Handle);

            if (ActiveEffect != null)
            {
                InternalExecutePeriodicGameplayEffect(ActiveEffect);
            }
        }
    }
    public class FGameplayEffectRemovalInfo
    {
        /** 当玩法效果的持续时间没有过期时是正确的，这意味着玩法效果被强行移除 */
        public bool bPrematureRemoval = false;

        /** 这个游戏效果在被移除之前的堆栈数量。 */
        public int StackCount = 0;

        /** 演员这个游戏效果是有针对性的。 */
        public FGameplayEffectContextHandle EffectContext;

    }
    public struct FScopeCurrentGameplayEffectBeingApplied
    {
        public FScopeCurrentGameplayEffectBeingApplied(FGameplayEffectSpec Spec, UAbilitySystemComponent AbilitySystemComponent)
        {
            //UAbilitySystemGlobals.Get().PushCurrentAppliedGE(Spec, AbilitySystemComponent);
        }
    };
}

