using UnityEngine;

namespace UEngine.GameFramework
{
    public class NavMovementComponent : UMovementComponent
    {
        public virtual void RequestDirectMove(Vector3 MoveVelocity, bool bForceMaxSpeed)
        {
            Velocity = MoveVelocity;
        }
    }
}

