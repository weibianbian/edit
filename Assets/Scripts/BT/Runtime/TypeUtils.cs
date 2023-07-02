using System;
using System.Collections.Generic;
using System.Reflection;

namespace BT.Runtime
{
    public static class TypeUtils
    {
        public static FieldInfo[] GetAllFields(Type t)
        {
            FieldInfo[] infoArray = null;
            List<FieldInfo> fieldList = new List<FieldInfo>();
            fieldList.Clear();
            GetFields(t, ref fieldList, 0x36);
            infoArray = fieldList.ToArray();
            return infoArray;
        }

        private static void GetFields(Type t, ref List<FieldInfo> fieldList, int flags)
        {
            if (t != null)
            {
                FieldInfo[] fields = t.GetFields((BindingFlags)flags);
                for (int i = 0; i < fields.Length; i++)
                {
                    fieldList.Add(fields[i]);
                }

                GetFields(t.BaseType, ref fieldList, flags);
            }
        }
    }
}
