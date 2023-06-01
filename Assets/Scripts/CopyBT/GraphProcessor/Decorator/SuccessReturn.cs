using GraphProcessor;

namespace BT.GraphProcessor
{
    [System.Serializable, NodeMenuItem("BT/Decorator/SuccessReturn")]
    public class SuccessReturn : DecoratorNode
    {
        protected override void OnVisit()
        {
            BehaviourNode child = ChildAtIndex(0);
            child.Visit();
            if (child.status != CopyBT.ENodeStatus.RUNNING)
            {
                status = CopyBT.ENodeStatus.SUCCESS;
            }
        }
    }
}
