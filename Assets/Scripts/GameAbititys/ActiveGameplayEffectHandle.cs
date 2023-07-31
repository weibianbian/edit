namespace GameplayAbilitySystem
{
    public struct ActiveGameplayEffectHandle
    {
        public int Handle;

    }
    public struct ActiveGameplayEffect
    {
        public GameplayEffectSpec Spec;
        public ActiveGameplayEffectHandle Handle;
    }
}

