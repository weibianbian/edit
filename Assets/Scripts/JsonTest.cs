using BT.Runtime;
using Newtonsoft.Json;
using UnityEngine;

public class JsonTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var setting = new JsonSerializerSettings();
        setting.Formatting = Formatting.Indented;
        setting.TypeNameHandling = TypeNameHandling.All;
        setting.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

        string str = JsonConvert.SerializeObject(new BehaviorTree(), setting);
        Debug.Log(str);

        BehaviorTree de= JsonConvert.DeserializeObject<BehaviorTree>(str);

        Debug.Log(de.rootNode);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
