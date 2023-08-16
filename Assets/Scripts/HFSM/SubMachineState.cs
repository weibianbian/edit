using RailShootGame;
using System.Collections.Generic;

namespace HFSMRuntime
{
    public class SubMachineState : HierarchicalStateMachine
    {
        protected State state=new State();
        public HierarchicalStateMachine hfsm;

        public SubMachineState(UWorld g) : base(g)
        {
        }

        public override List<IAction> GetActions()
        {
            return state.GetAction();
        }
        public override UpdateResult Update(UWorld g, Actor e)
        {
            return base.Update(g, e);
        }
        public override List<State> GetStates()
        {
            List<State> states = new List<State>();
            if (currentState != null)
            {
                states.Add(state);
                foreach (State s in currentState.GetStates())
                    states.Add(s);
            }
            else
                states.Add(state);
            return states;
        }
        public override string ToString()
        {
            if (currentState != null)
            {
                return base.currentState.name;
            }
            else return "NULL SMS";
        }
    }
}

