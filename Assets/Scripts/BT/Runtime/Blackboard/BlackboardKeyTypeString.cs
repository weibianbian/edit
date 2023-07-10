using System;

namespace BT.Runtime
{
    public class BlackboardKeyTypeString : BlackboardKeyType
    {
        public override Type valueType
        {
            get
            {
                return typeof(string);
            }
        }
    }
}
