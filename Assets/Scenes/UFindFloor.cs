using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;
using UnityEngine.Windows;
using static UnityEditor.AddressableAssets.Build.BuildPipelineTasks.GenerateLocationListsTask;

public class FHitResult
{
    public RaycastHit HitResult;
    public Vector3 ImpactNormal;
    public bool bBlockingHit = false;
    public float Time;
}
public class FStepDownResult
{
    public bool bComputedFloor = false;
    public FFindFloorResult FloorResult = new FFindFloorResult();
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
        Point2 = new Vector3(center.x, center.y - (GetScaledCapsuleHalfHeight_WithoutHemisphere()), center.z);
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

    public bool IsHit = false;
    public float MaxStepHeight = 2;
    public float MAX_FLOOR_DIST = 0.24f;
    public float MIN_FLOOR_DIST = 0.19f;
    float UE_KINDA_SMALL_NUMBER = (1.0e-4f);
    public FCapsuleShape CapsuleShape = new FCapsuleShape();
    void Start()
    {
        CapsuleShape.CapsuleHalfHeight = 1;
        CapsuleShape.CapsuleRadius = 0.5f;
        CurrentFloor = new FFindFloorResult();
        //FindFloorNew(CurrentFloor, null);

        FindFloor(player.transform.position, CurrentFloor, null);
        AdjustFloorHeight();
    }
    // Update is called once per frame
    void Update()
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
        PhysWalking(Time.deltaTime);

        //FindFloorNew(CurrentFloor, null);
    }
    private Vector3 ComputeGroundMovementDelta(Vector3 Delta, FHitResult RampHit)
    {
        Vector3 FloorNormal = RampHit.ImpactNormal;
        float FloorDotDelta = Vector3.Dot(FloorNormal, Delta);
        Vector3 RampMovement = (Delta - (FloorNormal * FloorDotDelta));
        return RampMovement;
    }
    private void StepUp(Vector3 Delta, FHitResult InHit, FStepDownResult OutStepDownResult)
    {
        float StepTravelUpHeight = MaxStepHeight;
        float StepTravelDownHeight = StepTravelUpHeight;
        Vector3 OldLocation = player.transform.position;
        FHitResult SweepUpHit = new FHitResult();
        //step up
        MoveUpdateComponent(new Vector3(0, StepTravelUpHeight, 0), SweepUpHit);
        FHitResult Hit = new FHitResult();
        //step forward
        MoveUpdateComponent(Delta, Hit);
        //step down
        MoveUpdateComponent(new Vector3(0, -StepTravelDownHeight, 0), Hit);
        FStepDownResult StepDownResult = new FStepDownResult();
        if (Hit.bBlockingHit)
        {
            if (OutStepDownResult != null)
            {
                FindFloor(player.transform.position, StepDownResult.FloorResult, Hit);
                if (Hit.HitResult.point.y > OldLocation.y)
                {

                }
                StepDownResult.bComputedFloor = true;
            }

        }
        if (OutStepDownResult != null)
        {
            OutStepDownResult = StepDownResult;
        }
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
            }
            else
            {

                Debug.LogError("װ���˷���==0��");
            }
            if (Hit.bBlockingHit)
            {
                StepUp(Delta * (1.0f - PercentTimeApplied), Hit, OutStepDownResult);
                //isStop = true;
            }
        }

    }
    private void MoveUpdateComponent(Vector3 Delta, FHitResult OutHit)
    {
        Vector3 playerPos = player.transform.position;
        Vector3 pos1 = new Vector3(playerPos.x, playerPos.y - CapsuleShape.CapsuleRadius, playerPos.z);
        Vector3 pos2 = new Vector3(playerPos.x, playerPos.y + CapsuleShape.CapsuleRadius, playerPos.z);

        if (Physics.CapsuleCast(pos1, pos2, 0.5f, Delta, out OutHit.HitResult, Delta.magnitude))
        {
            player.transform.position = playerPos + Delta.normalized * OutHit.HitResult.distance;
            OutHit.bBlockingHit = true;
            OutHit.Time = OutHit.HitResult.distance / Delta.magnitude;
        }
        else
        {
            player.transform.position = playerPos + Delta;
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
        CapsuleShape.UpdateShape(player.transform.position);
        if (Physics.CapsuleCast(CapsuleShape.Point1, CapsuleShape.Point2, CapsuleShape.CapsuleRadius, Delta, out OutHit.HitResult, Delta.magnitude))
        {
            player.transform.position = player.transform.position + Delta.normalized * OutHit.HitResult.distance;
            OutHit.bBlockingHit = true;
            OutHit.Time = OutHit.HitResult.distance / Delta.magnitude;
        }
        else
        {
            player.transform.position = player.transform.position + Delta;
            OutHit.bBlockingHit = false;
        }
    }
    private void FindFloor(Vector3 CapsuleLocation, FFindFloorResult OutFloorResult, FHitResult DownwardSweepResult)
    {
        float HeightCheckAdjust = MAX_FLOOR_DIST + UE_KINDA_SMALL_NUMBER;
        float FloorSweepTraceDist = Mathf.Max(MAX_FLOOR_DIST, MaxStepHeight + HeightCheckAdjust);
        float FloorLineTraceDist = FloorSweepTraceDist;
        bool bNeedToValidateFloor = true;

        if (FloorLineTraceDist > 0.0f || FloorSweepTraceDist > 0.0f)
        {
            ComputeFloorDist(CapsuleLocation, FloorLineTraceDist, FloorSweepTraceDist, OutFloorResult, CapsuleShape.CapsuleRadius, DownwardSweepResult);
        }
        //CapsuleShape.UpdateShape(CapsuleLocation);
        //if (Physics.CapsuleCast(CapsuleShape.Point1, CapsuleShape.Point2, CapsuleShape.CapsuleRadius, -Vector3.up, out OutFloorResult.HitResult.HitResult, MaxStepHeight))
        //{
        //    OutFloorResult.HitResult.bBlockingHit = true;
        //    OutFloorResult.HitResult.Time = 1;
        //    OutFloorResult.HitResult.ImpactNormal = OutFloorResult.HitResult.HitResult.normal;
        //    player.transform.position = player.transform.position - Vector3.up * (OutFloorResult.HitResult.HitResult.distance - 0.1f);
        //}
        //else
        //{
        //    OutFloorResult.HitResult.bBlockingHit = false;
        //    OutFloorResult.HitResult.Time = 0;
        //}
    }
    private void ComputeFloorDist(Vector3 CapsuleLocation, float LineDistance, float SweepDistance, FFindFloorResult OutFloorResult, float SweepRadius, FHitResult DownwardSweepResult)
    {
        if (SweepDistance > 0 & SweepRadius > 0)
        {
            float ShrinkScale = 0.9f;
            float ShrinkScaleOverlap = 0.1f;
            float ShrinkHeight = (CapsuleShape.CapsuleHalfHeight - CapsuleShape.CapsuleRadius) * (1.0f - ShrinkScale);
            float TraceDist = SweepDistance + ShrinkHeight;
            FHitResult Hit = new FHitResult();

            bool bBlockingHit = FloorSweepTest(Hit, CapsuleLocation, CapsuleLocation + new Vector3(0, -TraceDist, 0), CapsuleShape);

            if (bBlockingHit)
            {
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
    }
    private bool FloorSweepTest(FHitResult OutHit, Vector3 Start, Vector3 End, FCapsuleShape CollisionShape)
    {
        CollisionShape.UpdateShape(Start);
        if (Physics.CapsuleCast(CollisionShape.Point1, CollisionShape.Point2, CollisionShape.CapsuleRadius, End - Start, out OutHit.HitResult, (End - Start).magnitude))
        {
            OutHit.bBlockingHit = true;
            OutHit.ImpactNormal = OutHit.HitResult.normal;
            OutHit.Time = OutHit.HitResult.distance / (End - Start).magnitude;
            return true;
        }
        else
        {
            OutHit.bBlockingHit = false;
            return false;
        }
    }
    private void PhysWalking(float timeTick)
    {
        Vector3 MoveVelocity = Velocity;
        FStepDownResult StepDownResult = new FStepDownResult();
        MoveAlongFloor(MoveVelocity, timeTick, StepDownResult);

        if (StepDownResult.bComputedFloor)
        {
            CurrentFloor = StepDownResult.FloorResult;
        }
        else
        {
            FindFloor(player.transform.position, CurrentFloor, null);
        }
        if (CurrentFloor.IsWalkableFloor())
        {
            AdjustFloorHeight();
        }
    }
    private void PhysFalling(float deltaTime)
    {
        Velocity = Velocity + Gravity * deltaTime;
        //SafeMoveUpdatedComponent
        Vector3 Adjusted = Velocity * deltaTime;
        player.transform.position += Adjusted;
    }

    private void OnDrawGizmos()
    {
        if (IsHit)
        {
            //Vector3 FloorNormal = hitInfo.normal;
            //Vector3 from = hitInfo.point;
            //Vector3 to = FloorNormal * 10 + from;
            //Gizmos.color = Color.blue;
            //Gizmos.DrawLine(from, to);

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