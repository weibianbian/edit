using BT.Runtime;
using GraphProcessor;
using UnityEngine;

namespace BT.Editor
{
    [System.Serializable, NodeMenuItem("BT/EntryNode")]
    public class EntryGraphNode : BehaviourGraphNode
    {
        [Output("", false), Vertical]
        public BehaviourGraphNode output;
        public override string name => "入口节点";
        public override Color color => Color.green;
        public EntryGraphNode()
        {
            classData = typeof(BTEntryNode);
        }
        protected override void Enable()
        {
            base.Enable();
        }

        protected override void OnVisit()
        {
            var child = ChildAtIndex(0);
            child.Visit();
            status = child.status;
        }
    }
}
