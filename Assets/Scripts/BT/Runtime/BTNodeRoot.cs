using CopyBT;
using GraphProcessor;
using UnityEngine;

namespace BehaviorTree.Runtime
{
    public class BTNodeRoot : BTNode
    {
        [Output("", false), Vertical]
        public ENodeStatus output;
        public override Color color => Color.green;
    }
}

