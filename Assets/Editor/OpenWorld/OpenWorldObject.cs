using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OpenWorld.Runtime
{
    public class LightmapStore
    {
        public int index;
        public bool skip;
        public int LightmapIndex;
        public Vector4 LightmapScaleOffset;
    }
    public class OpenWorldObject : MonoBehaviour
    {
        public Vector3 Position;
        public Vector3 Rotation;
        public Vector3 Scale;
        
        public List<LightmapStore> LightmapStores;

        //todo error check
        public void Apply()
        {
            var renderers = GetComponentsInChildren<Renderer>();
            for (int ridx = 0; ridx < renderers.Length; ridx++)
            {
                var lmapStore = LightmapStores[ridx];
                if(lmapStore.skip)
                {
                    continue;
                }
                var renderer = renderers[ridx];
                renderer.lightmapIndex = lmapStore.index;
                renderer.lightmapScaleOffset = lmapStore.LightmapScaleOffset;
            }
        }
    }
}
