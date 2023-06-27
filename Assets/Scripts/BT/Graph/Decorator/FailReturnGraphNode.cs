using BT.Runtime;

namespace BT.Graph
{
    [System.Serializable, TreeNodeMenuItem("BT/Decorator/FailReturn")]
    public class FailReturnGraphNode : DecoratorGraphNode
    {
        public FailReturnGraphNode()
        {
            classData = typeof(FailReturnGraphNode);
        }
        protected override void OnVisit()
        {
            BehaviorGraphNode child= ChildAtIndex(0);
            child.Visit();
            status = ENodeStatus.FAILED;
        }
    }
}
