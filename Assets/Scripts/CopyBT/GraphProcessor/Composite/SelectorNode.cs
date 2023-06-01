using CopyBT;
using GraphProcessor;

namespace BT.GraphProcessor
{
    [System.Serializable, NodeMenuItem("BT/Composite/Selector")]
    public class SelectorNode : CompositieNode
    {
        public override void Reset()
        {
            base.Reset();
            idx = 0;
        }
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
