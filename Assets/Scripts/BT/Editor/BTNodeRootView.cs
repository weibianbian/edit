using BehaviorTree.Runtime;
using CopyBT.GraphProcessor;
using GraphProcessor;
using UnityEditor;

namespace BehaviorTree.Editor
{
    [NodeCustomEditor(typeof(EntryNode))]
    public class BTNodeRootView: BehaviourNodeView
    {
        public override void Enable()
        {
            base.Enable();
            
        }
    }
}

