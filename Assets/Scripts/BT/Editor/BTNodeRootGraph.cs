using BehaviorTree.Runtime;
using GraphProcessor;
using UnityEditor;

namespace BehaviorTree.Editor
{
    [NodeCustomEditor(typeof(BTNodeRoot))]
    public class BTNodeRootGraph: BTNodeGraphView
    {
        public override void Enable()
        {
            base.Enable();
            
        }
    }
}

