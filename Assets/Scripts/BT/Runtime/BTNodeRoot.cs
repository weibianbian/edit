using GraphProcessor;
using UnityEngine;

namespace BehaviorTree.Runtime
{
    [System.Serializable, NodeMenuItem("BT/Root")]
    public class BTNodeRoot : BTNode
    {
        [Output("", true), Vertical]
        public ENodeResult output;
        public override Color color => Color.green;
    }
}

