#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Sirenix.OdinInspector;
using UnityEngine.UI;
using UnityEditor;
using System.Linq;
using System;
using Sirenix.OdinInspector.Editor;
using UIToolkit.Runtime;
using Sirenix.Utilities;

[ShowOdinSerializedPropertiesInInspector]
public class AtlasPackerTestRunner : MonoBehaviour
{
    public class AtlasLayout
    { 
        [PreviewField]
        [TableColumnWidth(64)]
        public Texture2D texture;
        public RectInt rect;

        [HideInInspector]
        public AtlasPackerTestRunner creater;

        [HideInInspector]
        public bool InPacked;

        [TableColumnWidth(30)]
        [ResponsiveButtonGroup("Action")]
        public void Pack()
        {
            creater?.PackAtlas(this);
        }
    }


    public const int previewCol = 8;

    [VerticalGroup("Ready", Order = 0)]
    [ShowInInspector, FolderPath(RequireExistingPath = true)]
    public string assetPath;

    [VerticalGroup("Ready", Order = 1)]
    [TableList]
    public List<AtlasLayout> texture2DAtlasLayouts;

    public int textureSize = 1024;
    public int padding = 1;

    public bool useRectSpace = false;

    //[TypeFilter("GetFilteredTypeList")]
    //public ITexture2DPacker texture2DPacker;

    public int SelectType = 0;

    [TypeFilter("GetFilteredTypeList")]
    public ITexture2DPacker Packer;
    public IEnumerable<Type> GetFilteredTypeList()
    {
        var q = typeof(ITexture2DPacker).Assembly.GetTypes()
            .Where(x => !x.IsInterface || !x.IsAbstract)
            .Where(x => typeof(ITexture2DPacker).IsAssignableFrom(x)); 
        return q;
    }

    [VerticalGroup("Ready", Order = 2)]
    [Button("Load Textures", ButtonSizes.Medium)]
    void Load()
    {
        if (!AssetDatabase.IsValidFolder(assetPath))
            return;

        var guids = AssetDatabase.FindAssets("t:Texture2D", new string[] { assetPath });
        if (guids == null)
            return;

        texture2DAtlasLayouts = guids.Select(x => AssetDatabase.GUIDToAssetPath(x))
            .Select(x => AssetDatabase.LoadAssetAtPath<Texture2D>(x))
            .Select(v=> new AtlasLayout { 
                texture = v,
                creater = this
            }).ToList();

        if (texture2DAtlasLayouts == null)
            return;

        
        ITexture2DPacker packer = Packer;//new GreedyPacker(textureSize, textureSize); 
        if (packer == null)
            return;

        for (int i = 0; i < texture2DAtlasLayouts.Count; i++)
        {
            var texture2DAtlasLayout = texture2DAtlasLayouts[i];
            if (texture2DAtlasLayout.texture.width > textureSize || texture2DAtlasLayout.texture.height > textureSize)
                throw new Exception("A texture size is bigger than the sprite sheet size!");
            if (!packer.TryInsert(texture2DAtlasLayout.texture.width, texture2DAtlasLayout.texture.height, out var rectNew))
            {
                texture2DAtlasLayout.InPacked = false;
                continue;
            }
            texture2DAtlasLayout.rect = rectNew;
            texture2DAtlasLayout.InPacked = true;
        }
        Debug.Log("Atlas Rect: " + texture2DAtlasLayouts.Count + " successfully.");
    }

    //[InlineEditor(InlineEditorModes.LargePreview)]
    [HideLabel, ReadOnly]
    [HideReferenceObjectPicker]
    [PreviewField(512, ObjectFieldAlignment.Center)]
    [VerticalGroup("Pack", Order = 2)]
    public RenderTexture atlas;

    private HashSet<Texture2D> packedAtlasLayouts = new HashSet<Texture2D>();

    public ComputeShader fillTextureShader;

    public bool ForceReCreate = false;

    public IRectFillTextureCmd fillTextureCmd;

    void PackAtlas(AtlasLayout atlasLayout)
    {
        if (!atlasLayout.InPacked)
            return;

        if (fillTextureCmd == null)
        {
            fillTextureCmd = new RectFillTextureCmd(fillTextureShader);
        }

        var sourceTexture = atlasLayout.texture;
        var targetOffset = atlasLayout.rect;
        if (!packedAtlasLayouts.Add(sourceTexture))
        {
            return;
        }

        fillTextureCmd.Run(atlas, sourceTexture, new Vector2Int((int)targetOffset.x, (int)targetOffset.y));

        CreateRawImage(targetOffset);
        CreateImage(targetOffset);
    }

    [VerticalGroup("Pack", Order = 1)]
    [Button("Pack All Textures", ButtonSizes.Medium)]
    void PackAllAtlas()
    {
        packedAtlasLayouts.Clear();
        CreateNewAtlasTexture();
        foreach (var texture2DAtlasLayout in texture2DAtlasLayouts)
        {
            PackAtlas(texture2DAtlasLayout);
        }

        RenderTexture.active = atlas;
        combineTexture.ReadPixels(new Rect(0, 0, textureSize, textureSize), 0, 0);
        combineTexture.Apply();
        RenderTexture.active = null;
    }

    [VerticalGroup("Pack", Order = 1)]
    [Button]
    private void CreateNewAtlasTexture()
    {
        //if(atlas != null && atlas.IsCreated())
        //    atlas?.Release();
        // 创建目标贴图
        atlas = new RenderTexture(textureSize, textureSize, 0);
        atlas.enableRandomWrite = true;
        atlas.wrapMode = TextureWrapMode.Clamp;
        atlas.filterMode = FilterMode.Point;
        atlas.Create();

        combineTexture = new Texture2D(textureSize, textureSize);
    }

    public Texture2D combineTexture;
    public GameObject containerImageGo;
    public GameObject containerRawImageGo;
    public void CreateRawImage(RectInt rect)
    {
        var spriteRect = rect.Dev(atlas.width, atlas.height);
        var go = new GameObject($"{rect}", typeof(RectTransform));
        var rawImage = go.AddComponent<RawImage>();
        rawImage.texture = atlas;
        rawImage.uvRect = spriteRect;

        go.transform.SetParent(containerRawImageGo.transform);
        //go.transform.localPosition = new Vector2(rect.x, rect.y);
    }

    public void CreateImage(RectInt rect)
    {
        var spriteRect = rect.AsRect();
        var sprite = Sprite.Create(combineTexture, spriteRect, Vector2.one * 0.5f, 100, 0, SpriteMeshType.FullRect);
        var go = new GameObject($"{rect}", typeof(RectTransform), typeof(Image));
        go.GetComponent<Image>().sprite = sprite;

        go.transform.SetParent(containerImageGo.transform);
        //go.transform.localPosition = new Vector2(rect.x, rect.y);
    }


}

public static class RectExtension
{
    public static Rect Dev(this RectInt rect, float width, float height)
    {
        return new Rect()
        {
            x = (float)rect.x / width,
            y = (float)rect.y / height,
            width = (float)rect.width / width,
            height = (float)rect.height / height,
        };
    }

    public static Rect AsRect(this RectInt rect)
    {
        return new Rect()
        {
            x = (float)rect.x,
            y = (float)rect.y,
            width = (float)rect.width,
            height = (float)rect.height,
        };
    }
}

#endif