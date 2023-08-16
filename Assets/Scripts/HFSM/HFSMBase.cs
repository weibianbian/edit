using RailShootGame;
using System.Collections.Generic;

namespace HFSMRuntime
{
    public class HFSMBase
    {
        public HierarchicalStateMachine parent;
        public virtual List<State> GetStates()
        {
            return new List<State>(0);
        }
        public virtual List<IAction> GetActions()
        {
            return new List<IAction>(0);
        }
        public virtual UpdateResult Update(UWorld g, Actor e)
        {
            UpdateResult result = new UpdateResult();
            result.actions = GetActions();
            result.transition = null;
            result.level = 0;
            return result;
        }
    }
}

