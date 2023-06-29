﻿using BT.Runtime;
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
        public BehaviorTree treeAsset;
        public event Action initialized;

        public List<BehaviorGraphNodeView> nodeViews=new List<BehaviorGraphNodeView>();
        public BehaviorTreeGraphView(EditorWindow window) : base()
        {
            this.window = window;

            InitializeManipulators();
            //实现放大或者缩小
            SetupZoom(0.05f, 2f);

            GridBackground gridBackground = new GridBackground();
            Insert(0,gridBackground);
            createNodeMenu = ScriptableObject.CreateInstance<BTCreateNodeMenuWindow>();
            createNodeMenu.Initialize(this, window);
            this.StretchToParentSize();
        }
        protected virtual void InitializeManipulators()
        {
            ///添加拖拽
            this.AddManipulator(new SelectionDragger());
            ///添加框选
            this.AddManipulator(new RectangleSelector());
            //添加点击选择
            this.AddManipulator(new ClickSelector());
            //添加区域选择
            this.AddManipulator(new ContentDragger());
        }
        public void Initialize()
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
            foreach (var nodeMenuItem in BTNodeProvider.GetNodeMenuEntries())
                yield return nodeMenuItem;
        }
        public void AddNode(BehaviorGraphNodeView node)
        {
            //var viewType = BTNodeProvider.GetNodeViewTypeFromType(node.GetType());

            //if (viewType == null)
            //    viewType = typeof(BehaviorGraphNodeView);

            //var baseNodeView = Activator.CreateInstance(viewType) as BehaviorGraphNodeView;
            //baseNodeView.Initialize(this, node);
            AddElement(node);

            nodeViews.Add(node);
            //nodeViewsPerNode[node] = baseNodeView;
        }
        
    }
}

