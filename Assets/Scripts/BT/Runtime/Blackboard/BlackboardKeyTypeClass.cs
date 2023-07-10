using System;

namespace BT.Runtime
{
    public class BlackboardKeyTypeClass : BlackboardKeyType
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
