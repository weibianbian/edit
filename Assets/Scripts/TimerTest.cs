using System;
using System.Reflection;
using UEngine.Timer;
using UnityEngine;

public class TimerTest : MonoBehaviour
{
    TimerManagerTmp timerManagerTmp;
    // Start is called before the first frame update
    void Start()
    {
        timerManagerTmp = new TimerManagerTmp();
        TestClass testClass=new TestClass();
        testClass.Init(timerManagerTmp);


        timerManagerTmp.Tick();

    }
    public void TickCallBack()
    {
        Debug.Log("计时器回调");
    }
    // Update is called once per frame
    void Update()
    {
    }
    public class TimerCallBackAction<T>
    {
        Action<T> action;
        T arg;
        public static TimerCallBackAction<T> Create(Action<T> act,T InArg)
        {
            TimerCallBackAction<T> ret = new TimerCallBackAction<T>() { action = act,
                arg = InArg,
            };
            return ret;
        }
        public void Execute()
        {
            action?.Invoke(arg);
        }
    }

    public class TimerCallBack
    {
        public object owner;
        public string funcName;
        public object[] param;
        public static TimerCallBack Create(object owner, string funcName, params object[] InVar)
        {
            TimerCallBack ret = new TimerCallBack()
            {
                owner = owner,
                funcName = funcName,
                param = InVar
            };
            return ret;
        }

        public void Execute()
        {
            Type t = owner.GetType();
            MethodInfo mi = t.GetMethod(funcName);
            mi.Invoke(owner, param);
        }
    }
    public class RealInstance
    {
        public void CallBack(int t)
        {
            Debug.Log($"定时器执行了回调={t}");
        }
    }
    public class TestClass
    {
        public void Init(TimerManagerTmp timerMgr)
        {
            //
            RealInstance real = new RealInstance();
            TimerTmp timer = timerMgr.SetTimer();
            int xixi = 1;
            timer.callBack = TimerCallBack.Create(real, "CallBack", xixi);

            TimerDelegate<RealInstance, int>.Create((@real,xxx) =>
            {
                @real.CallBack(xxx);
            }, real, xixi);
            //timer.callBackAction= TimerCallBackAction<RealInstance>.Create((@real) =>
            //{
            //    @real.CallBack(xixi);
            //},real);
        }
    }
    public class TimerManagerTmp
    {
        public TimerTmp timer;
        public void Tick()
        {
            //时间到 执行回调
            timer.callBack.Execute();
            //timer.callBackAction.Execute();
        }
        public TimerTmp SetTimer()
        {
            timer = new TimerTmp();
            return timer;
        }
    }
    public class TimerTmp
    {
        public TimerCallBack callBack;
        //public TimerCallBackAction callBackAction;
    }
}
