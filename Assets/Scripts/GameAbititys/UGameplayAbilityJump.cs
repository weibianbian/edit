using RailShootGame;
using System.Diagnostics;
using Unity.VisualScripting;

namespace GameplayAbilitySystem
{
    public class UGameplayAbilityJump : UGameplayAbility
    {
        public override bool CanActivateAbility(FGameplayAbilitySpecHandle Handle, FGameplayAbilityActorInfo ActorInfo, FGameplayTagContainer SourceTags, FGameplayTagContainer TargetTags, FGameplayTagContainer OptionalRelevantTags)
        {
            if (!base.CanActivateAbility(Handle, ActorInfo, SourceTags, TargetTags, OptionalRelevantTags))
            {
                return false;
            };
            return true;
        }
        public override void ActivateAbility(FGameplayAbilitySpecHandle Handle, FGameplayAbilityActorInfo ActorInfo, FGameplayAbilityActivationInfo ActivationInfo, FGameplayEventData TriggerEventData)
        {
            if (!CommitAbility(Handle, ActorInfo, ActivationInfo))
            {
                return;
            }
            UnityEngine.Debug.Log($"Character->Jump();");
        }
        public override void CancelAbility(FGameplayAbilitySpecHandle Handle, FGameplayAbilityActorInfo ActorInfo, FGameplayAbilityActivationInfo ActivationInfo)
        {

            base.CancelAbility(Handle, ActorInfo, ActivationInfo);

            UnityEngine.Debug.Log($"Character->StopJumping();");
        }
        public virtual void InputReleased(FGameplayAbilitySpecHandle Handle, FGameplayAbilityActorInfo ActorInfo, FGameplayAbilityActivationInfo ActivationInfo)
        {
            if (ActorInfo != null && ActorInfo.AvatarActor != null)
            {
                CancelAbility(Handle, ActorInfo, ActivationInfo);
            }
        }
    }
}

