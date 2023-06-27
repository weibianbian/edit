using BT.Runtime;
using UnityEngine;

namespace BT.Graph
{
    public class CompositieGraphNode : BehaviorGraphNode
    {
        [Input(name = "", allowMultiple = false), Vertical]
        public BehaviorGraphNode input;
        [Output("", true), Vertical]
        public BehaviorGraphNode output;
        public override Color color => new Color(0.1f, 0.3f, 0.7f);
        public override void Reset()
        {
            base.Reset();
            idx = 0;
        }
    }
}
