using System;

namespace CopyBT
{
    public class ConditionNode : BehaviourNode
    {
        Func<bool> fn;
        public ConditionNode(Func<bool> fn, string name = "ConditionNode") : base(name)
        {
            this.fn = fn;
        }
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

