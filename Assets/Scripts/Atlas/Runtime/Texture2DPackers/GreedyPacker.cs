using System.Collections.Generic;
using UnityEngine;

namespace UIToolkit.Runtime
{
	public class GreedyPacker : ITexture2DPacker
	{
		private Rectangle root;
		private int maxWidth;
		private int maxHeight;
		public int Width => maxWidth;
		public int Height => maxHeight;



		public GreedyPacker() : this(1024, 1024)
		{

		}
		public GreedyPacker(int maxWidth, int maxHeight)
		{
			this.maxWidth = maxWidth;
			this.maxHeight = maxHeight;
			root = new Column(0, 0, maxWidth, maxHeight);
		}

		public bool TryInsert(int w, int h, out RectInt position)
		{
			position = default(RectInt);
			var rect = root.Insert(w, h);
			if (rect == null)
			{ 
				return false;
			}
			position = rect.position;
			return true;
		}

		public void Remove(RectInt pos)
		{
			root.Remove(pos);
		}

		abstract class Rectangle
		{
			public RectInt position;

			public bool used;

			public Rectangle(int x, int y, int w, int h)
			{
				position = new RectInt(x, y, w, h);
			}

			public bool Contains(RectInt rect)
			{
				return (position.x <= rect.x && position.y <= rect.y && position.xMax >= rect.xMax && position.yMax >= rect.yMax);
			}

			public abstract Rectangle Insert(int w, int h);

			public abstract bool Remove(RectInt rect);
		}

		class Row : Rectangle
		{
			private List<Rectangle> columns;

			private int widthAvailable
			{
				get
				{
					if (used)
						return 0;

					if (columns == null)
						return position.width;

					int width = position.width;
					foreach (var column in columns)
						width -= column.position.width;

					return width;
				}
			}

			public Row(int x, int y, int w, int h) : base(x, y, w, h)
			{
			}

			public override Rectangle Insert(int w, int h)
			{
				if (used)
					return null;

				if (h > position.height)
					return null;

				if (columns == null && w == position.width && h == position.height)
				{
					used = true;
					return this;
				}

				if (columns == null)
					columns = new List<Rectangle>();

				foreach (var column in columns)
				{
					var rect = column.Insert(w, h);
					if (rect != null)
						return rect;
				}

				if (h > widthAvailable)
					return null;

				var newColumn = new Column(position.x + position.width - widthAvailable, position.y, w, position.height);
				columns.Add(newColumn);

				return newColumn.Insert(w, h);
			}

			public override bool Remove(RectInt rect)
			{
				if (!Contains(rect))
					return false;

				if (used && position.Equals(rect))
				{
					used = false;
					return true;
				}

				if (columns != null)
				{
					foreach (var column in columns)
					{
						if (column.Remove(rect))
							return true;
					}
				}

				return false;
			}

		}

		class Column : Rectangle
		{
			private List<Rectangle> rows;

			private int heightAvailable
			{
				get
				{
					if (used)
						return 0;

					if (rows == null)
						return position.height;

					int height = position.height;
					foreach (var row in rows)
						height -= row.position.height;

					return height;
				}
			}

			public Column(int x, int y, int w, int h) : base(x, y, w, h)
			{
			}

			public override Rectangle Insert(int w, int h)
			{
				if (used)
					return null;

				if (w > position.width)
					return null;

				if (rows == null && position.width == w && position.height == h)
				{
					used = true;
					return this;
				}

				if (rows == null)
					rows = new List<Rectangle>();

				foreach (var row in rows)
				{
					var rect = row.Insert(w, h);
					if (rect != null)
						return rect;
				}

				if (h > heightAvailable)
					return null;

				var newRow = new Row(position.x, position.y + position.height - heightAvailable, position.width, h);
				rows.Add(newRow);
				rows.Sort((lhs, rhs) => { return lhs.position.height.CompareTo(rhs.position.height); });

				return newRow.Insert(w, h);
			}

			public override bool Remove(RectInt rect)
			{
				if (!Contains(rect))
					return false;

				if (used && position.Equals(rect))
				{
					used = false;
					return true;
				}

				if (rows != null)
				{
					foreach (var row in rows)
					{
						if (row.Remove(rect))
							return true;
					}
				}

				return false;
			}

		}
	}
}