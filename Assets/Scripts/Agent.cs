using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Agent : MonoBehaviour
{
    public FSMComponent fsmCompt;
    public NavMeshAgent navAgent;

    public EAgentSubStateType subState = EAgentSubStateType.Patrol;

    public void Awake()
    {
        navAgent.enabled = false;
    }
    public void Start()
    {
        //MoveTo(Vector3.zero);
    }
    public void SetPos(Vector3 pos)
    {
        gameObject.transform.position = pos;    
    }
    public void MoveTo(Vector3 pos)
    {
        navAgent.enabled = true;
        navAgent.speed = 1;
        navAgent.SetDestination(pos);
    }
    public void Update()
    {
        if (navAgent.enabled)
        {
            if (navAgent.remainingDistance <= 0.1f)
            {
                navAgent.enabled = false;
                Debug.LogError("到达目的地");
            }
        }
        if (fsmCompt!=null)
        {
            fsmCompt.Update();
        }
    }

}
