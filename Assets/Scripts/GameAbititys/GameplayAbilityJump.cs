using RailShootGame;

namespace GameplayAbilitySystem
{
    public class GameplayAbilityJump : GameplayAbility
    {
        public override void ActivateAbility(GameplayAbilitySpecHandle Handle, Character owner)
        {
            owner.Jump();
        }
    }
}

