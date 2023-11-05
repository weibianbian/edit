﻿using UnityEngine;
using UnityEngine.TextCore.Text;

namespace RailShootGame
{
    public class CharacterMovementComponent : NavMovementComponent
    {
        public EMovementMode MovementMode;
        public EMovementMode GroundMovementMode;
        protected ACharacter CharacterOwner;
        public float JumpZVelocity;
        public int CustomMovementMode;
        public bool IsFalling()
        {
            return MovementMode == EMovementMode.MOVE_Falling;
        }
        public bool DoJump()
        {
            if (CharacterOwner != null && CharacterOwner.CanJump())
            {
                SetMovementMode(EMovementMode.MOVE_Falling);
                return true;
            }
            return false;
        }
        public virtual void SetMovementMode(EMovementMode NewMovementMode, int NewCustomMode = 0)
        {
            if (NewMovementMode != EMovementMode.MOVE_Custom)
            {
                NewCustomMode = 0;
            }
            if (MovementMode == NewMovementMode)
            {
                if ((NewMovementMode != EMovementMode.MOVE_Custom) || NewCustomMode == CustomMovementMode)
                {
                    return;
                }
            }
            EMovementMode PrevMovementMode = MovementMode;
            int PrevCustomMode = CustomMovementMode;
            MovementMode = NewMovementMode;
            CustomMovementMode = NewCustomMode;

        }
        public virtual void OnMovementModeChanged(EMovementMode PreviousMovementMode, int PreviousCustomMode)
        {
            if (MovementMode == EMovementMode.MOVE_Walking)
            {
                Velocity.y = 0;
                GroundMovementMode = MovementMode;

            }
            else
            {
                if (MovementMode == EMovementMode.MOVE_None)
                {
                    CharacterOwner.ResetJumpState();
                }
            }
            if (MovementMode == EMovementMode.MOVE_Falling && PreviousMovementMode != EMovementMode.MOVE_Falling)
            {

            }
            CharacterOwner.OnMovementModeChanged(PreviousMovementMode, PreviousCustomMode);
        }
        public override void TickComponent(float DeltaTime)
        {
            ControlledCharacterMove(Vector3.zero, 1);
        }
        public void ControlledCharacterMove(Vector3 InputVector, float DeltaSeconds)
        {
            CharacterOwner.CheckJumpInput(DeltaSeconds);
            PerformMovement(DeltaSeconds);
        }
        public void PerformMovement(float DeltaSeconds)
        {

        }
    }
}

