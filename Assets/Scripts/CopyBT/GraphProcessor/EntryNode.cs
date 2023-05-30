using GraphProcessor;
using UnityEngine;

namespace CopyBT.GraphProcessor
{
    [System.Serializable, NodeMenuItem("BT/EntryNode")]
    public class EntryNode : BehaviourNode
    {
        [Output("", false), Vertical]
        public ENodeStatus output;
        public override Color color => Color.green;
    }
}
