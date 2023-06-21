using CopyBT;
using GraphProcessor;
using UnityEngine;

namespace BT.Editor
{

    public class DecoratorNode : BehaviourNode
    {
        [Input(name = "", allowMultiple = false), Vertical]
        public BehaviourNode input;
        [Output("", false), Vertical]
        public BehaviourNode output;
        public override Color color => new Color(0.7f, 0.3f, 0.1f);
    }
}
