using System.Collections.Generic;
using UnityEngine.UI;
using static HFSM.StateMachine;

namespace HFSM
{
    public class HierarchicalStateMachine : StateBase
    {
        public Dictionary<string, StateBase> states = new Dictionary<string, StateBase>();
        public StateBase initState = null;
        public StateBase currentState = null;
        public HierarchicalStateMachine(string name) : base(name)
        {
        }
        public override List<StateBase> GetStates()
        {
            if (currentState != null)
            {
                return currentState.GetStates();
            }
            else
            {
                return new List<StateBase>();
            }
        }
        public List<FSMAction> Update()
        {
            if (currentState == null)
            {

                currentState = initState;
                return currentState.GetEntryActions();
            }
            Transition triggeredTransition = null;
            for (int i = 0; i < currentState.GetTransitions().Count; i++)
            {
                if (currentState.GetTransitions()[i].IsTriggered())
                {
                    triggeredTransition = currentState.GetTransitions()[i];
                    break;
                }
            }
            UpdateResult result = null;
            if (triggeredTransition != null)
            {
                result = new UpdateResult();
                result.actions = new List<FSMAction>(0);
                result.transition = triggeredTransition;
                result.level = triggeredTransition.GetLevel();
            }
            else
            {
                result = currentState.Update();
            }
            if (result.transition!=null)
            {

            }
        }
    }
    public class FSMAction
    {

    }
}

