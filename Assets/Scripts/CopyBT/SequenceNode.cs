using System.Collections.Generic;

namespace CopyBT
{
    public class SequenceNode : BehaviourNode
    {
        public SequenceNode(string name, List<BehaviourNode> children) : base(name, children)
        {
            idx = 0;
            this.name = "SequenceNode";
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
                BehaviourNode child = children[idx];
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

