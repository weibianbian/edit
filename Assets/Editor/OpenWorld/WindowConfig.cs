using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OpenWorld.Editor
{
    [CreateAssetMenu(menuName = "OpenWorld/EditWindow Config")]
    internal class WindowConfig : ScriptableObject
    {
        [FolderPath(RequireExistingPath = true), AssetsOnly]
        public string SrcScenePath;
        [FolderPath(RequireExistingPath = true), AssetsOnly]
        public string DestScenePath;
    }
}
