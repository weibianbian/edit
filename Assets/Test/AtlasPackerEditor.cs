#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Sirenix.OdinInspector;
using UnityEngine.UI;
using UnityEditor;
using System.Linq;
using System;
using Unity.Collections;
using UIToolkit.Runtime;

[ShowOdinSerializedPropertiesInInspector]
public class AtlasPackerEditor : MonoBehaviour
{

    public class AtlasLayout
    {
        public Texture2D texture;
        public Vector4 scaleOffset;
        public RectInt IntRect;

        public bool Packed { get; private set; }
        public bool SetPacked(bool v)
        {
           return  Packed =v; 
        }
    }

    public const int previewCol = 8;

    [ShowInInspector, FolderPath(RequireExistingPath = true)]
    public string assetPath;

    public List<AtlasLayout> texture2DAtlasLayouts;

    [Button("Load Textures", ButtonSizes.Medium)]
    void Load()
    {
        if (!AssetDatabase.IsValidFolder(assetPath))
            return;

        var guids = AssetDatabase.FindAssets("t:Texture2D", new string[] { assetPath });
        if (guids == null)
            return;

        var textures = guids.Select(x => AssetDatabase.GUIDToAssetPath(x))
            .Select(x => AssetDatabase.LoadAssetAtPath<Texture2D>(x)).ToArray();

        int previewRow = Mathf.CeilToInt((float)textures.Length / previewCol);

        texture2DAtlasLayouts = new List<AtlasLayout>(textures.Length);
        for (int i = 0; i < textures.Length; i++)
        {
            texture2DAtlasLayouts.Add(new AtlasLayout()
            {
                texture = textures[i],
                scaleOffset = Vector4.zero,
            });
        }
        
        Debug.Log("Atlas Packer: " + texture2DAtlasLayouts.Count + " successfully loaded.");
    }

    [HideLabel]
    [PreviewField(2048f/2, ObjectFieldAlignment.Left)]
    public RenderTexture atlas;

    public Material blitCopyMaterial;
    private static int ShaderScaleOffset = Shader.PropertyToID("_MainTex_ScaleOffset");

    [Button]
    void ResetAtlasTexture()
    {
        atlas?.Release();
        RenderTextureDescriptor desc = new RenderTextureDescriptor(2048, 2048);
        desc.useMipMap = false;
        desc.autoGenerateMips = false;
        desc.depthBufferBits = 0;   
        desc.colorFormat = RenderTextureFormat.ARGB32;
        desc.enableRandomWrite = true;
        atlas = new RenderTexture(desc);
        atlas.wrapMode = TextureWrapMode.Clamp;
        atlas.filterMode = FilterMode.Point;
    }


    [Button]
    void GenRect()
    {
        var textureSize = 2048;
        if (texture2DAtlasLayouts == null)
            return;

        ITexture2DPacker packer = new RectanglePacker(textureSize, textureSize);
        texture2DAtlasLayouts.Sort((x, y)=>x.texture.texelSize.magnitude.CompareTo(y.texture.texelSize.magnitude));
        for (int i = 0; i < texture2DAtlasLayouts.Count; i++)
        {
            var texture2DAtlasLayout = texture2DAtlasLayouts[i];
            if (!packer.TryInsert(texture2DAtlasLayout.texture.width, texture2DAtlasLayout.texture.height, out var rect))
            {
                Debug.LogError("A texture size is bigger than the sprite sheet free size!");
                texture2DAtlasLayout.IntRect = default(RectInt);
            }
            else
            {
                texture2DAtlasLayout.IntRect = rect;
            }
        }
    }

    protected List<Sprite> mSprites = new List<Sprite>();
    [PreviewField(2048f / 2, ObjectFieldAlignment.Left)]
    public Texture2D sprite2D = null;
    [Button]
    void CreateSprites()
    {
        if (blitCopyMaterial != null && atlas == null || texture2DAtlasLayouts == null)
            return;
        sprite2D = new Texture2D(2048, 2048);
        RenderTexture.active = atlas;
        sprite2D.ReadPixels(new Rect(0, 0, 2048, 2048), 0, 0, false);
        sprite2D.Apply();
        RenderTexture.active = null;

        var canvasGo =  GameObject.Find("Content");
        foreach (var textureLayout in texture2DAtlasLayouts)
        {
            if (!textureLayout.Packed)
                continue;
            var rect = textureLayout.IntRect;
            var spriteRect = rect;
            var sprite = Sprite.Create(sprite2D, spriteRect.AsRect(), Vector2.one*0.5f, 100, 0, SpriteMeshType.FullRect);
            mSprites.Add(sprite);
            var go = new GameObject($"{rect}", typeof(RectTransform), typeof(Image));
            go.GetComponent<Image>().sprite = sprite;

            go.transform.SetParent(canvasGo.transform);
            go.transform.localPosition = new Vector2(rect.x, rect.y);
        }
    }

    [Button]
    void PackSelectAtlas()
    {
        if (blitCopyMaterial != null && atlas == null || texture2DAtlasLayouts == null)
            return;

        Rect rect = new Rect();
        foreach (var textureLayout in texture2DAtlasLayouts.Where(l => l.Packed))
        {
            if (textureLayout.IntRect.AsRect() == rect)
            {
                textureLayout.SetPacked(false);
                continue;
            }
            //new Vector2(rect.xMin, rect.yMin), rect.size
            RenderTexture.active = atlas;
            //blitCopyMaterial.SetVector(ShaderScaleOffset, textureLayout.scaleOffset);
            //Graphics.Blit(textureLayout.texture, atlas, blitCopyMaterial);

            Debug.Log($"CopyTexture {textureLayout.IntRect}, {textureLayout.texture.width},{textureLayout.texture.height}", textureLayout.texture);
            Graphics.CopyTexture(textureLayout.texture, 0, 0, 0, 0, (int)textureLayout.IntRect.width, (int)textureLayout.IntRect.height,
                atlas, 0, 0, (int)textureLayout.IntRect.x, (int)textureLayout.IntRect.y);
            RenderTexture.active = null;
            textureLayout.SetPacked(true);
        }
    }

    [Button]
    void PackAtlas()
    {
        if (blitCopyMaterial != null && atlas == null || texture2DAtlasLayouts == null)
            return;
        Rect rect = new Rect();
        foreach (var textureLayout in texture2DAtlasLayouts)
        {
            if (textureLayout.texture == null)
                continue;

            if (textureLayout.IntRect.AsRect() == rect)
            {
                textureLayout.SetPacked(false);
                continue;
            }
            Debug.Log($"CopyTexture {textureLayout.IntRect}, {textureLayout.texture.width},{textureLayout.texture.height}", textureLayout.texture);
            Graphics.CopyTexture(textureLayout.texture, 0, 0, 0, 0, (int)textureLayout.IntRect.width, (int)textureLayout.IntRect.height,
                atlas, 0, 0, (int)textureLayout.IntRect.x, (int)textureLayout.IntRect.y);
            textureLayout.SetPacked(true);
        }
    }
}
#endif