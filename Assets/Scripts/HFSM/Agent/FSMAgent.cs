using Sirenix.OdinInspector;
using System.Collections.Generic;

public class FSMAgent : FSMStateAgent
{
    [ShowInInspector]
    public FSMStateAgent startState;
    [ShowInInspector]
    [HideReferenceObjectPicker]
    [ListDrawerSettings(CustomAddFunction = "AddTransition")]
    public List<FSMTransitionAgent> transitions = new List<FSMTransitionAgent>();
    public bool isRoot = false;
    private FSMTransitionAgent AddTransition => new();
    [ShowInInspector]
    [HideReferenceObjectPicker]
    public List<FSMStateAgent> states = new List<FSMStateAgent>();

    public void Awake()
    {
        StateMachine fsm = new StateMachine();
        if (isRoot)
        {
            fsm.Init();
        }
        else
        {
            fsm.AddState(startState.state.name, startState.state);
            for (int i = 0; i < states.Count; i++)
            {
                fsm.AddState(states[i].state.name, states[i].state);
            }
        }
    }
}
public enum EStateType
{
    Patrol,
    Combat,
}
