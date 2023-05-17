using System.Collections.Generic;

namespace HFSM
{
    public class Transition
    {
        public int level = 0;
        public StateBase from;
        public StateBase to;

        public Condition condition;
        public Transition(StateBase from, StateBase to,int level)
        {
            this.from = from;
            this.to = to;
            this.level = level;
        }
        public bool IsTriggered()
        {
            return condition.Test();
        }
        public StateBase GetTargetState()
        {
            return to;
        }
        public virtual List<FSMAction> GetActions()
        {
            return new List<FSMAction>(0);
        }
        public int GetLevel()
        {
            return this.level;
        }
    }
}

