using System.Collections.Generic;
using UnityEngine.UIElements;

namespace GameplayAbilitySystem
{
    public class FScalableFloat
    {
        public float Value;
        public FScalableFloat(float InInitialValue)
        {
            Value = InInitialValue;
        }
        public float GetValueAtLevel(float Level)
        {
            float OutFloat = 0;
            EvaluateCurveAtLevel(ref OutFloat);
            return OutFloat;
        }
        public bool EvaluateCurveAtLevel(ref float OutValue)
        {
            OutValue = Value;
            return true;
        }
    }
    public class FGameplayModifierEvaluatedData
    {
        public FGameplayAttribute Attribute;
        public EGameplayModOp ModifierOp;
        public float Magnitude;
    }
    public class FGameplayModifierInfo
    {
        public FGameplayAttribute Attribute;
        public EGameplayModOp ModifierOp;
        public FGameplayEffectModifierMagnitude ModifierMagnitude;
        public FGameplayTagRequirements SourceTags;
        public FGameplayTagRequirements TargetTags;
        public FGameplayModifierInfo()
        {
            Attribute = new FGameplayAttribute();
        }
    }
    public enum EGameplayEffectAttributeCaptureSource
    {
        /** Source (caster) of the gameplay effect. */
        Source,
        /** Target (recipient) of the gameplay effect. */
        Target
    }
    public class FGameplayEffectAttributeCaptureDefinition { }

    public class FGameplayEffectModifierMagnitude
    {
        public static implicit operator FGameplayEffectModifierMagnitude(FScalableFloat InScalableFloatMagnitude)
        {
            return new FGameplayEffectModifierMagnitude(InScalableFloatMagnitude);
        }
        EGameplayEffectMagnitudeCalculation MagnitudeCalculationType;
        FScalableFloat ScalableFloatMagnitude;
        public FGameplayEffectModifierMagnitude(FScalableFloat InScalableFloatMagnitude)
        {
            ScalableFloatMagnitude = InScalableFloatMagnitude;
            MagnitudeCalculationType = EGameplayEffectMagnitudeCalculation.ScalableFloat;
        }
        public EGameplayEffectMagnitudeCalculation GetMagnitudeCalculationType() { return MagnitudeCalculationType; }
        public bool AttemptCalculateMagnitude(FGameplayEffectSpec InRelevantSpec, out float OutCalculatedMagnitude, bool WarnIfSetByCallerFail = true, float DefaultSetbyCaller = 0)
        {
            bool bCanCalc = CanCalculateMagnitude(InRelevantSpec);
            OutCalculatedMagnitude = 0;
            if (bCanCalc)
            {
                switch (MagnitudeCalculationType)
                {
                    case EGameplayEffectMagnitudeCalculation.ScalableFloat:
                        OutCalculatedMagnitude = ScalableFloatMagnitude.GetValueAtLevel(InRelevantSpec.GetLevel());
                        break;
                    case EGameplayEffectMagnitudeCalculation.AttributeBased:
                        OutCalculatedMagnitude = 0;
                        break;
                    case EGameplayEffectMagnitudeCalculation.CustomCalculationClass:
                        OutCalculatedMagnitude = 0;
                        break;
                    case EGameplayEffectMagnitudeCalculation.SetByCaller:
                        OutCalculatedMagnitude = 0;
                        break;
                    default:
                        break;
                }
            }
            else
            {
                OutCalculatedMagnitude = 0.0f;
            }

            return bCanCalc;
        }
        public bool CanCalculateMagnitude(FGameplayEffectSpec InRelevantSpec)
        {
            List<FGameplayEffectAttributeCaptureDefinition> ReqCaptureDefs = new List<FGameplayEffectAttributeCaptureDefinition>();
            GetAttributeCaptureDefinitions(ReqCaptureDefs);
            return InRelevantSpec.HasValidCapturedAttributes(ReqCaptureDefs);
        }
        public void GetAttributeCaptureDefinitions(List<FGameplayEffectAttributeCaptureDefinition> OutCaptureDefs)
        {
            switch (MagnitudeCalculationType)
            {
                case EGameplayEffectMagnitudeCalculation.ScalableFloat:
                    break;
                case EGameplayEffectMagnitudeCalculation.AttributeBased:
                    break;
                case EGameplayEffectMagnitudeCalculation.CustomCalculationClass:
                    break;
                case EGameplayEffectMagnitudeCalculation.SetByCaller:
                    break;
                default:
                    break;
            }
        }
    }
}

