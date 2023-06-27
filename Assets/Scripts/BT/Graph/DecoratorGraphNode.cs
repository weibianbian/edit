using BT.Runtime;
using UnityEngine;

namespace BT.Graph
{

    public class DecoratorGraphNode : BehaviorGraphNode
    {
        [Input(name = "", allowMultiple = false), Vertical]
        public BehaviorGraphNode input;
        [Output("", false), Vertical]
        public BehaviorGraphNode output;
        public override Color color => new Color(0.7f, 0.3f, 0.1f);
    }
}
