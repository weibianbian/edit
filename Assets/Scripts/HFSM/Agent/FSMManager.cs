using UnityEngine;
using Newtonsoft.Json;
public class FSMManager : MonoBehaviour
{
    public FSMGraph rootGraph;
    public StateMachine root;
    // Start is called before the first frame update
    void Start()
    {
        OnSave();
    }

    // Update is called once per frame
    void Update()
    {
        //if (root != null)
        //{
        //    root.OnLogic();
        //}
    }
    public void OnSave()
    {
        root = CreateFSMFromGraph();
        //root.Init();
    }

    public StateMachine CreateFSMFromGraph()
    {
        return rootGraph.CreateFSMFromGraph() as StateMachine;
    }
}
