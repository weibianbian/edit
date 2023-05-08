using System.Collections.Generic;

public class TransitionBase
{
    public EStateType from;
    public EStateType to;

    public bool forceInstantly;

    public IStateMachine fsm;
    public List<ConditionBase> conditions=new List<ConditionBase>();

    public TransitionBase(EStateType from, EStateType to, bool forceInstantly = false)
    {
        this.from = from;
        this.to = to;
        this.forceInstantly = forceInstantly;
    }
    public void AddCondition(ConditionBase condition)
    {
        conditions.Add(condition);
    }
    public virtual void Init()
    {

    }
    public virtual void OnEnter()
    {

    }
    public virtual bool ShouldTransition()
    {
        for (int i = 0; i < conditions.Count; i++)
        {

        }
        return true;
    }
}
