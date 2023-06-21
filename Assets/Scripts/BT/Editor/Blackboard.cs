using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace BT.Editor
{
    public class Blackboard : ScriptableObject
    {

    }
    [Serializable]
    public class BlackboardField
    {
        public string key;
        [SerializeReference]
        public Variable value;
    }

    [Serializable]
    public class BlackboardEx
    {
        [SerializeReference]
        public List<BlackboardField> m_Variables = new List<BlackboardField>();
        public List<BlackboardField> variables => m_Variables;

        Dictionary<string, Variable> m_VariableMap = new Dictionary<string, Variable>();
        public Dictionary<string, Variable> variableMap => m_VariableMap;

        public void Build()
        {
            variableMap.Clear();
            foreach (var variable in variables)
            {
                variableMap.Add(variable.key, variable.value);
            }
        }
    }
    public abstract class Variable
    {
        public abstract Type valueType { get; }
    }
    public interface IVariable<T>
    {
        T GetValue();
        void SetValue(T value);
    }
    [Serializable]
    public class VariableInt32 : VariableBase<int>
    {
        public override Type valueType => typeof(int);
    }
    public abstract class VariableBase<T> : Variable, IVariable<T>
    {
        [SerializeField]
        private T value;

        public T GetValue()
        {
            return value;
        }

        public void SetValue(T value)
        {
            this.value = value;
        }
    }
}
