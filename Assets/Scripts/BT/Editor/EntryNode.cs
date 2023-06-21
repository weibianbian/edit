using GraphProcessor;
using UnityEngine;

namespace BT.Editor
{
    [System.Serializable, NodeMenuItem("BT/EntryNode")]
    public class EntryNode : BehaviourNode
    {
        [Output("", false), Vertical]
        public BehaviourNode output;
        public override string name => "入口节点";
        public override Color color => Color.green;

        protected override void OnVisit()
        {
            var child = ChildAtIndex(0);
            child.Visit();
            status = child.status;
        }
    }
}
