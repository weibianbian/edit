using System;

namespace UEngine.Timer
{
    public class TimerDelegate<T1> : ITimerDelegate
    {
        public Action<T1> Act;
        public T1 Arg1;
        public static TimerDelegate<T1> Create(Action<T1> InAct ,T1 InArg)
        {
            return new TimerDelegate<T1>()
            {
                Arg1 = InArg,
                Act = InAct
            };
        }
        public void Execute()
        {
            Act?.Invoke(Arg1);
        }
    }
    public class TimerDelegate<T1, T2> : ITimerDelegate
    {
        public Action<T1, T2> Act;
        public T1 Arg1;
        public T2 Arg2;
        public static TimerDelegate<T1, T2> Create(Action<T1, T2> InAct, T1 InArg1, T2 InArg2)
        {
            return new TimerDelegate<T1, T2>()
            {
                Arg1 = InArg1,
                Act = InAct,
                Arg2 = InArg2
            };
        }
        public void Execute()
        {
            Act?.Invoke(Arg1, Arg2);
        }
    }
    public class TimerDelegate<T1, T2, T3> : ITimerDelegate
    {
        public Action<T1, T2, T3> Act;
        public T1 Arg1;
        public T2 Arg2;
        public T3 Arg3;
        public static TimerDelegate<T1, T2, T3> Create(Action<T1, T2, T3> InAct, T1 InArg1, T2 InArg2, T3 InArg3)
        {
            return new TimerDelegate<T1, T2, T3>()
            {
                Arg1 = InArg1,
                Act = InAct,
                Arg2 = InArg2,
                Arg3 = InArg3
            };
        }
        public void Execute()
        {
            Act?.Invoke(Arg1, Arg2, Arg3);
        }
    }
}

