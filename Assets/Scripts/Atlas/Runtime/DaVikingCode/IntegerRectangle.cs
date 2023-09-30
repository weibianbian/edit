using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UIToolkit.Runtime.DaVikingCode
{
	/**
		 * Class used to store rectangles values inside rectangle packer
		 * ID parameter needed to connect rectangle with the originally inserted rectangle
		 */
	public class IntegerRectangle
	{

		public int x;
		public int y;
		public int width;
		public int height;
		public int right;
		public int bottom;
		public int id;

		public IntegerRectangle(int x = 0, int y = 0, int width = 0, int height = 0)
		{

			this.x = x;
			this.y = y;
			this.width = width;
			this.height = height;
			this.right = x + width;
			this.bottom = y + height;
		}
	}
}
