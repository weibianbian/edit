using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Animations;

public class FHitResult
{
    public RaycastHit HitResult;
    public Vector3 ImpactNormal;
    public bool bBlockingHit = false;
    public float Time;
}
public class FFindFloorResult
{
    public FHitResult HitResult = new FHitResult();

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
    public float MAX_FLOOR_DIST = 2.4f;
    public bool isStop = false;
    void Start()
    {
        CurrentFloor = new FFindFloorResult();
        FindFloorNew(CurrentFloor);
    }

    // Update is called once per frame
    void Update()
    {
        if (isStop) { return; }
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
        FindFloorNew(CurrentFloor);
        MoveAlongFloor();
        FindFloorNew(CurrentFloor);
    }
    private Vector3 ComputeGroundMovementDelta(Vector3 Delta, FHitResult RampHit)
    {
        Vector3 FloorNormal = RampHit.ImpactNormal;
        float FloorDotDelta = Vector3.Dot(FloorNormal, Delta);
        Vector3 RampMovement = (Delta - (FloorNormal * FloorDotDelta));
        return RampMovement;
    }
    private void StepUp(Vector3 Delta)
    {
        float StepTravelUpHeight = MaxStepHeight;
        float StepTravelDownHeight = StepTravelUpHeight;

        FHitResult SweepUpHit = new FHitResult();
        //step up
        MoveUpdateComponent(new Vector3(0, StepTravelUpHeight, 0), SweepUpHit);
        FHitResult Hit = new FHitResult();
        //step forward
        MoveUpdateComponent(Delta, Hit);
        //step down
        MoveUpdateComponent(new Vector3(0, -StepTravelDownHeight, 0), Hit);
        if (Hit.bBlockingHit)
        {

        }
    }
    private void MoveAlongFloor()
    {
        Vector3 Delta = new Vector3(Velocity.x, 0, Velocity.z) * Time.deltaTime;
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
                
                Debug.LogError("装上了法线==0的");
            }
            if (Hit.bBlockingHit)
            {
                StepUp(Delta * (1.0f - PercentTimeApplied));
                Debug.LogError("完成了一次上下");
                //isStop = true;
            }
        }

    }
    private void MoveUpdateComponent(Vector3 Delta, FHitResult OutHit)
    {
        Vector3 playerPos = player.transform.position;
        Vector3 pos1 = new Vector3(playerPos.x, playerPos.y - 0.5f, playerPos.z);
        Vector3 pos2 = new Vector3(playerPos.x, playerPos.y + 0.5f, playerPos.z);

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
    private void SafeMoveUpdatedComponent(Vector3 Delta, FHitResult OutHit)
    {
        Vector3 playerPos = player.transform.position;
        Vector3 pos1 = new Vector3(playerPos.x, playerPos.y - 0.5f, playerPos.z);
        Vector3 pos2 = new Vector3(playerPos.x, playerPos.y + 0.5f, playerPos.z);
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
    private void FindFloorNew(FFindFloorResult OutFloorResult)
    {
        Vector3 playerPos = player.transform.position;
        Vector3 pos1 = new Vector3(playerPos.x, playerPos.y - 0.5f, playerPos.z);
        Vector3 pos2 = new Vector3(playerPos.x, playerPos.y + 0.5f, playerPos.z);
        if (Physics.CapsuleCast(pos1, pos2, 0.5f, -Vector3.up, out OutFloorResult.HitResult.HitResult, MaxStepHeight))
        {
            OutFloorResult.HitResult.bBlockingHit = true;
            OutFloorResult.HitResult.Time = 1;
            OutFloorResult.HitResult.ImpactNormal = OutFloorResult.HitResult.HitResult.normal;
            player.transform.position = playerPos - Vector3.up * (OutFloorResult.HitResult.HitResult.distance - 0.1f);
        }
        else
        {
            OutFloorResult.HitResult.bBlockingHit = false;
            OutFloorResult.HitResult.Time = 0;
        }
    }
    private void PhysFalling(float deltaTime)
    {
        Velocity = Velocity + Gravity * deltaTime;
        //SafeMoveUpdatedComponent
        Vector3 Adjusted = Velocity * deltaTime;
        player.transform.position += Adjusted;
    }

    private void FindFloor()
    {
        float HeightCheckAdjust = -MAX_FLOOR_DIST;
        float FloorSweepTraceDist = Mathf.Max(MAX_FLOOR_DIST, MaxStepHeight + HeightCheckAdjust);
        float FloorLineTraceDist = FloorSweepTraceDist;
        if (FloorLineTraceDist > 0.0f || FloorSweepTraceDist > 0.0f)
        {
            //ComputeFloorDist;

        }
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