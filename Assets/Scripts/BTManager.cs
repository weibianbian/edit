using BehaviorTree.Runtime;
using GraphProcessor;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public class BTManager : MonoBehaviour
{
    public BehaviorTreeGraph btGraph;
    public BTNodeRoot root;
    // Start is called before the first frame update
    void Start()
    {
        root= btGraph.nodes.Find(e => e is BTNodeRoot) as BTNodeRoot;
        for (int i = 0; i < root.outputPorts.Count; i++)
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
        SerializableEdge edge = root.outputPorts[0].GetEdges()[0];
    }
    public void RequestExecution(BTCompositeNode requestedOn,int requestedIndex,BTNode requestedBy)
    {

    }
    // Update is called once per frame
    void Update()
    {
    }
}
