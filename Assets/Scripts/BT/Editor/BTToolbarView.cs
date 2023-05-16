using GraphProcessor;

namespace BehaviorTree.Editor
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
            bool nodeInspectorVisible = graphView.GetPinnedElementStatus<NodeInspectorView>() != UnityEngine.UIElements.DropdownMenuAction.Status.Hidden;
            showNodeInspector = AddToggle("Node Inspector", nodeInspectorVisible, (v) => graphView.ToggleView<NodeInspectorView>());
        }
        public override void UpdateButtonStatus()
        {
            base.UpdateButtonStatus();
            if (showNodeInspector != null)
                showNodeInspector.value = graphView.GetPinnedElementStatus<ProcessorView>() != UnityEngine.UIElements.DropdownMenuAction.Status.Hidden;
        }
    }
}

