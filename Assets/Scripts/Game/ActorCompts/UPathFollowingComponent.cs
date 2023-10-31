using JetBrains.Annotations;
using System;
using UnityEngine;

namespace RailShootGame
{
    public enum EPathFollowingStatus
    {
        Idle,

        /** Request with incomplete path, will start after UpdateMove() */
        Waiting,

        /** Request paused, will continue after ResumeMove() */
        Paused,

        /** Following path */
        Moving,
    }
    public class UPathFollowingComponent : ActorComponent
    {
        public PathData Path;
        public Vector3 CurrentDestination = Vector3.zero;
        public NavMovementComponent MovementComp;
        public int MoveSegmentStartIndex;
        public int MoveSegmentEndIndex;
        public EPathFollowingStatus Status;
        public void RequestMove(PathData Path)
        {
            this.Path = Path;
        }
        public override void TickComponent(float DeltaTime)
        {
            base.TickComponent(DeltaTime);
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
        public void UpdatePathSegment()
        {
            if (!this.Path.IsValid())
            {
                return;
            }
            Vector3 CurrentLocation = Vector3.zero;
            if (Status == EPathFollowingStatus.Moving)
            {
                int LastSegmentEndIndex = Path.GetPathPoints().Count - 1;
                bool bFollowingLastSegment = (MoveSegmentEndIndex >= LastSegmentEndIndex);
            }
        }
        public bool HasReachedCurrentTarget(Vector3 CurrentLocation)
        {
            if (MovementComp == null)
            {
                return false;
            }
            Vector3 CurrentTarget = GetCurrentTargetLocation();
            Vector3 ToTarget = (CurrentTarget - MovementComp->GetActorFeetLocation());
        }
        public Vector3 GetCurrentTargetLocation()
        {
            return CurrentDestination;
        }
    }
}

