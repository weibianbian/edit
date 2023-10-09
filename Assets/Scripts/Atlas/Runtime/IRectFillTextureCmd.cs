using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace UIToolkit.Runtime
{
    public interface IComputeShader
    {
        int kernel { get; }
        int xGroup { get; }
        int yGroup { get; }
        int zGroup { get; }
    }

    public interface IRectFillTextureCmd : IComputeShader
    {
        int rtPropertyId{get;}
        int texPropertyId{get;}
        int offsetPropertyId{get;}

        public bool Run(RenderTexture rt, Texture2D texture2D, Vector2Int offset);
    }
}
