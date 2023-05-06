using System.Collections.Generic;

public class StateBundle
{
    public List<TransitionBase> transitions;
    public StateBase state;

    public void AddTransition(TransitionBase t)
    {
        transitions = transitions ?? new List<TransitionBase>();
        transitions.Add(t);
    }
}
