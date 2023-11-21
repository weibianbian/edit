using GameplayAbilitySystem;
using System.Security.Cryptography;
using TMPro;

namespace RailShootGame
{
    public class ACharacter : Pawn
    {
        public bool bPressedJump = false;
        public bool bIsCrouched = false;
        public bool bWasJumping = false;
        private int JumpCurrentCount = 0;
        private int JumpCurrentCountPreJump = 0;
        private int JumpMaxCount = 0;
        private float JumpForceTimeRemaining = 0;
        public float JumpKeyHoldTime = 0;
        public float JumpMaxHoldTime = 0.1f;

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
        protected virtual bool CanJumpInternal_Implementation()
        {
            return !bIsCrouched && JumpIsAllowedInternal();
        }
        protected bool JumpIsAllowedInternal()
        {
            bool bJumpIsAllowed = CharacterMovement.CanAttemptJump();

            if (bJumpIsAllowed)
            {
                if (!bWasJumping || GetJumpMaxHoldTime() <= 0.0f)
                {
                    if (JumpCurrentCount == 0 && CharacterMovement.IsFalling())
                    {
                        bJumpIsAllowed = JumpCurrentCount + 1 < JumpMaxCount;
                    }
                    else
                    {
                        bJumpIsAllowed = JumpCurrentCount < JumpMaxCount;
                    }
                }
                else
                {
                    // Only consider JumpKeyHoldTime as long as:
                    // A) The jump limit hasn't been met OR
                    // B) The jump limit has been met AND we were already jumping
                    bool bJumpKeyHeld = (bPressedJump && JumpKeyHoldTime < GetJumpMaxHoldTime());
                    bJumpIsAllowed = bJumpKeyHeld &&
                        ((JumpCurrentCount < JumpMaxCount) || (bWasJumping && JumpCurrentCount == JumpMaxCount));
                }
            }

            return bJumpIsAllowed;
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
        public void ClearJumpInput(float DeltaTime)
        {
            if (bPressedJump)
            {
                JumpKeyHoldTime += DeltaTime;

                // Don't disable bPressedJump right away if it's still held.
                // Don't modify JumpForceTimeRemaining because a frame of update may be remaining.
                if (JumpKeyHoldTime >= GetJumpMaxHoldTime())
                {
                    bPressedJump = false;
                }
            }
            else
            {
                JumpForceTimeRemaining = 0.0f;
                bWasJumping = false;
            }
        }
        private float GetJumpMaxHoldTime()
        {
            return JumpMaxHoldTime;
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

