using GraphProcessor;
using System.Collections.Generic;

namespace CopyBT.GraphProcessor
{
    [System.Serializable, NodeMenuItem("BT/CompositeNode/ParallelNode")]
    public class ParallelNode : BehaviourNode
    {
        public override void Step()
        {
            if (status != ENodeStatus.RUNNING)
            {
                Reset();
            }
            else
            {
                for (int i = 0; i < ChildCount; i++)
                {
                    if (ChildAtIndex(i).status == ENodeStatus.SUCCESS && ChildAtIndex(i) is ConditionNode)
                    {
                        ChildAtIndex(i).Reset();
                    }
                }
            }
        }
        public override void Visit()
        {
            bool done = true;
            bool anyDone = false;
            for (int i = 0; i < ChildCount; i++)
            {
                BehaviourNode child = ChildAtIndex(i);
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
}
