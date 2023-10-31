using UnityEngine;

namespace RailShootGame
{
    public class NavMovementComponent : MovementCompt
    {
        public virtual void RequestDirectMove(Vector3 MoveVelocity, bool bForceMaxSpeed)
        {
            Velocity = MoveVelocity;
        }
    }
}

