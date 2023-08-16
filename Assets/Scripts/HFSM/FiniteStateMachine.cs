using HFSMRuntime;
using RailShootGame;
using System.Collections.Generic;

namespace FSMRuntime
{
    public class FiniteStateMachine
    {
        List<State> states;
        State initalState;
        State currentState;
        public FiniteStateMachine(State intialS, params State[] state)
        {
            this.initalState = intialS;
            currentState = this.initalState;
            states = new List<State>
            {
                intialS
            };

            foreach (State s in state)
            {
                states.Add(s);
            }
        }
        public override string ToString()
        {
            return currentState.name;
        }
        public List<IAction> UpdateFSM(UWorld g, Actor e)
        {
            Transition triggeredTransition = null;
            foreach (Transition t in currentState.GetTransitions())
            {
                if (t.IsTriggered(g, e))
                {
                    triggeredTransition = t;
                    break;
                }
            }

            if (triggeredTransition != null)
            {
                State targetState = triggeredTransition.GetTargetState();
                UnityEngine.Debug.Log($"targetState={targetState.name}");
                List<IAction> actions = new List<IAction>();
                foreach (IAction a in currentState.GetExitAction())
                {
                    actions.Add(a);
                }
                foreach (IAction a in triggeredTransition.GetActions())
                {
                    actions.Add(a);
                }
                foreach (IAction a in targetState.GetEntryAction())
                {
                    actions.Add(a);
                }

                currentState = targetState;
                return actions;
            }
            else
            {
                return currentState.GetAction();
            }
        }
    }

}

