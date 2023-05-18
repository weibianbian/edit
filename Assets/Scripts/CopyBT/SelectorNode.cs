using System.Collections.Generic;

namespace CopyBT
{
    public class SelectorNode : BehaviourNode
    {
        public SelectorNode(string name, List<BehaviourNode> children) : base(name, children)
        {
            idx = 0;
            this.name = "SelectorNode";
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
                if (child.status == ENodeStatus.RUNNING || child.status == ENodeStatus.SUCCESS)
                {
                    status = child.status;
                    return;
                }
                idx++;
            }
            status = ENodeStatus.FAILED;
        }
    }
}

