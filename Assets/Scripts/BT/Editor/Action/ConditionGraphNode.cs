using BT.Runtime;
using GraphProcessor;

namespace BT.Editor
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
