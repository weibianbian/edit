using BT.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace BT.Editor
{
    public class BTCreateNodeMenuWindow : ScriptableObject, ISearchWindowProvider
    {
        BehaviorTreeGraphView graphView;
        EditorWindow window;
        EdgeView edgeFilter;
        NodePortView inputPortView;
        NodePortView outputPortView;
        public void Initialize(BehaviorTreeGraphView graphView, EditorWindow window, EdgeView edgeFilter = null)
        {
            this.graphView = graphView;
            this.window = window;
            this.edgeFilter = edgeFilter;
            this.inputPortView = edgeFilter?.input as NodePortView;
            this.outputPortView = edgeFilter?.output as NodePortView;
        }
        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            var tree = new List<SearchTreeEntry>
            {
                new SearchTreeGroupEntry(new GUIContent("Create Node"), 0),
            };

            if (edgeFilter == null)
                CreateStandardNodeMenu(tree);
            else
                CreateEdgeNodeMenu(tree);

            return tree;
        }

        public bool OnSelectEntry(SearchTreeEntry searchTreeEntry, SearchWindowContext context)
        {
            var windowRoot = window.rootVisualElement;
            var windowMousePosition = windowRoot.ChangeCoordinatesTo(windowRoot.parent, context.screenMousePosition - window.position.position);
            var graphMousePosition = graphView.contentViewContainer.WorldToLocal(windowMousePosition);

            var nodeType = searchTreeEntry.userData is Type ? (Type)searchTreeEntry.userData : ((BTNodeProvider.PortDescription)searchTreeEntry.userData).nodeType;
            //graphView.RegisterCompleteObjectUndo("Added " + nodeType);
            var nodeViewType = BTNodeProvider.GetNodeViewTypeFromType(nodeType);
            var nodeView = graphView.AddNode(nodeType, nodeViewType);
            nodeView.PostPlaceNewNode();
            nodeView.SetPosition(new Rect(graphMousePosition, new Vector2(200, 200)));

            if (searchTreeEntry.userData is BTNodeProvider.PortDescription desc)
            {
                if (inputPortView == null)
                {
                    graphView.Connect(nodeView.inputPortView, outputPortView);
                }
                else
                {
                    graphView.Connect(inputPortView, nodeView.outputPortView);
                }
            }
            return true;
        }

        void CreateEdgeNodeMenu(List<SearchTreeEntry> tree)
        {
            var nodeEntries = graphView.FilterCreateNodeMenuEntries().OrderBy(k => k.path);
            //var entries = BTNodeProvider.GetEdgeCreationNodeMenuEntry((edgeFilter.input ?? edgeFilter.output) as NodePortView);

            NodePortView portView = (edgeFilter.input ?? edgeFilter.output) as NodePortView;

            var nodePaths = BTNodeProvider.GetNodeMenuEntries();

            IOrderedEnumerable<((string, Type) port, string path)> sortedMenuItems = null;

            var titlePaths = new HashSet<string>();

            if (portView.direction == Direction.Output)
            {
                foreach (var nodeMenuItem in nodePaths)
                {
                    var nodePath = nodeMenuItem.path;
                    if (portView.node is BehaviorGraphNodeRootView && nodeMenuItem.type == typeof(BTEntryNode))
                    {
                        continue;
                    }
                    else if (portView.node is BehaviorGraphNodeCompositeView && nodeMenuItem.type == typeof(BTEntryNode))
                    {
                        continue;
                    }
                    else if (portView.node is BehaviorGraphNodeActionView)
                    {
                    }
                    // Ignore the node if it's not in the create menu
                    if (String.IsNullOrEmpty(nodePath))
                        continue;

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

                            // Add section title if the node is in subcategory
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

                    tree.Add(new SearchTreeEntry(new GUIContent($"{nodeName}"))
                    {
                        level = level + 1,
                        userData = new BTNodeProvider.PortDescription()
                        {
                            nodeType = nodeMenuItem.type,
                            isInput = false,
                        }
                    }); ;
                }
            }
            else
            {
                foreach (var nodeMenuItem in nodePaths)
                {
                    var nodePath = nodeMenuItem.path;
                    if (portView.node is BehaviorGraphNodeRootView && nodeMenuItem.type == typeof(BTEntryNode))
                    {
                        continue;
                    }
                    else if (portView.node is BehaviorGraphNodeCompositeView && nodeMenuItem.type.IsSubclassOf(typeof(BTActionNode)))
                    {
                        continue;
                    }
                    else if (portView.node is BehaviorGraphNodeActionView && nodeMenuItem.type.IsSubclassOf(typeof(BTActionNode)))
                    {
                        continue;
                    }
                    // Ignore the node if it's not in the create menu
                    if (String.IsNullOrEmpty(nodePath))
                        continue;

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

                            // Add section title if the node is in subcategory
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

                    tree.Add(new SearchTreeEntry(new GUIContent($"{nodeName}"))
                    {
                        level = level + 1,
                        userData = new BTNodeProvider.PortDescription()
                        {
                            nodeType = nodeMenuItem.type,
                            isInput = true,
                        }
                    }); ;
                }
            }
            // Sort menu by alphabetical order and submenus

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

