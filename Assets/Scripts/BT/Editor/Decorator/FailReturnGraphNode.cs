using BT.Runtime;
using GraphProcessor;

namespace BT.Editor
{
    [System.Serializable, NodeMenuItem("BT/Decorator/FailReturn")]
    public class FailReturnGraphNode : DecoratorGraphNode
    {
        public FailReturnGraphNode()
        {
            classData = typeof(FailReturnGraphNode);
        }
        protected override void OnVisit()
        {
            BehaviourGraphNode child= ChildAtIndex(0);
            child.Visit();
            status = ENodeStatus.FAILED;
        }
    }
}
