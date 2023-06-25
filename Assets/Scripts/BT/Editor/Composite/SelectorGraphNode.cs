using BT.Runtime;
using GraphProcessor;

namespace BT.Editor
{
    [System.Serializable, NodeMenuItem("BT/Composite/Selector")]
    public class SelectorGraphNode : CompositieGraphNode
    {
        public override string name => "选择结点";
        protected override void OnVisit()
        {
            if (status != ENodeStatus.RUNNING)
            {
                idx = 0;
            }
            bool done = false;
            while (idx < ChildCount)
            {
                BehaviourGraphNode child = ChildAtIndex(idx);
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
