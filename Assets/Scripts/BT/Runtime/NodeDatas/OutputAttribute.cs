using System;

namespace BT.Runtime
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class OutputAttribute : Attribute
    {
        public string name;
        public bool allowMultiple = true;
        public OutputAttribute(string name = null, bool allowMultiple = true)
        {
            this.name = name;
            this.allowMultiple = allowMultiple;
        }
    }
}
