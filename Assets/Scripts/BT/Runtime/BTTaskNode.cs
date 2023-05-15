using GraphProcessor;
using UnityEngine;

namespace BehaviorTree.Runtime
{
    
    public class BTTaskNode : BTNode
    {
        [Input(name = ""), Vertical]
        public ENodeResult input;
        public override Color color => new Color(104/255f, 50/255f,170/255f);
    }
    [System.Serializable, NodeMenuItem("BT/Task/MoveTo")]
    public class BTTaskMoveTo : BTTaskNode
    {

    }
}

