using Core.Timer;
using System.Collections.Generic;
using UnityEngine;

public class RailShoot : MonoBehaviour
{
    int NumPeriods = 10;
    float PeriodSecs = 1.0f;
    float DamagePerPeriod = 5.0f;
    float curDuration = 0;
    FTimerManager timerManager = new FTimerManager();
    FTimerHandle PeriodHandle = new FTimerHandle();
    FTimerHandle DurationHandle = new FTimerHandle();
    public void Start()
    {
        float Duration = NumPeriods * PeriodSecs;
        int NumApplications = 0;
        curDuration += Time.deltaTime;
        ITimerDelegate PeriodSecsDelegate = TimerDelegate<RailShoot>.Create((@owner) =>
        {
            //@owner.ExecutePeriodicEffect(@handle);
            Debug.Log("执行一次");
        }, this);
        ITimerDelegate DurationDelegate = TimerDelegate<RailShoot>.Create((@owner) =>
        {
            Debug.Log("清除");
            @owner.timerManager.ClearTimer(@owner.PeriodHandle);
            @owner.timerManager.ClearTimer(@owner.DurationHandle);

        }, this);
        timerManager.SetTimerForNextTick(PeriodSecsDelegate);
        timerManager.SetTimer(ref PeriodHandle, PeriodSecsDelegate, PeriodSecs, true);

        timerManager.SetTimer(ref DurationHandle, DurationDelegate, Duration, true);
    }
    public void Update()
    {
        timerManager.Tick(Time.deltaTime);
    }
}
