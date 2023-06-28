using BT.Runtime;

namespace BT.Graph
{
    [System.Serializable, TreeNodeMenuItem("BT/Action/Coondition")]
    public class ConditionGraphNode : ActionGraphNode
    {
        public ConditionGraphNode()
        {
            classData = typeof(BTConditionAction);
        }
    }
}
