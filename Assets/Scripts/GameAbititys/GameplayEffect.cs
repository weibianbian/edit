using RailShootGame;
using Sirenix.Utilities;
using System.Collections.Generic;
using UnityEngine.Rendering;

namespace GameplayAbilitySystem
{
    public enum EGameplayEffectDurationType
    {
        /** This effect applies instantly */
        Instant,
        /** This effect lasts forever */
        Infinite,
        /** The duration of this effect will be specified by a magnitude */
        HasDuration
    }
    public class GameplayEffect
    {
        public EGameplayEffectDurationType DurationPolicy;
        public EGameplayEffectStackingType StackingType;
        public List<GameplayModifierInfo> Modifiers = new List<GameplayModifierInfo>();
        public List<GameplayCue> GameplayCues = new List<GameplayCue>();
        public FInheritedTagContainer RemoveGameplayEffectsWithTags = new FInheritedTagContainer();
        public FGameplayEffectModifierMagnitude DurationMagnitude;

        public float Period;
        public float Duration;
        public int StackLimitCount;
        public bool bDenyOverflowApplication = false;
        public bool bClearStackOnOverflow = false;

        public GameplayEffect()
        {
            DurationPolicy = EGameplayEffectDurationType.Instant;
        }
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
        public FGameplayEffectSpec(GameplayEffect InDef, GameplayEffectContextHandle InEffectContext, float InLevel)
        {
            CapturedRelevantAttributes = new FGameplayEffectAttributeCaptureSpecContainer();
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
        public void SetLevel(float InLevel)
        {
            Level = InLevel;
        }
        public float GetLevel()
        {
            return Level;
        }
        public float GetPeriod()
        {
            return Period;
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
        public float CalculateModifiedDuration()
        {
            return 0;
        }
        public void CalculateModifierMagnitudes()
        {
            for (int ModIdx = 0; ModIdx < Modifiers.Count; ModIdx++)
            {
                GameplayModifierInfo ModDef = Def.Modifiers[ModIdx];
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
    public class FGameplayEffectAttributeCaptureSpecContainer
    {
        public bool HasValidCapturedAttributes(List<FGameplayEffectAttributeCaptureDefinition> InCaptureDefsToCheck)
        {
            bool bHasValid = true;
            return bHasValid;
        }
    }
    public class GameplayEffectSpecHandle
    {
        public FGameplayEffectSpec Data;
        public GameplayEffectSpecHandle(FGameplayEffectSpec InData)
        {
            Data = InData;
        }
    }
    public class GameplayEffectContext
    {
        public Actor Instigator;
        public Actor EffectCauser;
        public AbilitySystemComponent InstigatorAbilitySystemComponent;
        public void AddInstigator(Actor InInstigator, Actor InEffectCauser)
        {
            Instigator = InInstigator;
            EffectCauser = InEffectCauser;
        }
        public AbilitySystemComponent GetInstigatorAbilitySystemComponent()
        {
            return InstigatorAbilitySystemComponent;
        }
    }
    public class GameplayEffectContextHandle
    {
        public GameplayEffectContext Data;
        public GameplayEffectContextHandle(GameplayEffectContext InData)
        {
            Data = InData;
        }
        public void AddInstigator(Actor InInstigator, Actor InEffectCauser)
        {
            Data.AddInstigator(InInstigator, InEffectCauser);
        }
        public AbilitySystemComponent GetInstigatorAbilitySystemComponent()
        {
            return Data.GetInstigatorAbilitySystemComponent();
        }
    }
    public class GameplayCue { }
}

