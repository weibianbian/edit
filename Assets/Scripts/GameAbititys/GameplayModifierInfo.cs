namespace GameplayAbilitySystem
{
    public class GameplayModifierInfo
    {
        public EGameplayModOp ModifierOp;
        public FGameplayEffectModifierMagnitude ModifierMagnitude=new FGameplayEffectModifierMagnitude();
    }
    public class FGameplayEffectModifierMagnitude
    {
        public bool AttemptCalculateMagnitude(GameplayEffectSpec InRelevantSpec)
        {
            return false;
        }
        public bool CanCalculateMagnitude(GameplayEffectSpec InRelevantSpec)
        {

        }
    }
}

