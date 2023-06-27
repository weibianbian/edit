using BT.Runtime;

namespace BT.Graph
{
    [System.Serializable, TreeNodeMenuItem("BT/Decorator/Repeat")]
    public class RepeatGraphNode : DecoratorGraphNode
    {
        public RepeatGraphNode()
        {
            classData = typeof(RepeatGraphNode);
        }
        public override string name => "重复节点";
        protected override void OnVisit()
        {
            var child = ChildAtIndex(0);
            child.Visit();
            status = ENodeStatus.RUNNING;
        }
    }
}
