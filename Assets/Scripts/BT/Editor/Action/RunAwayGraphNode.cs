using BT.Runtime;
using GraphProcessor;

namespace BT.Editor
{
    [System.Serializable, NodeMenuItem("BT/Action/RunAway")]
    public class RunAwayGraphNode : ActionGraphNode
    {
        public RunAwayGraphNode()
        {
            classData = typeof(BTRunAwayAction);
        }
    }
}
