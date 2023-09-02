namespace GameplayAbilitySystem
{
    public class FGameplayEffectModCallbackData
    {
        public FGameplayEffectSpec EffectSpec;
        public FGameplayModifierEvaluatedData EvaluatedData;
        public UAbilitySystemComponent Target;

        public FGameplayEffectModCallbackData(FGameplayEffectSpec InEffectSpec, FGameplayModifierEvaluatedData InEvaluatedData, UAbilitySystemComponent InTarget)
        {
            EffectSpec = InEffectSpec;
            EvaluatedData = InEvaluatedData;
            Target = InTarget;
        }
    }

}

