using GraphProcessor;

namespace CopyBT.GraphProcessor
{
    [System.Serializable, NodeMenuItem("BT/Decorator/DecoratorNode")]
    public class DecoratorNode : BehaviourNode
    {
        [Input(name = "", allowMultiple = false), Vertical]
        public ENodeStatus input;
        [Output("", true), Vertical]
        public ENodeStatus output;
    }
}
