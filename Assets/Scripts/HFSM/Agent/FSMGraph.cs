using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
public class FSMGraph : FSMStateBaseGraph
{
    [ShowInInspector]
    public FSMStateBaseGraph startState;
    [ShowInInspector]
    [HideReferenceObjectPicker]
    [ListDrawerSettings(CustomAddFunction = "AddTransition")]
    public List<FSMTransitionGraph> transitions = new List<FSMTransitionGraph>();
    private FSMTransitionGraph AddTransition => new();
    [ShowInInspector]
    [HideReferenceObjectPicker]
    public List<FSMStateBaseGraph> states = new List<FSMStateBaseGraph>();

    protected override StateBase OnCreateFSMFromGraph(FSMComponentGraph graph)
    {
        StateMachine fsm = new StateMachine(graph.compt);
        for (int i = 0; i < states.Count; i++)
        {
            StateBase state = states[i].CreateFSMFromGraph(graph);
            fsm.AddState(state.name, state);
        }
        if (startState != null)
        {
            fsm.SetStartState(startState.stateName);
        }
        for (int i = 0; i < transitions.Count; i++)
        {
            TransitionBase transition = transitions[i].CreateFromGraph(graph);
            fsm.AddTransition(transition);
        }
        return fsm;
    }

    public void Start()
    {

    }
}
public enum EAgentSubStateType
{
    Patrol,
    Combat,
}
public class StateFactory
{
    //public static StateBase Create(EStateType stateType)
    //{
    //    switch (stateType)
    //    {
    //        case EStateType.Root:
    //            return new StateMachine();
    //        case EStateType.Combat:
    //            return new StateMachine();
    //        case EStateType.Patrol:
    //            return new StateMachine();
    //        case EStateType.Move: return new MoveActionState();
    //        case EStateType.TurnTo: return new TurnToActionState();
    //    }
    //    return null;
    //}
}
public class EStateAttribute : Attribute
{
    public EStateType state;
    public EStateAttribute(EStateType stateType)
    {
        this.state = stateType;
    }
}
