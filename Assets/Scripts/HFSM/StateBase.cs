using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateBase
{
    public bool needsExitTime = false;
    public bool isGhostState;
    public EStateType name;
    public IStateMachine fsm;
    public virtual void Init()
    {

    }
    public virtual void OnEnter()
    {

    }
    public virtual void OnLogic()
    {

    }
    public virtual void OnExit()
    {

    }
    public virtual void OnExitRequest()
    {

    }
}
public class WalkState: StateBase
{

}
