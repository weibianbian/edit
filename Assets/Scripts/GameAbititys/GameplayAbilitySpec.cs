namespace GameplayAbilitySystem
{
    public class GameplayAbilitySpec
    {
        public int id;
        public GameplayAbility Ability;
        public int level;
        public bool InputPressed;
        public GameplayAbilitySpecHandle Handle;
        public static GameplayAbilitySpec Default=new GameplayAbilitySpec();

        public bool IsActive()
        {
            return Ability != null;
        }
    }
}

