using System.Collections.Generic;
using System.Linq;

namespace HFSM
{
    public class SubMachineState : HierarchicalStateMachine
    {
        protected State state=new State();
        public HierarchicalStateMachine hfsm;

        public override List<IAction> GetActions()
        {
            return state.GetAction();
        }
        public override UpdateResult Update(Game g, Entity e)
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
    }
}

