using RailShootGame;
using System;
using Unity.VisualScripting;
using UnityEngine;

public class FScopedMovementUpdate
{
    public TestMoveComponent Owner;

    public Transform InitialTransform;

}
public class TestMoveComponent
{
    public FScopedMovementUpdate ScopedMovementStack;

    public Transform player;

    public void EndScopedMovementUpdate()
    {
        if (ScopedMovementStack == null)
        {
            return;
        }

    }
}
public class FHitResult
{
    public RaycastHit HitResult;
    public Vector3 ImpactPoint;
    public Vector3 ImpactNormal;
    public Vector3 Location;
    public bool bBlockingHit = false;
    public float Time;
    public FHitResult(float InTime)
    {
        Init();
        Time = InTime;
    }
    public FHitResult()
    {
        Init();
    }
    public void Init()
    {
        Time = 1.0f;
        HitResult = new RaycastHit();
        bBlockingHit = false;
    }
    public void Reset(float InTime)
    {
        HitResult = new RaycastHit();
        ImpactNormal = Vector3.zero;
        bBlockingHit = false;
        Time = InTime;
    }
    public void Copy(FHitResult other)
    {
        HitResult = other.HitResult;
        ImpactPoint = other.ImpactPoint;
        ImpactNormal = other.ImpactNormal;
        Location = other.Location;
        bBlockingHit = other.bBlockingHit;
        Time = other.Time;
    }
}
public class FStepDownResult
{
    public bool bComputedFloor = false;
    public FFindFloorResult FloorResult = new FFindFloorResult();

    public void Copy(FStepDownResult other)
    {
        bComputedFloor = other.bComputedFloor;
        FloorResult.Copy(other.FloorResult);
    }
}
public class FFindFloorResult
{
    public FHitResult HitResult = new FHitResult();
    public bool bBlockingHit = false;
    public bool bWalkableFloor = false;
    public bool bLineTrace = false;
    public float LineDist = 0;
    public float FloorDist = 0;

    public void SetFromSweep(FHitResult InHit, float InSweepFloorDist, bool bIsWalkableFloor)
    {
        bBlockingHit = InHit.bBlockingHit;
        bWalkableFloor = bIsWalkableFloor;
        bLineTrace = false;
        FloorDist = InSweepFloorDist;
        LineDist = 0.0f;
        HitResult = InHit;
    }
    public bool IsWalkableFloor()
    {
        return (bBlockingHit) && bWalkableFloor;
    }
    public float GetDistanceToFloor()
    {
        return FloorDist;
    }
    public void Clear()
    {
        bBlockingHit = false;
        bWalkableFloor = false;
        bLineTrace = false;
        FloorDist = 0.0f;
        LineDist = 0.0f;
        HitResult.Reset(1);
    }
    public void Copy(FFindFloorResult other)
    {
        bBlockingHit = other.bBlockingHit;
        bWalkableFloor = other.bWalkableFloor;
        bLineTrace = other.bLineTrace;
        FloorDist = other.FloorDist;
        LineDist = other.LineDist;
        HitResult.Copy(other.HitResult);
    }
}
public class FCapsuleShape
{
    public Vector3 Point1;
    public Vector3 Point2;
    public float CapsuleHalfHeight;
    public float CapsuleRadius;

    public void UpdateShape(Vector3 center)
    {
        Point1 = new Vector3(center.x, center.y - (GetScaledCapsuleHalfHeight_WithoutHemisphere()), center.z);
        Point2 = new Vector3(center.x, center.y + (GetScaledCapsuleHalfHeight_WithoutHemisphere()), center.z);
    }
    float GetScaledCapsuleHalfHeight_WithoutHemisphere()
    {
        return CapsuleHalfHeight - CapsuleRadius;
    }
}

public class UFindFloor : MonoBehaviour
{
    public GameObject player;

    public Vector3 Gravity = new Vector3(0, -9.80f, 0);

    // Start is called before the first frame update
    public FHitResult HitResult;
    public FFindFloorResult CurrentFloor;
    public Vector3 Velocity;
    public EMovementMode MovementMode = EMovementMode.MOVE_None;
    public bool IsHit = false;
    public float MaxStepHeight = 2;
    public float MAX_FLOOR_DIST = 0.024f;
    public float MIN_FLOOR_DIST = 0.019f;
    float UE_KINDA_SMALL_NUMBER = (1.0e-4f);
    float SWEEP_EDGE_REJECT_DISTANCE = 0.0015f;
    public FCapsuleShape CapsuleShape = new FCapsuleShape();
    public TestMoveComponent testMoveComponent = new TestMoveComponent();

    void Start()
    {
        testMoveComponent = new TestMoveComponent();
        testMoveComponent.player = player.transform;

        CapsuleShape.CapsuleHalfHeight = 1;
        CapsuleShape.CapsuleRadius = 0.5f;
        CurrentFloor = new FFindFloorResult();
        //FindFloorNew(CurrentFloor, null);

        //FindFloor(player.transform.position, CurrentFloor, null);
        //AdjustFloorHeight();
        SetMovementMode(EMovementMode.MOVE_Walking);
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        //Ray ray = new Ray(player.transform.position, Vector3.down);
        //if (Physics.Raycast(ray, out hitInfo, 100))
        //{
        //    IsHit = true;
        //}
        //else
        //{
        //    IsHit = false;
        //}
        //PhysFalling(Time.deltaTime);
        //FindFloorNew();
        //Move();
        //if (IsHit)
        //{
        //    Vector3 FloorNormal = hitInfo.normal;
        //    float FloorDotDelta = Vector3.Dot(FloorNormal, Velocity);
        //    Velocity = (Velocity - (FloorNormal * FloorDotDelta));
        //}
        //FindFloorNew(CurrentFloor, null);

        //FStepDownResult StepDownResult = new FStepDownResult();
        //PhysWalking(Time.deltaTime);

        StartNewPhysics(Time.deltaTime);
        //FindFloorNew(CurrentFloor, null);
    }
    bool IsMovingOnGround()
    {
        return ((MovementMode == EMovementMode.MOVE_Walking));
    }
    private Vector3 ComputeGroundMovementDelta(Vector3 Delta, FHitResult RampHit)
    {
        Vector3 FloorNormal = RampHit.ImpactNormal;
        if (FloorNormal.y < (1.0f - UE_KINDA_SMALL_NUMBER) && FloorNormal.y > UE_KINDA_SMALL_NUMBER && IsWalkable(RampHit))
        {
            float FloorDotDelta = Vector3.Dot(FloorNormal, Delta);
            Vector3 RampMovement = (Delta - (FloorNormal * FloorDotDelta));
            return RampMovement;
        }
        return Delta;
    }
    private bool IsWalkable(FHitResult Hit)
    {
        if (!Hit.bBlockingHit)
        {
            return false;
        }
        if (Hit.ImpactNormal.y < UE_KINDA_SMALL_NUMBER)
        {
            return false;
        }

        //float TestWalkableZ = WalkableFloorZ;

        //// Can't walk on this surface if it is too steep.
        //if (Hit.ImpactNormal.y < TestWalkableZ)
        //{
        //    return false;
        //}
        return true;
    }
    private bool StepUp(Vector3 GravDir, Vector3 Delta, FHitResult InHit, FStepDownResult OutStepDownResult)
    {
        if (MaxStepHeight <= 0)
        {
            return false;
        }
        float PawnHalfHeight = CapsuleShape.CapsuleHalfHeight;
        float PawnRadius = CapsuleShape.CapsuleRadius;
        float InitialImpactZ = InHit.ImpactPoint.y;
        Vector3 OldLocation = player.transform.position;

        Vector3 RawPos = player.transform.position;

        if (InitialImpactZ > OldLocation.y + (PawnHalfHeight - PawnRadius))
        {
            return false;
        }
        float StepTravelUpHeight = MaxStepHeight;
        float StepTravelDownHeight = StepTravelUpHeight;
        float PawnInitialFloorBaseZ = OldLocation.y - CapsuleShape.CapsuleHalfHeight;
        float PawnFloorPointZ = PawnInitialFloorBaseZ;
        if (IsMovingOnGround() && CurrentFloor.IsWalkableFloor())
        {
            float FloorDist = MathF.Max(0.0f, CurrentFloor.GetDistanceToFloor());
            PawnInitialFloorBaseZ -= FloorDist;
            StepTravelUpHeight = MathF.Max(StepTravelUpHeight - FloorDist, 0.0f);
            StepTravelDownHeight = (MaxStepHeight + MAX_FLOOR_DIST * 2.0f);
            bool bHitVerticalFace = !IsWithinEdgeTolerance(InHit.Location, InHit.ImpactPoint, CapsuleShape.CapsuleRadius);
            if (!CurrentFloor.bLineTrace && !bHitVerticalFace)
            {
                PawnFloorPointZ = CurrentFloor.HitResult.ImpactPoint.y;
            }
            else
            {
                PawnFloorPointZ -= CurrentFloor.FloorDist;
            }
        }

        if (InitialImpactZ <= PawnInitialFloorBaseZ)
        {
            return false;
        }
        //step up
        FHitResult SweepUpHit = new FHitResult(1);
        MoveUpdateComponent(-GravDir * StepTravelUpHeight, SweepUpHit);

        //step forward
        FHitResult Hit = new FHitResult(1);
        MoveUpdateComponent(Delta, Hit);
        FStepDownResult StepDownResult = new FStepDownResult();
        //step down
        Hit.Reset(1);
        MoveUpdateComponent(GravDir * StepTravelDownHeight, Hit);
        if (Hit.bBlockingHit)
        {
            float DeltaZ = Hit.ImpactPoint.y - PawnFloorPointZ;
            if (DeltaZ > MaxStepHeight)
            {
                player.transform.position = RawPos;
                return false;
            }
            if (!IsWithinEdgeTolerance(Hit.Location, Hit.ImpactPoint, PawnRadius))
            {
                Debug.LogError("StepUp----Stepdown的时候无法站在悬崖上");
                player.transform.position = RawPos;
                return false;
            }

            if (OutStepDownResult != null)
            {
                FindFloor(player.transform.position, StepDownResult.FloorResult, Hit);
                if (Hit.Location.y > OldLocation.y)
                {

                }
                StepDownResult.bComputedFloor = true;
            }
        }
        if (OutStepDownResult != null)
        {
            OutStepDownResult.Copy(StepDownResult);
        }
        return true;
    }
    private void MoveAlongFloor(Vector3 InVelocity, float DeltaSeconds, FStepDownResult OutStepDownResult)
    {
        Vector3 Delta = new Vector3(InVelocity.x, 0, InVelocity.z) * DeltaSeconds;
        FHitResult Hit = new FHitResult();
        Vector3 RampVector = ComputeGroundMovementDelta(Delta, CurrentFloor.HitResult);
        SafeMoveUpdatedComponent(RampVector, Hit);

        if (Hit.bBlockingHit)
        {
            float PercentTimeApplied = Hit.Time;
            if (Hit.Time > 0 && Hit.HitResult.normal.z > 1e-4f)
            {
                float InitialPercentRemaining = 1.0f - PercentTimeApplied;
                RampVector = ComputeGroundMovementDelta(Delta * InitialPercentRemaining, Hit);
                SafeMoveUpdatedComponent(RampVector, Hit);

                float SecondHitPercent = Hit.Time * InitialPercentRemaining;
                PercentTimeApplied = Math.Clamp(PercentTimeApplied + SecondHitPercent, 0.0f, 1.0f);
            }
            else
            {

                Debug.LogError("撞上了法线==0的");
            }
            if (Hit.bBlockingHit)
            {
                Vector3 GravDir = new Vector3(0.0f, -1f, 0f);
                Debug.Log($"Delta * (1.0f - PercentTimeApplied)={(Delta * (1.0f - PercentTimeApplied)).magnitude}");
                if (!StepUp(GravDir, Delta * (1.0f - PercentTimeApplied), Hit, OutStepDownResult))
                {
                    Debug.LogError("StepUp   失败");
                }
                else
                {
                    Debug.LogError("StepUp   成功");
                }
                //isStop = true;
            }
        }

    }
    private void MoveUpdateComponent(Vector3 Delta, FHitResult OutHit)
    {
        CapsuleShape.UpdateShape(player.transform.position);

        if (Physics.CapsuleCast(CapsuleShape.Point1, CapsuleShape.Point2, CapsuleShape.CapsuleRadius, Delta.normalized, out OutHit.HitResult, Delta.magnitude))
        {
            player.transform.position = player.transform.position + Delta.normalized * OutHit.HitResult.distance;
            OutHit.bBlockingHit = true;
            OutHit.ImpactNormal = OutHit.HitResult.normal;
            OutHit.ImpactPoint = OutHit.HitResult.point;
            OutHit.Location = player.transform.position;
            OutHit.Time = OutHit.HitResult.distance / Delta.magnitude;
        }
        else
        {
            player.transform.position = player.transform.position + Delta;
            OutHit.bBlockingHit = false;
        }
    }
    private void AdjustFloorHeight()
    {
        if (!CurrentFloor.IsWalkableFloor())
        {
            return;
        }
        float OldFloorDist = CurrentFloor.FloorDist;
        if (OldFloorDist < MIN_FLOOR_DIST || OldFloorDist > MAX_FLOOR_DIST)
        {
            FHitResult AdjustHit = new FHitResult();
            float InitialY = player.transform.position.y;
            float AvgFloorDist = (MIN_FLOOR_DIST + MAX_FLOOR_DIST) * 0.5f;
            float MoveDist = AvgFloorDist - OldFloorDist;
            SafeMoveUpdatedComponent(new Vector3(0, MoveDist, 0), AdjustHit);
            Debug.Log($"Adjust floor height {MoveDist} (Hit = {AdjustHit.bBlockingHit})");
            if (!AdjustHit.bBlockingHit)
            {
                CurrentFloor.FloorDist += MoveDist;
            }
            else if (MoveDist > 0)
            {
                CurrentFloor.FloorDist += player.transform.position.y - InitialY;
            }
            else
            {
                CurrentFloor.FloorDist = player.transform.position.y - AdjustHit.HitResult.point.y;
                CurrentFloor.SetFromSweep(AdjustHit, CurrentFloor.FloorDist, true);
            }
        }
    }
    private void SafeMoveUpdatedComponent(Vector3 Delta, FHitResult OutHit)
    {
        MoveUpdateComponent(Delta, OutHit);
    }
    private void FindFloor(Vector3 CapsuleLocation, FFindFloorResult OutFloorResult, FHitResult DownwardSweepResult)
    {
        OutFloorResult.Clear();
        float HeightCheckAdjust = MAX_FLOOR_DIST + UE_KINDA_SMALL_NUMBER;
        float FloorSweepTraceDist = Mathf.Max(MAX_FLOOR_DIST, MaxStepHeight + HeightCheckAdjust);
        float FloorLineTraceDist = FloorSweepTraceDist;
        bool bNeedToValidateFloor = true;

        if (FloorLineTraceDist > 0.0f || FloorSweepTraceDist > 0.0f)
        {
            ComputeFloorDist(CapsuleLocation, FloorLineTraceDist, FloorSweepTraceDist, OutFloorResult, CapsuleShape.CapsuleRadius, DownwardSweepResult);
        }
        if (bNeedToValidateFloor && OutFloorResult.bBlockingHit)
        {

        }
    }
    private void ComputeFloorDist(Vector3 CapsuleLocation, float LineDistance, float SweepDistance, FFindFloorResult OutFloorResult, float SweepRadius, FHitResult DownwardSweepResult)
    {
        if (SweepDistance > 0 & SweepRadius > 0)
        {
            float ShrinkScale = 0.9f;
            float ShrinkScaleOverlap = 0.1f;
            float ShrinkHeight = (CapsuleShape.CapsuleHalfHeight - CapsuleShape.CapsuleRadius) * (1.0f - ShrinkScale);
            float TraceDist = SweepDistance + ShrinkHeight;

            FCapsuleShape ShrinkCapsuleShape = MakeCapsule(SweepRadius, CapsuleShape.CapsuleHalfHeight - ShrinkHeight);

            FHitResult Hit = new FHitResult();

            bool bBlockingHit = FloorSweepTest(Hit, CapsuleLocation, CapsuleLocation + new Vector3(0, -TraceDist, 0), ShrinkCapsuleShape);

            if (bBlockingHit)
            {
                if (!IsWithinEdgeTolerance(CapsuleLocation, Hit.ImpactPoint, ShrinkCapsuleShape.CapsuleRadius))
                {
                    ShrinkCapsuleShape.CapsuleRadius = MathF.Max(0.0f, ShrinkCapsuleShape.CapsuleRadius - SWEEP_EDGE_REJECT_DISTANCE - UE_KINDA_SMALL_NUMBER);

                    if (!(ShrinkCapsuleShape.CapsuleRadius <= UE_KINDA_SMALL_NUMBER))
                    {
                        ShrinkHeight = (CapsuleShape.CapsuleHalfHeight - CapsuleShape.CapsuleRadius) * (1.0f - ShrinkScaleOverlap);
                        TraceDist = SweepDistance + ShrinkHeight;
                        ShrinkCapsuleShape.CapsuleHalfHeight = MathF.Max(ShrinkCapsuleShape.CapsuleHalfHeight - ShrinkHeight, ShrinkCapsuleShape.CapsuleRadius);
                        Hit.Reset(1.0f);

                        bBlockingHit = FloorSweepTest(Hit, CapsuleLocation, CapsuleLocation + new Vector3(0, -TraceDist, 0), ShrinkCapsuleShape);
                    }
                }
                float MaxPenetrationAdjust = Mathf.Max(MAX_FLOOR_DIST, CapsuleShape.CapsuleRadius);
                float SweepResult = Mathf.Max(-MaxPenetrationAdjust, Hit.Time * TraceDist - ShrinkHeight);

                OutFloorResult.SetFromSweep(Hit, SweepResult, false);
                if (Hit.bBlockingHit)
                {
                    if (SweepResult <= SweepDistance)
                    {
                        OutFloorResult.bWalkableFloor = true;
                        return;
                    }
                }
            }

        }
        if (!OutFloorResult.bBlockingHit)
        {
            OutFloorResult.FloorDist = SweepDistance;
            return;
        }
        OutFloorResult.bWalkableFloor = false;
    }
    private FCapsuleShape MakeCapsule(float CapsuleRadius, float CapsuleHalfHeight)
    {
        FCapsuleShape CapsuleShape = new FCapsuleShape();
        CapsuleShape.CapsuleRadius = CapsuleRadius;
        CapsuleShape.CapsuleHalfHeight = CapsuleHalfHeight;
        return CapsuleShape;
    }
    private bool FloorSweepTest(FHitResult OutHit, Vector3 Start, Vector3 End, FCapsuleShape CollisionShape)
    {
        CollisionShape.UpdateShape(Start);
        if (Physics.CapsuleCast(CollisionShape.Point1, CollisionShape.Point2, CollisionShape.CapsuleRadius, End - Start, out OutHit.HitResult, (End - Start).magnitude))
        {
            OutHit.bBlockingHit = true;
            OutHit.ImpactNormal = OutHit.HitResult.normal;
            OutHit.ImpactPoint = OutHit.HitResult.point;
            OutHit.Time = OutHit.HitResult.distance / (End - Start).magnitude;
            OutHit.Location = Start + (End - Start).normalized * OutHit.HitResult.distance;
            return true;
        }
        else
        {
            OutHit.bBlockingHit = false;
            OutHit.Time = 1;
            return false;
        }
    }
    private bool IsWithinEdgeTolerance(Vector3 CapsuleLocation, Vector3 TestImpactPoint, float CapsuleRadius)
    {
        Vector3 delta = (TestImpactPoint - CapsuleLocation);
        float DistFromCenterSq = delta.x * delta.x + delta.z * delta.z;
        float ReducedRadiusSq = MathF.Pow(MathF.Max(SWEEP_EDGE_REJECT_DISTANCE + UE_KINDA_SMALL_NUMBER, CapsuleRadius - SWEEP_EDGE_REJECT_DISTANCE), 2);
        //Debug.Log($"DistFromCenterSq={DistFromCenterSq}   ReducedRadiusSq={ReducedRadiusSq}");
        return DistFromCenterSq < ReducedRadiusSq;
    }
    private void PhysWalking(float timeTick)
    {
        Vector3 MoveVelocity = Velocity;
        Vector3 Delta = timeTick * MoveVelocity;
        bool bZeroDelta = false;
        bool bCheckedFall = false;
        float remainingTime = timeTick;
        Vector3 OldLocation = player.transform.position;
        FFindFloorResult OldFloor = CurrentFloor;
        if (Delta.x <= float.Epsilon && Delta.y <= float.Epsilon && Delta.z <= float.Epsilon)
        {
            bZeroDelta = true;
        }

        FStepDownResult StepDownResult = new FStepDownResult();
        MoveAlongFloor(MoveVelocity, timeTick, StepDownResult);

        if (StepDownResult.bComputedFloor)
        {
            CurrentFloor.Copy(StepDownResult.FloorResult);
        }
        else
        {
            FindFloor(player.transform.position, CurrentFloor, null);
        }
        if (CurrentFloor.IsWalkableFloor())
        {
            AdjustFloorHeight();
        }
        if (!CurrentFloor.IsWalkableFloor())
        {
            bool bMustJump = bZeroDelta;
            if ((bMustJump || !bCheckedFall) && CheckFall(OldFloor, CurrentFloor.HitResult, Delta, OldLocation, remainingTime, timeTick, 0, bMustJump))
            {
                return;
            }
            bCheckedFall = true;
        }
    }
    private bool CheckFall(FFindFloorResult OldFloor, FHitResult Hit, Vector3 Delta, Vector3 OldLocation, float remainingTime, float timeTick, int Iterations, bool bMustJump)
    {
        //if (bMustJump)
        {
            //HandleWalkingOffLedge(OldFloor.HitResult.ImpactNormal, OldFloor.HitResult.Normal, OldLocation, timeTick);
            if (IsMovingOnGround())
            {
                // If still walking, then fall. If not, assume the user set a different mode they want to keep.
                StartFalling(Iterations, remainingTime, timeTick, Delta, OldLocation);
            }
            return true;
        }
        return false;
    }
    void StartFalling(int Iterations, float remainingTime, float timeTick, Vector3 Delta, Vector3 subLoc)
    {
        // start falling 
        if (IsMovingOnGround())
        {
            SetMovementMode(EMovementMode.MOVE_Falling); //default behavior if script didn't change physics
        }
        StartNewPhysics(remainingTime);
    }
    private void PhysFalling(float deltaTime)
    {
        Velocity = Velocity + Gravity * deltaTime;
        //SafeMoveUpdatedComponent
        Vector3 Adjusted = Velocity * deltaTime;
        player.transform.position += Adjusted;
    }
    private void SetMovementMode(EMovementMode NewMovementMode)
    {
        if (MovementMode == NewMovementMode)
        {
            return;
        }
        Debug.Log($"运动模式从{MovementMode}迁移到{NewMovementMode}");
        EMovementMode PrevMovementMode = MovementMode;
        MovementMode = NewMovementMode;
        OnMovementModeChanged(PrevMovementMode);
    }
    private void OnMovementModeChanged(EMovementMode PreviousMovementMode)
    {
        if (MovementMode == EMovementMode.MOVE_Walking)
        {
            // Walking uses only XY velocity, and must be on a walkable floor, with a Base.
            Velocity.y = 0.0f;

            // make sure we update our new floor/base on initial entry of the walking physics
            FindFloor(player.transform.position, CurrentFloor, null);
            AdjustFloorHeight();
            //SetBaseFromFloor(CurrentFloor);
        }
        else
        {
            CurrentFloor.Clear();

            if (MovementMode == EMovementMode.MOVE_Falling)
            {
                //DecayingFormerBaseVelocity = GetImpartedMovementBaseVelocity();
                //Velocity += DecayingFormerBaseVelocity;
                //if (bMovementInProgress && CurrentRootMotion.HasAdditiveVelocity())
                //{
                //    // If we leave a base during movement and we have additive root motion, we need to add the imparted velocity so that it retains it next tick
                //    CurrentRootMotion.LastPreAdditiveVelocity += DecayingFormerBaseVelocity;
                //}
                //if (!CharacterMovementCVars::bAddFormerBaseVelocityToRootMotionOverrideWhenFalling || FormerBaseVelocityDecayHalfLife == 0.f)
                //{
                //    DecayingFormerBaseVelocity = FVector::ZeroVector;
                //}
                //CharacterOwner->Falling();
            }
            if (MovementMode == EMovementMode.MOVE_None)
            {
                // Kill velocity and clear queued up events
                //StopMovementKeepPathing();
                //CharacterOwner->ResetJumpState();
                //ClearAccumulatedForces();
            }
        }
        if (MovementMode == EMovementMode.MOVE_Falling && PreviousMovementMode != EMovementMode.MOVE_Falling)
        {

        }
    }
    private void StartNewPhysics(float deltaTime)
    {
        switch (MovementMode)
        {
            case EMovementMode.MOVE_None:
                break;
            case EMovementMode.MOVE_Walking:
                PhysWalking(deltaTime);
                break;
            case EMovementMode.MOVE_NavWalking:
                break;
            case EMovementMode.MOVE_Falling:
                PhysFalling(deltaTime);
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
                break;
        }
    }
    private void OnDrawGizmos()
    {
        if (CurrentFloor != null)
        {
            Vector3 FloorNormal = CurrentFloor.HitResult.ImpactNormal;
            Vector3 from = CurrentFloor.HitResult.HitResult.point;
            Vector3 to = FloorNormal * 100 + from;
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(from, to);

            //Vector3 oldVelocity = Vector3.forward * 10;
            //from = player.transform.position;
            //to = from + oldVelocity;
            //Gizmos.color = Color.red;
            //Gizmos.DrawLine(from, to);

            //float FloorDotDelta = Vector3.Dot(FloorNormal, oldVelocity);

            //// Vector3 newVelocity = (oldVelocity - (FloorNormal * FloorDotDelta));
            //Vector3 newVelocity = new Vector3(oldVelocity.x, -FloorDotDelta / FloorNormal.y, oldVelocity.z);

            //from = player.transform.position;
            //to = from + newVelocity.normalized * oldVelocity.magnitude;

            //Gizmos.color = Color.green;

            //Gizmos.DrawLine(from, to);

            //Vector3 from = hitInfo.point;
            //Vector3 to = player.transform.position;
            //Gizmos.color = Color.blue;
            //Gizmos.DrawLine(from, to);
        }
    }
}