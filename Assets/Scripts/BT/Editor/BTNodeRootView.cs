using BT.GraphProcessor;
using GraphProcessor;

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

