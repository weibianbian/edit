using BT.Runtime;
using GraphProcessor;

namespace BT.Editor
{
    [System.Serializable, NodeMenuItem("BT/Decorator/FailReturn")]
    public class FailReturn : DecoratorNode
    {
        protected override void OnVisit()
        {
            BehaviourNode child= ChildAtIndex(0);
            child.Visit();
            status = ENodeStatus.FAILED;
        }
    }
}
