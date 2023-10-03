namespace GameplayAbilitySystem
{
    public class UAbilitySystemGlobals
    {
        public GameplayCueManager GlobalGameplayCueManager;
        protected bool bIgnoreAbilitySystemCooldowns;
        protected bool bIgnoreAbilitySystemCosts;
        public static UAbilitySystemGlobals Get()
        {
            return GameplayAbilitiesModule.Get().GetAbilitySystemGlobals();
        }
        public GameplayCueManager GetGameplayCueManager()
        {
            if (GlobalGameplayCueManager == null)
            {
                GlobalGameplayCueManager = new GameplayCueManager();
            }
            GlobalGameplayCueManager.OnCreated();
            return GlobalGameplayCueManager;
        }
        public bool ShouldIgnoreCooldowns()
        {
            return bIgnoreAbilitySystemCooldowns;
        }

        public bool ShouldIgnoreCosts()
        {
            return bIgnoreAbilitySystemCosts;
        }
    }
}

