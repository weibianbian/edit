using BehaviorTree.Runtime;
using BT.GraphProcessor;
using GraphProcessor;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
public class BTManager : MonoBehaviour
{
    public BehaviorTreeGraph btGraph;
    public EntryNode entry;
    private CopyBT.ENodeStatus lastResult = CopyBT.ENodeStatus.READY;
    // Start is called before the first frame update
    void Start()
    {
        entry = btGraph.nodes.Find(e => e is EntryNode) as EntryNode;
        Find(entry);
    }
    public void Find(BaseNode node)
    {
        List<SerializableEdge> edges = SortChildren(node);

        for (int i = 0; i < edges.Count; i++)
        {
            Find(edges[i].inputNode);
        }
        return;
    }
    private List<SerializableEdge> SortChildren(BaseNode node)
    {
        if (node.outputPorts.Count > 0)
        {
            var edges = node.outputPorts[0].GetEdges();
            edges.Sort((e1, e2) => e1.inputNode.position.x < e2.inputNode.position.x ? -1 : 1);
            return edges;
        }
        return new List<SerializableEdge>();
    }
    // Update is called once per frame
    void Update()
    {
        if (entry != null && lastResult == CopyBT.ENodeStatus.READY || lastResult == CopyBT.ENodeStatus.RUNNING)
        {
            entry.Visit();
            entry.SaveStatus();
            entry.Step();
            lastResult = entry.lastResult;
        }
    }
}
