﻿using GameplayAbilitySystem;
using UnityEditor.AnimatedValues;

namespace RailShootGame
{
    public class GameplayAttributeData
    {
        public float BaseAnimValue;
        public float currentValue;
         
    }

    public class Character : Actor
    {
        public bool bPressedJump = false;
        public float JumpKeyHoldTime = 0;
        public bool bWasJumping = false;
        private int JumpCurrentCount = 0;
        private float JumpForceTimeRemaining = 0;

        public CharacterMovementComponent CharacterMovement;
        public AbilitySystemComponent asc;

        public void Input()
        {
            asc.GiveAbility(new GameplayAbilitySpec());
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
    }
}
