using BT.Runtime;
using GraphProcessor;

namespace BT.Editor
{
    [System.Serializable, NodeMenuItem("BT/Decorator/Repeat")]
    public class Repeat : DecoratorNode
    {
        public override string name => "重复节点";
        protected override void OnVisit()
        {
            var child = ChildAtIndex(0);
            child.Visit();
            status = ENodeStatus.RUNNING;
        }
    }
}
