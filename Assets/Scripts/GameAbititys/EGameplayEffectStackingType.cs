namespace GameplayAbilitySystem
{
    public enum EGameplayEffectStackingType
    {
        /** No stacking. Multiple applications of this GameplayEffect are treated as separate instances. */
        None,
        /** Each caster has its own stack. */
        AggregateBySource,
        /** Each target has its own stack. */
        AggregateByTarget,
    };
}

