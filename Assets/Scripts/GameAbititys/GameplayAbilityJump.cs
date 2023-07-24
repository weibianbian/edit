using RailShootGame;

namespace GameplayAbilitySystem
{
    public class GameplayAbilityJump : GameplayAbility
    {
        public override bool ActivateAbility(Character character)
        {
            character.Jump();
            return false;
        }
    }
}

