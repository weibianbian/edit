using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

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
            graphView.initialized += () => {
                AddButtons();
            };
            this.StretchToParentSize();
        }
        protected void AddButtons()
        {
            AddButton("Center",null);
            //AddButton(new GUIContent("Blackboard", "黑板"),
            //    () =>
            //    {
            //        Selection.activeObject = (graphView as BehaviorTreeGraphView).BlackboardInspector;
            //    }, false);
        }
        protected void AddButton(string name, Action callback, bool left = true)
            => AddButton(new GUIContent(name), callback, left);

        protected void AddButton(GUIContent content, Action callback, bool left = true)
        {
            var centerButton = new ToolbarButton(() => { });
            centerButton.style.unityTextAlign = TextAnchor.MiddleLeft;
            centerButton.text = "Center";
            Add(centerButton);
        }
        //public override void UpdateButtonStatus()
        //{
        //    base.UpdateButtonStatus();
        //    //if (showNodeInspector != null)
        //    //    showNodeInspector.value = graphView.GetPinnedElementStatus<NodeInspectorView>() != UnityEngine.UIElements.DropdownMenuAction.Status.Hidden;
        //}
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

