using BT.Runtime;

namespace BT.Graph
{
    [System.Serializable, TreeNodeMenuItem("BT/Decorator/Loop")]
    public class LoopGraphNode : DecoratorGraphNode
    {
        public LoopGraphNode()
        {
            classData = typeof(LoopGraphNode);
        }
        public int loop = 4;
        protected override void OnVisit()
        {
            var child = ChildAtIndex(0);
            child.Visit();
            if (child.status != ENodeStatus.RUNNING)
            {
                loop--;
                if (loop <= 0)
                {
                    status = child.status;
                    UnityEngine.Debug.Log(status);
                    return;
                }
            }
            status = ENodeStatus.RUNNING;
        }
        public override void Reset()
        {
            base.Reset();
            loop = 4;
        }
    }
}
