using GraphProcessor;
using Plugins.Examples.Editor.BaseGraph;
using UnityEditor.Callbacks;
using UnityEditor;
using UnityEngine;
namespace BehaviorTree.Editor
{
    public class GraphAssetCallbacks
    {
        [MenuItem("Assets/Create/GraphProcessor_BehaviourTree", false, 10)]
        public static void GraphProcessor_BehaviourTree()
        {
            var graph = ScriptableObject.CreateInstance<BehaviorTreeGraph>();
            ProjectWindowUtil.CreateAsset(graph, "BehaviorTreeGraph.asset");
        }
        [OnOpenAsset(1)]
        public static bool OnBaseGraphOpened(int instanceID, int line)
        {
            var baseGraph = EditorUtility.InstanceIDToObject(instanceID) as BaseGraph;
            return InitializeGraph(baseGraph);
        }

        public static bool InitializeGraph(BaseGraph baseGraph)
        {
            if (baseGraph == null) return false;

            switch (baseGraph)
            {
                case BehaviorTreeGraph btGraph:
                    EditorWindow.GetWindow<BehaviorTreeGrahpWindow>().InitializeGraph(btGraph);
                    break;
                default:
                    break;
            }

            return true;
        }
    }
}

