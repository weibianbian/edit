using System;

namespace BT.Runtime
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class TreeNodeMenuItemAttribute : Attribute
    {
        public string menuTitle;
        public Type onlyCompatibleWithGraph;

        public TreeNodeMenuItemAttribute(string menuTitle = null, Type onlyCompatibleWithGraph = null)
        {
            this.menuTitle = menuTitle;
            this.onlyCompatibleWithGraph = onlyCompatibleWithGraph;
        }
    }
}
