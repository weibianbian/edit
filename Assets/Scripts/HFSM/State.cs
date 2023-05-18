using System;
using System.Collections.Generic;

namespace HFSM
{
    public class State : HFSMBase
    {
        public List<IAction> actions, entryActions, exitActions;
        List<Transition> transitions;
        public string name;
        public State()
        {
            actions = new List<IAction>();
            entryActions = new List<IAction>();
            exitActions = new List<IAction>();
            transitions = new List<Transition>();
        }
        public State(string name, IAction entryAction, IAction action, IAction exitAction)
             : this()
        {
            this.name = name;
            entryActions.Add(entryAction);
            actions.Add(action);
            exitActions.Add(exitAction);
        }
        public State(string name, IAction action)
             : this()
        {
            this.name = name;
            actions.Add(action);
        }
        public State(string name, IAction action1, IAction action2)
             : this()
        {
            this.name = name;
            actions.Add(action1);
            actions.Add(action2);
        }
        public override UpdateResult Update(Game g, Entity e)
        {
            return new UpdateResult() { actions = GetAction(), transition = null, level = 0 };
        }
        public override List<State> GetStates()
        {
            List<State> states = base.GetStates();
            states.Add(this);
            return states;
        }
        public void AddActions(params IAction[] actions)
        {
            for (int i = 0; i < actions.Length; ++i)
            {
                this.actions.Add(actions[i]);
            }
        }
        public void AddAction(IAction action)
        {
            this.actions.Add(action);
        }
        public List<IAction> GetAction()
        {
            return actions;
        }
        public List<IAction> GetEntryAction()
        {
            return entryActions;
        }
        public List<IAction> GetExitAction()
        {
            return exitActions;
        }
        public List<Transition> GetTransitions()
        {
            return transitions;
        }
        public void AddTransition(params Transition[] t)
        {
            foreach (Transition tran in t)
            {
                transitions.Add(tran);
            }
        }
       
    }
}

