using BT.Runtime;
using GraphProcessor;

namespace BT.Editor
{
    [System.Serializable, NodeMenuItem("BT/Decorator/SuccessReturn")]
    public class SuccessReturn : DecoratorNode
    {
        protected override void OnVisit()
        {
            BehaviourNode child = ChildAtIndex(0);
            child.Visit();
            if (child.status != ENodeStatus.RUNNING)
            {
                status = ENodeStatus.SUCCESS;
            }
        }
    }
}
