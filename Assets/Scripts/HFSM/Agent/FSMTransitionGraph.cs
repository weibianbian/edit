using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
//或 条件
public class OrConditionGroup : ConditionGroup
{
    public OrConditionGroup(FSMComponent compt) : base(compt)
    {
    }

    public override bool Condition(TransitionBase transition)
    {
        for (int i = 0; i < conditions.Count; i++)
        {
            if (conditions[i].Check(compt))
            {
                return true;
            }
        }
        return false;
    }
}
//与 条件
public class AndConditionGroup : ConditionGroup
{
    public AndConditionGroup(FSMComponent compt) : base(compt)
    {
    }

    public override bool Condition(TransitionBase transition)
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
}
public class NormalConditionGroup : ConditionGroup
{
    List< AndConditionGroup> andConditionGroups = new List< AndConditionGroup>();
    List<OrConditionGroup> orConditionGroups = new List<OrConditionGroup>();
    public NormalConditionGroup(FSMComponent compt) : base(compt)
    {

    }
    public override bool Condition(TransitionBase transition)
    {
        for (int i = 0; i < andConditionGroups.Count; i++)
        {
            if (!andConditionGroups[i].Condition(transition))
            {
                return false;
            }
        }
        for (int i = 0; i < orConditionGroups.Count; i++)
        {
            if (!orConditionGroups[i].Condition(transition))
            {
                return false;
            }
        }
        return true;
    }

}
public class ConditionGroup : ICondition
{
    protected List<ConditionBase> conditions = new List<ConditionBase>();
    protected FSMComponent compt;
    public ConditionGroup(FSMComponent compt)
    {
        this.compt = compt;
    }
    public virtual bool Condition(TransitionBase transition)
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
    public FSMStateBaseGraph from;

    [ShowInInspector]
    [HideReferenceObjectPicker]
    public FSMStateBaseGraph to;

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
