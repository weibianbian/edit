using System.Collections.Generic;

namespace CopyBT
{
    public class SequenceNode : CopyBTBehaviourNode
    {
        public SequenceNode(List<CopyBTBehaviourNode> children) : base("SequenceNode", children)
        {
            idx = 0;
        }
        public override void Visit()
        {
            if (status != ENodeStatus.RUNNING)
            {
                idx = 0;
            }
            bool done = false;
            while (idx < children.Count)
            {
                CopyBTBehaviourNode child = children[idx];
                child.Visit();
                if (child.status == ENodeStatus.RUNNING || child.status == ENodeStatus.FAILED)
                {
                    status = child.status;
                    return;
                }
                idx++;  
            }
            status = ENodeStatus.SUCCESS;
        }
    }
}

