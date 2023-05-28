using CopyBT;
using GraphProcessor;
using UnityEngine;

namespace BehaviorTree.Runtime
{
    public abstract class BTDecoratorNode : BTNode
    {
        [Input(name = "", allowMultiple = false), Vertical]
        public ENodeStatus input;
        [Output("", allowMultiple = false), Vertical]
        public ENodeStatus output;
        public override Color color => new Color(0, 53/255f, 131/255f);
    }
}

