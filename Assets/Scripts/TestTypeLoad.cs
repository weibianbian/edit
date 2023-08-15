using GameplayAbilitySystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class TestTypeLoad : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(typeof(GameplayCueNotifyBurst).IsSubclassOf(typeof(GameplayCueNotifyStatic)));
        System.Reflection.Assembly ass = System.Reflection.Assembly.Load(("Assembly-CSharp"));
        System.Type t = ass.GetType("GameplayAbilitySystem.GameplayCueSet");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
