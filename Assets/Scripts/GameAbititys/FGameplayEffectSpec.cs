using RailShootGame;
using System.Collections.Generic;

namespace GameplayAbilitySystem
{
    public class FGameplayEffectSpec
    {
        public UGameplayEffect Def;
        public float Duration;
        public float Period;
        public float Level;
        //在0.0-1.0范围内，这个GameplayEffect将应用于目标属性或GameplayEffect的概率
        public float ChanceToApplyToTarget;
        private FGameplayEffectContextHandle EffectContext;
        public int StackCount;
        public bool bDurationLocked = false;
        public List<FGameplayEffectSpecHandle> TargetEffectSpecs = new List<FGameplayEffectSpecHandle>();
        public List<FModifierSpec> Modifiers = new List<FModifierSpec>();
        public FGameplayEffectAttributeCaptureSpecContainer CapturedRelevantAttributes;
        public List<FGameplayEffectModifiedAttribute> ModifiedAttributes;
        //捕获的源标签GameplayEffectSpec创建
        public FTagContainerAggregator CapturedSourceTags = new FTagContainerAggregator();
        public FTagContainerAggregator CapturedTargetTags = new FTagContainerAggregator();
        //被授予且不是来自UGameplayEffect def的标签。它们被复制
        public FGameplayTagContainer DynamicGrantedTags = new FGameplayTagContainer();
        //在这个效果规范上的标签不是来自UGameplayEffect def。这些是重复的
        public FGameplayTagContainer DynamicAssetTags = new FGameplayTagContainer();
        public FGameplayEffectSpec(UGameplayEffect InDef, FGameplayEffectContextHandle InEffectContext, float InLevel)
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
        public void Initialize(UGameplayEffect InDef, FGameplayEffectContextHandle InEffectContext, float InLevel)
        {
            Def = InDef;
            Level = InLevel;
            SetContext(InEffectContext);
            SetLevel(InLevel);
            for (int i = 0; i < InDef.Modifiers.Count; i++)
            {
                Modifiers.Add(new FModifierSpec());
            }
            CapturedSourceTags.GetSpecTags().AppendTags(InDef.InheritableGameplayEffectTags.CombinedTags);
        }
        public void SetContext(FGameplayEffectContextHandle NewEffectContext)
        {
            EffectContext = NewEffectContext;
        }
        public FGameplayEffectContextHandle GetEffectContext()
        {
            return EffectContext;
        }
        public FGameplayEffectContextHandle GetContext()
        {
            return EffectContext;
        }
        public FGameplayTagContainer GetDynamicAssetTags()
        {
            return DynamicAssetTags;
        }
        public float GetChanceToApplyToTarget()
        {
            return ChanceToApplyToTarget;
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

