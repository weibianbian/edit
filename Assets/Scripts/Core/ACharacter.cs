using GameplayAbilitySystem;

namespace RailShootGame
{
    //放置在AttributeSet中，创建一个可以使用FGameplayAttribute访问的属性。强烈建议使用这个而不是原始的float属性
    public class FGameplayAttributeData
    {
        public float BaseValue;
        public float CurrentValue;

        public FGameplayAttributeData(float DefaultValue)
        {
            BaseValue = DefaultValue;
            CurrentValue = DefaultValue;
        }
        public float GetBaseValue()
        {
            return BaseValue;
        }
        public float GetCurrentValue()
        {
            return CurrentValue;
        }
        //修改当前值，通常仅由能力系统调用或在初始化期间调用
        public void SetCurrentValue(float NewValue)
        {
            CurrentValue = NewValue;
        }
        //修改永久基值，通常只在技能系统或初始化时调用
        public void SetBaseValue(float NewValue)
        {
            BaseValue = NewValue;
        }
    }

    public class ACharacter : Pawn
    {
        public bool bPressedJump = false;
        public float JumpKeyHoldTime = 0;
        public bool bWasJumping = false;
        private int JumpCurrentCount = 0;
        private int JumpCurrentCountPreJump = 0;
        private float JumpForceTimeRemaining = 0;

        public CharacterMovementComponent CharacterMovement;
        public UAbilitySystemComponent asc;

        public void Input()
        {
            asc.GiveAbility(new FGameplayAbilitySpec());
        }
        public void Jump()
        {
            bPressedJump = true;
            JumpKeyHoldTime = 0.0f;
        }
        public bool CanJump()
        {
            return CanJumpInternal();
        }
        protected bool CanJumpInternal()
        {
            bool bCanJump = false;
            if (bCanJump)
            {
                if (!bWasJumping || GetJumpMaxHoldTime() <= 0)
                {

                }
            }
            return bCanJump;
        }
        public void CheckJumpInput(float DeltaSeconds)
        {
            if (CharacterMovement != null)
            {
                if (bPressedJump)
                {
                    bool bFirstJump = JumpCurrentCount == 0;
                    if (bFirstJump && CharacterMovement.IsFalling()) { JumpCurrentCount++; }
                    bool bDidJump = CanJump() && CharacterMovement.DoJump();
                    if (bDidJump)
                    {
                        if (!bWasJumping)
                        {
                            JumpCurrentCount++;
                            JumpForceTimeRemaining = GetJumpMaxHoldTime();
                            //OnJumped();
                        }
                    }
                    bWasJumping = bDidJump;
                }
            }
        }
        private float GetJumpMaxHoldTime()
        {
            return JumpKeyHoldTime;
        }
        public void ResetJumpState()
        {
            bPressedJump = false;
            bWasJumping = false;
            JumpKeyHoldTime = 0.0f;
            JumpForceTimeRemaining = 0.0f;

            if (CharacterMovement != null && !CharacterMovement.IsFalling())
            {
                JumpCurrentCount = 0;
                JumpCurrentCountPreJump = 0;
            }
        }
        public void OnMovementModeChanged(EMovementMode PrevMovementMode, int PrevCustomMode)
        {
            if (!bPressedJump || !CharacterMovement.IsFalling())
            {
                ResetJumpState();
            }

            // Record jump force start time for proxies. Allows us to expire the jump even if not continually ticking down a timer.
            //if (bProxyIsJumpForceApplied && CharacterMovement->IsFalling())
            //{
            //    ProxyJumpForceStartedTime = GetWorld()->GetTimeSeconds();
            //}

            //K2_OnMovementModeChanged(PrevMovementMode, CharacterMovement->MovementMode, PrevCustomMode, CharacterMovement->CustomMovementMode);
            //MovementModeChangedDelegate.Broadcast(this, PrevMovementMode, PrevCustomMode);
        }
    }
}

