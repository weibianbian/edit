using RailShootGame;

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
            CharacterJumpStart();
            UAbilityTask_StartAbilityState.StartAbilityState(this,"Jumping",true);
            UAbilityTask_WaitInputRelease task= UAbilityTask_WaitInputRelease.WaitInputRelease(this);
            task.ReadyForActivation();
            UnityEngine.Debug.Log($"Character->Jump();");
        }
        public void CharacterJumpStart()                                           
        {
            //if (ALyraCharacter * LyraCharacter = GetLyraCharacterFromActorInfo())
            //{
            //    if (LyraCharacter->IsLocallyControlled() && !LyraCharacter->bPressedJump)
            //    {
            //        LyraCharacter->UnCrouch();
            //        LyraCharacter->Jump();
            //    }
            //}
        }
        public void CharacterJumpStop()
        {
            //if (ALyraCharacter * LyraCharacter = GetLyraCharacterFromActorInfo())
            //{
            //    if (LyraCharacter->IsLocallyControlled() && LyraCharacter->bPressedJump)
            //    {
            //        LyraCharacter->StopJumping();
            //    }
            //}
        }
        public override void EndAbility(FGameplayAbilitySpecHandle Handle, FGameplayAbilityActorInfo ActorInfo, FGameplayAbilityActivationInfo ActivationInfo, bool bWasCancelled)
        {
            CharacterJumpStop();
            base.EndAbility(Handle, ActorInfo, ActivationInfo, bWasCancelled);
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

