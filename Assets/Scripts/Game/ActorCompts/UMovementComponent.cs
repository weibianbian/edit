using System;
using UnityEngine;
using UnityEngine.AI;

namespace RailShootGame
{
    public class UMovementComponent : ActorComponent
    {
        public USceneComponent UpdatedComponent;
        public NavMeshAgent agent;
        public Vector3 targetPos;
        public Vector3 Velocity;
        public Vector3 Acceleration;
        public void Init()
        {

        }
        public override void OnRegister()
        {
            base.OnRegister();
        }

        public override void TickComponent(float DeltaTime)
        {

        }
        public Vector3 GetActorFeetLocation()
        {
            return Vector3.zero;
        }
        public virtual bool IsMovingOnGround()
        {
            return false;
        }
        public virtual float GetMaxSpeed()
        {
            return 0;
        }
        public virtual bool IsExceedingMaxSpeed(float MaxSpeed)
        {
            MaxSpeed = MathF.Max(0.0f, MaxSpeed);
            float MaxSpeedSquared = MathF.Sqrt(MaxSpeed);
            //允许1%的误差，以考虑数值的不精确性。
            float OverVelocityPercent = 1.01f;
            return (Velocity.sqrMagnitude > MaxSpeedSquared * OverVelocityPercent);
        }
        void SetUpdatedComponent(USceneComponent NewUpdatedComponent)
        {

        }
    }
}

