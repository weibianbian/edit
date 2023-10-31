using UnityEngine;

namespace RailShootGame
{
    public class UPathFollowingComponent : ActorComponent
    {
        public PathData Path;
        public Vector3 CurrentDestination = Vector3.zero;
        public NavMovementComponent MovementComp;
        public int MoveSegmentStartIndex;
        public void RequestMove(PathData Path)
        {
            this.Path = Path;
        }
        public override void TickComponent(float DeltaTime)
        {
            FollowPathSegment(DeltaTime);

        }
        public void FollowPathSegment(float DeltaTime)
        {
            if (!this.Path.IsValid())
            {
                return;
            }
            Vector3 CurrentLocation = Vector3.zero;
            Vector3 CurrentTarget = GetCurrentTargetLocation();
            Vector3 MoveVelocity = (CurrentTarget - CurrentLocation) / DeltaTime;
            int LastSegmentStartIndex = Path.GetPathPoints().Count - 2;
            bool bNotFollowingLastSegment = (MoveSegmentStartIndex < LastSegmentStartIndex);
            MovementComp.RequestDirectMove(MoveVelocity, bNotFollowingLastSegment);
        }
        public Vector3 GetCurrentTargetLocation()
        {
            return CurrentDestination;
        }
    }
}

