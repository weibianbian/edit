using Sirenix.OdinInspector;
using System;

namespace BT.Runtime
{
    public interface IBlackboardKeyType
    {
    }
    public class BlackboardKeyType<T> : IBlackboardKeyType
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
