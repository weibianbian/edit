﻿using Sirenix.OdinInspector;
using System;

namespace BT.Runtime
{
    public interface IBlackboardKeyType<T>
    {
    }
    public class BlackboardKeyTypeBase {
        
    }

    public abstract class BlackboardKeyType<T> : BlackboardKeyTypeBase,IBlackboardKeyType<T>
    {
        public T value;

        public T GetValue() => value;
        public bool SetValue(T val)
        {
            value = val;
            return true;
        }
    }
}