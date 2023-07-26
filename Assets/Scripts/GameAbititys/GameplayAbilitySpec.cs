namespace GameplayAbilitySystem
{
    public class GameplayAbilitySpec
    {
        public int id;
        public GameplayAbility ability;
        public int level;
        public bool InputPressed = false;

        public bool IsActive()
        {
            return ability != null;
        }
    }
}

