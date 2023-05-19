﻿using System.Collections.Generic;

namespace CopyBT
{
    public class SelectorNode : BehaviourNode
    {
        public SelectorNode(List<BehaviourNode> children) : base("SelectorNode", children)
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

