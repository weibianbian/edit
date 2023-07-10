using System;

namespace BT.Runtime
{
    public class BlackboardKeyTypeFloat : BlackboardKeyType
    {
        public override Type valueType
        {
            get
            {
                return typeof(float);
            }
        }
    }
}
