using GraphProcessor;
using UnityEngine;

namespace BehaviorTree.Runtime
{
    public abstract class BTDecoratorNode : BTNode
    {
        [Input(name = "", allowMultiple = false), Vertical]
        public ENodeResult input;
        [Output("", allowMultiple = false), Vertical]
        public ENodeResult output;
        public override Color color => new Color(0, 53/255f, 131/255f);
    }
}

