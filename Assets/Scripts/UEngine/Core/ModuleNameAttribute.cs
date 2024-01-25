using System;

namespace UEngine.Core
{
    public class ModuleNameAttribute : Attribute
    {
        public string ModuleName;
        public ModuleNameAttribute(string InModuleName)
        {
            ModuleName = InModuleName;
        }
    }
}

