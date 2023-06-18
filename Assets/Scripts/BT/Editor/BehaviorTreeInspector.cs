using BehaviorTree.Runtime;
using GraphProcessor;
using UnityEditor;
using UnityEngine.UIElements;

namespace BehaviorTree.Editor
{
    [CustomEditor(typeof(BehaviorTreeGraph))]
    public class BehaviorTreeInspector : GraphInspector
    {

        protected override void CreateInspector()
        {
            base.CreateInspector();

            root.Add(new Button(() => EditorWindow.GetWindow<BehaviorTreeGrahpWindow>().InitializeGraph(target as BehaviorTreeGraph))
            {
                text = "Open"
            });
        }
    }

}

