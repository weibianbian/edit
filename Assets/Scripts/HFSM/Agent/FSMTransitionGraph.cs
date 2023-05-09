using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ConditionGroup : ICondition
{
    List<ConditionBase> conditions = new List<ConditionBase>();
    FSMComponent compt;
    public ConditionGroup(FSMComponent compt)
    {
        this.compt = compt;
    }
    public bool Condition(TransitionBase transition)
    {
        for (int i = 0; i < conditions.Count; i++)
        {
            if (!conditions[i].Check(compt))
            {
                return false;
            }
        }
        return true;
    }
    public void AddCondition(ConditionBase condition)
    {
        conditions.Add(condition);
    }
}
public interface ICondition
{
    bool Condition(TransitionBase transition);
}
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
    [ValueDropdown("@ConditionTypes", ExpandAllMenuItems = true)]
    [HideReferenceObjectPicker]
    public List<ConditionBase> conditions = new List<ConditionBase>();
    public TransitionBase CreateFromGraph(FSMComponentGraph graph)
    {
        ConditionGroup group = null;
        if (conditions.Count > 0)
        {
            group = new ConditionGroup(graph.compt);
            for (int i = 0; i < conditions.Count; i++)
            {
                group.AddCondition(conditions[i]);
            }
        }
        TransitionBase tran = new TransitionBase(from.stateName, to.stateName, group);
        return tran;
    }
    public static IEnumerable ConditionTypes = new ValueDropdownList<ConditionBase>()
    {
        { "敌人状态", new AIStateCondition() },
    };
}
