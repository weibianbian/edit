using Sirenix.Utilities;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.Rendering;

namespace GameplayAbilitySystem
{
    public class GameplayEffect
    {
        public EGameplayEffectDurationType DurationPolicy;
        public EGameplayEffectStackingType StackingType;
        public EGameplayEffectStackingExpirationPolicy StackExpirationPolicy;
        public List<FGameplayModifierInfo> Modifiers = new List<FGameplayModifierInfo>();
        public List<GameplayCue> GameplayCues = new List<GameplayCue>();
        public FInheritedTagContainer RemoveGameplayEffectsWithTags = new FInheritedTagContainer();
        public FGameplayEffectModifierMagnitude DurationMagnitude;
        public FGameplayTagRequirements OngoingTagRequirements;
        public FScalableFloat Period;
        public float Duration;
        public int StackLimitCount;
        public bool bDenyOverflowApplication = false;
        public bool bClearStackOnOverflow = false;
        /*如果为true，效果在应用程序上执行，然后在每个周期间隔执行。如果为false，则在第一个周期结束之前不会执行。*/
        public bool bExecutePeriodicEffectOnApplication;

        public GameplayEffect()
        {
            DurationPolicy = EGameplayEffectDurationType.Instant;
            Period = new FScalableFloat(0);
            OngoingTagRequirements = new FGameplayTagRequirements();
            bExecutePeriodicEffectOnApplication = true;
        }
    }
    public class FAggregatorRef
    {

    }
    public class FModifierSpec
    {
        public float EvaluatedMagnitude;
        public float GetEvaluatedMagnitude() { return EvaluatedMagnitude; }
    }
    public enum EGameplayEffectMagnitudeCalculation
    {
        ScalableFloat,
        /** Perform a calculation based upon an attribute. */
        AttributeBased,
        /** Perform a custom calculation, capable of capturing and acting on multiple attributes, in either BP or native. */
        CustomCalculationClass,
        /** This magnitude will be set explicitly by the code/blueprint that creates the spec. */
        SetByCaller,
    }
    public class FGameplayEffectSpec
    {
        public GameplayEffect Def;
        public float Duration;
        public float Period;
        public float Level;
        private GameplayEffectContextHandle EffectContext;
        public int StackCount;
        public bool bDurationLocked = false;
        public List<GameplayEffectSpecHandle> TargetEffectSpecs = new List<GameplayEffectSpecHandle>();
        public List<FModifierSpec> Modifiers = new List<FModifierSpec>();
        public FGameplayEffectAttributeCaptureSpecContainer CapturedRelevantAttributes;
        public List<FGameplayEffectModifiedAttribute> ModifiedAttributes;
        public FTagContainerAggregator CapturedSourceTags = new FTagContainerAggregator();
        public FTagContainerAggregator CapturedTargetTags = new FTagContainerAggregator();
        public FGameplayEffectSpec(GameplayEffect InDef, GameplayEffectContextHandle InEffectContext, float InLevel)
        {
            CapturedRelevantAttributes = new FGameplayEffectAttributeCaptureSpecContainer();
            ModifiedAttributes = new List<FGameplayEffectModifiedAttribute>();
            StackCount = 1;
            Initialize(InDef, InEffectContext, InLevel);
        }
        public FGameplayEffectSpec(FGameplayEffectSpec Other)
        {
            CapturedRelevantAttributes = new FGameplayEffectAttributeCaptureSpecContainer();
            StackCount = 1;
        }
        public void Initialize(GameplayEffect InDef, GameplayEffectContextHandle InEffectContext, float InLevel)
        {
            Def = InDef;
            Level = InLevel;
            SetContext(InEffectContext);
            SetLevel(InLevel);
            for (int i = 0; i < InDef.Modifiers.Count; i++)
            {
                Modifiers.Add(new FModifierSpec());
            }
        }
        public void SetContext(GameplayEffectContextHandle NewEffectContext)
        {
            EffectContext = NewEffectContext;
        }
        public GameplayEffectContextHandle GetEffectContext()
        {
            return EffectContext;
        }
        public GameplayEffectContextHandle GetContext()
        {
            return EffectContext;
        }
        public float GetModifierMagnitude(int ModifierIdx, bool bFactorInStackCount)
        {
            float SingleEvaluatedMagnitude = Modifiers[ModifierIdx].GetEvaluatedMagnitude();
            float ModMagnitude = SingleEvaluatedMagnitude;
            if (bFactorInStackCount)
            {
                ModMagnitude = GameplayEffectUtilities.ComputeStackedModifierMagnitude(SingleEvaluatedMagnitude, StackCount, Def.Modifiers[ModifierIdx].ModifierOp);
            }
            return ModMagnitude;
        }
        public FGameplayEffectModifiedAttribute AddModifiedAttribute(FGameplayAttribute Attribute)
        {
            FGameplayEffectModifiedAttribute NewAttribute = new FGameplayEffectModifiedAttribute();
            NewAttribute.Attribute = Attribute;
            ModifiedAttributes.Add(NewAttribute);
            return NewAttribute;
        }
        public void SetLevel(float InLevel)
        {
            Level = InLevel;

            Period = Def.Period.GetValueAtLevel(Level);
        }
        public float GetLevel()
        {
            return Level;
        }
        public float GetPeriod()
        {
            return Period;
        }
        public FGameplayEffectModifiedAttribute GetModifiedAttribute(FGameplayAttribute Attribute)
        {
            for (int i = 0; i < ModifiedAttributes.Count; i++)
            {
                FGameplayEffectModifiedAttribute ModifiedAttribute = ModifiedAttributes[i];
                if (ModifiedAttribute.Attribute == Attribute)
                {
                    return ModifiedAttribute;
                }
            }
            return null;
        }
        public void SetDuration(float NewDuration, bool bLockDuration)
        {
            if (!bDurationLocked)
            {
                Duration = NewDuration;
                bDurationLocked = bLockDuration;
                if (Duration > 0.0f)
                {
                    //如果游戏应用基于持续时间的即时效果的游戏效果，我们可能会有潜在的问题
                    // (例如, 每一次火焰伤害, 一个点也适用)。我们可能需要持续一段时间才能被捕获。
                    //CapturedRelevantAttributes.AddCaptureDefinition(AbilitySystemComponent.GetOutgoingDurationCapture());
                }
            }
        }
        public float GetDuration()
        {
            return Duration;
        }
        //辅助函数，在应用源和目标技能系统组件的相关修饰符后返回持续时间
        public float CalculateModifiedDuration()
        {
            FAggregator DurationAgg = new FAggregator();

            FAggregatorEvaluateParameters Params = new FAggregatorEvaluateParameters();
            Params.SourceTags = CapturedSourceTags.GetAggregatedTags();
            Params.TargetTags = CapturedTargetTags.GetAggregatedTags();
            return DurationAgg.EvaluateWithBase(GetDuration(), Params);
        }
        public void CalculateModifierMagnitudes()
        {
            for (int ModIdx = 0; ModIdx < Modifiers.Count; ModIdx++)
            {
                FGameplayModifierInfo ModDef = Def.Modifiers[ModIdx];
                FModifierSpec ModSpec = Modifiers[ModIdx];
                if (!ModDef.ModifierMagnitude.AttemptCalculateMagnitude(this, out ModSpec.EvaluatedMagnitude))
                {
                    ModSpec.EvaluatedMagnitude = 0.0f;
                }
            }
        }
        public bool HasValidCapturedAttributes(List<FGameplayEffectAttributeCaptureDefinition> InCaptureDefsToCheck)
        {
            return CapturedRelevantAttributes.HasValidCapturedAttributes(InCaptureDefsToCheck);
        }
        public bool AttemptCalculateDurationFromDef(out float OutDefDuration)
        {
            bool bCalculatedDuration = true;
            OutDefDuration = 0;
            EGameplayEffectDurationType DurType = Def.DurationPolicy;
            if (DurType == EGameplayEffectDurationType.Infinite)
            {
                OutDefDuration = -1;
            }
            else if (DurType == EGameplayEffectDurationType.Instant)
            {
                OutDefDuration = 0;
            }
            else
            {
                bCalculatedDuration = Def.DurationMagnitude.AttemptCalculateMagnitude(this, out OutDefDuration, false, 1.0f);
            }
            return bCalculatedDuration;
        }
    }
}

