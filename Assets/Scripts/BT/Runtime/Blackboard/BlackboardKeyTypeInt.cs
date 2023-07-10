using System;

namespace BT.Runtime
{
    public class BlackboardKeyTypeInt : BlackboardKeyType
    {
        public override Type valueType
        {
            get
            {
                return typeof(int);
            }
        }
    }
}
