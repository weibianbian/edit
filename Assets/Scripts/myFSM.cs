using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class myFSM : MonoBehaviour
{
    [ShowInInspector]
    [TypeFilter(nameof(GetActionList))]
    public MeStateMachine fsm;
    // Start is called before the first frame update
    void Start()
    {
        fsm = new MeStateMachine();
        fsm.AddState("a", new MeStateBase());
        fsm.AddState("b", new MeStateBase());
        fsm.AddTransition("a", "b");
        fsm.Init();


    }
    private IEnumerable<Type> GetActionList()
    {
        var q = typeof(MeStateMachine).Assembly.GetTypes()
            .Where(x => !x.IsAbstract)
            .Where(x => !x.IsGenericTypeDefinition)
            .Where(x => typeof(MeStateMachine).IsAssignableFrom(x));

        return q;
    }
    // Update is called once per frame
    void Update()
    {
        fsm.OnLogic();
    }
}
