using FSMRuntime;
using HFSMRuntime;
using RailShootGame;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class HFSMTest : MonoBehaviour
{
    public Game game;
    public Actor entity;
    public HierarchicalStateMachine hfsm;
    public TimeSpan initialTime;
    public string curString = "";
    void Start()
    {
        State l = new State(EStatus.Patrol.ToString(), null);

        State m = new State(EStatus.Combat.ToString(), null);

        l.AddTransition(new Transition(new SoundSensorCondition(), m, 0));
        game = new Game();
        entity = new Actor();
        hfsm = new HierarchicalStateMachine(game, l, m);
    }

    void Update()
    {
        UpdateResult ret = hfsm.Update(game, entity);
        curString = hfsm.ToString();
        foreach (IAction action in ret.actions)
        {
            if (action!=null)
            {
                action.Execute(game, entity);
            }
        }
    }
}
