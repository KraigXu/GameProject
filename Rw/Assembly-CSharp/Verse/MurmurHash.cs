using System;

namespace Verse
{
	// Token: 0x0200003B RID: 59
	public static class MurmurHash
	{
		// Token: 0x06000340 RID: 832 RVA: 0x00010DF0 File Offset: 0x0000EFF0
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

		// Token: 0x040000C7 RID: 199
		private const uint Const1 = 3432918353u;

		// Token: 0x040000C8 RID: 200
		private const uint Const2 = 461845907u;

		// Token: 0x040000C9 RID: 201
		private const uint Const3 = 3864292196u;

		// Token: 0x040000CA RID: 202
		private const uint Const4Mix = 2246822507u;

		// Token: 0x040000CB RID: 203
		private const uint Const5Mix = 3266489909u;

		// Token: 0x040000CC RID: 204
		private const uint Const6StreamPosition = 2834544218u;
	}
}
