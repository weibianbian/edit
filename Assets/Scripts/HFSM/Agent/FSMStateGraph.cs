using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;

public class FSMStateGraph : SerializedMonoBehaviour
{
    [LabelText("说明")]
    public string des="";
    [LabelText("名称")]
    public string stateName = "";

    public void Awake()
    {
    }
    public virtual void OnSave()
    {

    }
    public virtual StateBase CreateFSMFromGraph()
    {
        return new StateBase();
    }
}
