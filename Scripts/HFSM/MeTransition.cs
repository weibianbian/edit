using FSM;
using System;

public class MeTransition : MeTransitionBase
{
    public Func<MeTransition, bool> condition;
    public MeTransition(
            string from,
            string to,
            Func<MeTransition, bool> condition = null,
            bool forceInstantly = false) : base(from, to, forceInstantly)
    {
        this.condition = condition;
    }
    public override bool ShouldTransition()
    {
        if (condition == null)
            return true;

        return condition(this);
    }
}
