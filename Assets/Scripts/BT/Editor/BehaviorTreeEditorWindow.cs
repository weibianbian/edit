using GraphProcessor;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace BehaviorTree.Editor
{
    public class BehaviorTreeEditorWindow: BaseGraphWindow
    {
        [MenuItem("Window/01_DefaultGraph")]
        public static BaseGraphWindow Open()
        {
            var graphWindow = GetWindow<BehaviorTreeEditorWindow>();

            graphWindow.Show();

            return graphWindow;
        }
        protected override void InitializeWindow(BaseGraph graph)
        {
            // Set the window title
            titleContent = new GUIContent("Default Graph");

            // Here you can use the default BaseGraphView or a custom one (see section below)
            var graphView = new BaseGraphView(this);

            rootView.Add(graphView);
        }
    }

}

