using System;

namespace Ionic.Zlib
{
	// Token: 0x020012B3 RID: 4787
	internal class WorkItem
	{
		// Token: 0x06007164 RID: 29028 RVA: 0x0027B3C0 File Offset: 0x002795C0
		public WorkItem(int size, CompressionLevel compressLevel, CompressionStrategy strategy, int ix)
		{
			this.buffer = new byte[size];
			int num = size + (size / 32768 + 1) * 5 * 2;
			this.compressed = new byte[num];
			this.compressor = new ZlibCodec();
			this.compressor.InitializeDeflate(compressLevel, false);
			this.compressor.OutputBuffer = this.compressed;
			this.compressor.InputBuffer = this.buffer;
			this.index = ix;
		}

		// Token: 0x040045EB RID: 17899
		public byte[] buffer;

		// Token: 0x040045EC RID: 17900
		public byte[] compressed;

		// Token: 0x040045ED RID: 17901
		public int crc;

		// Token: 0x040045EE RID: 17902
		public int index;

		// Token: 0x040045EF RID: 17903
		public int ordinal;

		// Token: 0x040045F0 RID: 17904
		public int inputBytesAvailable;

		// Token: 0x040045F1 RID: 17905
		public int compressedBytesAvailable;

		// Token: 0x040045F2 RID: 17906
		public ZlibCodec compressor;
	}
}
