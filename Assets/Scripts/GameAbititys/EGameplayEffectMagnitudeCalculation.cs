namespace GameplayAbilitySystem
{
    public enum EGameplayEffectMagnitudeCalculation
    {
        ScalableFloat,
        /** Perform a calculation based upon an attribute. */
        AttributeBased,
        /** Perform a custom calculation, capable of capturing and acting on multiple attributes, in either BP or native. */
        CustomCalculationClass,
        /** This magnitude will be set explicitly by the code/blueprint that creates the spec. */
        SetByCaller,
    }
}

