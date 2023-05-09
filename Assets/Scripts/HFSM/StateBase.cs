using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
public class StateBase: IJsonConvertible
{
    public bool needsExitTime = false;
    public bool isGhostState;
    public string name;
    public IStateMachine fsm;
    public FSMComponent compt;
    public StateBase(FSMComponent compt)
    {
        this.compt = compt;
    }
    public virtual void Init()
    {

    }
    public virtual void OnEnter()
    {

    }
    public virtual void OnLogic()
    {

    }
    public virtual void OnExit()
    {

    }
    public virtual void OnExitRequest()
    {

    }

    public virtual void WriteJson(JObject writer)
    {
        writer.Add("needsExitTime", needsExitTime);
        writer.Add("isGhostState", isGhostState);
        writer.Add("name", name);
    }

    public void ReadJson(JObject reader)
    {
    }
}
