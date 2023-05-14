using GraphProcessor;
using UnityEngine;

namespace BehaviorTree.Runtime
{
    [System.Serializable, NodeMenuItem("BT/TaskNode")]
    public class BTTaskNode : BTNode
    {
        [Input(name = ""), Vertical]
        public ENodeResult input;
    }
}

