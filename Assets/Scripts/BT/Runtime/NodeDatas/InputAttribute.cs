using System;

namespace BT.Runtime
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class InputAttribute : Attribute
    {
        public string name;
        public bool allowMultiple = false;
        public InputAttribute(string name = null, bool allowMultiple = false)
        {
            this.name = name;
            this.allowMultiple = allowMultiple;
        }
    }
}
