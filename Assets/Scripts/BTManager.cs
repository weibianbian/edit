using BehaviorTree.Runtime;
using Unity.VisualScripting;
using UnityEngine;
public class BTManager : MonoBehaviour
{
    public BehaviorTreeGraph btGraph;
    public BTCompositeNodeRoot root;
    // Start is called before the first frame update
    void Start()
    {
        root= btGraph.nodes.Find(e => e is BTCompositeNodeRoot) as BTCompositeNodeRoot;
        root.outputPorts.ForEach((x) => { Debug.Log(x); });
    }

    // Update is called once per frame
    void Update()
    {
    }
}
