using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace World
{
    public sealed class MapInfo
    {

        private Vector3Int _int3;

		public int NumberMax
		{
			get
			{
				return this._int3.x * this._int3.y * this._int3.z;
			}
		}



	}
}
