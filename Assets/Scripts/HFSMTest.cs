using HFSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HFSMTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StateMachine root = new StateMachine("Root");

        StateMachine sub = new StateMachine("L");
        sub.AddState("L.x", new StateBase("L.x"));
        sub.AddState("L.y", new StateBase("L.y"));
        sub.SetInitState("L.x");

        root.AddState("L", sub);
        root.AddState("M", new StateBase("M"));

        root.SetInitState("L");

        root.AddTrasition("L.x", "M");

        root.Enter();

        for (int i = 0; i < 100; i++)
        {
            root.OnLogic();
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}
