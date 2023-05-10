using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : StateBase, IStateMachine, IActionable
{
    private static readonly List<TransitionBase> noTransitions = new List<TransitionBase>(0);
    private List<TransitionBase> activeTransitions = noTransitions;
    private StateBase activeState = null;
    private Dictionary<string, StateBundle> nameToStateBundle = new Dictionary<string, StateBundle>();
    private (string state, bool hasState) startState = (default, false);
    private (string state, bool isPending) pendingState = (default, false);

    public StateMachine(FSMComponent compt) : base(compt)
    {

    }

    public StateBase ActiveState => activeState;
    private bool IsActive => activeState != null;
    public string ActiveStateName => ActiveState.name;
    private bool IsRootFsm => fsm == null;
    public override void Init()
    {
        if (!IsRootFsm) return;
        OnEnter();
    }
    public override void OnEnter()
    {
        if (!startState.hasState)
        {
            Debug.LogError("No start state is selected");
        }
        ChangeState(startState.state);
    }
    public void SetStartState(string name)
    {
        startState = (name, true);
    }
    private void ChangeState(string name)
    {
        UnityEngine.Debug.LogError($"changeState={name}");
        activeState?.OnExit();
        StateBundle bundle;
        if (nameToStateBundle.TryGetValue(name, out bundle))
        {

        }
        activeTransitions = bundle.transitions ?? noTransitions;
        activeState = bundle.state;
        activeState.OnEnter();

        for (int i = 0; i < activeTransitions.Count; i++)
        {
            activeTransitions[i].OnEnter();
        }
    }
    public void AddState(string name, StateBase state)
    {
        state.fsm = this;
        state.name = name;
        state.Init();

        StateBundle bundle = GetOrCreateStateBundle(name);
        bundle.state = state;

        if (nameToStateBundle.Count == 1 && !startState.hasState)
        {
            SetStartState(name);
        }
    }
    public void AddTransition(
            string from,
            string to,
            ICondition condition,
            bool forceInstantly = false)
    {
        AddTransition(CreateOptimizedTransition(from, to, condition, forceInstantly));
    }
    private TransitionBase CreateOptimizedTransition(
        string from,
        string to,
        ICondition condition,
        bool forceInstantly = false)
    {
        return new TransitionBase(from, to, condition, forceInstantly);
    }
    public void AddTransition(TransitionBase transition)
    {
        InitTransition(transition);

        StateBundle bundle = GetOrCreateStateBundle(transition.from);
        bundle.AddTransition(transition);
    }
    private void InitTransition(TransitionBase transition)
    {
        transition.fsm = this;
        transition.Init();
    }
    private StateBundle GetOrCreateStateBundle(string name)
    {
        StateBundle bundle;

        if (!nameToStateBundle.TryGetValue(name, out bundle))
        {
            bundle = new StateBundle();
            nameToStateBundle.Add(name, bundle);
        }

        return bundle;
    }
    public override void OnLogic()
    {
        TryAllDirectTransitions();
        activeState.OnLogic();
    }
    public override void OnExit()
    {
        if (activeState != null)
        {
            activeState.OnExit();
            activeState = null;
        }
    }
    private bool TryAllDirectTransitions()
    {
        for (int i = 0; i < activeTransitions.Count; i++)
        {
            TransitionBase transition = activeTransitions[i];
            if (TryTransition(transition))
                return true;
        }

        return false;
    }
    private bool TryTransition(TransitionBase transition)
    {
        if (!transition.ShouldTransition())
            return false;

        RequestStateChange(transition.to, transition.forceInstantly);

        return true;
    }
    public void RequestStateChange(string name, bool forceInstantly = false)
    {
        if (!activeState.needsExitTime || forceInstantly)
        {
            ChangeState(name);
        }
        else
        {
            pendingState = (name, true);
            activeState.OnExitRequest();
        }
    }
    public void  StateCanExit()
    {
        if (pendingState.isPending)
        {
            string state = pendingState.state;
            pendingState = (default, false);
            ChangeState(state);
        }

        fsm?.StateCanExit();
    }
    public override void OnExitRequest()
    {
        if (activeState.needsExitTime)
        {
            activeState.OnExitRequest();
            return;
        }

        fsm?.StateCanExit();
    }
    public void OnAction(string trigger)
    {
        throw new System.NotImplementedException();
    }

    public void OnAction<TData>(string trigger, TData data)
    {
        throw new System.NotImplementedException();
    }
  
    public override void WriteJson(JObject writer)
    {
        foreach (var item in nameToStateBundle)
        {
            JObject obj = new JObject();
            writer.Add($"{item.Key}", obj);
            item.Value.WriteJson(obj);
        }
        base.WriteJson(writer);

    }
}
