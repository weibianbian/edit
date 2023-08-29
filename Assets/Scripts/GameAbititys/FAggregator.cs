namespace GameplayAbilitySystem
{
    public class FAggregator
    {
        public float BaseValue = 0;
        public float GetBaseValue()
        {
            return BaseValue;
        }
        public void SetBaseValue(float NewBaseValue)
        {
            BaseValue = NewBaseValue;
        }
        public static float StaticExecModOnBaseValue(float BaseValue, EGameplayModOp ModifierOp, float EvaluatedMagnitude)
        {
            switch (ModifierOp)
            {
                case EGameplayModOp.Override:
                    {
                        BaseValue = EvaluatedMagnitude;
                        break;
                    }
                case EGameplayModOp.Additive:
                    {
                        BaseValue += EvaluatedMagnitude;
                        break;
                    }
                case EGameplayModOp.Multiplicitive:
                    {
                        BaseValue *= EvaluatedMagnitude;
                        break;
                    }
                case EGameplayModOp.Division:
                    {
                        BaseValue /= EvaluatedMagnitude;
                        break;
                    }
            }

            return BaseValue;
        }
    }

}

