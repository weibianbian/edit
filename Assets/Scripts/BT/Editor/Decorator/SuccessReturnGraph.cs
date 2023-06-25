using BT.Runtime;
using GraphProcessor;

namespace BT.Editor
{
    [System.Serializable, NodeMenuItem("BT/Decorator/SuccessReturn")]
    public class SuccessReturnGraph : DecoratorGraphNode
    {
        protected override void OnVisit()
        {
            BehaviourGraphNode child = ChildAtIndex(0);
            child.Visit();
            if (child.status != ENodeStatus.RUNNING)
            {
                status = ENodeStatus.SUCCESS;
            }
        }
    }
}
