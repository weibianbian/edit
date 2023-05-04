using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class myFSM : MonoBehaviour
{
    [ShowInInspector]
    public MeStateMachine fsm;
    // Start is called before the first frame update
    void Start()
    {
        fsm=new MeStateMachine();
        fsm.AddState("a",new MeStateBase());
        fsm.AddState("b", new MeStateBase());
        fsm.AddTransition("a","b");
        fsm.Init();


    }

    // Update is called once per frame
    void Update()
    {
        fsm.OnLogic();
    }
}
