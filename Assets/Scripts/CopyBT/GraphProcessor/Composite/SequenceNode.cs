using CopyBT;
using GraphProcessor;
using System.Collections.Generic;

namespace BT.GraphProcessor
{
    [System.Serializable, NodeMenuItem("BT/Composite/Sequence")]
    public class SequenceNode : CompositieNode
    {
        protected override void OnVisit()
        {
            if (status != ENodeStatus.RUNNING)
            {
                idx = 0;
            }
            bool done = false;
            while (idx < ChildCount)
            {
                BehaviourNode child = ChildAtIndex(idx);
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
