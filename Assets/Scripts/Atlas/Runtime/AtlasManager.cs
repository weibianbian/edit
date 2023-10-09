using UnityEngine;
using System.Collections.Generic;
namespace UIToolkit.Runtime
{
	public class Atlas
	{
		public ITexture2DPacker packer {get; private set;}
		public RenderTexture rt { get; private set; }

		public Atlas() : this(1024, 1024) { }
		public Atlas(int width, int height):this(new BinaryTreePacker(width, height)) { }
		public Atlas(ITexture2DPacker packer) 
		{
			this.packer = packer;
			this.rt = new RenderTexture(packer.Width, packer.Height, 0);
			this.rt.enableRandomWrite = true;
			this.rt.wrapMode = TextureWrapMode.Clamp;
			this.rt.filterMode = FilterMode.Point;
			this.rt.Create();
		}

		public Texture2D AsNewTexture2D(bool disposeRT = false)
		{ 
			RenderTexture.active = this.rt;
			Texture2D tex2D = new Texture2D(this.rt.width, this.rt.height);
			tex2D.ReadPixels(new Rect(0, 0, this.rt.width, this.rt.height),0,0, false);
			tex2D.Apply();
			RenderTexture.active = null;

			if (disposeRT)
			{
				this.rt.Release();
				this.rt = null;
			}
			return tex2D;
		}

		public Texture2D AsTexture2D(Texture2D tex2D, bool disposeRT = false)
		{
			RenderTexture.active = this.rt;
			tex2D.ReadPixels(new Rect(0, 0, tex2D.width, tex2D.height), 0, 0, false);
			tex2D.Apply();
			RenderTexture.active = null;

			if (disposeRT)
			{
				this.rt.Release();
				this.rt = null;
			}
			return tex2D;
		}
	}

	public class AtlasManager:MonoBehaviour
	{
		Dictionary<string, Atlas> allAtlas = new Dictionary<string, Atlas>();
		ITexture2DPacker packer = new BinaryTreePacker();

		[SerializeField]
		private ComputeShader fillTextureShader;
		IRectFillTextureCmd fillTextureCmd;
		public void PackAtlas(IDynamicAtlasTextureProxy proxy)
		{
			RectInt rectInt;
			if (!allAtlas.TryGetValue(proxy.AtlasId, out Atlas atlas))
			{
				allAtlas.Add(proxy.AtlasId, atlas = new Atlas());
			}
			if (!atlas.packer.TryInsert(proxy.Texture.width, proxy.Texture.height, out rectInt))
			{
				proxy.OnPackFail();
			}

			// init if need
			if (fillTextureCmd == null)
			{
				fillTextureCmd = new RectFillTextureCmd(fillTextureShader);
			}
			fillTextureCmd.Run(atlas.rt, proxy.Texture, new Vector2Int(rectInt.x, rectInt.y));
			proxy.OnPackSuccess(atlas.rt, rectInt);
		}

        private void Awake()
        {
			Instance = this;
		}
        public static AtlasManager Instance { get; private set; }
	}
}
