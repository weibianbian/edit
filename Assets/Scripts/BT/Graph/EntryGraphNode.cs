using BT.Runtime;
using UnityEngine;

namespace BT.Graph
{
    [System.Serializable, TreeNodeMenuItem("BT/EntryNode")]
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

        protected override void OnVisit()
        {
            var child = ChildAtIndex(0);
            child.Visit();
            status = child.status;
        }
    }
}
