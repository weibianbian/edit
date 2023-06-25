using BT.Runtime;
using GraphProcessor;

namespace BT.Editor
{
    [System.Serializable, NodeMenuItem("BT/Decorator/FailReturn")]
    public class FailReturnGraph : DecoratorGraphNode
    {
        protected override void OnVisit()
        {
            BehaviourGraphNode child= ChildAtIndex(0);
            child.Visit();
            status = ENodeStatus.FAILED;
        }
    }
}
