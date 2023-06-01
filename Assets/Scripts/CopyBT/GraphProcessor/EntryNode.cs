using CopyBT;
using GraphProcessor;
using System.Threading.Tasks;
using UnityEngine;

namespace BT.GraphProcessor
{
    [System.Serializable, NodeMenuItem("BT/EntryNode")]
    public class EntryNode : BehaviourNode
    {
        [Output("", false), Vertical]
        public ENodeStatus output;
        public override Color color => Color.green;

        protected override void OnVisit()
        {
            var child = ChildAtIndex(0);
            child.Visit();
            status = child.status;
        }
    }
}
