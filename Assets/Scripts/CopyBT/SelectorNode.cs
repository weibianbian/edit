using System.Collections.Generic;

namespace CopyBT
{
    public class LoopNode : CopyBTBehaviourNode
    {
        public LoopNode(string name) : base(name)
        {
        }
    }
    public class SelectorNode : CopyBTBehaviourNode
    {
        public SelectorNode(List<CopyBTBehaviourNode> children) : base("SelectorNode", children)
        {
            idx = 0;
        }
        public override string DBString()
        {
            return idx.ToString();
        }
        public override void Reset()
        {
            base.Reset();
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

