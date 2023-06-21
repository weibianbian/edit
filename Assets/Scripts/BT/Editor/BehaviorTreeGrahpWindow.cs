using GraphProcessor;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace BehaviorTree.Editor
{
   
    public class BehaviorTreeGrahpWindow: BaseGraphWindow
    {
        UnityEditor.Experimental.GraphView.Blackboard xxxxxxx;
        protected override void OnEnable()
        {
            base.OnEnable();

            titleContent = new GUIContent("BehaviorTree Graph",
                AssetDatabase.LoadAssetAtPath<Texture2D>($"{GraphCreateAndSaveHelper.NodeGraphProcessorPathPrefix}/Editor/Icon_Dark.png"));
            m_HasInitGUIStyles = false;
        }
      
        private bool m_HasInitGUIStyles;
        protected override void InitializeWindow(BaseGraph graph)
        {
            var graphView = new BehaviorTreeGraphView(this);
            rootView.Add(graphView);

            graphView.Add(new BTToolbarView(graphView));

            Debug.LogError("InitializeWindow");
        }
        private void OnGUI()
        {
            InitGUIStyles(ref m_HasInitGUIStyles);

        }
        private void InitGUIStyles(ref bool result)
        {
            if (!result)
            {
                EditorGUIStyleHelper.SetGUIStylePadding(nameof(EditorStyles.toolbarButton), new RectOffset(15, 15, 0, 0));
                result = true;
            }
        }

    }

}

