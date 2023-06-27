using BT.Runtime;
using UnityEngine;

namespace BT.Graph
{

    public class DecoratorGraphNode : BehaviourGraphNode
    {
        [Input(name = "", allowMultiple = false), Vertical]
        public BehaviourGraphNode input;
        [Output("", false), Vertical]
        public BehaviourGraphNode output;
        public override Color color => new Color(0.7f, 0.3f, 0.1f);
    }
}
