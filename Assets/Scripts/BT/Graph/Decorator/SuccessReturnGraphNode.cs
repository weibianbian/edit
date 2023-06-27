using BT.Runtime;

namespace BT.Graph
{
    [System.Serializable, TreeNodeMenuItem("BT/Decorator/SuccessReturn")]
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
