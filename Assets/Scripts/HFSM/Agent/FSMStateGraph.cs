using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Unity.VisualScripting;

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
    public virtual StateBase CreateFSMFromGraph(FSMComponentGraph graph)
    {
        return new StateBase(graph.compt);
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

