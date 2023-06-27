using BT.Runtime;
using GraphProcessor;

namespace BT.Graph
{
    [System.Serializable, NodeMenuItem("BT/Action/FaceEntity")]
    public class FaceEntityGraphNode : ActionGraphNode
    {
        public FaceEntityGraphNode()
        {
            classData = typeof(BTFaceEntityAction);
        }
    }
}
