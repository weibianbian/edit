using Sirenix.OdinInspector;
using System.Collections.Generic;

public class FSMAgent : FSMStateAgent
{
    [ShowInInspector]
    public FSMStateAgent startState;
    [ShowInInspector]
    [HideReferenceObjectPicker]
    public List<FSMTransitionAgent> transitions = new List<FSMTransitionAgent>();
    [ShowInInspector]
    [HideReferenceObjectPicker]
    public List<FSMStateAgent> states = new List<FSMStateAgent>();
}
