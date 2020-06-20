using System;

namespace Ionic.Zlib
{
	// Token: 0x020012C2 RID: 4802
	public static class ZlibConstants
	{
		// Token: 0x04004673 RID: 18035
		public const int WindowBitsMax = 15;

		// Token: 0x04004674 RID: 18036
		public const int WindowBitsDefault = 15;

		// Token: 0x04004675 RID: 18037
		public const int Z_OK = 0;

		// Token: 0x04004676 RID: 18038
		public const int Z_STREAM_END = 1;

		// Token: 0x04004677 RID: 18039
		public const int Z_NEED_DICT = 2;

		// Token: 0x04004678 RID: 18040
		public const int Z_STREAM_ERROR = -2;

		// Token: 0x04004679 RID: 18041
		public const int Z_DATA_ERROR = -3;

		// Token: 0x0400467A RID: 18042
		public const int Z_BUF_ERROR = -5;

		// Token: 0x0400467B RID: 18043
		public const int WorkingBufferSizeDefault = 16384;

		// Token: 0x0400467C RID: 18044
		public const int WorkingBufferSizeMin = 1024;
	}
}
