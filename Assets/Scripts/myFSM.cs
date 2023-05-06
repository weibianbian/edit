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
    public StateMachine fsm;
    // Start is called before the first frame update
    void Start()
    {
        fsm = new StateMachine();
        fsm.AddState("a", new StateBase());
        fsm.AddState("b", new StateBase());
        fsm.AddTransition("a", "b");
        fsm.Init();


    }
    private IEnumerable<Type> GetActionList()
    {
        var q = typeof(StateMachine).Assembly.GetTypes()
            .Where(x => !x.IsAbstract)
            .Where(x => !x.IsGenericTypeDefinition)
            .Where(x => typeof(StateMachine).IsAssignableFrom(x));

        return q;
    }
    // Update is called once per frame
    void Update()
    {
        fsm.OnLogic();
    }
}
