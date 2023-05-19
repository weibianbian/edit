using System;
using System.Collections.Generic;

namespace CopyBT
{
    public class ParallelNode : BehaviourNode
    {
        public ParallelNode(List<BehaviourNode> children, string name = "ParallelNode") : base(name, children)
        {

        }
        public override void Visit()
        {
            bool done = true;
            bool anyDone = false;
            for (int i = 0; i < children.Count; i++)
            {
                BehaviourNode child = children[i];
                if (child is ConditionNode)
                {
                    child.Reset();
                }
                if (child.status != ENodeStatus.SUCCESS)
                {
                    child.Visit();
                    if (child.status == ENodeStatus.FAILED)
                    {
                        status = ENodeStatus.FAILED;
                        return;
                    }
                }
                if (child.status == ENodeStatus.RUNNING)
                {
                    done = false;
                }
                else
                {
                    anyDone = true;
                }
            }
            if (done)
            {
                status = ENodeStatus.SUCCESS;
            }
            else
            {
                status = ENodeStatus.RUNNING;
            }
        }
    }
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

