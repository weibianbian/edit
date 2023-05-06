﻿using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;

public class FSMTransitionAgent
{
    [LabelText("说明")]
    public string des;

    [ShowInInspector]
    [HideReferenceObjectPicker]
    //[TypeFilter(nameof(Get))]
    public FSMStateAgent from;
    
    [ShowInInspector]
    [HideReferenceObjectPicker]
    //[TypeFilter(nameof(Get))]
    public FSMStateAgent to;
    
    [ShowInInspector]
    [HideReferenceObjectPicker]
    [ListDrawerSettings(CustomAddFunction = "AddCondition")]
    public List<FSMCondition> conditions=new List<FSMCondition>();
    private FSMCondition AddCondition => new();

}
public class FSMCondition
{
    public EStateType stateType;
}