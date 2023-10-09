using System.Collections.Generic;
using UnityEngine;

namespace UIToolkit.Runtime
{
	public class RectanglePacker : ITexture2DPacker
	{
        private int maxWidth;
        private int maxHeight;
        private List<RectInt> texturePositions;
        public int Width => maxWidth;
        public int Height => maxHeight;

        public RectanglePacker() : this(1024, 1024)
        {

        }

        public RectanglePacker(int maxWidth, int maxHeight)
        {
            this.maxWidth = maxWidth;
            this.maxHeight = maxHeight;
            texturePositions = new List<RectInt>();
        }

        public bool TryInsert(int width, int height, out RectInt position)
        {
            int x = 0;
            int y = 0;

            // 在图集中寻找可用的位置
            while (true)
            {
                // 检查当前位置是否与已放置的纹理相交
                bool positionFound = true;
                foreach (var pos in texturePositions)
                {
                    if (x < pos.x + pos.width && x + width > pos.x &&
                        y < pos.y + pos.height && y + height > pos.y)
                    {
                        positionFound = false;
                        break;
                    }
                }

                if (positionFound)
                    break;

                // 尝试下一个位置
                x += width;

                // 如果当前行放不下纹理，换到下一行
                if (x + width > maxWidth)
                {
                    x = 0;
                    y += height;

                    // 如果下一行超过图集的最大高度，表示无法放置纹理
                    if (y + height > maxHeight)
                    {
                        position = new RectInt();
                        return false;
                    }
                }
            }

            // 创建新的纹理位置并添加到图集中
            position = new RectInt(x, y, width, height);
            texturePositions.Add(position);
            return true;
        }

        public void Remove(RectInt position)
        {
            texturePositions.Remove(position);
        }

        public override string ToString()
        {
            return "图集中的纹理 (RectanglePacker): " + texturePositions.Count;
        }
    }
}