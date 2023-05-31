using BehaviorTree.Runtime;
using BT.GraphProcessor;
using GraphProcessor;
using UnityEngine;
public class BTManager : MonoBehaviour
{
    public BehaviorTreeGraph btGraph;
    public EntryNode entry;
    // Start is called before the first frame update
    void Start()
    {
        entry = btGraph.nodes.Find(e => e is EntryNode) as EntryNode;
        for (int i = 0; i < entry.outputPorts.Count; i++)
        {
            //NodePort port = root.outputPorts[i];
            //List < SerializableEdge > edges = port.GetEdges();
            //edges.Sort((e1, e2) => e1.inputNode.position.x < e2.inputNode.position.x ? -1 : 1);
            //for (int j = 0; j< edges.Count; j++)
            //{
            //    SerializableEdge edge = edges[j];
            //    Debug.Log(edge);
            //}
        }
        SerializableEdge edge = entry.outputPorts[0].GetEdges()[0];
        BaseNode node = edge.inputPort.owner;
        Debug.Log($"根节点的第一个子节点 {node}");

        Find(node);
    }
    public void Find(BaseNode node)
    {
        foreach (var outNode in node.GetOutputNodes())
        {
            {
                Debug.Log($"节点信息=父节点 {node}   子节点  {outNode}={outNode}");
                Find(outNode);
            }
        }
        return;
    }
    // Update is called once per frame
    void Update()
    {
        if (entry != null)
        {
            entry.Visit();
            entry.SaveStatus();
            entry.Step();
        }
    }
}
