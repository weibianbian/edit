using GraphProcessor;
using System.Collections.Generic;

namespace CopyBT.GraphProcessor
{
    [System.Serializable, NodeMenuItem("BT/CompositeNode/Sequence")]
    public class SequenceNode : BehaviourNode
    {
        [Input(name = "", allowMultiple = false), Vertical]
        public ENodeResult input;
        [Output("", true), Vertical]
        public ENodeResult output;
        public override void Visit()
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
