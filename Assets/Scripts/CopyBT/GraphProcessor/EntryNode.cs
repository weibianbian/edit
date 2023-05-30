using GraphProcessor;
using UnityEngine;

namespace CopyBT.GraphProcessor
{
    [System.Serializable, NodeMenuItem("BT/EntryNode")]
    public class EntryNode : PriorityNode
    {
        public override Color color => Color.green;

    }
}
