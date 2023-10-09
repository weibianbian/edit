using UnityEngine;

namespace RailShootGame
{
    public class CharacterMovementComponent : NavMovementComponent
    {
        public EMovementMode MovementMode;
        protected Character CharacterOwner;
        public float JumpZVelocity;

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
        public void SetMovementMode(EMovementMode NewMovementMode)
        {
            if (MovementMode == NewMovementMode)
            {
                return;
            }
            EMovementMode PrevMovementMode = MovementMode;
            MovementMode = NewMovementMode;
        }
        public override void TickComponent(float DeltaTime)
        {
            ControlledCharacterMove(Vector3.zero,1);
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

