using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

public class FSMStateGraph : SerializedMonoBehaviour
{
    [LabelText("说明")]
    public string des="";
    [LabelText("名称")]
    public string stateName ="";
    public void Awake()
    {
    }
    public virtual void OnSave()
    {

    }
    public virtual StateBase CreateFSMFromGraph()
    {
        return new StateBase();
    }
    public static IEnumerable GetStateType1()
    {

        return new ValueDropdownList<StateBase>()
        {
            { EStateType.Root.ToString(), new StateMachine() },
            { "StateBase", new StateBase() },
            { "ActionState", new ActionState() },
        };
    }
}
public enum EStateType { 
    Root,
    Patrol,
    Combat,
    Idle,
    Move,
    TurnTo,
    Walk,
}

