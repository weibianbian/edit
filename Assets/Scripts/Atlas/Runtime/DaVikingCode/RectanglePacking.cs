using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UIToolkit.Runtime.DaVikingCode
{
	/**
     * Class used for sorting the inserted rectangles based on the dimensions
     */
	public class SortableSize
	{

		public int width;
		public int height;
		public int id;

		public SortableSize(int width, int height, int id)
		{

			this.width = width;
			this.height = height;
			this.id = id;
		}
	}
}
