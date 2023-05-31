using CopyBT;
using GraphProcessor;
using UnityEngine;

namespace BT.GraphProcessor
{
    public class CompositieNode : BehaviourNode
    {
        [Input(name = "", allowMultiple = false), Vertical]
        public ENodeStatus input;
        [Output("", true), Vertical]
        public ENodeStatus output;
        public override Color color => new Color(0.1f, 0.3f, 0.7f);
    }
}
