using System;
using System.Collections.Generic;
using UnityEngine;

namespace HFSM
{
    public class StateBase
    {
        public class UpdateResult
        {
            public List<FSMAction> actions;
            public Transition transition;
            public int level = 0;
        }
        public string name;
        public StateBase(string name)
        {
            this.name = name;
        }
        public virtual List<StateBase> GetStates()
        {
            return new List<StateBase>();
        }
        public virtual List<FSMAction> GetActions()
        {
            return new List<FSMAction>(0);
        }
        public virtual List<FSMAction> GetEntryActions()
        {
            return new List<FSMAction>(0);
        }
        public virtual List<FSMAction> GetExitActions()
        {
            return new List<FSMAction>(0);
        }
        public virtual List<Transition> GetTransitions()
        {
            return new List<Transition>(0);
        }

        public virtual UpdateResult Update()
        {
            UpdateResult result = new UpdateResult();
            result.actions = GetActions();
            result.transition = null;
            result.level = 0;
            return result;
        }
    }
}

