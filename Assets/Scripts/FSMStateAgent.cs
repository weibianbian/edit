using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;

public class FSMStateAgent : SerializedMonoBehaviour
{
    [LabelText("说明")]
    public string des="";
    [TypeFilter(nameof(Get))]
    public MeStateBase state;

    public IEnumerable<Type> Get()
    {
        var q = typeof(MeStateBase).Assembly.GetTypes()
           .Where(x => !x.IsAbstract)
           .Where(x => !x.IsGenericTypeDefinition)
           .Where(x => typeof(MeStateBase).IsAssignableFrom(x));

        return q;
    }
}
