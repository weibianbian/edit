using Sirenix.OdinInspector;
using System;
using System.Collections;
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
    [ValueDropdown("@ConditionTypes",ExpandAllMenuItems =true)]
    [HideReferenceObjectPicker]
    public List<ConditionBase> conditions = new List<ConditionBase>();
    public TransitionBase CreateFromGraph()
    {
        TransitionBase tran = new TransitionBase(from.stateName, to.stateName);
        for (int i = 0; i < conditions.Count; i++)
        {
            tran.AddCondition(conditions[i]);
        }
        return tran;
    }
    public static IEnumerable ConditionTypes = new ValueDropdownList<ConditionBase>()
    {
        { "敌人状态", new AIStateCondition() },
    };
}
public class ConditionBase
{
   
}
public class AIStateCondition : ConditionBase
{
    public EAgentSubStateType stateType;
}