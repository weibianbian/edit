using BT.Runtime;

namespace BT.Graph
{
    [System.Serializable, TreeNodeMenuItem("BT/Action/FaceEntity")]
    public class FaceEntityGraphNode : ActionGraphNode
    {
        public FaceEntityGraphNode()
        {
            classData = typeof(BTFaceEntityAction);
        }
    }
}
