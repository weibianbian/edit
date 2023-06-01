using CopyBT;
using GraphProcessor;

namespace BT.GraphProcessor
{
    [System.Serializable, NodeMenuItem("BT/Decorator/Loop")]
    public class Loop : DecoratorNode
    {
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
