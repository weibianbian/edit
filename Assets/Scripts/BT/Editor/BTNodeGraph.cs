using BehaviorTree.Runtime;
using GraphProcessor;
using UnityEditor;

namespace BehaviorTree.Editor
{
    [CustomEditor(typeof(BTNode))]
    public class BTNodeGraph : BaseNodeView
    {

    }
    [CustomEditor(typeof(BTCompositeNodeRoot))]
    public class BTNodeRootGraph: BTNodeGraph
    {
        public override void Enable()
        {
            base.Enable();
            nodeTarget.SetCustomName("Root");
            title = nodeTarget.GetCustomName();
        }
    }
}

