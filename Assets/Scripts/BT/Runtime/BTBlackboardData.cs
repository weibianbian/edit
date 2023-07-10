using System;
using System.Collections.Generic;
using UnityEngine;

namespace BT.Runtime
{

    public class BTBlackboardData
    {
        List<BlackboardEntry> keys = new List<BlackboardEntry>();
        protected int firstKeyID = 0;

        public void PostInitProperties()
        {
            UpdatePersistentKeys(this);
        }

        public T UpdatePersistentKey<T>(string keyName)
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
        public void UpdatePersistentKeys(BTBlackboardData asset)
        {
            object selfKeyType = asset.UpdatePersistentKey<object>("SelfActor");
            object str = asset.UpdatePersistentKey<string>("str");
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
    }
}
