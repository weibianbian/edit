namespace GameplayAbilitySystem
{
    public class FGameplayEffectModCallbackData
    {
        public FGameplayEffectSpec EffectSpec;
        public FGameplayModifierEvaluatedData EvaluatedData;
        public AbilitySystemComponent Target;

        public FGameplayEffectModCallbackData(FGameplayEffectSpec InEffectSpec, FGameplayModifierEvaluatedData InEvaluatedData, AbilitySystemComponent InTarget)
        {
            EffectSpec = InEffectSpec;
            EvaluatedData = InEvaluatedData;
            Target = InTarget;
        }
    }

}

