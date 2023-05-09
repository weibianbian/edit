using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using Unity.VisualScripting;
using Newtonsoft.Json.Linq;

public class FSMComponentGraph : MonoBehaviour
{
    public FSMGraph rootGraph;
    public FSMComponent compt;

    // Start is called before the first frame update
    void Start()
    {
       
    }
    public void GenerateFSM(Agent agent)
    {
        compt = new FSMComponent(agent);
        agent.fsmCompt = compt;
        compt.fsm = CreateFSMFromGraph();
        compt.fsm.Init();

        //JObject obj = new JObject();
        //compt.fsm.WriteJson(obj);
        //Debug.Log(obj.ToString());
    }

    // Update is called once per frame
    void Update()
    {

    }
    public StateMachine CreateFSMFromGraph()
    {
        return rootGraph.CreateFSMFromGraph(this) as StateMachine;
    }
}
public class FSMComponent
{
    public StateMachine fsm;
    public Agent agent;

    public FSMComponent(Agent agent)
    {
        this.agent = agent;
    }

    public void Update()
    {
        if (fsm != null)
        {
            fsm.OnLogic();
        }
    }
}
