using Sirenix.OdinInspector.Editor.Drawers;
using System.Collections.Generic;
using UnityEngine;
namespace HFSM
{
    public class StateMachine : StateBase
    {
        public class StateBundle
        {
            public StateBase state = null;
            public List<Transition> transitions = new List<Transition>();
            public StateBundle(StateBase state)
            {
                this.state = state;
            }
            public void AddTransition(Transition t)
            {
                transitions.Add(t);
            }
        }
        public StateBase initState = null;
        public StateBase currentState = null;
        public Dictionary<string, StateBundle> stateBundles = new Dictionary<string, StateBundle>();
        public static readonly List<Transition> nullTransition = new List<Transition>(0);
        public List<Transition> currentTransitions= nullTransition;

        public StateMachine(string name) : base(name)
        {
        }
        public override void Enter()
        {
            base.Enter();
            if (currentState!=null)
            {

            }
            else
            {
                ChangeState(initState.name);
            }
        }
        public void SetInitState(string name)
        {
            if (stateBundles.TryGetValue(name, out StateBundle bundle))
            {
                initState = bundle.state;
            }
            else
            {
                Debug.LogError($"SetInitState is Error   ={name}");
            }
        }
        public void AddState(string name, StateBase state)
        {
            if (stateBundles.ContainsKey(name))
            {
                Debug.LogError($"stateBundles.ContainsKey   ={name}");
                return;
            }
            StateBundle bundle = new StateBundle(state);
            stateBundles.Add(name, bundle);
        }
        public void AddTrasition(string from, string to,int level)
        {
            if (stateBundles.TryGetValue(from, out StateBundle bundle))
            {
                bundle.AddTransition(new Transition(from, to, level));
            }
            else
            {
                Debug.LogError($"not from state   ={from}");
            }
        }
        public void ChangeState(string name)
        {
            if (!stateBundles.TryGetValue(name, out StateBundle bundle))
            {
                Debug.LogError($"ChangeState  dont find state   ={name}");
                return;
            }
            string oldName = "";
            if (currentState != null)
            {
                currentState.Exit();
                oldName = currentState.name;
            }
           
           
            currentTransitions = bundle.transitions!=null ? bundle.transitions : nullTransition;
            currentState = bundle.state;
            currentState.Enter();
            Debug.Log($"========");
        }
        public override void Update()
        {
            for (int i = 0; i < currentTransitions.Count; i++)
            {
                ChangeState(currentTransitions[i].to);
            }
            if (currentState != null)
            {
                currentState.Update();
            }
        }
    }
}

