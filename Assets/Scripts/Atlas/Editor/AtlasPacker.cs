using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;
using System.Linq;

using Sirenix.Serialization;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.OdinInspector.Internal;
using Sirenix.Utilities;
using UObject = UnityEngine.Object;
using UnityEngine.Experimental.Rendering;

namespace UIToolkit.Editor
{
    public class AtlasPacker : OdinEditorWindow
    {
        [MenuItem("Tools/Atlas Packer")] 
        public static void ShowWindow()
        {
            EditorWindow.GetWindow(typeof(AtlasPacker));
        }

        public const int previewCol = 8;

        [VerticalGroup("Ready", Order = 0)]
        [ShowInInspector, FolderPath(RequireExistingPath =true)]
        public string assetPath;

        [VerticalGroup("Ready", Order = 1), TableMatrix(SquareCells = true)]
        public Texture2D[,] texture2Ds;

        [VerticalGroup("Ready", Order = 2)]
        [Button("Load Textures", ButtonSizes.Medium)]
        void Load()
        {
            if (!AssetDatabase.IsValidFolder(assetPath))
                return;

            var guids = AssetDatabase.FindAssets("t:Texture2D", new string[] { assetPath });
            if (guids == null)
                return;

            var textures = guids.Select(x=>AssetDatabase.GUIDToAssetPath(x))
                .Select(x => AssetDatabase.LoadAssetAtPath<Texture2D>(x)).ToArray();

            int previewRow = Mathf.CeilToInt((float)textures.Length / previewCol);

            texture2Ds = new Texture2D[previewCol, previewRow];
            for (int row = 0; row < previewRow; row++)
            {
                for (int col = 0; col < previewCol; col++)
                {
                    int idx = col + row * previewCol;
                    if (idx >= textures.Length)
                    {
                        goto OUT_OF_BOUND;
                    }
                    //todo 排除本身就是图集的贴图
                    texture2Ds[col, row] = textures[idx];
                }
            }

        OUT_OF_BOUND:
            Debug.Log("Atlas Packer: " + texture2Ds.Length + " successfully loaded.");
        }

        [VerticalGroup("Pack", Order = 0)]
        [OdinSerialize, HideLabel, HideReferenceObjectPicker]
        PackTextures packTextures = new PackTextures();

        //[InlineEditor(InlineEditorModes.LargePreview)]
        [HideLabel, ReadOnly]
        [HideReferenceObjectPicker]
        [PreviewField(512, ObjectFieldAlignment.Center)]
        [VerticalGroup("Pack", Order = 2)]
        public Texture2D atlas;
        
        [VerticalGroup("Pack", Order = 1)]
        [Button("Pack Textures", ButtonSizes.Medium)]
        void PackAtlas()
        {
            List<Texture2D> _texture2Ds = new List<Texture2D>();
            foreach (var texture in texture2Ds)
            {
                //todo 排除本身就是图集的贴图
                if(texture != null && !_texture2Ds.Contains(texture))
                {
                    _texture2Ds.Add(texture);
                }
            }
            atlas = packTextures.Pack(_texture2Ds.ToArray(), assetPath);
        }

        void GatherTexture2Ds(List<string> assets)
        { 
        
        }

        [HideMonoScript, HideReferenceObjectPicker]
        public sealed class PackTextures
        {
            [SerializeField] private int _Padding;
            [SerializeField] private int _MaximumSize = 8192;
            public Texture2D Pack(Texture2D[] textures, string defaultAtlasPath = "")
            {
                Texture2D atlas = null;

                BackupTextures backup = new BackupTextures(textures);
                backup.ReadableAndUncompress();

                var path = EditorUtility.SaveFilePanelInProject("Save Packed Texture", "PackedTexture", "png",
                    "save the packed texture?", defaultAtlasPath);

                if (string.IsNullOrEmpty(path))
                    return atlas;

                try
                {
                    const string ProgressTitle = "Packing";
                    EditorUtility.DisplayProgressBar(ProgressTitle, "Packing", 0);

                    var packedTexture = new Texture2D(0, 0, TextureFormat.ARGB32, false);

                    var uvs = packedTexture.PackTextures(textures, _Padding, _MaximumSize);

                    EditorUtility.DisplayProgressBar(ProgressTitle, "Encoding", 0.4f);
                    var bytes = packedTexture.EncodeToPNG();
                    if (bytes == null)
                        return atlas;

                    EditorUtility.DisplayProgressBar(ProgressTitle, "Writing", 0.5f);
                    File.WriteAllBytes(path, bytes);
                    AssetDatabase.Refresh();

                    var importer = (TextureImporter)AssetImporter.GetAtPath(path);
                    importer.maxTextureSize = Math.Max(packedTexture.width, packedTexture.height);
                    importer.textureType = TextureImporterType.Sprite;
                    importer.spriteImportMode = SpriteImportMode.Multiple;
                    importer.spritesheet = new SpriteMetaData[0];
                    EditorUtility.SetDirty(importer);
                    importer.SaveAndReimport();

                    EditorUtility.DisplayProgressBar(ProgressTitle, "Generating Sprites", 0.7f);
                    var sprites = new List<Sprite>();
                    var spriteSheet = new List<SpriteMetaData>();
                    for (int iTexture = 0; iTexture < textures.Length; iTexture++)
                    {
                        var texture = textures[iTexture];

                        sprites.Clear();
                        GatherSprites(sprites, texture);

                        var rect = uvs[iTexture];
                        rect.x *= packedTexture.width;
                        rect.y *= packedTexture.height;
                        rect.width *= packedTexture.width;
                        rect.height *= packedTexture.height;

                        for (int iSprite = 0; iSprite < sprites.Count; iSprite++)
                        {
                            var sprite = sprites[iSprite];

                            var spriteRect = rect;
                            spriteRect.x += spriteRect.width * sprite.rect.x / sprite.texture.width;
                            spriteRect.y += spriteRect.height * sprite.rect.y / sprite.texture.height;
                            spriteRect.width *= sprite.rect.width / sprite.texture.width;
                            spriteRect.height *= sprite.rect.height / sprite.texture.height;

                            spriteSheet.Add(new SpriteMetaData
                            {
                                name = sprite.name,
                                rect = spriteRect,
                                alignment = (int)GetAlignment(sprite.pivot),
                                pivot = sprite.pivot,
                                border = sprite.border,
                            });
                        }
                    }
                    importer.spritesheet = spriteSheet.ToArray();

                    EditorUtility.SetDirty(importer);
                    importer.SaveAndReimport();
                    atlas = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
                    Selection.activeObject = atlas;
                    return atlas;
                }
                finally
                {
                    backup.Recovery();
                    EditorUtility.ClearProgressBar();
                }
            }

            private static void GatherSprites(List<Sprite> sprites, Texture2D texture)
            {
                var path = AssetDatabase.GetAssetPath(texture);
                var assets = AssetDatabase.LoadAllAssetsAtPath(path);
                var foundSprite = false;
                for (int i = 0; i < assets.Length; i++)
                {
                    if (assets[i] is Sprite sprite)
                    {
                        sprites.Add(sprite);
                        foundSprite = true;
                    }
                }

                if (!foundSprite)
                {
                    var sprite = Sprite.Create(texture,
                        new Rect(0, 0, texture.width, texture.height),
                        new Vector2(0.5f, 0.5f));
                    sprite.name = texture.name;
                    sprites.Add(sprite);
                }
            }

            private static SpriteAlignment GetAlignment(Vector2 pivot)
            {
                switch (pivot.x)
                {
                    case 0:
                        switch (pivot.y)
                        {
                            case 0: return SpriteAlignment.BottomLeft;
                            case 0.5f: return SpriteAlignment.BottomCenter;
                            case 1: return SpriteAlignment.BottomRight;
                        }
                        break;
                    case 0.5f:
                        switch (pivot.y)
                        {
                            case 0: return SpriteAlignment.LeftCenter;
                            case 0.5f: return SpriteAlignment.Center;
                            case 1: return SpriteAlignment.RightCenter;
                        }
                        break;
                    case 1:
                        switch (pivot.y)
                        {
                            case 0: return SpriteAlignment.TopLeft;
                            case 0.5f: return SpriteAlignment.TopCenter;
                            case 1: return SpriteAlignment.TopRight;
                        }
                        break;
                }

                return SpriteAlignment.Custom;
            }
        }

        class BackupTextures
        {
            bool[] backupReadables;
            TextureImporterCompression[] backupCompressions;
            Texture2D[] texture2Ds;
            public BackupTextures(Texture2D[] texture2Ds)
            {
                this.texture2Ds = texture2Ds;
                this.backupReadables = new bool[texture2Ds.Length];
                this.backupCompressions = new TextureImporterCompression[texture2Ds.Length];
                Backup();
            }

            private void Backup()
            {
                for (int i = 0; i < texture2Ds.Length; i++)
                {
                    var texture = texture2Ds[i];
                    var path = AssetDatabase.GetAssetPath(texture);
                    Debug.Log(path);
                    var importer = (TextureImporter)AssetImporter.GetAtPath(path);
                    this.backupReadables[i] = importer.isReadable;
                    this.backupCompressions[i] = importer.textureCompression;
                }
            }

            public void ReadableAndUncompress()
            {
                for (int i = 0; i < texture2Ds.Length; i++)
                {
                    var texture = texture2Ds[i];
                    var path = AssetDatabase.GetAssetPath(texture);
                    var importer = (TextureImporter)AssetImporter.GetAtPath(path);
                    if (importer.isReadable && importer.textureCompression == TextureImporterCompression.Uncompressed)
                        continue;
                    importer.isReadable = true;
                    importer.textureCompression =  TextureImporterCompression.Uncompressed;
                    importer.SaveAndReimport();
                }
            }


            public void Recovery()
            {
                for (int i = 0; i < texture2Ds.Length; i++)
                {
                    var texture = texture2Ds[i];
                    var path = AssetDatabase.GetAssetPath(texture);
                    var importer = (TextureImporter)AssetImporter.GetAtPath(path);
                    importer.isReadable = this.backupReadables[i];
                    importer.textureCompression = this.backupCompressions[i];
                    importer.SaveAndReimport();
                }
            }
        }
    }
}
