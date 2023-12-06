using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Imaging;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Animations;

public class UFindFloor : MonoBehaviour
{
    public GameObject player;

    public Vector3 Gravity = new Vector3(0, -9.80f, 0);

    // Start is called before the first frame update
    public RaycastHit hitInfo;
    public Vector3 Velocity;
    public bool IsHit = false;
    public float MaxStepHeight = 2;
    public float MAX_FLOOR_DIST = 2.4f;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // Ray ray = new Ray(player.transform.position, Vector3.down);
        // if (Physics.Raycast(ray, out hitInfo, 100))
        // {
        //     IsHit = true;
        // }
        // else
        // {
        //     IsHit = false;
        // }
        PhysFalling(Time.deltaTime);
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
            Vector3 FloorNormal = hitInfo.normal;
            Vector3 from = hitInfo.point;
            Vector3 to = FloorNormal * 10 + from;
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(from, to);

            Vector3 oldVelocity = Vector3.forward * 10;
            from = player.transform.position;
            to = from + oldVelocity;
            Gizmos.color = Color.red;
            Gizmos.DrawLine(from, to);

            float FloorDotDelta = Vector3.Dot(FloorNormal, oldVelocity);

            // Vector3 newVelocity = (oldVelocity - (FloorNormal * FloorDotDelta));
            Vector3 newVelocity = new Vector3(oldVelocity.x, -FloorDotDelta / FloorNormal.y, oldVelocity.z);

            from = player.transform.position;
            to = from + newVelocity.normalized * oldVelocity.magnitude;

            Gizmos.color = Color.green;

            Gizmos.DrawLine(from, to);
        }
    }
}