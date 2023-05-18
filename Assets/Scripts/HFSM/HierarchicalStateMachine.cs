using System.Collections.Generic;

namespace HFSM
{
    public class HierarchicalStateMachine : State
    {
        List<State> states = new List<State>();
        State initialState;
        protected State currentState;
        public override List<State> GetStates()
        {
            if (currentState != null)
            {
                return currentState.GetStates();
            }
            else
            {
                return new List<State>();
            }
        }
        public override UpdateResult Update(Game g, Entity e)
        {
            UpdateResult result;
            if (currentState == null)
            {
                currentState = initialState;
                result = new UpdateResult() { actions = currentState.GetEntryAction(), transition = null, level = 0 };
                return result;
            }
            Transition triggeredTransition = null;
            for (int i = 0; i < currentState.GetTransitions().Count; i++)
            {
                if (currentState.GetTransitions()[i].IsTriggered(g, e))
                {
                    triggeredTransition = currentState.GetTransitions()[i];
                    break;
                }
            }

            if (triggeredTransition != null)
            {
                result = new UpdateResult();
                result.actions = new List<IAction>(0);
                result.transition = triggeredTransition;
                result.level = triggeredTransition.GetLevel();
            }
            else
            {
                if (currentState is SubMachineState)
                    result = ((SubMachineState)currentState).Update(g, e);
                else
                    result = currentState.Update(g, e);
            }
            if (result.transition != null)
            {
                State targetState;
                if (result.level == 0)
                {
                    targetState = result.transition.GetTargetState();
                    foreach (IAction a in currentState.GetExitAction())
                        result.actions.Add(a);
                    foreach (IAction a in result.transition.GetActions())
                        result.actions.Add(a);
                    foreach (IAction a in targetState.GetEntryAction())
                        result.actions.Add(a);

                    currentState = targetState;

                    foreach (IAction a in GetActions())
                        result.actions.Add(a);

                    result.transition = null;
                }
                else if (result.level > 0)
                {
                    foreach (IAction a in currentState.GetExitAction())
                        result.actions.Add(a);
                    currentState = null;

                    result.level -= 1;

                }
                else
                {
                    targetState = result.transition.GetTargetState();
                    HierarchicalStateMachine targetMachine = targetState.parent;
                    foreach (IAction a in result.transition.GetActions())
                        result.actions.Add(a);
                    foreach (IAction a in targetMachine.UpdateDown(targetState, -result.level - 1))
                        result.actions.Add(a);

                    result.transition = null;
                }
                result.transition = null;
            }
            else
            {
                foreach (IAction a in GetActions())
                    result.actions.Add(a);
            }

            return result;
        }
        public List<IAction> UpdateDown(State state, int level)
        {
            List<IAction> actions;
            if (level > 0)
                actions = parent.UpdateDown(this, level - 1);
            else actions = new List<IAction>();

            if (currentState != null)
                foreach (IAction a in currentState.GetExitAction())
                    actions.Add(a);

            currentState = state;
            foreach (IAction a in state.GetEntryAction())
                actions.Add(a);

            return actions;
        }
    }
}

