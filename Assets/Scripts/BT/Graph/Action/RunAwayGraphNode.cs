using BT.Runtime;

namespace BT.Graph
{
    [System.Serializable, TreeNodeMenuItem("BT/Action/RunAway")]
    public class RunAwayGraphNode : ActionGraphNode
    {
        public RunAwayGraphNode()
        {
            classData = typeof(BTRunAwayAction);
        }
    }
}
