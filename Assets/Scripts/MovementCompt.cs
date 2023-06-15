using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.PlayerSettings;

public class MovementCompt : MonoBehaviour
{
    public NavMeshAgent agent;
    public GameObject targetPos;
    // Start is called before the first frame update
    void Start()
    {
        MoveToPosition(targetPos.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void MoveToPosition(Vector3 pos)
    {
        if (ReachPosition(pos))
        {
            StopMove(EMoveStatus.MOVE_STATUS_DONE);
            return;
        }
        agent.SetDestination(pos);
    }

    public void StopMove(EMoveStatus status)
    {
    }  
    public bool ReachPosition(Vector3 pos)
    {
        return true;
    }
}
