using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

public class TransitionBase:IJsonConvertible
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

    public void WriteJson(JObject writer)
    {
        writer.Add("from",from);
        writer.Add("to", to);
    }
    public void ReadJson(JObject writer)
    {
    }
}
