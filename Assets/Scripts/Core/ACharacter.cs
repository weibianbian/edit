using GameplayAbilitySystem;

namespace RailShootGame
{
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

        public ACharacter()
        {
            CharacterMovement = CreateDefaultSubobject<CharacterMovementComponent>();

        }
        public override void PostInitializeComponents()
        {
            base.PostInitializeComponents();
            CharacterMovement.SetDefaultMovementMode();
        }
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

