using HFSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HFSMTest : MonoBehaviour
{
    Game game;
    Entity entity;
    FiniteStateMachine fsm;
    void Start()
    {
        State l = new State("L", null);
        State m = new State("M", null);
        State n = new State("N", null);
        fsm = new FiniteStateMachine(l, m, n);
        game = new Game();
        entity = new Entity();
    }

    // Update is called once per frame
    void Update()
    {
        List<IAction> actions = fsm.UpdateFSM(game, entity);
    }
}
