using System;

namespace BT.Runtime
{
    public class BlackboardKeyTypeObject : BlackboardKeyType
    {
        public override Type valueType
        {
            get
            {
                return typeof(object);
            }
        }
    }
}
