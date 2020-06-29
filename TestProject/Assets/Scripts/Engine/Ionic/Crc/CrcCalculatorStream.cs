using System;
using System.IO;

namespace Ionic.Crc
{
	
	public class CrcCalculatorStream : Stream, IDisposable
	{
		
		public CrcCalculatorStream(Stream stream) : this(true, CrcCalculatorStream.UnsetLengthLimit, stream, null)
		{
		}

		
		public CrcCalculatorStream(Stream stream, bool leaveOpen) : this(leaveOpen, CrcCalculatorStream.UnsetLengthLimit, stream, null)
		{
		}

		
		public CrcCalculatorStream(Stream stream, long length) : this(true, length, stream, null)
		{
			if (length < 0L)
			{
				throw new ArgumentException("length");
			}
		}

		
		public CrcCalculatorStream(Stream stream, long length, bool leaveOpen) : this(leaveOpen, length, stream, null)
		{
			if (length < 0L)
			{
				throw new ArgumentException("length");
			}
		}

		
		public CrcCalculatorStream(Stream stream, long length, bool leaveOpen, CRC32 crc32) : this(leaveOpen, length, stream, crc32)
		{
			if (length < 0L)
			{
				throw new ArgumentException("length");
			}
		}

		
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

		
		// (get) Token: 0x06007200 RID: 29184 RVA: 0x0027E108 File Offset: 0x0027C308
		public long TotalBytesSlurped
		{
			get
			{
				return this._crc32.TotalBytesRead;
			}
		}

		
		// (get) Token: 0x06007201 RID: 29185 RVA: 0x0027E115 File Offset: 0x0027C315
		public int Crc
		{
			get
			{
				return this._crc32.Crc32Result;
			}
		}

		
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

		
		public override void Write(byte[] buffer, int offset, int count)
		{
			if (count > 0)
			{
				this._crc32.SlurpBlock(buffer, offset, count);
			}
			this._innerStream.Write(buffer, offset, count);
		}

		
		// (get) Token: 0x06007206 RID: 29190 RVA: 0x0027E1C4 File Offset: 0x0027C3C4
		public override bool CanRead
		{
			get
			{
				return this._innerStream.CanRead;
			}
		}

		
		// (get) Token: 0x06007207 RID: 29191 RVA: 0x00010306 File Offset: 0x0000E506
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		
		// (get) Token: 0x06007208 RID: 29192 RVA: 0x0027E1D1 File Offset: 0x0027C3D1
		public override bool CanWrite
		{
			get
			{
				return this._innerStream.CanWrite;
			}
		}

		
		public override void Flush()
		{
			this._innerStream.Flush();
		}

		
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

		
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException();
		}

		
		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		
		void IDisposable.Dispose()
		{
			this.Close();
		}

		
		public override void Close()
		{
			base.Close();
			if (!this._leaveOpen)
			{
				this._innerStream.Close();
			}
		}

		
		private static readonly long UnsetLengthLimit = -99L;

		
		private readonly Stream _innerStream;

		
		private readonly CRC32 _crc32;

		
		private readonly long _lengthLimit = -99L;

		
		private bool _leaveOpen;
	}
}
