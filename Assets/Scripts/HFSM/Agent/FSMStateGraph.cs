using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class FSMStateGraph : SerializedMonoBehaviour
{
    [LabelText("说明")]
    public string des="";
    [LabelText("名称")]
    public EStateType stateName =EStateType.Idle;
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
            { "StateMachine", new StateMachine() },
            { "StateBase", new StateBase() },
            { "ActionState", new ActionState() },
        };
    }
}
public enum EStateType { 
    Idle,
    Move,
    Walk,
}

