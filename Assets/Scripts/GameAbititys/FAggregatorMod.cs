namespace GameplayAbilitySystem
{
    public class FAggregatorMod
    {
        public FGameplayTagRequirements SourceTagReqs;
        public FGameplayTagRequirements TargetTagReqs;

        public float EvaluatedMagnitude;       // Magnitude this mod was last evaluated at
        public float StackCount;

        public FActiveGameplayEffectHandle ActiveHandle;   // Handle of the active GameplayEffect we are tied to (if any)
        public bool IsPredicted;
    }
}

