using System;
using System.Reflection;
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
        Debug.Log("��ʱ���ص�");
    }
    // Update is called once per frame
    void Update()
    {
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
            Debug.Log($"��ʱ��ִ���˻ص�={t}");
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
        }
    }
    public class TimerManagerTmp
    {
        public TimerTmp timer;
        public void Tick()
        {
            //ʱ�䵽 ִ�лص�
            timer.callBack.Execute();
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
    }
}
