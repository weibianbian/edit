using System.Collections.Generic;

namespace CopyBT
{
    public class ParallelNode : BehaviourNode
    {
        public ParallelNode(List<BehaviourNode> children, string name = "ParallelNode") : base(name, children)
        {

        }
        public override void Step()
        {
            if (status != ENodeStatus.RUNNING)
            {
                Reset();
            }
            else
            {
                if (children != null)
                {
                    for (int i = 0; i < children.Count; i++)
                    {
                        if (children[i].status == ENodeStatus.SUCCESS && children[i] is ConditionNode)
                        {
                            children[i].Reset();
                        }
                    }
                }
            }
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
}

