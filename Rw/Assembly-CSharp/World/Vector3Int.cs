using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace World
{
    public  struct Vector3Int : IEquatable<Vector3Int>
	{
        public int x;
        public int y;
        public int z;

		public Vector3Int(int x,int y,int z)
        {
			this.x = x;
			this.y = y;
			this.z = z;
		}

		public Vector3Int(int x,int y)
        {
			this.x = x;
			this.y = y;
			this.z = 1;
        }

		public static Vector3Int operator +(Vector3Int a, Vector3Int b)
		{
			return new Vector3Int(a.x + b.x, a.y + b.y, a.z + b.z);
		}
		public static Vector3Int operator -(Vector3Int a, Vector3Int b)
		{
			return new Vector3Int(a.x - b.x, a.y - b.y, a.z - b.z);
		}
		public static Vector3Int operator *(Vector3Int a, int i)
		{
			return new Vector3Int(a.x * i, a.y * i, a.z * i);
		}
		public static bool operator ==(Vector3Int a, Vector3Int b)
		{
			return a.x == b.x && a.z == b.z && a.y == b.y;
		}

		public static bool operator !=(Vector3Int a, Vector3Int b)
		{
			return a.x != b.x || a.z != b.z || a.y != b.y;
		}
		public override bool Equals(object obj)
		{
			return obj is Vector3Int && this.Equals((Vector3Int)obj);
		}
		public bool Equals(Vector3Int other)
		{
			return this.x == other.x && this.z == other.z && this.y == other.y;
		}
        //public override int GetHashCode()
        //{
        //    return Gen.HashCombineInt(Gen.HashCombineInt(Gen.HashCombineInt(0, this.x), this.y), this.z);
        //}
        public ulong UniqueHashCode()
		{
			return (ulong)(0L + (long)this.x + 4096L * (long)this.z + 16777216L * (long)this.y);
		}

		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"(",
				this.x.ToString(),
				", ",
				this.y.ToString(),
				", ",
				this.z.ToString(),
				")"
			});
		}




	}
}
