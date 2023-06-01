using CopyBT;
using GraphProcessor;
using UnityEngine;

namespace BT.GraphProcessor
{

    public class DecoratorNode : BehaviourNode
    {
        [Input(name = "", allowMultiple = false), Vertical]
        public ENodeStatus input;
        [Output("", false), Vertical]
        public ENodeStatus output;
        public override Color color => new Color(0.7f, 0.3f, 0.1f);
    }
}
