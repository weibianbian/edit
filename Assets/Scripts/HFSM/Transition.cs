﻿using System;

public class Transition : TransitionBase
{
    public Func<Transition, bool> condition;
    public Transition(
            string from,
            string to,
            Func<Transition, bool> condition = null,
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
