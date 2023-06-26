using BT.Runtime;
using GraphProcessor;

namespace BT.Editor
{
    [System.Serializable, NodeMenuItem("BT/Action/TurnToward")]
    public class TurnTowardGraphNode : ActionGraphNode
    {
        public TurnTowardGraphNode()
        {
            classData = typeof(BTTurnTowardAction);
        }
    }
}
