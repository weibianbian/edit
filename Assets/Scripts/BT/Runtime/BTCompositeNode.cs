using GraphProcessor;
using UnityEngine;

namespace BehaviorTree.Runtime
{
    public class BTCompositeNode: BTNode
    {
        [Input(name = "", allowMultiple=false), Vertical]
        public ENodeResult input;
        [Output("", true), Vertical]
        public ENodeResult output;
    }
}

