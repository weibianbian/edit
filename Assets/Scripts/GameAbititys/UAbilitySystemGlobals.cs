namespace GameplayAbilitySystem
{
    public class UAbilitySystemGlobals
    {
        public GameplayCueManager GlobalGameplayCueManager;
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
    }
}

