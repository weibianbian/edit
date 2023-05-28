using BehaviorTree.Runtime;
using GraphProcessor;
using UnityEditor;

namespace BehaviorTree.Editor
{
    [NodeCustomEditor(typeof(BTNodeRoot))]
    public class BTNodeRootView: BTNodeGraphView
    {
        public override void Enable()
        {
            base.Enable();
            
        }
    }
}

