using Newtonsoft.Json.Linq;
using System.Collections.Generic;

public class StateBundle:IJsonConvertible
{
    public List<TransitionBase> transitions;
    public StateBase state;

    public void AddTransition(TransitionBase t)
    {
        transitions = transitions ?? new List<TransitionBase>();
        transitions.Add(t);
    }

    public void ReadJson(JObject writer)
    {
        
    }

    public void WriteJson(JObject writer)
    {
        state?.WriteJson(writer);
        if (transitions != null)
        {
            JObject obj=new JObject();
            writer.Add("transitions", obj);
            for (int i = 0; i < transitions.Count; i++)
            {
                JObject obj2 = new JObject();
                obj.Add($"{i}",obj2);
                TransitionBase t= transitions[i];
                t.WriteJson(obj2);
            }
        }
    }
}
