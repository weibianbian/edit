namespace GameplayAbilitySystem
{
    public class GameplayAbilitySpec
    {
        public int id;
        public GameplayAbility Ability;
        public int level;
        public bool InputPressed = false;
        public GameplayAbilitySpecHandle Handle;

        public bool IsActive()
        {
            return Ability != null;
        }
    }
}

