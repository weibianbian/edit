using UnityEngine;
using Newtonsoft.Json;
public class FSMComponentGraph : MonoBehaviour
{
    public FSMGraph rootGraph;
    public FSMComponent compt;

    // Start is called before the first frame update
    void Start()
    {
        Agent agent= new Agent();
        compt = new FSMComponent(agent);
        agent.fsmCompt = compt;
        OnSave();
    }

    // Update is called once per frame
    void Update()
    {
        compt.Update();
    }
    public void OnSave()
    {
        compt.root = CreateFSMFromGraph();
        compt.root.Init();
    }

    public StateMachine CreateFSMFromGraph()
    {
        return rootGraph.CreateFSMFromGraph(this) as StateMachine;
    }
}
public class FSMComponent
{
    public StateMachine root;
    public Agent agent;

    public FSMComponent(Agent agent)
    {
        this.agent = agent;
    }

    public void Update()
    {
        if (root != null)
        {
            root.OnLogic();
        }
    }
}
