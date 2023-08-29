namespace GameplayAbilitySystem
{
    public class FGameplayEffectModCallbackData
    {
        public GameplayEffectSpec EffectSpec;
        public FGameplayModifierEvaluatedData EvaluatedData;
        public AbilitySystemComponent Target;

        public FGameplayEffectModCallbackData(GameplayEffectSpec InEffectSpec, FGameplayModifierEvaluatedData InEvaluatedData, AbilitySystemComponent InTarget)
        {
            EffectSpec = InEffectSpec;
            EvaluatedData = InEvaluatedData;
            Target = InTarget;
        }
    }

}

