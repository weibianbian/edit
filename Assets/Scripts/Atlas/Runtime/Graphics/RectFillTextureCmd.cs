using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace UIToolkit.Runtime
{
 
    public class RectFillTextureCmd: IRectFillTextureCmd
    {
        public int rtPropertyId => Shader.PropertyToID("rt");

        public int texPropertyId => Shader.PropertyToID("tex2D");

        public int offsetPropertyId => Shader.PropertyToID("offset");

        public int kernel { get; private set; }

        public int xGroup { get; private set; }

        public int yGroup { get; private set; }

        public int zGroup { get; private set; }

        private ComputeShader compute;

        public RectFillTextureCmd(ComputeShader compute)
        { 
            this.compute = compute;
            this.kernel = compute.FindKernel("GenAtlas");
            uint x, y, z;
            compute.GetKernelThreadGroupSizes(this.kernel, out x, out y, out z);

            this.xGroup = (int)x;
            this.yGroup = (int)y;
            this.zGroup = (int)z;
        }

        public bool Run(RenderTexture rt, Texture2D texture2D, Vector2Int offset)
        {
            if (rt == null || texture2D == null)
            {
                return false;
            }

#if DEBUG
            if (rt.width < (offset.x + texture2D.width) || rt.height < (offset.y + texture2D.height))
            {
                return false;
            }
#endif
            compute.SetTexture(this.kernel, rtPropertyId, rt);
            compute.SetTexture(this.kernel, texPropertyId, texture2D);
            compute.SetInts(this.offsetPropertyId, offset.x, offset.y);
            compute.Dispatch(this.kernel, Mathf.CeilToInt(texture2D.width / xGroup), Mathf.CeilToInt(texture2D.height / yGroup), zGroup);
            return true;
        }
    }
}
