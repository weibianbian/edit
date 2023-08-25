using System.Collections.Generic;

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
        public GameplayAttribute Attribute;
        public EGameplayModOp ModifierOp;
        public float Magnitude;
    }
    public class GameplayModifierInfo
    {
        public GameplayAttribute Attribute;
        public EGameplayModOp ModifierOp;
        public FGameplayEffectModifierMagnitude ModifierMagnitude;
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
        public bool AttemptCalculateMagnitude(GameplayEffectSpec InRelevantSpec, ref float OutCalculatedMagnitude)
        {
            bool bCanCalc = CanCalculateMagnitude(InRelevantSpec);
            if (bCanCalc)
            {
                switch (MagnitudeCalculationType)
                {
                    case EGameplayEffectMagnitudeCalculation.ScalableFloat:
                        OutCalculatedMagnitude = ScalableFloatMagnitude.GetValueAtLevel(InRelevantSpec.GetLevel());
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
            else
            {
                OutCalculatedMagnitude = 0.0f;
            }

            return bCanCalc;
        }
        public bool CanCalculateMagnitude(GameplayEffectSpec InRelevantSpec)
        {
            List<FGameplayEffectAttributeCaptureDefinition> ReqCaptureDefs=new List<FGameplayEffectAttributeCaptureDefinition>();
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

