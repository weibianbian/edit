using BT.Graph;
using BT.Runtime;
using GraphProcessor;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace BT.Editor
{
    public class BehaviorTreeGraphView : GraphView
    {
        BTCreateNodeMenuWindow createNodeMenu;
        EditorWindow window;
        public event Action initialized;
        public BehaviorTreeGraphView(EditorWindow window) : base()
        {
            this.window = window;
            createNodeMenu = ScriptableObject.CreateInstance<BTCreateNodeMenuWindow>();
            createNodeMenu.Initialize(this, window);
            this.StretchToParentSize();
        }
        public void Initialize(BehaviorTree graph)
        {
            InitializeGraphView();

            initialized?.Invoke();
        }
        void InitializeGraphView()
        {
            nodeCreationRequest += OpenSearchWindow;
        }
        void OpenSearchWindow(NodeCreationContext c)
        {
            if (EditorWindow.focusedWindow == window)
            {
                SearchWindow.Open(new SearchWindowContext(c.screenMousePosition), createNodeMenu);
            }
        }
        public IEnumerable<(string path, Type type)> FilterCreateNodeMenuEntries()
        {
            foreach (var nodeMenuItem in TreeNodeProvider.GetNodeMenuEntries())
                yield return nodeMenuItem;
        }
        public BehaviorGraphNodeView AddNode(BehaviorGraphNode node)
        {
            return null;
        }
        public BehaviorGraphNodeView AddNodeView(BehaviorGraphNode node)
        {
            var viewType = TreeNodeProvider.GetNodeViewTypeFromType(node.GetType());

            if (viewType == null)
                viewType = typeof(BehaviorGraphNodeView);

            var baseNodeView = Activator.CreateInstance(viewType) as BehaviorGraphNodeView;
            baseNodeView.Initialize(this, node);
            AddElement(baseNodeView);

            nodeViews.Add(baseNodeView);
            nodeViewsPerNode[node] = baseNodeView;

            return baseNodeView;
        }
    }
}

