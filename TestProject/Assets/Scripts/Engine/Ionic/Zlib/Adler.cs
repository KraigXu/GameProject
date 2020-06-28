using System;

namespace Ionic.Zlib
{
	// Token: 0x020012BE RID: 4798
	public sealed class Adler
	{
		// Token: 0x0600719A RID: 29082 RVA: 0x0027C590 File Offset: 0x0027A790
		public static uint Adler32(uint adler, byte[] buf, int index, int len)
		{
			if (buf == null)
			{
				return 1u;
			}
			uint num = adler & 65535u;
			uint num2 = adler >> 16 & 65535u;
			while (len > 0)
			{
				int i = (len < Adler.NMAX) ? len : Adler.NMAX;
				len -= i;
				while (i >= 16)
				{
					num += (uint)buf[index++];
					num2 += num;
					num += (uint)buf[index++];
					num2 += num;
					num += (uint)buf[index++];
					num2 += num;
					num += (uint)buf[index++];
					num2 += num;
					num += (uint)buf[index++];
					num2 += num;
					num += (uint)buf[index++];
					num2 += num;
					num += (uint)buf[index++];
					num2 += num;
					num += (uint)buf[index++];
					num2 += num;
					num += (uint)buf[index++];
					num2 += num;
					num += (uint)buf[index++];
					num2 += num;
					num += (uint)buf[index++];
					num2 += num;
					num += (uint)buf[index++];
					num2 += num;
					num += (uint)buf[index++];
					num2 += num;
					num += (uint)buf[index++];
					num2 += num;
					num += (uint)buf[index++];
					num2 += num;
					num += (uint)buf[index++];
					num2 += num;
					i -= 16;
				}
				if (i != 0)
				{
					do
					{
						num += (uint)buf[index++];
						num2 += num;
					}
					while (--i != 0);
				}
				num %= Adler.BASE;
				num2 %= Adler.BASE;
			}
			return num2 << 16 | num;
		}

		// Token: 0x0400464C RID: 17996
		private static readonly uint BASE = 65521u;

		// Token: 0x0400464D RID: 17997
		private static readonly int NMAX = 5552;
	}
}
