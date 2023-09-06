using System;
using System.Collections.Generic;

namespace Core
{
}

namespace RailShootGame
{
    public static class ReferencePool
    {

        private sealed class ReferenceCollection
        {
            private readonly Queue<ReferencePoolObject> References;
            private readonly Type ReferenceType;
            private int AddReferenceCount;
            public ReferenceCollection(Type referenceType)
            {
                References = new Queue<ReferencePoolObject>();
                ReferenceType = referenceType;
            }
            public T Acquire<T>() where T : ReferencePoolObject, new()
            {
                if (References.Count > 0)
                {
                    ReferencePoolObject reference = References.Dequeue();
                    reference.State = ReferencePoolObject.EPoolObjectState.OutPool;
                    return reference as T;
                }
                else
                {
                    ReferencePoolObject reference = new T();
                    reference.State = ReferencePoolObject.EPoolObjectState.OutPool;
                    AddReferenceCount++;
                    return reference as T;
                }

            }
            public ReferencePoolObject Acquire()
            {
                if (References.Count > 0)
                {
                    ReferencePoolObject reference = References.Dequeue();
                    reference.State = ReferencePoolObject.EPoolObjectState.OutPool;
                    return reference;
                }
                else
                {
                    ReferencePoolObject reference= (ReferencePoolObject)Activator.CreateInstance(ReferenceType);
                    reference.State = ReferencePoolObject.EPoolObjectState.OutPool;
                    AddReferenceCount++;
                    return reference;
                }

            }
            public void Release(ReferencePoolObject reference)
            {
                if (reference.State == ReferencePoolObject.EPoolObjectState.InPool)
                {
                    return;
                }
                References.Enqueue(reference);
                reference.State = ReferencePoolObject.EPoolObjectState.InPool;
            }
        }
        private static readonly Dictionary<Type, ReferenceCollection> ReferenceCollections = new Dictionary<Type, ReferenceCollection>();
        public static T Acquire<T>() where T : ReferencePoolObject, new()
        {
            return GetReferenceCollection(typeof(T)).Acquire<T>();
        }
        public static ReferencePoolObject Acquire(Type referenceType)
        {
            return GetReferenceCollection(referenceType).Acquire();
        }
        public static void Release(ReferencePoolObject reference)
        {
            GetReferenceCollection(reference.GetType()).Release(reference);
        }
        private static ReferenceCollection GetReferenceCollection(Type referenceType)
        {
            ReferenceCollection referenceCollection = null;
            if (!ReferenceCollections.TryGetValue(referenceType, out referenceCollection))
            {
                referenceCollection = new ReferenceCollection(referenceType);
                ReferenceCollections.Add(referenceType, referenceCollection);
            }

            return referenceCollection;
        }

    }
}