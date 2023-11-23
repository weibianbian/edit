using System;
using UnityEngine;

namespace RailShootGame
{
    public class CharacterMovementComponent : NavMovementComponent
    {
        const float MIN_TICK_TIME = 1e-6f;
        const float MIN_FLOOR_DIST = 1.9f;
        const float MAX_FLOOR_DIST = 2.4f;
        public EMovementMode MovementMode;
        public EMovementMode GroundMovementMode;
        protected ACharacter CharacterOwner;
        public float JumpZVelocity;
        //ClampMin="0.0166", ClampMax="0.50", UIMin="0.0166", UIMax="0.50"
        float MaxSimulationTimeStep = 0.0166f;
        float GroundFriction = 0;
        float MaxAcceleration = 0;
        float MaxWalkSpeed = 0;
        float MinAnalogWalkSpeed = 0;
        float AnalogInputModifier = 0;
        public int CustomMovementMode;
        public int MaxSimulationIterations;
        public bool bMaintainHorizontalGroundVelocity = false;
        public virtual void SetDefaultMovementMode()
        {
            //SetMovementMode(DefaultLandMovementMode);

            //// Avoid 1-frame delay if trying to walk but walking fails at this location.
            //if (MovementMode == EMovementMode.MOVE_Walking && GetMovementBase() == NULL)
            //{
            //    Velocity.z = SavedVelocityZ; // Prevent temporary walking state from zeroing Z velocity.
            //    SetMovementMode(EMovementMode.MOVE_Falling);
            //}
        }
        public override void OnRegister()
        {
            base.OnRegister();
            CharacterOwner = GetOwner() as ACharacter;
        }
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
        public float GetSimulationTimeStep(float RemainingTime, int Iterations)
        {
            if (RemainingTime > MaxSimulationTimeStep)
            {
                if (Iterations < MaxSimulationIterations)
                {
                    RemainingTime = MathF.Min(MaxSimulationTimeStep, RemainingTime * 0.5f);
                }
            }
            return MathF.Max(MIN_TICK_TIME, RemainingTime);
        }
        public virtual bool CanAttemptJump()
        {
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
        public virtual void StartNewPhysics(float deltaTime, int Iterations)
        {
            switch (MovementMode)
            {
                case EMovementMode.MOVE_None:
                    break;
                case EMovementMode.MOVE_Walking:
                    PhysWalking(deltaTime, Iterations);
                    break;
                case EMovementMode.MOVE_NavWalking:
                    break;
                case EMovementMode.MOVE_Falling:
                    break;
                case EMovementMode.MOVE_Swimming:
                    break;
                case EMovementMode.MOVE_Flying:
                    break;
                case EMovementMode.MOVE_Custom:
                    break;
                case EMovementMode.MOVE_MAX:
                    break;
                default:
                    SetMovementMode(EMovementMode.MOVE_None);
                    break;
            }
        }
        public virtual void PhysWalking(float deltaTime, int Iterations)
        {
            if (deltaTime < MIN_TICK_TIME)
            {
                return;
            }
            if (CharacterOwner == null)
            {
                Acceleration = Vector3.zero;
                Velocity = Vector3.zero;
                return;
            }
            float remainingTime = deltaTime;
            while ((remainingTime >= MIN_TICK_TIME) && (Iterations < MaxSimulationIterations))
            {
                Iterations++;
                float timeTick = GetSimulationTimeStep(remainingTime, Iterations);
                remainingTime -= timeTick;
                //确保速度是水平的。
                MaintainHorizontalGroundVelocity();
                Vector3 OldVelocity = Velocity;
                Acceleration.z = 0.0f;
                //应用加速度
                CalcVelocity(timeTick, GroundFriction, false, GetMaxBrakingDeceleration());
            }
            if (IsMovingOnGround())
            {
                MaintainHorizontalGroundVelocity();
            }
        }
        public virtual float GetMaxBrakingDeceleration()
        {
            switch (MovementMode)
            {
                case EMovementMode.MOVE_None:
                    return 0.0f;
                case EMovementMode.MOVE_Walking:
                    return 0.0f;
                case EMovementMode.MOVE_NavWalking:
                    return 0.0f;
                case EMovementMode.MOVE_Falling:
                    return 0.0f;
                case EMovementMode.MOVE_Swimming:
                    return 0.0f;
                case EMovementMode.MOVE_Flying:
                    return 0.0f;
                case EMovementMode.MOVE_Custom:
                    return 0.0f;
                case EMovementMode.MOVE_MAX:
                    return 0.0f;
                default:
                    return 0.0f;
            }
            //return 0;
        }
        public virtual float GetMinAnalogSpeed()
        {
            switch (MovementMode)
            {
                case EMovementMode.MOVE_Walking:
                case EMovementMode.MOVE_NavWalking:
                case EMovementMode.MOVE_Falling:
                    return MinAnalogWalkSpeed;
                default:
                    return 0.0f;
            }
        }
        public override float GetMaxSpeed()
        {
            switch (MovementMode)
            {
                case EMovementMode.MOVE_Walking:
                case EMovementMode.MOVE_NavWalking:
                    return MaxWalkSpeed;
                case EMovementMode.MOVE_Falling:
                    return MaxWalkSpeed;
                case EMovementMode.MOVE_Swimming:
                    return 0.0f;
                case EMovementMode.MOVE_Flying:
                    return 0.0f;
                case EMovementMode.MOVE_Custom:
                    return 0.0f;
                case EMovementMode.MOVE_MAX:
                    return 0.0f;
                default:
                    return 0.0f;
            }
        }
        public virtual void CalcVelocity(float DeltaTime, float Friction, bool bFluid, float BrakingDeceleration)
        {
            Friction = MathF.Max(0.0f, Friction);
            float MaxAccel = GetMaxAcceleration();
            float MaxSpeed = GetMaxSpeed();
            float MaxInputSpeed = MathF.Max(MaxSpeed * AnalogInputModifier, GetMinAnalogSpeed());
            bool bZeroAcceleration = Acceleration == Vector3.zero;
            bool bVelocityOverMax = IsExceedingMaxSpeed(MaxSpeed);
            if (!bZeroAcceleration)
            {
                Vector3 AccelDir = Acceleration.normalized;
                float VelSize = Velocity.magnitude;
                Velocity = Velocity - (Velocity - AccelDir * VelSize) * MathF.Min(DeltaTime * Friction, 1.0f);
            }

            //应用输入加速
            if (!bZeroAcceleration)
            {
                float NewMaxInputSpeed = IsExceedingMaxSpeed(MaxInputSpeed) ? Velocity.magnitude : MaxInputSpeed;
                Velocity += Acceleration * DeltaTime;
                Velocity = Vector3.ClampMagnitude(Velocity, NewMaxInputSpeed);
            }
        }

        public virtual float GetMaxAcceleration()
        {
            return MaxAcceleration;
        }
        public virtual void MaintainHorizontalGroundVelocity()
        {
            if (Velocity.z != 0.0f)
            {
                if (bMaintainHorizontalGroundVelocity)
                {
                    // Ramp movement already maintained the velocity, so we just want to remove the vertical component.
                    Velocity.z = 0.0f;
                }
                else
                {
                    // Rescale velocity to be horizontal but maintain magnitude of last update.
                    Vector3 normal2D = new Vector3(Velocity.x, 0, Velocity.z);
                    Velocity = normal2D.normalized * Vector3.Magnitude(Velocity);
                }
            }
        }
        public virtual void AdjustFloorHeight()
        {
            //float OldFloorDist = CurrentFloor.FloorDist;
            float OldFloorDist = 0;
            if (OldFloorDist < MIN_FLOOR_DIST || OldFloorDist > MAX_FLOOR_DIST)
            {

            }
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
        public override bool IsMovingOnGround()
        {
            return ((MovementMode == EMovementMode.MOVE_Walking) || (MovementMode == EMovementMode.MOVE_NavWalking));
        }
        public override void TickComponent(float DeltaTime)
        {
            ControlledCharacterMove(Vector3.zero, DeltaTime);
        }
        public void ControlledCharacterMove(Vector3 InputVector, float DeltaSeconds)
        {
            CharacterOwner.CheckJumpInput(DeltaSeconds);
            PerformMovement(DeltaSeconds);
        }
        public void PerformMovement(float DeltaSeconds)
        {
            CharacterOwner.ClearJumpInput(DeltaSeconds);
        }
    }
}

