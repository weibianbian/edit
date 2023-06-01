using GraphProcessor;
using Sirenix.OdinInspector.Editor.Drawers;

namespace BT.GraphProcessor
{
    [System.Serializable, NodeMenuItem("BT/Decorator/FailReturn")]
    public class FailReturn : DecoratorNode
    {
        protected override void OnVisit()
        {
            BehaviourNode child= ChildAtIndex(0);
            child.Visit();
            status = CopyBT.ENodeStatus.FAILED;
        }
    }
}
