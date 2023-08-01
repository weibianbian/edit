using RailShootGame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerTest : MonoBehaviour
{
    // Start is called before the first frame update
    TimerManager timerManager;
    void Start()
    {
        timerManager = new TimerManager();

        TimerHandle timerHandle = new TimerHandle();
        timerManager.SetTimer(ref timerHandle, TickCallBack, 1, false);
    }
    public void TickCallBack()
    {
        Debug.Log("计时器回调");
    }
    // Update is called once per frame
    void Update()
    {
        timerManager.Tick(Time.deltaTime);
    }
}
