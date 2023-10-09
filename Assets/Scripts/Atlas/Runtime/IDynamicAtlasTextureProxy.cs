using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace UIToolkit.Runtime
{
    public interface IDynamicAtlasTextureProxy
    {
        int uid { get; }
        string AtlasId { get; }

        Texture2D Texture { get; }

        void PackToAtlas();

        void OnPackFail();
        
        void OnPackSuccess(Texture texture, RectInt rectInt);
        
    }
}