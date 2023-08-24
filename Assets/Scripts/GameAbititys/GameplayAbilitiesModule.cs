using Core;

namespace GameplayAbilitySystem
{
    [ModuleNameAttribute("GameplayAbilities")]
    public class GameplayAbilitiesModule : IModuleInterface
    {
        UAbilitySystemGlobals AbilitySystemGlobals;
        public static GameplayAbilitiesModule Get()
        {
            GameplayAbilitiesModule Singleton = FModuleManager.LoadModuleChecked<GameplayAbilitiesModule>("GameplayAbilities");
            return Singleton;

        }
        public virtual UAbilitySystemGlobals GetAbilitySystemGlobals()
        {
            if (AbilitySystemGlobals == null)
            {
                AbilitySystemGlobals = new UAbilitySystemGlobals();
            }
            return AbilitySystemGlobals;
        }
    }
}

