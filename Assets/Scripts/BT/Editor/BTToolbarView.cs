using BT.Runtime;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using static Codice.CM.WorkspaceServer.WorkspaceTreeDataStore;

namespace BT.Editor
{
    public class BTToolbarView : Toolbar
    {
        protected enum ElementType
        {
            Button,
            Toggle,
            DropDownButton,
            Separator,
            Custom,
            FlexibleSpace,
        }

        public BTToolbarView(BehaviorTreeGraphView graphView)
        {
            graphView.initialized += () =>
            {
                AddButtons();
            };
            this.StretchToParentSize();
        }
        protected void AddButtons()
        {
            AddButton("Center", () => { }, TextAnchor.MiddleLeft);
            AddButton("CreateBehaviorTree", CreateBehaviorTree, TextAnchor.MiddleLeft);
        }
        protected void AddButton(string content, Action callback, TextAnchor anchor)
        {
            var btn = new ToolbarButton(callback);
            btn.style.unityTextAlign = TextAnchor.MiddleLeft;
            btn.text = content;
            Add(btn);
        }
        //public override void UpdateButtonStatus()
        //{
        //    base.UpdateButtonStatus();
        //    //if (showNodeInspector != null)
        //    //    showNodeInspector.value = graphView.GetPinnedElementStatus<NodeInspectorView>() != UnityEngine.UIElements.DropdownMenuAction.Status.Hidden;
        //}
        public void CreateBehaviorTree()
        {
            Debug.Log("CreateBehaviorTree");
            BehaviorTree tree=new BehaviorTree();

        }
        protected virtual void DrawImGUIToolbar()
        {
            //GUILayout.BeginHorizontal(EditorStyles.toolbar);

            //DrawImGUIButtonList(leftButtonDatas);

            //GUILayout.FlexibleSpace();

            //DrawImGUIButtonList(rightButtonDatas);

            //GUILayout.EndHorizontal();
        }
    }
}

