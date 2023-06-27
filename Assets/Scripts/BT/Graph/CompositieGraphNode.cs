using BT.Runtime;
using UnityEngine;

namespace BT.Graph
{
    public class CompositieGraphNode : BehaviourGraphNode
    {
        [Input(name = "", allowMultiple = false), Vertical]
        public BehaviourGraphNode input;
        [Output("", true), Vertical]
        public BehaviourGraphNode output;
        public override Color color => new Color(0.1f, 0.3f, 0.7f);
        public override void Reset()
        {
            base.Reset();
            idx = 0;
        }
    }
}
