using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;

public class FSMTransitionGraph
{
    [LabelText("说明")]
    public string des;

    [ShowInInspector]
    [HideReferenceObjectPicker]
    public FSMStateGraph from;

    [ShowInInspector]
    [HideReferenceObjectPicker]
    public FSMStateGraph to;

    [ShowInInspector]
    [HideReferenceObjectPicker]
    [ListDrawerSettings(CustomAddFunction = "AddCondition")]
    public List<FSMCondition> conditions = new List<FSMCondition>();
    private FSMCondition AddCondition => new();

    public TransitionBase CreateFromGraph()
    {
        Transition tran = new Transition(from.stateName, to.stateName);
        return tran;
    }
}
public class FSMCondition
{
    public EStateType stateType;
}