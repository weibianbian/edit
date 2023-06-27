using BT.Runtime;

namespace BT.Graph
{
    [System.Serializable, TreeNodeMenuItem("BT/Action/TurnToward")]
    public class TurnTowardGraphNode : ActionGraphNode
    {
        public TurnTowardGraphNode()
        {
            classData = typeof(BTTurnTowardAction);
        }
    }
}
