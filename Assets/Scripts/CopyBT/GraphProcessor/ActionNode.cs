using System;

namespace CopyBT.GraphProcessor
{
    public class ActionNode : BehaviourNode
    {
        public Action action;
        public override void Visit()
        {
            action.Invoke();
            status = ENodeStatus.SUCCESS;
        }
    }
}
