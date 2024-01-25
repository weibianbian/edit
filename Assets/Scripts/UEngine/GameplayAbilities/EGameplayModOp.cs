namespace UEngine.GameplayAbilities
{
    public enum EGameplayModOp
    {
        /** Numeric. */
        Additive = 0,
        /** Numeric. */
        Multiplicitive,
        /** Numeric. */
        Division,

        /** Other. */
        Override,    // This should always be the first non numeric ModOp

        // This must always be at the end.
        Max
    }
}

