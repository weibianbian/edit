using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

public class FSMGraph : FSMStateGraph
{
    [ShowInInspector]
    public FSMStateGraph startState;
    [ShowInInspector]
    [HideReferenceObjectPicker]
    [ListDrawerSettings(CustomAddFunction = "AddTransition")]
    public List<FSMTransitionGraph> transitions = new List<FSMTransitionGraph>();
    public bool isRoot = false;
    private FSMTransitionGraph AddTransition => new();
    [ShowInInspector]
    [HideReferenceObjectPicker]
    public List<FSMStateGraph> states = new List<FSMStateGraph>();

    public void Awake()
    {
       
        
    }
    public void Start()
    {
    }
}
public enum EStateType
{
    Patrol,
    Combat,
}
