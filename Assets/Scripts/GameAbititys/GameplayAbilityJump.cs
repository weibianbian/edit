using RailShootGame;

namespace GameplayAbilitySystem
{
    public class GameplayAbilityJump : UGameplayAbility
    {
        public override bool CanActivateAbility(FGameplayAbilitySpecHandle Handle, FGameplayAbilityActorInfo ActorInfo, FGameplayTagContainer SourceTags, FGameplayTagContainer TargetTags, FGameplayTagContainer OptionalRelevantTags)
        {
            if (!base.CanActivateAbility(Handle, ActorInfo, SourceTags, TargetTags, OptionalRelevantTags))
            {
                return false;
            } ;
            return true;
        }
    }
}

