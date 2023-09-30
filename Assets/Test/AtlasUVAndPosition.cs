#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Sirenix.OdinInspector;
using UnityEngine.UI;

[ShowOdinSerializedPropertiesInInspector]
public class AtlasUVAndPosition : MonoBehaviour
{
    //[PreviewField(16f, ObjectFieldAlignment.Left)]
    [InlineEditor(InlineEditorModes.LargePreview)]
    [OnValueChanged("PackAtlas")]
    public Texture2D texture;

    [PreviewFieldAttribute(102.4f, ObjectFieldAlignment.Left)]
    [OnValueChanged("PackAtlas")]
    public RenderTexture atlas;

    [OnValueChanged("PackAtlas")]
    public Vector2 _scale;

    [OnValueChanged("PackAtlas")]
    public Vector2 _offset;

    public Material blitCopyMaterial;
    private static int ShaderScaleOffset = Shader.PropertyToID("_MainTex_ScaleOffset");


    [Button]
    void ResetAtlasTexture()
    {
        RenderTextureDescriptor desc = new RenderTextureDescriptor(1024, 1024);
        desc.useMipMap = false;
        desc.autoGenerateMips = false;
        desc.depthBufferBits = 0;
        desc.colorFormat = RenderTextureFormat.ARGB32;
        desc.enableRandomWrite = true;
        atlas = new RenderTexture(desc)
        { 
            name = "ATTTT"
        };
        atlas.wrapMode = TextureWrapMode.Clamp;
        atlas.filterMode = FilterMode.Point;
        rawImageAtlas.texture = atlas;
    }

    [Button]
    void PackAtlas()
    {
        if(texture == null || atlas == null)
            return;

        blitCopyMaterial.SetVector(ShaderScaleOffset, new Vector4(_scale.x, _scale.y, -_offset.x, -_offset.y));
        Graphics.Blit(texture, atlas, blitCopyMaterial);
    }

    [InlineEditor(InlineEditorModes.LargePreview)]
    [ReadOnly,HideReferenceObjectPickerAttribute]
    public Texture2D textureDest;

    [OnValueChanged("ReadAtlas")]
    [MaxValue(864f)]
    [MinValue(0)]
    public Vector2 readOffset;


    [Button]
    void ReadAtlas()
    {
        if (texture == null|| atlas == null)
            return;

        if (textureDest == null)
        {
            textureDest = new Texture2D(texture.width, texture.height, TextureFormat.ARGB32, false);
        }

        RenderTexture.active = atlas;
        textureDest.ReadPixels(new Rect(readOffset, new Vector2(texture.width, texture.height)), 0, 0, false);
        textureDest.Apply();
        RenderTexture.active = null;

        rawImage.texture = textureDest;
    }

    #region 测试
    public RawImage rawImage;
    public RawImage rawImageAtlas;

    public bool anim = false;

    private IEnumerator<Vector2> rectItor;

    private void Start()
    {
        PackAtlas();
    }

    private void Update()
    {
        if (!anim || rawImage == null)
            return;
        
        if(rectItor == null || rectItor.Current == null || !rectItor.MoveNext())
        {
            rectItor = ForEachReadRect();
        }
        readOffset.x = rectItor.Current.x;
        readOffset.y = rectItor.Current.y;
        ReadAtlas();

        rawImageAtlas.texture = atlas;
        rawImage.texture = textureDest;
        rawImage.transform.localPosition = new Vector2(readOffset.x, -readOffset.y);
    }


    private IEnumerator<Vector2> ForEachReadRect()
    {
        var xLength = 1024 - texture.width;
        var yLength = 1024 - texture.height;
        Vector2[,] matrix = new Vector2[xLength, yLength];
        for (int i = 0; i < xLength; i++)
        {
            for (int j = 0; j < yLength; j++)
            {
                matrix[i, j] = new Vector2(i, j);
            }
        }
 
        int lx = 0;
        int ly = 0;
        int rx = xLength - 1;
        int ry = yLength - 1;
        // 每打印一趟，左上角和右下角都往中心移动一格
        while (lx <= rx && ly <= ry)
        {
            var edgeItor = SelectEdagePoints(matrix, lx++, ly++, rx--, ry--);
            while (edgeItor.MoveNext())
            {
                yield return edgeItor.Current;
            }
        }

        yield break;
    }

    public static IEnumerator<T> SelectEdagePoints<T>(T[,] matrix, int lx, int ly, int rx, int ry)
    {
        if (lx == rx)
        {
            for (int i = ly; i <= ry; i++)
            {
                yield return matrix[lx,i];
            }
        }
        else if (ly == ry)
        {
            for (int i = lx; i <= rx; i++)
            {
                yield return matrix[i,ly];
            }
        }
        else
        {
            int x = lx;
            int y = ly;
            while (y != ry)
            {
                yield return (matrix[x,y]);
                y++;
            }
            while (x != rx)
            {
                yield return (matrix[x,y]);
                x++;
            }
            while (y != ly)
            {
                yield return (matrix[x,y]);
                y--;
            }
            while (x != lx)
            {
                yield return (matrix[x,y]);
                x--;
            }
        }
    }
    #endregion
}
#endif