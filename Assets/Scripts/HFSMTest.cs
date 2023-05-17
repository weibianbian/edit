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
        sub.AddState("L.A", new StateBase("L.A"));
        sub.AddState("L.B", new StateBase("L.B"));
        sub.AddState("L.C", new StateBase("L.C"));
        sub.SetInitState("L.A");

        root.AddState("L", sub);
        root.AddState("M", new StateBase("M"));

        root.SetInitState("L");

        root.AddTrasition("L.A", "M",1);

        root.Enter();

        for (int i = 0; i < 100; i++)
        {
            root.Update();
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}
