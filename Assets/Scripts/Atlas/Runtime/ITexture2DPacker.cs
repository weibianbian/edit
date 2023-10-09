using UnityEngine;

namespace UIToolkit.Runtime
{
	public interface ITexture2DPacker
	{
		int Width{get;}
		int Height{get;}
		bool TryInsert(int w, int h, out RectInt position);

		void Remove(RectInt pos);

		string ToString();
	}
}
