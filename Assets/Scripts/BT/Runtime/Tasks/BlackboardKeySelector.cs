using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace BT.Runtime
{
    public class BlackboardKeySelector
    {
        public string selectedKeyName;
        public BlackboardKeyTypeBase selectdKeyType;
        private List<BlackboardKeyTypeBase> allowedTypes;
        protected int selectedKeyID = -1;

        public BlackboardKeySelector()
        {
            allowedTypes = new List<BlackboardKeyTypeBase>();
        }
        public void ResolveSelectedKey(BTBlackboardData bbAsset)
        {
            InitSelection(bbAsset);
            if (!string.IsNullOrEmpty(selectedKeyName))
            {
                selectedKeyID = bbAsset.GetKeyID(selectedKeyName);
                selectdKeyType = bbAsset.GetKeyType(selectedKeyID);
            }
        }
        public void AddObjectFilter(BTNode owner, string propertyName, Type allowedClass)
        {
            BlackboardKeyTypeObject filterOb=new BlackboardKeyTypeObject();
            filterOb.baseClass = allowedClass;
            allowedTypes.Add(filterOb);
        }
        public void AddEnumFilter(BTNode owner, string propertyName, Enum allowedEnum)
        {
            BlackboardKeyTypeEnum filterOb = new BlackboardKeyTypeEnum();
            filterOb.enumType = allowedEnum;
            allowedTypes.Add(filterOb);
        }
        public void AddStringFilter(BTNode owner, string propertyName)
        {
            BlackboardKeyTypeString filterOb = new BlackboardKeyTypeString();
            allowedTypes.Add(filterOb);
        }
        public void InitSelection(BTBlackboardData bbAsset)
        {
            for (int i = 0; i < bbAsset.GetKeys().Count; i++)
            {
                BlackboardEntry entryInfo = bbAsset.GetKeys()[i];
                if (entryInfo.keyType != null)
                {
                    bool filterPassed = true;
                    if (allowedTypes.Count > 0)
                    {
                        filterPassed = false;
                        for (int n = 0; n < allowedTypes.Count; n++)
                        {
                            if (entryInfo.keyType.IsAllowedByFilter(allowedTypes[n]))
                            {
                                filterPassed = true;
                                break;
                            }
                        }
                    }
                    if (filterPassed)
                    {
                        selectedKeyName = entryInfo.entryName;
                        break;
                    }
                }
            }
        }
    }
}
