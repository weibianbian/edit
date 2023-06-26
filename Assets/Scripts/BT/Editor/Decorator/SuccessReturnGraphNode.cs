using BT.Runtime;
using GraphProcessor;

namespace BT.Editor
{
    [System.Serializable, NodeMenuItem("BT/Decorator/SuccessReturn")]
    public class SuccessReturnGraphNode : DecoratorGraphNode
    {
        public SuccessReturnGraphNode()
        {
            classData = typeof(SuccessReturnGraphNode);
        }
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
