namespace GameplayAbilitySystem
{
    public class UAbilitySystemGlobals
    {
        public static UAbilitySystemGlobals Get()
        {
            return GameplayAbilitiesModule.Get().GetAbilitySystemGlobals();
        }
    }
}

