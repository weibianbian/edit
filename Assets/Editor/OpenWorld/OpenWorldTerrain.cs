using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OpenWorld.Runtime
{
    public class OpenWorldTerrain : MonoBehaviour
    {
        public Bounds Bounds;
        public Bounds TerrainBounds;

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(Bounds.center, Bounds.size);

            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(TerrainBounds.center, TerrainBounds.size);
        }
#endif
    }
}
