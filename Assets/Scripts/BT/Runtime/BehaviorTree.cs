using System;
using System.Collections.Generic;

namespace BT.Runtime
{
    public static class BTTypeUtils
    {
        public static bool TypesAreConnectable(Type t1, Type t2)
        {
            if (t1 == null || t2 == null)
                return false;

            if (TypeAdapter.AreIncompatible(t1, t2))
                return false;

            ////Check if there is custom adapters for this assignation
            //if (CustomPortIO.IsAssignable(t1, t2))
            //    return true;

            //Check for type assignability
            if (t2.IsReallyAssignableFrom(t1))
                return true;

            // User defined type convertions
            if (Runtime.TypeAdapter.AreAssignable(t1, t2))
                return true;

            return false;
        }
    }

    public class BehaviorTree
    {
        public BTCompositeNode rootNode=null;
    }
}
