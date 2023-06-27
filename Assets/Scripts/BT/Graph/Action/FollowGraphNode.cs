using BT.Runtime;

namespace BT.Graph
{
    [System.Serializable, TreeNodeMenuItem("BT/Action/Follow")]
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
