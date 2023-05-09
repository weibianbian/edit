using System;
using System.Collections.Generic;

public class TransitionBase
{
    public string from;
    public string to;

    public bool forceInstantly;

    public IStateMachine fsm;
    public ICondition condition = null;
    public TransitionBase(string from, string to, ICondition condition =null, bool forceInstantly = false)
    {
        this.from = from;
        this.to = to;
        this.condition = condition;
        this.forceInstantly = forceInstantly;
    }
    public virtual void Init()
    {

    }
    public virtual void OnEnter()
    {

    }
    public virtual bool ShouldTransition()
    {
        if (condition==null) {
            return true;
        }
        return condition.Condition(this);
    }
}
