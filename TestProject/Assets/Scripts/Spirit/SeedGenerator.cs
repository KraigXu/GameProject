using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Spirit
{
    public static  class SeedGenerator
    {

		private  static  ulong _mark = 1640531527;

		public static int HashCombine<T>(int seed, T obj)
		{
			int num = (obj == null) ? 0 : obj.GetHashCode();
			return (int)((long)seed ^ (long)num + (long)_mark + (long)((long)seed << 6) + (long)(seed >> 2));
		}

		public static int HashCombineStruct<T>(int seed, T obj) where T : struct
		{
			return (int)((long)seed ^ (long)obj.GetHashCode() + (long)_mark + (long)((long)seed << 6) + (long)(seed >> 2));
		}
		public static int HashCombineInt(int seed, int value)
		{
			return (int)((long)seed ^ (long)value + (long)_mark + (long)((long)seed << 6) + (long)(seed >> 2));
		}

	}
}
