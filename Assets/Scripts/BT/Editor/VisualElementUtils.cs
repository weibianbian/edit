using UnityEngine;
using UnityEngine.UIElements;

namespace BT.Editor
{
    public static class VisualElementUtils
    {
        public static VisualElement GetSpace(int _width, int _height)
        {
            VisualElement space = new VisualElement()
            {
                style = { width = _width, height = _height }
            };
            return space;
        }

        public static VisualElement GetRowContainer()
        {
            VisualElement space = new VisualElement()
            {
                style = { flexDirection = FlexDirection.Row }
            };
            return space;
        }

        public static VisualElement GetColumnContainer()
        {
            VisualElement space = new VisualElement()
            {
                style = { flexDirection = FlexDirection.Column }
            };
            return space;
        }

        public static VisualElement GetHorizontalLine(int lineWidth)
        {
            VisualElement line = new VisualElement()
            {
                style = { flexGrow = 0, height = 2, borderBottomWidth = lineWidth, borderBottomColor = Color.gray }
            };
            return line;
        }

        public static VisualElement GetHorizontalLine(int lineWidth, Color r)
        {
            VisualElement line = new VisualElement()
            {
                style = { flexGrow = 0, height = 2, borderBottomWidth = lineWidth, borderBottomColor = r }
            };
            return line;
        }

        public static VisualElement GetVerticalLine(int lineWidth)
        {
            VisualElement line = new VisualElement()
            {
                style = { flexGrow = 0, width = 2, borderLeftWidth = lineWidth, borderLeftColor = Color.gray }
            };
            return line;
        }

        public static Rect GetElementsRect(VisualElement[] aRects)
        {
            if (aRects.Length == 0) return new Rect(0, 0, 0, 0);
            int length = aRects.Length;
            Rect minRect = aRects[0].contentRect;
            Rect maxRect = aRects[0].contentRect;
            for (int i = 1; i < length; i++)
            {
                if (aRects[i].contentRect.center.x < minRect.center.x && aRects[i].contentRect.center.y < minRect.center.y)
                {
                    minRect = aRects[i].contentRect;
                }
                if (aRects[i].contentRect.center.x > minRect.center.x && aRects[i].contentRect.center.y < minRect.center.y)
                {
                    maxRect = aRects[i].contentRect;
                }
            }
            Rect aTarget = new Rect(minRect.center.x, minRect.center.y, maxRect.center.x - minRect.center.x, maxRect.center.y - minRect.center.y);
            return aTarget;
        }
    }
}
