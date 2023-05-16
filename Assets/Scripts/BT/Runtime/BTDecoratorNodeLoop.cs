using GraphProcessor;

namespace BehaviorTree.Runtime
{
    [System.Serializable, NodeMenuItem("BT/Decorator/Loop")]
    public class BTDecoratorNodeLoop: BTDecoratorNode
    {
        public int loopTime = 0;
        public int i = 0;
    }
}

