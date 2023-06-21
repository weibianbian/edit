using BT.Editor;
using GraphProcessor;
using UnityEditor;
using UnityEngine;

namespace BT.Editor
{
    public class BTToolbarView : ToolbarView
    {
        ToolbarButtonData showNodeInspector;
        
        public BTToolbarView(BaseGraphView graphView) : base(graphView)
        {

        }
        protected override void AddButtons()
        {
            base.AddButtons();
            AddButton(new GUIContent("Blackboard", "黑板"),
                () =>
                {
                    Selection.activeObject = (graphView as BehaviorTreeGraphView).BlackboardInspector;
                }, false);
        }
        public override void UpdateButtonStatus()
        {
            base.UpdateButtonStatus();
            //if (showNodeInspector != null)
            //    showNodeInspector.value = graphView.GetPinnedElementStatus<NodeInspectorView>() != UnityEngine.UIElements.DropdownMenuAction.Status.Hidden;
        }
    }
}

