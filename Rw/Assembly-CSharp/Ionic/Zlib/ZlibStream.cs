using System;
using System.IO;

namespace Ionic.Zlib
{
	// Token: 0x020012C3 RID: 4803
	public class ZlibStream : Stream
	{
		// Token: 0x060071CD RID: 29133 RVA: 0x0027D822 File Offset: 0x0027BA22
		public ZlibStream(Stream stream, CompressionMode mode) : this(stream, mode, CompressionLevel.Default, false)
		{
		}

		// Token: 0x060071CE RID: 29134 RVA: 0x0027D82E File Offset: 0x0027BA2E
		public ZlibStream(Stream stream, CompressionMode mode, CompressionLevel level) : this(stream, mode, level, false)
		{
		}

		// Token: 0x060071CF RID: 29135 RVA: 0x0027D83A File Offset: 0x0027BA3A
		public ZlibStream(Stream stream, CompressionMode mode, bool leaveOpen) : this(stream, mode, CompressionLevel.Default, leaveOpen)
		{
		}

		// Token: 0x060071D0 RID: 29136 RVA: 0x0027D846 File Offset: 0x0027BA46
		public ZlibStream(Stream stream, CompressionMode mode, CompressionLevel level, bool leaveOpen)
		{
			this._baseStream = new ZlibBaseStream(stream, mode, level, ZlibStreamFlavor.ZLIB, leaveOpen);
		}

		// Token: 0x1700134A RID: 4938
		// (get) Token: 0x060071D1 RID: 29137 RVA: 0x0027D863 File Offset: 0x0027BA63
		// (set) Token: 0x060071D2 RID: 29138 RVA: 0x0027D870 File Offset: 0x0027BA70
		public virtual FlushType FlushMode
		{
			get
			{
				return this._baseStream._flushMode;
			}
			set
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException("ZlibStream");
				}
				this._baseStream._flushMode = value;
			}
		}

		// Token: 0x1700134B RID: 4939
		// (get) Token: 0x060071D3 RID: 29139 RVA: 0x0027D891 File Offset: 0x0027BA91
		// (set) Token: 0x060071D4 RID: 29140 RVA: 0x0027D8A0 File Offset: 0x0027BAA0
		public int BufferSize
		{
			get
			{
				return this._baseStream._bufferSize;
			}
			set
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException("ZlibStream");
				}
				if (this._baseStream._workingBuffer != null)
				{
					throw new ZlibException("The working buffer is already set.");
				}
				if (value < 1024)
				{
					throw new ZlibException(string.Format("Don't be silly. {0} bytes?? Use a bigger buffer, at least {1}.", value, 1024));
				}
				this._baseStream._bufferSize = value;
			}
		}

		// Token: 0x1700134C RID: 4940
		// (get) Token: 0x060071D5 RID: 29141 RVA: 0x0027D90C File Offset: 0x0027BB0C
		public virtual long TotalIn
		{
			get
			{
				return this._baseStream._z.TotalBytesIn;
			}
		}

		// Token: 0x1700134D RID: 4941
		// (get) Token: 0x060071D6 RID: 29142 RVA: 0x0027D91E File Offset: 0x0027BB1E
		public virtual long TotalOut
		{
			get
			{
				return this._baseStream._z.TotalBytesOut;
			}
		}

		// Token: 0x060071D7 RID: 29143 RVA: 0x0027D930 File Offset: 0x0027BB30
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (!this._disposed)
				{
					if (disposing && this._baseStream != null)
					{
						this._baseStream.Close();
					}
					this._disposed = true;
				}
			}
			finally
			{
				base.Dispose(disposing);
			}
		}

		// Token: 0x1700134E RID: 4942
		// (get) Token: 0x060071D8 RID: 29144 RVA: 0x0027D97C File Offset: 0x0027BB7C
		public override bool CanRead
		{
			get
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException("ZlibStream");
				}
				return this._baseStream._stream.CanRead;
			}
		}

		// Token: 0x1700134F RID: 4943
		// (get) Token: 0x060071D9 RID: 29145 RVA: 0x00010306 File Offset: 0x0000E506
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001350 RID: 4944
		// (get) Token: 0x060071DA RID: 29146 RVA: 0x0027D9A1 File Offset: 0x0027BBA1
		public override bool CanWrite
		{
			get
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException("ZlibStream");
				}
				return this._baseStream._stream.CanWrite;
			}
		}

		// Token: 0x060071DB RID: 29147 RVA: 0x0027D9C6 File Offset: 0x0027BBC6
		public override void Flush()
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException("ZlibStream");
			}
			this._baseStream.Flush();
		}

		// Token: 0x17001351 RID: 4945
		// (get) Token: 0x060071DC RID: 29148 RVA: 0x0027BDC1 File Offset: 0x00279FC1
		public override long Length
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x17001352 RID: 4946
		// (get) Token: 0x060071DD RID: 29149 RVA: 0x0027D9E8 File Offset: 0x0027BBE8
		// (set) Token: 0x060071DE RID: 29150 RVA: 0x0027BDC1 File Offset: 0x00279FC1
		public override long Position
		{
			get
			{
				if (this._baseStream._streamMode == ZlibBaseStream.StreamMode.Writer)
				{
					return this._baseStream._z.TotalBytesOut;
				}
				if (this._baseStream._streamMode == ZlibBaseStream.StreamMode.Reader)
				{
					return this._baseStream._z.TotalBytesIn;
				}
				return 0L;
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x060071DF RID: 29151 RVA: 0x0027DA34 File Offset: 0x0027BC34
		public override int Read(byte[] buffer, int offset, int count)
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException("ZlibStream");
			}
			return this._baseStream.Read(buffer, offset, count);
		}

		// Token: 0x060071E0 RID: 29152 RVA: 0x0027BDC1 File Offset: 0x00279FC1
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException();
		}

		// Token: 0x060071E1 RID: 29153 RVA: 0x0027BDC1 File Offset: 0x00279FC1
		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x060071E2 RID: 29154 RVA: 0x0027DA57 File Offset: 0x0027BC57
		public override void Write(byte[] buffer, int offset, int count)
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException("ZlibStream");
			}
			this._baseStream.Write(buffer, offset, count);
		}

		// Token: 0x060071E3 RID: 29155 RVA: 0x0027DA7C File Offset: 0x0027BC7C
		public static byte[] CompressString(string s)
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				Stream compressor = new ZlibStream(memoryStream, CompressionMode.Compress, CompressionLevel.BestCompression);
				ZlibBaseStream.CompressString(s, compressor);
				result = memoryStream.ToArray();
			}
			return result;
		}

		// Token: 0x060071E4 RID: 29156 RVA: 0x0027DAC4 File Offset: 0x0027BCC4
		public static byte[] CompressBuffer(byte[] b)
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				Stream compressor = new ZlibStream(memoryStream, CompressionMode.Compress, CompressionLevel.BestCompression);
				ZlibBaseStream.CompressBuffer(b, compressor);
				result = memoryStream.ToArray();
			}
			return result;
		}

		// Token: 0x060071E5 RID: 29157 RVA: 0x0027DB0C File Offset: 0x0027BD0C
		public static string UncompressString(byte[] compressed)
		{
			string result;
			using (MemoryStream memoryStream = new MemoryStream(compressed))
			{
				Stream decompressor = new ZlibStream(memoryStream, CompressionMode.Decompress);
				result = ZlibBaseStream.UncompressString(compressed, decompressor);
			}
			return result;
		}

		// Token: 0x060071E6 RID: 29158 RVA: 0x0027DB50 File Offset: 0x0027BD50
		public static byte[] UncompressBuffer(byte[] compressed)
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream(compressed))
			{
				Stream decompressor = new ZlibStream(memoryStream, CompressionMode.Decompress);
				result = ZlibBaseStream.UncompressBuffer(compressed, decompressor);
			}
			return result;
		}

		// Token: 0x0400467D RID: 18045
		internal ZlibBaseStream _baseStream;

		// Token: 0x0400467E RID: 18046
		private bool _disposed;
	}
}
