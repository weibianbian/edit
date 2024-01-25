using System.Collections.Generic;

namespace UEngine.Core
{
    public abstract class ReferencePoolObject
    {
        public enum EPoolObjectState
        {
            InPool,
            OutPool,
        }
        public EPoolObjectState State = EPoolObjectState.InPool;
        protected virtual void OnRelease()
        {
        }
        public void Release()
        {
            OnRelease();
            ReferencePool.Release(this);
        }
    }
}