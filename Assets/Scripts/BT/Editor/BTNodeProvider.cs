using BT.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
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
            public List<PortDescription> nodeCreatePortDescription = new List<PortDescription>();
        }
        static NodeDescriptions genericNodes = new NodeDescriptions();

        static BTNodeProvider()
        {
            BuildGenericNodeCache();
        }
        public static void LoadGraph()
        {

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

            ProvideNodePortCreationDescription(nodeType, targetDescription);
        }
        static void ProvideNodePortCreationDescription(Type nodeType, NodeDescriptions targetDescription)
        {
            var node = Activator.CreateInstance(nodeType) as BTNode;
            try
            {
                //SetGraph.SetValue(node, graph);
                //node.InitializePorts();
                //node.UpdateAllPorts();
            }
            catch (Exception) { }

            foreach (var p in node.inputPorts)
                AddPort(p, true);
            foreach (var p in node.outputPorts)
                AddPort(p, false);

            void AddPort(NodePort p, bool input)
            {
                targetDescription.nodeCreatePortDescription.Add(new PortDescription
                {
                    nodeType = nodeType,
                    portType = p.portData.displayType ?? p.fieldInfo.FieldType,
                    isInput = input,
                    portFieldName = p.fieldName,
                    portDisplayName = p.portData.displayName ?? p.fieldName,
                    portIdentifier = p.portData.identifier,
                });
            }
        }
        public static IEnumerable<(string path, Type type)> GetNodeMenuEntries()
        {
            foreach (var node in genericNodes.nodePerMenuTitle)
                yield return (node.Key, node.Value);
        }
        public static IEnumerable<PortDescription> GetEdgeCreationNodeMenuEntry(NodePortView portView)
        {
            foreach (var description in genericNodes.nodeCreatePortDescription)
            {
                if (!IsPortCompatible(description))
                    continue;

                yield return description;
            }

            bool IsPortCompatible(PortDescription description)
            {
                if ((portView.direction == Direction.Input && description.isInput) || (portView.direction == Direction.Output && !description.isInput))
                    return false;
                return true;
            }
        }
        public static Type GetNodeViewTypeFromType(Type nodeType)
        {
            if (nodeType == typeof(BTEntryNode))
            {
                return typeof(BehaviorGraphNodeRootView);
            }
            else if(nodeType.IsSubclassOf(typeof(BTActionNode)))
            {
                return typeof(BehaviorGraphNodeActionView);
            }
            return typeof(BehaviorGraphNodeView); ;
        }
    }

}

