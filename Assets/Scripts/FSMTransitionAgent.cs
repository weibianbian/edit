using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;

public class FSMTransitionAgent : SerializedMonoBehaviour
{
    [LabelText("说明")]
    public string des;
    [HideReferenceObjectPicker]
    //[TypeFilter(nameof(Get))]
    public FSMStateAgent from;
    [HideReferenceObjectPicker]
    //[TypeFilter(nameof(Get))]
    public FSMStateAgent to;

}
