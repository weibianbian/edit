﻿using GraphProcessor;
using UnityEngine;

namespace BehaviorTree.Runtime
{
    public class BTNodeRoot : BTNode
    {
        [Output("", false), Vertical]
        public ENodeResult output;
        public override Color color => Color.green;
    }
}

