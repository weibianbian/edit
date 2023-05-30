using System;

namespace CopyBT.GraphProcessor
{
    public class ConditionNode : BehaviourNode
    {
        Func<bool> fn;
        public override void Visit()
        {
            if (fn())
            {
                status = ENodeStatus.SUCCESS;
            }
            else
            {
                status = ENodeStatus.FAILED;
            }

        }
    }
}
