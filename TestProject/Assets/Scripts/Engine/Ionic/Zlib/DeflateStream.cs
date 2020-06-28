using System;
using System.IO;

namespace Ionic.Zlib
{
	// Token: 0x020012AC RID: 4780
	public class DeflateStream : Stream
	{
		// Token: 0x06007107 RID: 28935 RVA: 0x002774ED File Offset: 0x002756ED
		public DeflateStream(Stream stream, CompressionMode mode) : this(stream, mode, CompressionLevel.Default, false)
		{
		}

		// Token: 0x06007108 RID: 28936 RVA: 0x002774F9 File Offset: 0x002756F9
		public DeflateStream(Stream stream, CompressionMode mode, CompressionLevel level) : this(stream, mode, level, false)
		{
		}

		// Token: 0x06007109 RID: 28937 RVA: 0x00277505 File Offset: 0x00275705
		public DeflateStream(Stream stream, CompressionMode mode, bool leaveOpen) : this(stream, mode, CompressionLevel.Default, leaveOpen)
		{
		}

		// Token: 0x0600710A RID: 28938 RVA: 0x00277511 File Offset: 0x00275711
		public DeflateStream(Stream stream, CompressionMode mode, CompressionLevel level, bool leaveOpen)
		{
			this._innerStream = stream;
			this._baseStream = new ZlibBaseStream(stream, mode, level, ZlibStreamFlavor.DEFLATE, leaveOpen);
		}

		// Token: 0x1700131F RID: 4895
		// (get) Token: 0x0600710B RID: 28939 RVA: 0x00277535 File Offset: 0x00275735
		// (set) Token: 0x0600710C RID: 28940 RVA: 0x00277542 File Offset: 0x00275742
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
					throw new ObjectDisposedException("DeflateStream");
				}
				this._baseStream._flushMode = value;
			}
		}

		// Token: 0x17001320 RID: 4896
		// (get) Token: 0x0600710D RID: 28941 RVA: 0x00277563 File Offset: 0x00275763
		// (set) Token: 0x0600710E RID: 28942 RVA: 0x00277570 File Offset: 0x00275770
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
					throw new ObjectDisposedException("DeflateStream");
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

		// Token: 0x17001321 RID: 4897
		// (get) Token: 0x0600710F RID: 28943 RVA: 0x002775DC File Offset: 0x002757DC
		// (set) Token: 0x06007110 RID: 28944 RVA: 0x002775E9 File Offset: 0x002757E9
		public CompressionStrategy Strategy
		{
			get
			{
				return this._baseStream.Strategy;
			}
			set
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException("DeflateStream");
				}
				this._baseStream.Strategy = value;
			}
		}

		// Token: 0x17001322 RID: 4898
		// (get) Token: 0x06007111 RID: 28945 RVA: 0x0027760A File Offset: 0x0027580A
		public virtual long TotalIn
		{
			get
			{
				return this._baseStream._z.TotalBytesIn;
			}
		}

		// Token: 0x17001323 RID: 4899
		// (get) Token: 0x06007112 RID: 28946 RVA: 0x0027761C File Offset: 0x0027581C
		public virtual long TotalOut
		{
			get
			{
				return this._baseStream._z.TotalBytesOut;
			}
		}

		// Token: 0x06007113 RID: 28947 RVA: 0x00277630 File Offset: 0x00275830
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

		// Token: 0x17001324 RID: 4900
		// (get) Token: 0x06007114 RID: 28948 RVA: 0x0027767C File Offset: 0x0027587C
		public override bool CanRead
		{
			get
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException("DeflateStream");
				}
				return this._baseStream._stream.CanRead;
			}
		}

		// Token: 0x17001325 RID: 4901
		// (get) Token: 0x06007115 RID: 28949 RVA: 0x00010306 File Offset: 0x0000E506
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001326 RID: 4902
		// (get) Token: 0x06007116 RID: 28950 RVA: 0x002776A1 File Offset: 0x002758A1
		public override bool CanWrite
		{
			get
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException("DeflateStream");
				}
				return this._baseStream._stream.CanWrite;
			}
		}

		// Token: 0x06007117 RID: 28951 RVA: 0x002776C6 File Offset: 0x002758C6
		public override void Flush()
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException("DeflateStream");
			}
			this._baseStream.Flush();
		}

		// Token: 0x17001327 RID: 4903
		// (get) Token: 0x06007118 RID: 28952 RVA: 0x000255BF File Offset: 0x000237BF
		public override long Length
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x17001328 RID: 4904
		// (get) Token: 0x06007119 RID: 28953 RVA: 0x002776E8 File Offset: 0x002758E8
		// (set) Token: 0x0600711A RID: 28954 RVA: 0x000255BF File Offset: 0x000237BF
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
				throw new NotImplementedException();
			}
		}

		// Token: 0x0600711B RID: 28955 RVA: 0x00277734 File Offset: 0x00275934
		public override int Read(byte[] buffer, int offset, int count)
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException("DeflateStream");
			}
			return this._baseStream.Read(buffer, offset, count);
		}

		// Token: 0x0600711C RID: 28956 RVA: 0x000255BF File Offset: 0x000237BF
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600711D RID: 28957 RVA: 0x000255BF File Offset: 0x000237BF
		public override void SetLength(long value)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600711E RID: 28958 RVA: 0x00277757 File Offset: 0x00275957
		public override void Write(byte[] buffer, int offset, int count)
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException("DeflateStream");
			}
			this._baseStream.Write(buffer, offset, count);
		}

		// Token: 0x0600711F RID: 28959 RVA: 0x0027777C File Offset: 0x0027597C
		public static byte[] CompressString(string s)
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				Stream compressor = new DeflateStream(memoryStream, CompressionMode.Compress, CompressionLevel.BestCompression);
				ZlibBaseStream.CompressString(s, compressor);
				result = memoryStream.ToArray();
			}
			return result;
		}

		// Token: 0x06007120 RID: 28960 RVA: 0x002777C4 File Offset: 0x002759C4
		public static byte[] CompressBuffer(byte[] b)
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				Stream compressor = new DeflateStream(memoryStream, CompressionMode.Compress, CompressionLevel.BestCompression);
				ZlibBaseStream.CompressBuffer(b, compressor);
				result = memoryStream.ToArray();
			}
			return result;
		}

		// Token: 0x06007121 RID: 28961 RVA: 0x0027780C File Offset: 0x00275A0C
		public static string UncompressString(byte[] compressed)
		{
			string result;
			using (MemoryStream memoryStream = new MemoryStream(compressed))
			{
				Stream decompressor = new DeflateStream(memoryStream, CompressionMode.Decompress);
				result = ZlibBaseStream.UncompressString(compressed, decompressor);
			}
			return result;
		}

		// Token: 0x06007122 RID: 28962 RVA: 0x00277850 File Offset: 0x00275A50
		public static byte[] UncompressBuffer(byte[] compressed)
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream(compressed))
			{
				Stream decompressor = new DeflateStream(memoryStream, CompressionMode.Decompress);
				result = ZlibBaseStream.UncompressBuffer(compressed, decompressor);
			}
			return result;
		}

		// Token: 0x0400458A RID: 17802
		internal ZlibBaseStream _baseStream;

		// Token: 0x0400458B RID: 17803
		internal Stream _innerStream;

		// Token: 0x0400458C RID: 17804
		private bool _disposed;
	}
}
