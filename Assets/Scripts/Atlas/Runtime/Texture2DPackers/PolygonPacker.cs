using System.Collections.Generic;
using UnityEngine;

namespace UIToolkit.Runtime
{
	public class PolygonPacker : ITexture2DPacker
	{
        private List<Polygon> polygons;
        private int maxWidth;
        private int maxHeight;
        public int Width => maxWidth;
        public int Height => maxHeight;

        public PolygonPacker() : this(1024, 1024)
        {

        }
        public PolygonPacker(int maxWidth, int maxHeight)
        {
            this.maxWidth = maxWidth;
            this.maxHeight = maxHeight;
            polygons = new List<Polygon>();
        }

        public bool TryInsert(int width, int height, out RectInt position)
        {
            position = default(RectInt);

            float aspectRatio = (float)width / height;

            // 遍历已放置的多边形
            for (int i = 0; i < polygons.Count; i++)
            {
                Polygon polygon = polygons[i];

                // 如果找到合适的位置，则将多边形放置在该位置
                if (FitPolygon(polygon, width, height, aspectRatio, out position))
                {
                    polygon.isOccupied = true;
                    return true;
                }
            }

            // 如果无法找到合适的位置，则创建新的多边形并放置在图集中
            if (width <= maxWidth && height <= maxHeight)
            {
                Polygon newPolygon = new Polygon(0, 0, width, height);
                newPolygon.isOccupied = true;
                polygons.Add(newPolygon);
                position = new RectInt(0, 0, width, height);
                return true;
            }

            return false;
        }

        private bool FitPolygon(Polygon polygon, int width, int height, float aspectRatio, out RectInt position)
        {
            position = default(RectInt);

            if (!polygon.isOccupied && polygon.width >= width && polygon.height >= height)
            {
                float polygonAspectRatio = (float)polygon.width / polygon.height;

                // 如果多边形的宽高比与纹理的宽高比相似，则将纹理放置在多边形内
                if (Mathf.Approximately(polygonAspectRatio, aspectRatio))
                {
                    position = new RectInt(polygon.x, polygon.y, width, height);
                    return true;
                }
            }

            return false;
        }

        public void Remove(RectInt position)
        {
            for (int i = 0; i < polygons.Count; i++)
            {
                Polygon polygon = polygons[i];
                if (polygon.x == position.x && polygon.y == position.y && polygon.width == position.width && polygon.height == position.height)
                {
                    polygon.isOccupied = false;
                    return;
                }
            }
        }

        public override string ToString()
        {
            int occupiedCount = CountOccupiedPolygons();
            return "图集中的纹理 (PolygonPacker): " + occupiedCount;
        }

        private int CountOccupiedPolygons()
        {
            int count = 0;
            for (int i = 0; i < polygons.Count; i++)
            {
                if (polygons[i].isOccupied)
                    count++;
            }
            return count;
        }

        private class Polygon
        {
            public int x;
            public int y;
            public int width;
            public int height;
            public bool isOccupied;

            public Polygon(int x, int y, int width, int height)
            {
                this.x = x;
                this.y = y;
                this.width = width;
                this.height = height;
                isOccupied = false;
            }
        }
    }
}