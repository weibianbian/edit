using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace UIToolkit.Runtime
{
    public abstract class DynamicAtlasTextureProxy : MonoBehaviour, IDynamicAtlasTextureProxy
    {
        public abstract int uid { get; }
        public abstract string AtlasId { get; }

        public Texture2D Texture => throw new NotImplementedException();

        public virtual void OnPackFail()
        {
            throw new NotImplementedException();
        }

        public virtual void OnPackSuccess(Texture texture, RectInt rectInt)
        {
            throw new NotImplementedException();
        }

        public void PackToAtlas()
        {
            AtlasManager.Instance.PackAtlas(this);
        }
    }
}