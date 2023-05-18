using System.Collections.Generic;

namespace HFSM
{
    public struct UpdateResult
    {
        public List<IAction> actions;
        public Transition transition;
        public int level;
        public UpdateResult(List<IAction> actions, Transition trans, int lvl)
        {
            this.actions = actions;
            this.transition = trans;
            this.level = lvl;
        }
        public UpdateResult(List<IAction> actions)
        {
            this.actions = actions;
            this.transition = null;
            this.level = 0;
        }
    }
}

