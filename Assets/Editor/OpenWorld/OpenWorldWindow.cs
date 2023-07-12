using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Sirenix;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.OdinInspector.Internal;
using Sirenix.Serialization;
using Sirenix.Serialization.Editor;
using Sirenix.Serialization.Utilities;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using Sirenix.Utilities.Unsafe;
using System.IO;
using OpenWorld.Runtime;
using UnityEditorInternal;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System.Linq;

namespace OpenWorld.Editor
{
    struct NewTerrainData
    {
        public int xIndex;
        public int zIndex;
        public string name;
        public GameObject gameObject;
    }

    struct NewSceneData
    {
        public int xIndex;
        public int zIndex;
        public string name;
        public NewTerrainData newTerrainData;
    }

    public class OpenWorldWindow : OdinEditorWindow
    {
        enum SceneGridNum
        {
            _2x2 = 2 * 2,
            _4x4 = 4 * 4,
            _8x8 = 8 * 8,
            _16x16 = 16 * 16,
        }
        [SerializeField]
        private WindowConfig config;

        [SerializeField, EnumToggleButtons]
        private SceneGridNum useSceneGridNum;

        [SerializeField]
        private LayerMask ignoreLayerMask;

        [Button]
        void SplitCurrentScene()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode)
                return;
            if( EditorSceneManager.sceneCount > 1)
            {
                //todo
                return;
            }

            StageUtility.GoToMainStage();// stage
            var currentScene = EditorSceneManager.GetActiveScene();
            if (string.IsNullOrEmpty(currentScene.path))
            {
                return;
            }

            List<Terrain> terrainList = new List<Terrain>();
            if(!currentScene.TryGetComponents<Terrain>(terrainList))
            {
                return;
            }

            if(terrainList.Count > 1)
            {
                return;
            }    
            var terrainGameObject = terrainList[0].gameObject;

            List<NewTerrainData> newTerrainList = new List<NewTerrainData>();
            if (!terrainGameObject.SplitTerrain((int)useSceneGridNum, newTerrainList))
            {
                return;
            }

            //Create SubScene
            {
                var newBasicScene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Additive);
                newBasicScene.name = currentScene.name+"_MainScene";

                //var runtimeSceneManager = new GameObject("OpenWorldSceneManager", typeof(OpenWorldSceneManager))
                //    .GetComponent<OpenWorldSceneManager>();
                //EditorSceneManager.MoveGameObjectToScene(runtimeSceneManager.gameObject, newBasicScene);
                //foreach (var newTerrainData in newTerrainList)
                //{
                //    var newScene = EditorSceneManager.NewScene( NewSceneSetup.EmptyScene, NewSceneMode.Additive);
                //    newScene.name = newTerrainData.name;
                //    EditorSceneManager.MoveGameObjectToScene(newTerrainData.gameObject, newScene);
                //}
            }

            // GameObjects
            {
                var rootGameObjects = currentScene.GetRootGameObjects();
                if (rootGameObjects != null)
                {
                    foreach (var rootGameObject in rootGameObjects)
                    {
                        //.ToLower
                        //SceneObject Node
                        if (rootGameObject.name == "SceneObjects")
                        {
                            var childCount = rootGameObject.transform.childCount;
                            for (int i = 0; i < childCount; i++)
                            {
                                var child = rootGameObject.transform.GetChild(i);
                                var childGameObject = child.gameObject;

                                var newObject = new GameObject(childGameObject.name);
                                newObject.transform.localPosition = rootGameObject.transform.localPosition;
                                newObject.transform.localRotation = rootGameObject.transform.localRotation;
                                newObject.transform.localScale = rootGameObject.transform.localScale;

                                var openWorldObject = newObject.AddComponent<OpenWorldObject>();

                                var newRealObject = childGameObject.IsNormalPrefab()?
                                    (PrefabUtility.InstantiateAttachedAsset(childGameObject) as GameObject):
                                    GameObject.Instantiate(childGameObject);
                                {
                                    var renderers = newRealObject.GetComponentsInChildren<Renderer>();
                                    if (renderers != null)
                                    {
                                        for (int ridx = 0; ridx < renderers.Length; ridx++)
                                        {
                                            var renderer = renderers[ridx];
                                            var storeLightingMap = new LightmapStore()
                                            {
                                                index = ridx,
                                            };
                                            if (renderer.lightmapIndex >= 0)
                                            {
                                                storeLightingMap.LightmapIndex = renderer.lightmapIndex;
                                                storeLightingMap.LightmapScaleOffset = renderer.lightmapScaleOffset;
                                            }
                                            else
                                            {
                                                storeLightingMap.skip = true;
                                            }
                                        }
                                    }
                                }
                                newRealObject.transform.SetParent(newObject.transform, false);
                                newRealObject.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
                                newRealObject.transform.localScale = Vector3.zero;
                            }
                        }

                        // Effect Node
                        if (rootGameObject.name == "Effects")
                        {

                        }

                        // xxx Node
                       
                    }
                }
            }
            //
            //EditorBuildSettings.scenes
        }


    }

    internal static class OpenWorldMenus
    {
        [MenuItem("OpenWorld/EditWindow")]
        static void OpenEditWindow()
        {
            var win = OpenWorldWindow.GetWindow<OpenWorldWindow>();
            win.titleContent = new GUIContent("�����༭��");
            win.minSize = Vector2.one * 200;
            win.Show();
            win.Focus();
        }
    }

    internal static class Utility
    {

        public static bool IsNormalPrefab(this GameObject gameObject)
        {
            return PrefabUtility.IsPartOfRegularPrefab(gameObject)
                                    && !PrefabUtility.IsPrefabAssetMissing(gameObject);
        }
        public static bool TryGetComponents<T>(this Scene scene, List<T> ts) where T : Component
        {
            if(scene == null)
                return false;

            var rootGameObjects = scene.GetRootGameObjects();
            if (rootGameObjects == null)
                return false;

            if (ts == null)
                ts = new List<T>();
            foreach (var gameObject in rootGameObjects)
            {
                var cTs = gameObject.GetComponentsInChildren<T>(true);
                if(cTs != null && cTs.Any())
                {
                    ts.AddRange(cTs);
                }
            }
            return ts.Any();
        }
        public static bool SplitTerrain(this GameObject terrainGameObject, int sceneGridNum, List<NewTerrainData> newTerrainDatas)
        {
            if (terrainGameObject == null)
                return false;

            Terrain terrain = terrainGameObject.GetComponent<Terrain>();
            var terrainCollider = terrainGameObject.GetComponent<TerrainCollider>();

            if (terrain == null || terrainCollider)
                return false;

            if(!sceneGridNum.IsPow2())
            {
                //log error
                return false;
            }

            string terrainName = terrainGameObject.name;

            int widthNums = sceneGridNum;
            float yTerrainSize = terrain.terrainData.size.y;
            float xSize = terrain.terrainData.size.x / widthNums;
            float ySize = yTerrainSize / widthNums;
            float zSize = terrain.terrainData.size.z / widthNums;

            int detailWidth = terrain.terrainData.detailWidth / widthNums;
            int detailHeight = terrain.terrainData.detailHeight / widthNums;
            int alphaWidth = terrain.terrainData.alphamapWidth / widthNums;
            int alphaHeight = terrain.terrainData.alphamapHeight / widthNums;

            int blockUnitPixel = (terrain.terrainData.heightmapResolution / widthNums);

            for (int xIdx = 0; xIdx < widthNums; xIdx++)
            {
                for (int zIdx = 0; zIdx < widthNums; zIdx++)
                {
                    float x = xIdx * xSize;
                    float y = 0;
                    float z = zIdx * zSize;

                    //for (int heightIdx = 0; heightIdx < 1; heightIdx++)
                    {
                        string blockName = string.Format("{0}_{1}_{2}", terrainName, xIdx, zIdx);
                        var blockTerrainGameObject = new GameObject(blockName);
                        newTerrainDatas.Add(new NewTerrainData()
                        {
                            xIndex = xIdx,
                            zIndex = zIdx,
                            name = blockName,
                            gameObject = blockTerrainGameObject,
                        });
                        OpenWorldTerrain newTerrainProxy = blockTerrainGameObject.AddComponent<OpenWorldTerrain>();
                        // Transform
                        {
                            blockTerrainGameObject.layer = terrain.gameObject.layer;
                            blockTerrainGameObject.tag = terrain.gameObject.tag;

                            blockTerrainGameObject.transform.localRotation = Quaternion.identity;
                            blockTerrainGameObject.transform.localScale = Vector3.one;
                            blockTerrainGameObject.isStatic = true;

                            blockTerrainGameObject.transform.position = new Vector3(x, y, z) + terrainGameObject.transform.position;
                        }

                        var newTerrain = blockTerrainGameObject.AddComponent<Terrain>();
                        //Terrain
                        {
                            newTerrain.allowAutoConnect = false;
                            //Ϊ�����ϵ����決�ڲ�����̽�����顣ֻ���� Editor ��ʹ�á�
                            newTerrain.bakeLightProbesForTrees = terrain.bakeLightProbesForTrees;
                            //������ͼ����ĸ߶���ͼ�߿齫ʹ��Ԥ�ȼ���ĵͷֱ��ʵ�ͼ��
                            newTerrain.basemapDistance = terrain.basemapDistance;
                            //Ĭ������£�������ֵΪ true����ζ�ŵ������е�ϸ�ڰ߿鲻�ɼ�ʱ��������ڴ���ɾ��������ֱ��Terrain �����ٲŻ��ա�
                            newTerrain.collectDetailPatches = terrain.collectDetailPatches;

                            // ����dering
                            newTerrain.deringLightProbesForTrees = terrain.deringLightProbesForTrees;

                            //ϸ�ڶ�����ܶȡ������ֵķ�ΧΪ 0.0 �� 1.0������ 1.0 Ϊԭʼ�ܶȣ� ��С�����ֽ�������Ⱦ��ϸ�ڶ����������١�
                            newTerrain.detailObjectDensity = terrain.detailObjectDensity;
                            newTerrain.detailObjectDistance = terrain.detailObjectDistance;

                            //TODO
                            //newTerrain.drawHeightmap = terrain.drawHeightmap;
                            //newTerrain.drawInstanced = terrain.drawInstanced;
                            //newTerrain.drawTreesAndFoliage = terrain.drawTreesAndFoliage;

                            newTerrain.editorRenderFlags = terrain.editorRenderFlags;

                            //�ͷ� 100֡δ��Ⱦ������Դ��
                            newTerrain.freeUnusedRenderingResources = terrain.freeUnusedRenderingResources;

                            //TODO
                            //newTerrain.groupingID = terrain.groupingID;
                            newTerrain.heightmapMaximumLOD = terrain.heightmapMaximumLOD;
                            newTerrain.heightmapPixelError = terrain.heightmapPixelError;

                            //�����ù�����ͼ
                            newTerrain.lightmapIndex = -1;
                            //newTerrain.lightmapScaleOffset = Vector4.zero;

                            //������Ⱦ���ε��Զ�����ʡ�
                            newTerrain.materialTemplate = terrain.materialTemplate;

                            //TODO���õ��ΰ�Χ�����š�
                            //newTerrain.patchBoundsMultiplier = terrain.patchBoundsMultiplier;
                            //newTerrain.preserveTreePrototypeLayers = terrain.preserveTreePrototypeLayers;
                            newTerrain.renderingLayerMask = terrain.renderingLayerMask;
                            newTerrain.shadowCastingMode = terrain.shadowCastingMode;
                            //��������ľ��루��������þ��룬���������ȾΪ�����ƣ���
                            newTerrain.treeBillboardDistance = terrain.treeBillboardDistance;

                            //ʹ�������ӹ����Ʒ�����ɵ���������ܾ�����������С��ֵ�ɼӿ���ɡ� ��������Ϊ 0 ���ڴ������л��������Ʊ�ʾʱ�����ɼ��ĵ���Ч����
                            newTerrain.treeCrossFadeLength = terrain.treeCrossFadeLength;
                            newTerrain.treeDistance = terrain.treeDistance;

                            //������Ⱦϸ�ڼ��������� SpeedTree �����ĵ�ǰϸ�ڼ���ƫ��ĳ�����
                            //��ֵ������� 0����Ĭ��ֵΪ 1������Ⱦʹ�õ�ȷ��ϸ�ڼ���ƫ��ֵΪ QualitySettings.lodBias* ֵ����ֵ���� Terrain ���һ�����л���
                            newTerrain.treeLODBiasMultiplier = terrain.treeLODBiasMultiplier;
                            newTerrain.treeMaximumFullLODCount = terrain.treeMaximumFullLODCount;

                            //Neighbors
                            //newTerrain.SetNeighbors(null, null, null, null);
                        }

                        //TerrainData
                        {
                            newTerrain.terrainData = Create<TerrainData>(null, blockName, new TerrainData());
                            EditorUtility.SetDirty(newTerrain.terrainData);

                            //������heightmapResolution ��size��
                            newTerrain.terrainData.heightmapResolution = blockUnitPixel;
                            newTerrain.terrainData.size = new Vector3(xSize, terrain.terrainData.size.y, zSize);

                            int heightmapBaseX = xIdx * blockUnitPixel;
                            int heightmapBaseY = zIdx * blockUnitPixel;
                            int heightmapWidthX = blockUnitPixel
                                + (widthNums > 1 ? 1 : 0);// x��Ե��϶
                            int heightmapWidthY = blockUnitPixel
                                + (widthNums > 1 ? 1 : 0); // y��Ե��϶

                            newTerrain.terrainData.SetHeights(0, 0, terrain.terrainData.GetHeights(
                                heightmapBaseX,
                                heightmapBaseY,
                                heightmapWidthX,
                                heightmapWidthY
                                )
                            );

                            //��ǰ����ʹ�õĵ��β㡣
                            newTerrain.terrainData.terrainLayers = terrain.terrainData.terrainLayers;
                            newTerrain.terrainData.baseMapResolution = terrain.terrainData.baseMapResolution / widthNums;
                            // Copy alpha maps
                            {
                                int alphaBaseX = alphaWidth * xIdx;
                                int alphaBaseY = alphaHeight * zIdx;
                                newTerrain.terrainData.alphamapResolution = terrain.terrainData.alphamapResolution / widthNums;
                                newTerrain.terrainData.SetAlphamaps(0, 0, terrain.terrainData.GetAlphamaps(alphaBaseX, alphaBaseY, alphaWidth, alphaHeight));
                            }
                            // bounds
                            {
                                //�߶�ʹ��ԭʼ�߶�, ֻ����xy��
                                newTerrainProxy.Bounds = new Bounds(
                                    new Vector3(x + xSize * 0.5f, ySize, z + zSize * 0.5f),
                                    new Vector3(xSize, yTerrainSize, zSize)
                                    );

                                newTerrainProxy.TerrainBounds =
                                    new Bounds(
                                    newTerrain.terrainData.bounds.center + newTerrain.transform.position,
                                    newTerrain.terrainData.bounds.size
                                    );
                            }
                            // �ر��ϵ�ϸ�ڡ�
                            {
                                //�������ξ��е�ϸ������/����
                                newTerrain.terrainData.detailPrototypes = terrain.terrainData.detailPrototypes;
                                //ָ��ÿ��������Ⱦ��ϸ�ڰ߿�Ĵ�С��������Ϊ��λ�����ϴ�����ֿɼ��ٻ��Ƶ��ã������ܻ�������������������Ϊ���������޳�ϸ�ڰ߿飩���Ƽ�ֵΪ 16�����ʹ�÷ǳ����ϸ�ڶ�����룬���Ҳݺ�ϡ�裬����Կ��������ֵ��

                                //var detailResolution = terrain.terrainData.detailResolution/ widthNums;
                                var detailResolution = 8;
                                newTerrain.terrainData.SetDetailResolution(terrain.terrainData.detailResolution / widthNums, detailResolution);

                                int detailBaseX = detailWidth * xIdx;
                                int detailBaseY = detailHeight * zIdx;
                                int numLayers = terrain.terrainData.detailPrototypes.Length;
                                for (int layer = 0; layer < numLayers; ++layer)
                                {
                                    newTerrain.terrainData.SetDetailLayer(0, 0, layer, terrain.terrainData.GetDetailLayer(detailBaseX, detailBaseY, detailWidth, detailHeight, layer));
                                }

                                // ���Ͳݽ��и��ơ�
                                {
                                    //������ҡ�ڵĲݵ�������
                                    newTerrain.terrainData.wavingGrassAmount = terrain.terrainData.wavingGrassAmount;
                                    //ҡ�ڲݵ��ٶȡ�
                                    newTerrain.terrainData.wavingGrassSpeed = terrain.terrainData.wavingGrassSpeed;
                                    //������ҡ�ڵĲݵ�ǿ�ȡ�
                                    newTerrain.terrainData.wavingGrassStrength = terrain.terrainData.wavingGrassStrength;
                                    //���ξ��е�ҡ�ڵĲݵ���ɫ��
                                    newTerrain.terrainData.wavingGrassTint = terrain.terrainData.wavingGrassTint;
                                    //���� Inspector ��ʹ�õ���ԭ�͵��б�
                                    newTerrain.terrainData.treePrototypes = terrain.terrainData.treePrototypes;
                                    //����ԭ�ͺ����ˢ�¡�
                                    newTerrain.terrainData.RefreshPrototypes();

                                    //����ԭ�е���ʵ�����и��ơ�ʹ��bounds �ж��Ƿ��ڴ�Terrain�ϡ�
                                    foreach (TreeInstance treeInstace in terrain.terrainData.treeInstances)
                                    {
                                        if (treeInstace.prototypeIndex >= 0 && treeInstace.prototypeIndex < newTerrain.terrainData.treePrototypes.Length &&
                                            newTerrain.terrainData.treePrototypes[treeInstace.prototypeIndex].prefab)
                                        {
                                            //��ľ�ڵ����е�������꣬��Χ�ڣ�0,1��֮�䡣��Ҫ����ת���ſ��Ա�Ϊ�������ꡣ
                                            Vector3 worldSpaceTreePos = Vector3.Scale(treeInstace.position, terrain.terrainData.size) + terrain.transform.position;
                                            if (newTerrainProxy.Bounds.Contains(worldSpaceTreePos))
                                            {
                                                Vector3 localSpaceTreePos = new Vector3((worldSpaceTreePos.x - newTerrain.transform.position.x) / xSize,
                                                    treeInstace.position.y,
                                                    (worldSpaceTreePos.z - newTerrain.transform.position.z) / zSize);
                                                TreeInstance newInstance = treeInstace;
                                                newInstance.position = localSpaceTreePos;
                                                newTerrain.AddTreeInstance(newInstance);
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        newTerrain.Flush();
                        var newCollider = blockTerrainGameObject.AddComponent<TerrainCollider>();
                        newCollider.sharedMaterial = terrainCollider.sharedMaterial;
                        newCollider.terrainData = newTerrain.terrainData;
                        UnityEditor.EditorUtility.SetDirty(newTerrain.terrainData);
                    }
                }
            }
            return true;
        }

        public static T Create<T>(string folder, string name, T asset) where T : UnityEngine.Object
        {
            string path = folder;

            if (string.IsNullOrEmpty(path))
            {
                path = AssetDatabase.GetAssetPath(Selection.activeObject);
                if (string.IsNullOrEmpty(path))
                {
                    path = "Assets";
                }
                else if (Path.GetExtension(path) != "")
                {
                    path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
                }
            }

            string fileName = name;
            if (string.IsNullOrEmpty(fileName))
            {
                fileName = "New" + typeof(T).ToString();
            }

            string assetPath = AssetDatabase.GenerateUniqueAssetPath(path + "/" + fileName + ".asset");
            AssetDatabase.CreateAsset(asset, assetPath);
            AssetDatabase.SaveAssets();
            return asset;
        }
        public static bool IsPow2(this int v)
        {
            if (v < 1) return false;// ignore 0
            return ((v >> 1) & v) == 0;
        }
    }
}
