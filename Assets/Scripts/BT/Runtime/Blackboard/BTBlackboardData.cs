using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace BT.Runtime
{
    public class BTBlackboardData
    {
        public List<BlackboardEntry> keys = new List<BlackboardEntry>();
        public int firstKeyID = 0;

        public void PostInitProperties()
        {
            UpdatePersistentKeys();
        }

        public T UpdatePersistentKey<T>(string keyName) where T : IBlackboardKeyType
        {
            T createKeyType = default(T);
            int keyID = InternalGetKeyID(keyName);
            if (keyID == -1)
            {
                BlackboardEntry entry = new BlackboardEntry();
                entry.entryName = keyName;
                createKeyType = Activator.CreateInstance<T>();
                entry.keyType = createKeyType;

                keys.Add(entry);

            }
            else
            {
                keys.RemoveAt(keyID - firstKeyID);
            }
            return createKeyType;
        }
        public void UpdatePersistentKeys()
        {
            BlackboardKeyTypeObject selfKeyType = UpdatePersistentKey<BlackboardKeyTypeObject>("SelfActor");
            BlackboardKeyTypeString str =UpdatePersistentKey<BlackboardKeyTypeString>("字符串");
            BlackboardKeyTypeBool bo =UpdatePersistentKey<BlackboardKeyTypeBool>("布尔值");
        }
        public int GetKeyID(string entryName)
        {
            return InternalGetKeyID(entryName);
        }
        public BlackboardEntry GetKey(int keyID)
        {
            if (keyID != -1)
            {
                if (keyID >= firstKeyID)
                {
                    return keys[keyID - firstKeyID];
                }
            }
            return null;
        }
        public string GetKeyName(int keyID)
        {
            BlackboardEntry keyEntry = GetKey(keyID);
            return keyEntry != null ? keyEntry.entryName : string.Empty;
        }
        private int InternalGetKeyID(string entryName)
        {
            for (int i = 0; i < keys.Count; i++)
            {
                if (keys[i].entryName == entryName)
                {
                    return i + firstKeyID;
                }
            }
            return -1;
        }
        public List<BlackboardEntry> GetKeys()
        {
            return keys;
        }
    }
}
