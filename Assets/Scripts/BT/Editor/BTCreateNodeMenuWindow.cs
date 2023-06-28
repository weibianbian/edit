using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using System.Linq;
using System;
using BT.Runtime;
using BT.Graph;

namespace BT.Editor
{
    class BTCreateNodeMenuWindow : ScriptableObject, ISearchWindowProvider
    {
        BehaviorTreeGraphView graphView;
        EditorWindow window;
        public void Initialize(BehaviorTreeGraphView graphView, EditorWindow window)
        {
            this.graphView = graphView;
            this.window = window;
        }
        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            var tree = new List<SearchTreeEntry>
            {
                new SearchTreeGroupEntry(new GUIContent("Create Node"), 0),
            };

            //if (edgeFilter == null)
            CreateStandardNodeMenu(tree);
            //else
            //    CreateEdgeNodeMenu(tree);

            return tree;
        }

        public bool OnSelectEntry(SearchTreeEntry SearchTreeEntry, SearchWindowContext context)
        {
            var windowRoot = window.rootVisualElement;
            var windowMousePosition = windowRoot.ChangeCoordinatesTo(windowRoot.parent, context.screenMousePosition - window.position.position);
            var graphMousePosition = graphView.contentViewContainer.WorldToLocal(windowMousePosition);

            var nodeType = SearchTreeEntry.userData is Type ? (Type)SearchTreeEntry.userData : ((BTNodeProvider.PortDescription)SearchTreeEntry.userData).nodeType;
            //graphView.RegisterCompleteObjectUndo("Added " + nodeType);
            BehaviorGraphNodeView nodeView=new  BehaviorGraphNodeView();
            graphView.AddNode(nodeView);
            nodeView.classData = new GraphNodeClassData();
            nodeView.classData.classType = nodeType;
            nodeView.PostPlaceNewNode();
            nodeView.SetPosition(new Rect(graphMousePosition, new Vector2(100, 100)));
            //BTNode node= BehaviorGraphNode.CreateFromType(nodeType, graphMousePosition);
            //nodeView.position = ;
            return true;
        }

        public static BehaviorGraphNode CreateFromType(Type nodeType, Vector2 position)
        {
            if (!nodeType.IsSubclassOf(typeof(BehaviorGraphNode)))
                return null;

            var node = Activator.CreateInstance(nodeType) as BehaviorGraphNode;

            //node.position = new Rect(position, new Vector2(100, 100));

            node.OnNodeCreated();

            return node;
        }

        void CreateStandardNodeMenu(List<SearchTreeEntry> tree)
        {
            // Sort menu by alphabetical order and submenus
            var nodeEntries = graphView.FilterCreateNodeMenuEntries().OrderBy(k => k.path);
            var titlePaths = new HashSet<string>();

            foreach (var nodeMenuItem in nodeEntries)
            {
                var nodePath = nodeMenuItem.path;
                var nodeName = nodePath;
                var level = 0;
                var parts = nodePath.Split('/');

                if (parts.Length > 1)
                {
                    level++;
                    nodeName = parts[parts.Length - 1];
                    var fullTitleAsPath = "";

                    for (var i = 0; i < parts.Length - 1; i++)
                    {
                        var title = parts[i];
                        fullTitleAsPath += title;
                        level = i + 1;

                        if (!titlePaths.Contains(fullTitleAsPath))
                        {
                            tree.Add(new SearchTreeGroupEntry(new GUIContent(title))
                            {
                                level = level
                            });
                            titlePaths.Add(fullTitleAsPath);
                        }
                    }
                }

                tree.Add(new SearchTreeEntry(new GUIContent(nodeName))
                {
                    level = level + 1,
                    userData = nodeMenuItem.type
                });
            }
        }
    }
}

