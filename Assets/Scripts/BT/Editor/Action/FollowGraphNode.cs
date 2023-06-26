using BT.Runtime;
using GraphProcessor;

namespace BT.Editor
{
    [System.Serializable, NodeMenuItem("BT/Action/Follow")]
    public class FollowGraphNode : ActionGraphNode
    {
        public FollowGraphNode()
        {
            classData = typeof(BTFollowAction);
        }
        protected override void OnVisit()
        {
            base.OnVisit();
        }
    }
}
