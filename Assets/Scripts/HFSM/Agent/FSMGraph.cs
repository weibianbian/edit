using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

public class FSMGraph : FSMStateGraph
{
    [ShowInInspector]
    public FSMStateGraph startState;
    [ShowInInspector]
    [HideReferenceObjectPicker]
    [ListDrawerSettings(CustomAddFunction = "AddTransition")]
    public List<FSMTransitionGraph> transitions = new List<FSMTransitionGraph>();
    private FSMTransitionGraph AddTransition => new();
    [ShowInInspector]
    [HideReferenceObjectPicker]
    public List<FSMStateGraph> states = new List<FSMStateGraph>();
    [Title("动态创建内容")]
    public StateMachine fsm = null;

    public override StateBase CreateFSMFromGraph()
    {
        fsm = new StateMachine();
        for (int i = 0; i < states.Count; i++)
        {
            StateBase state = states[i].CreateFSMFromGraph();
            fsm.AddState(states[i].stateName, state);
        }
        if (startState != null)
        {
            fsm.SetStartState(startState.stateName);
        }
        for (int i = 0; i < transitions.Count; i++)
        {
            TransitionBase transition = transitions[i].CreateFromGraph();
            fsm.AddTransition(transition);
        }
        return fsm;
    }

    public void Start()
    {

    }
}
public enum EStateType
{
    Patrol,
    Combat,
}
