using BT.GraphProcessor;
using GraphProcessor;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace BehaviorTree.Editor
{
    public class BehaviorTreeGraphView : BaseGraphView
    {
        public Blackboard BlackboardInspector
        {
            get
            {
                if (_BlackboardInspectorViewer == null)
                {
                    _BlackboardInspectorViewer = ScriptableObject.CreateInstance<Blackboard>();
                }

                return _BlackboardInspectorViewer;
            }
        }
        private Blackboard _BlackboardInspectorViewer;
        public BehaviorTreeGraphView(EditorWindow window) : base(window)
        {
        }
        protected void BuildStackNodeContextualMenu(ContextualMenuPopulateEvent evt)
        {
            evt.menu.AppendSeparator();
            Vector2 position =
                (evt.currentTarget as VisualElement).ChangeCoordinatesTo(contentViewContainer, evt.localMousePosition);
            evt.menu.AppendAction("New Stack", (e) => AddStackNode(new BaseStackNode(position)),
                DropdownMenuAction.AlwaysEnabled);
        }
        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            BuildStackNodeContextualMenu(evt);
            base.BuildContextualMenu(evt);
        }
    }
}

