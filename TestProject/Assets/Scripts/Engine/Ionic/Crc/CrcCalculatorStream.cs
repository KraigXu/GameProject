using System;
using System.IO;

namespace Ionic.Crc
{
	// Token: 0x020012C5 RID: 4805
	public class CrcCalculatorStream : Stream, IDisposable
	{
		// Token: 0x060071FA RID: 29178 RVA: 0x0027E042 File Offset: 0x0027C242
		public CrcCalculatorStream(Stream stream) : this(true, CrcCalculatorStream.UnsetLengthLimit, stream, null)
		{
		}

		// Token: 0x060071FB RID: 29179 RVA: 0x0027E052 File Offset: 0x0027C252
		public CrcCalculatorStream(Stream stream, bool leaveOpen) : this(leaveOpen, CrcCalculatorStream.UnsetLengthLimit, stream, null)
		{
		}

		// Token: 0x060071FC RID: 29180 RVA: 0x0027E062 File Offset: 0x0027C262
		public CrcCalculatorStream(Stream stream, long length) : this(true, length, stream, null)
		{
			if (length < 0L)
			{
				throw new ArgumentException("length");
			}
		}

		// Token: 0x060071FD RID: 29181 RVA: 0x0027E07E File Offset: 0x0027C27E
		public CrcCalculatorStream(Stream stream, long length, bool leaveOpen) : this(leaveOpen, length, stream, null)
		{
			if (length < 0L)
			{
				throw new ArgumentException("length");
			}
		}

		// Token: 0x060071FE RID: 29182 RVA: 0x0027E09A File Offset: 0x0027C29A
		public CrcCalculatorStream(Stream stream, long length, bool leaveOpen, CRC32 crc32) : this(leaveOpen, length, stream, crc32)
		{
			if (length < 0L)
			{
				throw new ArgumentException("length");
			}
		}

		// Token: 0x060071FF RID: 29183 RVA: 0x0027E0B8 File Offset: 0x0027C2B8
		private CrcCalculatorStream(bool leaveOpen, long length, Stream stream, CRC32 crc32)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			this._innerStream = stream;
			this._crc32 = (crc32 ?? new CRC32());
			this._lengthLimit = length;
			this._leaveOpen = leaveOpen;
		}

		// Token: 0x17001355 RID: 4949
		// (get) Token: 0x06007200 RID: 29184 RVA: 0x0027E108 File Offset: 0x0027C308
		public long TotalBytesSlurped
		{
			get
			{
				return this._crc32.TotalBytesRead;
			}
		}

		// Token: 0x17001356 RID: 4950
		// (get) Token: 0x06007201 RID: 29185 RVA: 0x0027E115 File Offset: 0x0027C315
		public int Crc
		{
			get
			{
				return this._crc32.Crc32Result;
			}
		}

		// Token: 0x17001357 RID: 4951
		// (get) Token: 0x06007202 RID: 29186 RVA: 0x0027E122 File Offset: 0x0027C322
		// (set) Token: 0x06007203 RID: 29187 RVA: 0x0027E12A File Offset: 0x0027C32A
		public bool LeaveOpen
		{
			get
			{
				return this._leaveOpen;
			}
			set
			{
				this._leaveOpen = value;
			}
		}

		// Token: 0x06007204 RID: 29188 RVA: 0x0027E134 File Offset: 0x0027C334
		public override int Read(byte[] buffer, int offset, int count)
		{
			int count2 = count;
			if (this._lengthLimit != CrcCalculatorStream.UnsetLengthLimit)
			{
				if (this._crc32.TotalBytesRead >= this._lengthLimit)
				{
					return 0;
				}
				long num = this._lengthLimit - this._crc32.TotalBytesRead;
				if (num < (long)count)
				{
					count2 = (int)num;
				}
			}
			int num2 = this._innerStream.Read(buffer, offset, count2);
			if (num2 > 0)
			{
				this._crc32.SlurpBlock(buffer, offset, num2);
			}
			return num2;
		}

		// Token: 0x06007205 RID: 29189 RVA: 0x0027E1A2 File Offset: 0x0027C3A2
		public override void Write(byte[] buffer, int offset, int count)
		{
			if (count > 0)
			{
				this._crc32.SlurpBlock(buffer, offset, count);
			}
			this._innerStream.Write(buffer, offset, count);
		}

		// Token: 0x17001358 RID: 4952
		// (get) Token: 0x06007206 RID: 29190 RVA: 0x0027E1C4 File Offset: 0x0027C3C4
		public override bool CanRead
		{
			get
			{
				return this._innerStream.CanRead;
			}
		}

		// Token: 0x17001359 RID: 4953
		// (get) Token: 0x06007207 RID: 29191 RVA: 0x00010306 File Offset: 0x0000E506
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700135A RID: 4954
		// (get) Token: 0x06007208 RID: 29192 RVA: 0x0027E1D1 File Offset: 0x0027C3D1
		public override bool CanWrite
		{
			get
			{
				return this._innerStream.CanWrite;
			}
		}

		// Token: 0x06007209 RID: 29193 RVA: 0x0027E1DE File Offset: 0x0027C3DE
		public override void Flush()
		{
			this._innerStream.Flush();
		}

		// Token: 0x1700135B RID: 4955
		// (get) Token: 0x0600720A RID: 29194 RVA: 0x0027E1EB File Offset: 0x0027C3EB
		public override long Length
		{
			get
			{
				if (this._lengthLimit == CrcCalculatorStream.UnsetLengthLimit)
				{
					return this._innerStream.Length;
				}
				return this._lengthLimit;
			}
		}

		// Token: 0x1700135C RID: 4956
		// (get) Token: 0x0600720B RID: 29195 RVA: 0x0027E108 File Offset: 0x0027C308
		// (set) Token: 0x0600720C RID: 29196 RVA: 0x0027BDC1 File Offset: 0x00279FC1
		public override long Position
		{
			get
			{
				return this._crc32.TotalBytesRead;
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x0600720D RID: 29197 RVA: 0x0027BDC1 File Offset: 0x00279FC1
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600720E RID: 29198 RVA: 0x0027BDC1 File Offset: 0x00279FC1
		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600720F RID: 29199 RVA: 0x0027E20C File Offset: 0x0027C40C
		void IDisposable.Dispose()
		{
			this.Close();
		}

		// Token: 0x06007210 RID: 29200 RVA: 0x0027E214 File Offset: 0x0027C414
		public override void Close()
		{
			base.Close();
			if (!this._leaveOpen)
			{
				this._innerStream.Close();
			}
		}

		// Token: 0x04004685 RID: 18053
		private static readonly long UnsetLengthLimit = -99L;

		// Token: 0x04004686 RID: 18054
		private readonly Stream _innerStream;

		// Token: 0x04004687 RID: 18055
		private readonly CRC32 _crc32;

		// Token: 0x04004688 RID: 18056
		private readonly long _lengthLimit = -99L;

		// Token: 0x04004689 RID: 18057
		private bool _leaveOpen;
	}
}
