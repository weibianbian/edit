using UEngine.Components;
using UEngine.GameFramework;
using UnityEngine;

namespace RailShootGame
{
    public class UPathFollowingComponent : UActorComponent
    {
        public PathData Path;
        public NavMovementComponent MovementComp;
        public Vector3 CurrentDestination = Vector3.zero;
        public Vector3 CurrentDirection = Vector3.zero;
        public Vector3 MoveSegmentDirection = Vector3.zero;
        public EPathFollowingStatus Status;
        public int CurrentRequestId;
        public int MoveSegmentStartIndex;
        public int MoveSegmentEndIndex;
        public int PreciseAcceptanceRadiusCheckStartNodeIndex;
        public float CurrentAcceptanceRadius;
        public float PathfollowingRegularPathPointAcceptanceRadius;
        public float AcceptanceRadius;
        public float MyDefaultAcceptanceRadius;
        public float MinAgentRadiusPct;
        public bool bStopMovementOnFinish = false;
        public bool bReachTestIncludesGoalRadius = false;
        public bool bReachTestIncludesAgentRadius = false;
        public static int NextRequestId = 0;
        public int RequestMove(PathData InPath)
        {
            if (MovementComp == null)
            {
                return -1;
            }
            int MoveId = CurrentRequestId;

            if (Status != EPathFollowingStatus.Idle)
            {
                bStopMovementOnFinish = false;
                OnPathFinished(EPathFollowingResult.Aborted, FPathFollowingResultFlags.NewRequest);
            }
            bStopMovementOnFinish = true;
            Reset();
            StoreRequestId();
            MoveId = CurrentRequestId;
            Path = InPath;
            OnPathUpdated();
            if (CurrentRequestId == MoveId)
            {
                Status = EPathFollowingStatus.Moving;

                // determine with path segment should be followed
                int CurrentSegment = DetermineStartingPathPoint(InPath);
                SetMoveSegment(CurrentSegment);
            }
            return MoveId;
        }
        public void StoreRequestId() { CurrentRequestId = UPathFollowingComponent.GetNextRequestId(); }

        public static int GetNextRequestId() { return NextRequestId++; }
        public override void TickComponent(float DeltaTime)
        {
            base.TickComponent(DeltaTime);
            if (Status == EPathFollowingStatus.Moving)
            {
                UpdatePathSegment();
            }
            if (Status == EPathFollowingStatus.Moving)
            {
                FollowPathSegment(DeltaTime);
            }
        }
        public void FollowPathSegment(float DeltaTime)
        {
            if (!this.Path.IsValid())
            {
                return;
            }
            Vector3 CurrentLocation = MovementComp.GetActorFeetLocation();
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
                if (MoveSegmentEndIndex > PreciseAcceptanceRadiusCheckStartNodeIndex && HasReachedDestination(CurrentLocation))
                {
                    OnSegmentFinished();
                    OnPathFinished(EPathFollowingResult.Success, FPathFollowingResultFlags.None);
                }
                else if (HasReachedCurrentTarget(CurrentLocation))
                {
                    OnSegmentFinished();
                    SetNextMoveSegment();
                }
            }
        }
        public void OnPathFinished(EPathFollowingResult ResultCode, FPathFollowingResultFlags ExtraResultFlags)
        {

        }
        public bool HasReachedDestination(Vector3 CurrentLocation)
        {
            Vector3 GoalLocation = Path.GetPathPointLocation(Path.GetPathPoints().Count - 1);
            float GoalRadius = 0.0f;
            float GoalHalfHeight = 0.0f;

            float AcceptanceRangeToUse = GetFinalAcceptanceRadius(Path, Vector3.zero, Vector3.zero);
            return HasReachedInternal(GoalLocation, bReachTestIncludesGoalRadius ? GoalRadius : 0.0f, GoalHalfHeight, CurrentLocation
                , AcceptanceRangeToUse, bReachTestIncludesAgentRadius ? MinAgentRadiusPct : 0.0f);
        }
        public bool HasReachedCurrentTarget(Vector3 CurrentLocation)
        {
            if (MovementComp == null)
            {
                return false;
            }
            Vector3 CurrentTarget = GetCurrentTargetLocation();
            Vector3 ToTarget = (CurrentTarget - MovementComp.GetActorFeetLocation());
            float SegmentDot = Vector3.Dot(ToTarget, CurrentDirection);
            if (SegmentDot < 0)
            {
                return true;
            }
            float GoalRadius = 0.0f;
            float GoalHalfHeight = 0.0f;
            return HasReachedInternal(CurrentTarget, GoalRadius, GoalHalfHeight, CurrentLocation, CurrentAcceptanceRadius, 0.05f); ;
        }
        private bool HasReachedInternal(Vector3 GoalLocation, float GoalRadius, float GoalHalfHeight, Vector3 AgentLocation, float RadiusThreshold, float AgentRadiusMultiplier)
        {
            if (MovementComp == null)
            {
                return false;
            }
            float AgentRadius = 0.0f;
            float AgentHalfHeight = 0.0f;
            Vector3 ToGoal = GoalLocation - AgentLocation;
            float Dist2DSq = ToGoal.x * ToGoal.x + ToGoal.z * ToGoal.z;
            float UseRadius = RadiusThreshold + GoalRadius + (AgentRadius * AgentRadiusMultiplier);
            if (Dist2DSq > Mathf.Sqrt(UseRadius))
            {
                return false;
            }
            //float ZDiff = Mathf.Abs(ToGoal.y);
            //float UseHeight = GoalHalfHeight + (AgentHalfHeight * MinAgentHalfHeightPct);
            //if (ZDiff > UseHeight)
            //{
            //    return false;
            //}
            return true;
        }
        public Vector3 GetCurrentTargetLocation()
        {
            return CurrentDestination;
        }
        public virtual void OnSegmentFinished()
        {

        }
        public void SetNextMoveSegment()
        {
            SetMoveSegment(GetNextPathIndex());
        }
        public void SetMoveSegment(int SegmentStartIndex)
        {
            float PathPointAcceptanceRadius = PathfollowingRegularPathPointAcceptanceRadius;
            int EndSegmentIndex = SegmentStartIndex + 1;
            if (Path.IsValidIndex(SegmentStartIndex) && Path.IsValidIndex(EndSegmentIndex))
            {
                EndSegmentIndex = DetermineCurrentTargetPathPoint(SegmentStartIndex);
                MoveSegmentStartIndex = SegmentStartIndex;
                MoveSegmentEndIndex = EndSegmentIndex;
                CurrentDestination = Path.GetPathPointLocation(MoveSegmentEndIndex);

                Vector3 SegmentStart = Path.GetPathPointLocation(MoveSegmentStartIndex);

                Vector3 SegmentEnd = CurrentDestination;
                if (SegmentStart.Equals(SegmentEnd) && Path.IsValidIndex(MoveSegmentEndIndex + 1))
                {
                    MoveSegmentEndIndex++;

                    CurrentDestination = Path.GetPathPointLocation(MoveSegmentEndIndex);
                    SegmentEnd = CurrentDestination;
                }
                CurrentAcceptanceRadius = (Path.GetPathPoints().Count == (MoveSegmentEndIndex + 1))
                ? GetFinalAcceptanceRadius(Path, Vector3.zero, Vector3.zero) : PathPointAcceptanceRadius;

                MoveSegmentDirection = Vector3.Normalize(SegmentEnd - SegmentStart);
            }
            UpdateMoveFocus();
            UpdateDecelerationData();
        }
        void UpdateMoveFocus()
        {
            AAIController AIOwner = (AAIController)(GetOwner());
            if (AIOwner != null)
            {
                //if (Status == EPathFollowingStatus.Moving)
                //{
                //    Vector3 MoveFocus = GetMoveFocus(AIOwner.bAllowStrafe);
                //    AIOwner.SetFocalPoint(MoveFocus, EAIFocusPriority::Move);
                //}
                //else if (Status == EPathFollowingStatus.Idle)
                //{
                //    AIOwner->ClearFocus(EAIFocusPriority::Move);
                //}
            }
        }
        public void UpdateDecelerationData()
        {
            if (MovementComp == null)
            {
                return;
            }

            if (!bStopMovementOnFinish)
            {
                return;
            }
        }
        public int DetermineCurrentTargetPathPoint(int StartIndex)
        {
            return StartIndex + 1;
        }
        public int DetermineStartingPathPoint(PathData ConsideredPath)
        {
            int PickedPathPoint = -1;
            if (ConsideredPath != null && ConsideredPath.IsValid())
            {
                if (MovementComp != null && PickedPathPoint == -1)
                {
                    if (ConsideredPath.GetPathPoints().Count > 2)
                    {
                        // check if is closer to first or second path point (don't assume AI's standing)
                        Vector3 CurrentLocation = MovementComp.GetActorFeetLocation();
                        Vector3 PathPt0 = ConsideredPath.GetPathPointLocation(0);
                        Vector3 PathPt1 = ConsideredPath.GetPathPointLocation(1);
                        // making this test in 2d to avoid situation where agent's Z location not being in "navmesh plane"
                        // would influence the result
                        float SqDistToFirstPoint = (CurrentLocation - PathPt0).x * (CurrentLocation - PathPt0).x + (CurrentLocation - PathPt0).z * (CurrentLocation - PathPt0).z;
                        float SqDistToSecondPoint = (CurrentLocation - PathPt1).x * (CurrentLocation - PathPt1).x + (CurrentLocation - PathPt1).z * (CurrentLocation - PathPt1).z;

                        PickedPathPoint = ((SqDistToFirstPoint < SqDistToSecondPoint) ? 0 : 1);
                    }
                    else
                    {
                        // If there are only two point we probably should start from the beginning
                        PickedPathPoint = 0;
                    }
                }
            }
            return PickedPathPoint;
        }
        public float GetFinalAcceptanceRadius(PathData Path, Vector3 OriginalGoalLocation, Vector3 PathEndOverride)
        {
            return AcceptanceRadius;
        }
        public Vector3 GetMoveFocus(bool bAllowStrafe)
        {

            Vector3 MoveFocus = Vector3.zero;
            {
                Vector3 CurrentMoveDirection = GetCurrentDirection();
                MoveFocus = CurrentDestination + (CurrentMoveDirection * 1);
            }

            return MoveFocus;
        }
        public Vector3 GetCurrentDirection()
        {
            {
                // calculate direction to based destination
                Vector3 SegmentStartLocation = Path.GetPathPointLocation(MoveSegmentStartIndex);
                Vector3 SegmentEndLocation = CurrentDestination;

                return Vector3.Normalize(SegmentEndLocation - SegmentStartLocation);
            }

            // use cached direction of current path segment
            return MoveSegmentDirection;
        }
        public int GetNextPathIndex()
        {
            return MoveSegmentEndIndex;
        }
        public virtual void OnPathUpdated()
        {

        }
        public void Reset()
        {

        }
    }
}

