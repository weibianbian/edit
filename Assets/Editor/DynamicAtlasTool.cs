using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.U2D;


public class DynamicAtlasTool : OdinEditorWindow
{
    public class AtlasData
    {
        public string atlasName;
        public string assetPath;
        /// <summary>
        /// 缓存中的SpriteAtlas，不直接指向本地资源
        /// </summary>
        public SpriteAtlas atlas;
        public List<Sprite> sprites;

        //编辑器界面数据
        public bool isShowDital;
    }
    [MenuItem("Tools/图集打包工具")]
    private static void Open()
    {
        var window = GetWindow<global::DynamicAtlasTool>(true, "图集打包工具", true);
        window.minSize = new Vector2(400f, 370f);
    }
    [Space(10)]
    [BoxGroup("图集相关设定", centerLabel: true)]
    [ValueDropdown("GetMaxSpriteAtlasSize")]
    [OnValueChanged("OnMaxSpriteAtlasSizeChange")]
    [LabelText("图集最大尺寸")]
    public int maxSpriteAtlasSize = 2048;

    [Button("打出图集")]
    public void Exe()
    {
        ChackAssetFile($"Assets/RuntimeResources/UIAtlas/");
        SaveAtlasData();
    }
    private IEnumerable GetMaxSpriteAtlasSize()
    {
        var items = new ValueDropdownList<int>();
        items.Add("512", 512);
        items.Add("1024", 1024);
        items.Add("2048", 2048);

        return items;
    }
    private void OnMaxSpriteAtlasSizeChange()
    {

    }

    [BoxGroup("Sprite资源限制策略", centerLabel: true)]
    [ValueDropdown("GetMaxSpriteSize")]
    [OnValueChanged("OnMaxSpriteSizeChange")]
    [LabelText("Sprite最大尺寸限制")]
    public int maxSpriteSize = 1024;
    private IEnumerable GetMaxSpriteSize()
    {
        var items = new ValueDropdownList<int>();
        items.Add("1024", 1024);
        items.Add("512", 512);
        return items;
    }
    private void OnMaxSpriteSizeChange()
    {

    }
    float maxSpritepixelNum;

    bool isIncludeInBuild = true;
    string[] sizeStrs = new string[] { };
    int[] sizes = new int[] { 1024 };
    [BoxGroup("图集资源列表： ", centerLabel: true)]
    [LabelText("图集名字")]
    [ReadOnly]
    public Dictionary<string, AtlasData> atlasDatas = new Dictionary<string, AtlasData>();
   
    void ChackAssetFile(string relativePath)
    {
        List<Sprite> sprites = GetFileSprites(relativePath);
        if (sprites != null && sprites.Count > 1)
        {
            string atlasname = GetAtlasNameFromPath(relativePath);
            string atlasPath = relativePath + atlasname;
            CreateSpriteAtlas(atlasname, atlasPath, sprites);
        }

        DirectoryInfo direction = new DirectoryInfo((relativePath));
        if (direction == null) return;
        DirectoryInfo[] dirChild = direction.GetDirectories();
        foreach (var item in dirChild)
        {
            ChackAssetFile(relativePath + item.Name + "/");
        }
    }
    List<Sprite> GetFileSprites(string relativePath)
    {
        if (Directory.Exists(relativePath))
        {
            DirectoryInfo direction = new DirectoryInfo(relativePath);
            FileInfo[] files = direction.GetFiles("*");//只查找本文件夹下
            if (files == null) return null;

            List<Sprite> sprites = new List<Sprite>();
            foreach (var file in files)
            {
                if (file.Name.EndsWith(".meta")) continue;
                Sprite item = AssetDatabase.LoadAssetAtPath<Sprite>(relativePath + file.Name);
                if (item != null && ChackSpritePackerState(item))
                {
                    sprites.Add(item);
                }
            }
            return sprites;
        }
        return null;
    }
    private string GetAtlasNameFromPath(string relativePath)
    {
        string dir = Path.GetDirectoryName(relativePath);
        dir = dir.Replace("\\", "/");
        string name = $"{dir.Substring(dir.LastIndexOf('/') + 1)}.spriteatlas";
        return name;
    }
    private bool ChackSpritePackerState(Sprite sprite)
    {
        if (sprite.rect.width > maxSpriteSize)
        {
            if (sprite.rect.width % 2 != 0 || sprite.rect.height % 2 != 0)
            {
                Debug.LogError($"{sprite.name} 尺寸不符合压缩规范（宽高均为2的倍数），请注意");
            }
            return false;
        }

        if (sprite.rect.height > maxSpriteSize)
        {
            if (sprite.rect.width % 2 != 0 || sprite.rect.height % 2 != 0)
            {
                Debug.LogError($"{sprite.name} 宽度不符合压缩规范（宽高均为2的倍数），请注意");
            }
            return false;
        }

        //if (sprite.rect.width * sprite.rect.height > maxSpritepixelNum * 1024)
        //{
        //    return false;
        //}
        return true;
    }
    void CreateSpriteAtlas(string atlasname, string atlasPath, List<Sprite> sprites)
    {
        if (atlasDatas.ContainsKey(atlasPath))
        {
            Debug.LogError("警告，有相同名字的Sprite资源文件夹！！！");
            return;
        }
        AtlasData data = new AtlasData()
        {
            atlasName = atlasname.Replace(".asset", ""),
            assetPath = atlasPath,
            sprites = sprites
        };
        atlasDatas.Add(atlasPath, data);
    }
    void SetUpAtlasInfo(ref SpriteAtlas atlas)
    {
        atlas.SetIncludeInBuild(true);
        //A区域参数设定
        SpriteAtlasPackingSettings packSetting = new SpriteAtlasPackingSettings()
        {
            blockOffset = 1,
            enableRotation = false,
            enableTightPacking = false,
            padding = 2,
        };
        atlas.SetPackingSettings(packSetting);
        //B区域参数设定
        SpriteAtlasTextureSettings textureSetting = new SpriteAtlasTextureSettings()
        {
            readable = false,
            generateMipMaps = false,
            sRGB = true,
            filterMode = FilterMode.Bilinear,
        };
        atlas.SetTextureSettings(textureSetting);
        //C区域参数设定
        TextureImporterPlatformSettings platformSetting = new TextureImporterPlatformSettings()
        {

            maxTextureSize = (int)maxSpriteAtlasSize,
            format = TextureImporterFormat.Automatic,
            textureCompression = TextureImporterCompression.Uncompressed
        };
        atlas.SetPlatformSettings(platformSetting);
    }
    void SaveAtlasData()
    {
        List<SpriteAtlas> all = new List<SpriteAtlas>();
        foreach (var item in atlasDatas.Values)
        {
            string path = item.assetPath;
             //= AssetDatabase.LoadAssetAtPath<SpriteAtlas>(path);
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            //if (atlas != null)
            //{

            //    List<Sprite> sprites = new List<Sprite>();
            //    Sprite[] sps = new Sprite[atlas.spriteCount];
            //    int count = atlas.GetSprites(sps);
            //    foreach (var sprite in item.sprites)
            //    {
            //        if (atlas.GetSprite(sprite.name) == null)
            //        {
            //            sprites.Add(sprite);
            //        }
            //    }
            //    atlas.Add(sprites.ToArray());
            //    item.atlas = Instantiate(atlas);
            //    SetUpAtlasInfo(ref atlas);
            //    all.Add(atlas);
            //    continue;
            //}
            SpriteAtlas atlas = new SpriteAtlas();
            atlas.Add(item.sprites.ToArray());
            item.atlas = atlas;
            SetUpAtlasInfo(ref atlas);
            AssetDatabase.CreateAsset(atlas, path);
            all.Add(atlas);

        }
        SpriteAtlasUtility.PackAtlases(all.ToArray(), EditorUserBuildSettings.activeBuildTarget);
        AssetDatabase.Refresh();
    }
}
