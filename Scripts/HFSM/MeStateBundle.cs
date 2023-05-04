using FSM;
using System.Collections.Generic;

public class MeStateBundle
{
    public List<MeTransitionBase> transitions;
    public MeStateBase state;

    public void AddTransition(MeTransitionBase t)
    {
        transitions = transitions ?? new List<MeTransitionBase>();
        transitions.Add(t);
    }
}
