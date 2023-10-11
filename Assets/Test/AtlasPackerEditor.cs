//#if UNITY_EDITOR
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//using Sirenix.OdinInspector;
//using UnityEngine.UI;
//using UnityEditor;
//using System.Linq;
//using System;
//using UIToolkit.Runtime.DaVikingCode;

//[ShowOdinSerializedPropertiesInInspector]
//public class AtlasPackerEditor : MonoBehaviour
//{

//    public struct AtlasLayout
//    {
//        public Texture2D texture;
//        public Vector4 scaleOffset;
//        public Vector4 IntRect;

//        public bool Pack;
//    }

//    public const int previewCol = 8;

//    [ShowInInspector, FolderPath(RequireExistingPath = true)]
//    public string assetPath;

//    public List<AtlasLayout> texture2DAtlasLayouts;

//    [Button("Load Textures", ButtonSizes.Medium)]
//    void Load()
//    {
//        if (!AssetDatabase.IsValidFolder(assetPath))
//            return;

//        var guids = AssetDatabase.FindAssets("t:Texture2D", new string[] { assetPath });
//        if (guids == null)
//            return;

//        var textures = guids.Select(x => AssetDatabase.GUIDToAssetPath(x))
//            .Select(x => AssetDatabase.LoadAssetAtPath<Texture2D>(x)).ToArray();

//        int previewRow = Mathf.CeilToInt((float)textures.Length / previewCol);

//        texture2DAtlasLayouts = new List<AtlasLayout>(textures.Length);
//        for (int i = 0; i < textures.Length; i++)
//        {
//            texture2DAtlasLayouts.Add(new AtlasLayout()
//            {
//                texture = textures[i],
//                scaleOffset = Vector4.zero,
//            });
//        }
        
//        Debug.Log("Atlas Packer: " + texture2DAtlasLayouts.Count + " successfully loaded.");
//    }

//    [HideLabel]
//    [PreviewField(1024f/2, ObjectFieldAlignment.Left)]
//    [OnValueChanged("PackAtlas")]
//    public RenderTexture atlas;

//    public Material blitCopyMaterial;
//    private static int ShaderScaleOffset = Shader.PropertyToID("_MainTex_ScaleOffset");

//    [Button]
//    void ResetAtlasTexture()
//    {
//        atlas?.Release();
//        RenderTextureDescriptor desc = new RenderTextureDescriptor(1024, 1024);
//        desc.useMipMap = false;
//        desc.autoGenerateMips = false;
//        desc.depthBufferBits = 0;   
//        desc.colorFormat = RenderTextureFormat.ARGB32;
//        desc.enableRandomWrite = true;
//        atlas = new RenderTexture(desc);
//        atlas.wrapMode = TextureWrapMode.Clamp;
//        atlas.filterMode = FilterMode.Point;
//    }


//    [Button]
//    void GenRect()
//    {
//        var textureSize = 1024;
//        if (texture2DAtlasLayouts == null)
//            return;

//        List<Rect> rectangles = new List<Rect>();
//        foreach (var texture2DAtlasLayout in texture2DAtlasLayouts)
//        {
//            if (texture2DAtlasLayout.texture.width > textureSize || texture2DAtlasLayout.texture.height > textureSize)
//                throw new Exception("A texture size is bigger than the sprite sheet size!");
//            else
//                rectangles.Add(new Rect(0, 0, texture2DAtlasLayout.texture.width, texture2DAtlasLayout.texture.height));
//        }

//        const int padding = 1;
//        RectanglePacker packer = new RectanglePacker(textureSize, textureSize, padding);
//        for (int i = 0; i < rectangles.Count; i++)
//            packer.insertRectangle((int)rectangles[i].width, (int)rectangles[i].height, i);

//        packer.packRectangles();
//        if (packer.rectangleCount > 0)
//        {
//            IntegerRectangle rect = new IntegerRectangle();
//            for (int j = 0; j < packer.rectangleCount; j++)
//            {

//                rect = packer.getRectangle(j, rect);

//                int index = packer.getRectangleId(j);
//                var texture2DAtlasLayout = texture2DAtlasLayouts[index];
//                texture2DAtlasLayout.IntRect = new Vector4(rect.x, rect.y, rect.width, rect.height);
//                Vector2 _scale;
//                _scale.x = (float)textureSize/ texture2DAtlasLayout.texture.width;
//                _scale.y = (float)textureSize / texture2DAtlasLayout.texture.height;

//                var x = (textureSize/ rect.width);
//                var y = (textureSize / rect.height);
//                var z = (rect.x + rect.width) / texture2DAtlasLayout.texture.width;
//                var w = (rect.y + rect.height)/ texture2DAtlasLayout.texture.height;

//                texture2DAtlasLayout.scaleOffset = new Vector4(x, y, -z, -w);
//                texture2DAtlasLayouts[index] = texture2DAtlasLayout;
//            }
//        }
//    }

//    protected List<Sprite> mSprites = new List<Sprite>();
//    Texture2D sprite2D = null;
//    [Button]
//    void CreateSprites()
//    {
//        sprite2D = new Texture2D(1024, 1024);
//        RenderTexture.active = atlas;
//        sprite2D.ReadPixels(new Rect(0, 0, 1024, 1024), 0, 0, false);
//        sprite2D.Apply();
//        RenderTexture.active = null;

//        var canvasGo =  GameObject.Find("Content");
//        for (int j = 0; j < texture2DAtlasLayouts.Count; j++)
//        {
//            var texture2DAtlasLayout = texture2DAtlasLayouts[j];
//            var rect = texture2DAtlasLayout.IntRect;
//            var sprite = Sprite.Create(sprite2D, new Rect(rect.x, rect.y, rect.z, rect.w), Vector2.one*0.5f, 100, 0, SpriteMeshType.FullRect);
//            mSprites.Add(sprite);
//            var go = new GameObject($"{rect}", typeof(RectTransform), typeof(Image));
//            go.GetComponent<Image>().sprite = sprite;

//            go.transform.SetParent(canvasGo.transform);
//            go.transform.localPosition = new Vector2(rect.x, rect.y);
//        }
//    }

//    [Button]
//    void PackSelectAtlas()
//    {
//        if (blitCopyMaterial != null && atlas == null || texture2DAtlasLayouts == null)
//            return;
//        foreach (var textureLayout in texture2DAtlasLayouts.Where(l => l.Pack))
//        {
//            if (textureLayout.texture == null)
//                continue;
//            //new Vector2(rect.xMin, rect.yMin), rect.size
//            RenderTexture.active = atlas;
//            blitCopyMaterial.SetVector(ShaderScaleOffset, textureLayout.scaleOffset);
//            Graphics.Blit(textureLayout.texture, atlas, blitCopyMaterial);
//            RenderTexture.active = null;
//        }
//    }

//    [Button]
//    void PackAtlas()
//    {
//        if (blitCopyMaterial != null && atlas == null || texture2DAtlasLayouts == null)
//            return;

//        for (int i = 0; i < texture2DAtlasLayouts.Count; i++)
//        {

//            var textureLayout = texture2DAtlasLayouts[i];
//            if (textureLayout.texture == null)
//                continue;
//            //new Vector2(rect.xMin, rect.yMin), rect.size
//            RenderTexture.active = atlas;
//            blitCopyMaterial.SetVector(ShaderScaleOffset, textureLayout.scaleOffset);
//            Graphics.Blit(textureLayout.texture, atlas, blitCopyMaterial);
//            RenderTexture.active = null;
//        }
//    }

//}
//#endif