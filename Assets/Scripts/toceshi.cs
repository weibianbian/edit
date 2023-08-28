using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class toceshi : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ClassC c = new ClassC();

        c.classList.Add(new ClassB());

        ClassBase instance = c.GetObj(typeof(ClassB));


        instance.GetType().GetField("Health").SetValue(instance,10);

        Debug.Log(instance.GetType().GetField("Health").GetValue(instance));
    }
    public class ClassBase { }

    public class ClassB : ClassBase
    {
        public float Health;
    }
    public class ClassC
    {
        public List<ClassBase> classList = new List<ClassBase>();

        public ClassBase GetObj(Type type)
        {
            for (int i = 0; i < classList.Count; i++)
            {
                if (classList[i].GetType() == type)
                {
                    return classList[i];
                }
            }
            return null;
        }
    }
}
