using CopyBT;
using GraphProcessor;
using UnityEngine;

namespace BehaviorTree.Runtime
{
    [System.Serializable, NodeMenuItem("BT/Root")]
    public class BTNodeRoot : BTNode
    {
        [Output("", false), Vertical]
        public ENodeStatus output;
        public override Color color => Color.green;
    }
}

