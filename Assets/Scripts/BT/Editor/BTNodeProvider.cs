using BT.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Graphs;
using UnityEngine;

namespace BT.Editor
{
    public static class BTNodeProvider
    {
        public struct PortDescription
        {
            public Type nodeType;
            public Type portType;
            public bool isInput;
            public string portFieldName;
            public string portIdentifier;
            public string portDisplayName;
        }
        public class NodeDescriptions
        {
            public Dictionary<string, Type> nodePerMenuTitle = new Dictionary<string, Type>();
        }
        static NodeDescriptions genericNodes = new NodeDescriptions();

        static BTNodeProvider()
        {
            BuildGenericNodeCache();
        }
        public static void LoadGraph()
        {
            BuildGenericNodeCache();
        }
        static void BuildGenericNodeCache()
        {
            foreach (var nodeType in TypeCache.GetTypesDerivedFrom<BTNode>())
            {
                if (!IsNodeAccessibleFromMenu(nodeType))
                    continue;
                BuildCacheForNode(nodeType, genericNodes);
            }
        }
        static bool IsNodeAccessibleFromMenu(Type nodeType)
        {
            if (nodeType.IsAbstract)
                return false;

            return nodeType.GetCustomAttributes<TreeNodeMenuItemAttribute>().Count() > 0;
        }
        static void BuildCacheForNode(Type nodeType, NodeDescriptions targetDescription)
        {
            var attrs = nodeType.GetCustomAttributes(typeof(TreeNodeMenuItemAttribute), false) as TreeNodeMenuItemAttribute[];

            if (attrs != null && attrs.Length > 0)
            {
                foreach (var attr in attrs)
                    targetDescription.nodePerMenuTitle[attr.menuTitle] = nodeType;
            }
        }
        public static IEnumerable<(string path, Type type)> GetNodeMenuEntries()
        {
            foreach (var node in genericNodes.nodePerMenuTitle)
                yield return (node.Key, node.Value);
        }
        //public static IEnumerable<PortDescription> GetEdgeCreationNodeMenuEntry(NodePortView portView)
        //{
        //    foreach (var description in genericNodes.nodeCreatePortDescription)
        //    {
        //        if (!IsPortCompatible(description))
        //            continue;

        //        yield return description;
        //    }

        //    bool IsPortCompatible(PortDescription description)
        //    {
        //        if ((portView.direction == Direction.Input && description.isInput) || (portView.direction == Direction.Output && !description.isInput))
        //            return false;
        //        if (!BTTypeUtils.TypesAreConnectable(description.portType, portView.portType))
        //            return false;
        //        return true;
        //    }
        //}
        public static Type GetNodeViewTypeFromType(Type nodeType)
        {
            if (nodeType.IsSubclassOf(typeof(BTTaskNode)))
            {
                return typeof(BehaviorGraphNodeActionView);
            }
            else if (nodeType.IsSubclassOf(typeof(BTCompositeNode)))
            {
                return typeof(BehaviorGraphNodeCompositeView);
            }
            return typeof(BehaviorGraphNodeView); ;
        }
    }
    public class BTGraphNodeCreator<T> where T : BehaviorGraphNodeView
    {
        public T node;
        BehaviorTreeGraphView graph;
        public BTGraphNodeCreator(BehaviorTreeGraphView inGraph)
        {
            graph = inGraph;
        }
        public T CreateNode()
        {
            node = graph.CreateNode(typeof(T)) as T;
            return node;
        }
        public void OnFinalize()
        {
            node.PostPlacedNewNode();
        }
    }
}

