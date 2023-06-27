using BT.Runtime;
using GraphProcessor;

namespace BT.Graph
{
    [System.Serializable, NodeMenuItem("BT/Action/Coondition")]
    public class ConditionGraphNode : ActionGraphNode
    {
        public ConditionGraphNode()
        {
            classData = typeof(BTConditionAction);
        }
    }
}
