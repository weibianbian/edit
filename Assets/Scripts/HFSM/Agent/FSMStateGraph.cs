using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;

public class FSMStateGraph : SerializedMonoBehaviour
{
    [LabelText("说明")]
    public string des="";
    //[TypeFilter(nameof(Get))]
    //public StateBase state=new StateBase();

    public IEnumerable<Type> Get()
    {
        var q = typeof(StateBase).Assembly.GetTypes()
           .Where(x => !x.IsAbstract)
           .Where(x => !x.IsGenericTypeDefinition)
           .Where(x => typeof(StateBase).IsAssignableFrom(x));

        return q;
    }
}
