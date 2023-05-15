using BehaviorTree.Runtime;
using GraphProcessor;
using UnityEditor;

namespace BehaviorTree.Editor
{
    [CustomEditor(typeof(BTNode))]
    public class BTNodeGraph : BaseNodeView
    {
        public override void Enable()
        {
            style.height = 140;
            base.Enable();
           
        }
    }
    [CustomEditor(typeof(BTNodeRoot))]
    public class BTNodeRootGraph: BTNodeGraph
    {
        public override void Enable()
        {
            base.Enable();
            
        }
    }
}

