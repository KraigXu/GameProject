using System;

namespace Verse
{
	
	public static class MurmurHash
	{
		
		public static int GetInt(uint seed, uint input)
		{
			uint num = input * 3432918353u;
			num = (num << 15 | num >> 17);
			num *= 461845907u;
			uint num2 = seed ^ num;
			num2 = (num2 << 13 | num2 >> 19);
			num2 = num2 * 5u + 3864292196u;
			num2 ^= 2834544218u;
			num2 ^= num2 >> 16;
			num2 *= 2246822507u;
			num2 ^= num2 >> 13;
			num2 *= 3266489909u;
			return (int)(num2 ^ num2 >> 16);
		}

		
		private const uint Const1 = 3432918353u;

		
		private const uint Const2 = 461845907u;

		
		private const uint Const3 = 3864292196u;

		
		private const uint Const4Mix = 2246822507u;

		
		private const uint Const5Mix = 3266489909u;

		
		private const uint Const6StreamPosition = 2834544218u;
	}
}
