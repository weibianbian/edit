using UnityEngine;

namespace BT.GraphProcessor
{
    public class EntryNodeData : NodeDataBase
    {

    }
    [System.Serializable]
    public class MoveToActionData: NodeDataBase
    {
        public Vector3 target;
    }
}
