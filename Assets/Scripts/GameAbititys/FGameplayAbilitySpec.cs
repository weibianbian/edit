namespace GameplayAbilitySystem
{
    public class FGameplayAbilitySpec
    {
        public int id;
        public UGameplayAbility Ability;
        public int level;
        public bool InputPressed;
        public FGameplayAbilitySpecHandle Handle;
        public static FGameplayAbilitySpec Default=new FGameplayAbilitySpec();

        public bool IsActive()
        {
            return Ability != null;
        }
    }
}

