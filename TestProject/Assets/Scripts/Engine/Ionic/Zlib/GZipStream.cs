using System;
using System.IO;
using System.Text;

namespace Ionic.Zlib
{
	// Token: 0x020012AD RID: 4781
	public class GZipStream : Stream
	{
		// Token: 0x17001329 RID: 4905
		// (get) Token: 0x06007123 RID: 28963 RVA: 0x00277894 File Offset: 0x00275A94
		// (set) Token: 0x06007124 RID: 28964 RVA: 0x0027789C File Offset: 0x00275A9C
		public string Comment
		{
			get
			{
				return this._Comment;
			}
			set
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException("GZipStream");
				}
				this._Comment = value;
			}
		}

		// Token: 0x1700132A RID: 4906
		// (get) Token: 0x06007125 RID: 28965 RVA: 0x002778B8 File Offset: 0x00275AB8
		// (set) Token: 0x06007126 RID: 28966 RVA: 0x002778C0 File Offset: 0x00275AC0
		public string FileName
		{
			get
			{
				return this._FileName;
			}
			set
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException("GZipStream");
				}
				this._FileName = value;
				if (this._FileName == null)
				{
					return;
				}
				if (this._FileName.IndexOf("/") != -1)
				{
					this._FileName = this._FileName.Replace("/", "\\");
				}
				if (this._FileName.EndsWith("\\"))
				{
					throw new Exception("Illegal filename");
				}
				if (this._FileName.IndexOf("\\") != -1)
				{
					this._FileName = Path.GetFileName(this._FileName);
				}
			}
		}

		// Token: 0x1700132B RID: 4907
		// (get) Token: 0x06007127 RID: 28967 RVA: 0x0027795F File Offset: 0x00275B5F
		public int Crc32
		{
			get
			{
				return this._Crc32;
			}
		}

		// Token: 0x06007128 RID: 28968 RVA: 0x00277967 File Offset: 0x00275B67
		public GZipStream(Stream stream, CompressionMode mode) : this(stream, mode, CompressionLevel.Default, false)
		{
		}

		// Token: 0x06007129 RID: 28969 RVA: 0x00277973 File Offset: 0x00275B73
		public GZipStream(Stream stream, CompressionMode mode, CompressionLevel level) : this(stream, mode, level, false)
		{
		}

		// Token: 0x0600712A RID: 28970 RVA: 0x0027797F File Offset: 0x00275B7F
		public GZipStream(Stream stream, CompressionMode mode, bool leaveOpen) : this(stream, mode, CompressionLevel.Default, leaveOpen)
		{
		}

		// Token: 0x0600712B RID: 28971 RVA: 0x0027798B File Offset: 0x00275B8B
		public GZipStream(Stream stream, CompressionMode mode, CompressionLevel level, bool leaveOpen)
		{
			this._baseStream = new ZlibBaseStream(stream, mode, level, ZlibStreamFlavor.GZIP, leaveOpen);
		}

		// Token: 0x1700132C RID: 4908
		// (get) Token: 0x0600712C RID: 28972 RVA: 0x002779A8 File Offset: 0x00275BA8
		// (set) Token: 0x0600712D RID: 28973 RVA: 0x002779B5 File Offset: 0x00275BB5
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
					throw new ObjectDisposedException("GZipStream");
				}
				this._baseStream._flushMode = value;
			}
		}

		// Token: 0x1700132D RID: 4909
		// (get) Token: 0x0600712E RID: 28974 RVA: 0x002779D6 File Offset: 0x00275BD6
		// (set) Token: 0x0600712F RID: 28975 RVA: 0x002779E4 File Offset: 0x00275BE4
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
					throw new ObjectDisposedException("GZipStream");
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

		// Token: 0x1700132E RID: 4910
		// (get) Token: 0x06007130 RID: 28976 RVA: 0x00277A50 File Offset: 0x00275C50
		public virtual long TotalIn
		{
			get
			{
				return this._baseStream._z.TotalBytesIn;
			}
		}

		// Token: 0x1700132F RID: 4911
		// (get) Token: 0x06007131 RID: 28977 RVA: 0x00277A62 File Offset: 0x00275C62
		public virtual long TotalOut
		{
			get
			{
				return this._baseStream._z.TotalBytesOut;
			}
		}

		// Token: 0x06007132 RID: 28978 RVA: 0x00277A74 File Offset: 0x00275C74
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (!this._disposed)
				{
					if (disposing && this._baseStream != null)
					{
						this._baseStream.Close();
						this._Crc32 = this._baseStream.Crc32;
					}
					this._disposed = true;
				}
			}
			finally
			{
				base.Dispose(disposing);
			}
		}

		// Token: 0x17001330 RID: 4912
		// (get) Token: 0x06007133 RID: 28979 RVA: 0x00277AD4 File Offset: 0x00275CD4
		public override bool CanRead
		{
			get
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException("GZipStream");
				}
				return this._baseStream._stream.CanRead;
			}
		}

		// Token: 0x17001331 RID: 4913
		// (get) Token: 0x06007134 RID: 28980 RVA: 0x00010306 File Offset: 0x0000E506
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001332 RID: 4914
		// (get) Token: 0x06007135 RID: 28981 RVA: 0x00277AF9 File Offset: 0x00275CF9
		public override bool CanWrite
		{
			get
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException("GZipStream");
				}
				return this._baseStream._stream.CanWrite;
			}
		}

		// Token: 0x06007136 RID: 28982 RVA: 0x00277B1E File Offset: 0x00275D1E
		public override void Flush()
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException("GZipStream");
			}
			this._baseStream.Flush();
		}

		// Token: 0x17001333 RID: 4915
		// (get) Token: 0x06007137 RID: 28983 RVA: 0x000255BF File Offset: 0x000237BF
		public override long Length
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x17001334 RID: 4916
		// (get) Token: 0x06007138 RID: 28984 RVA: 0x00277B40 File Offset: 0x00275D40
		// (set) Token: 0x06007139 RID: 28985 RVA: 0x000255BF File Offset: 0x000237BF
		public override long Position
		{
			get
			{
				if (this._baseStream._streamMode == ZlibBaseStream.StreamMode.Writer)
				{
					return this._baseStream._z.TotalBytesOut + (long)this._headerByteCount;
				}
				if (this._baseStream._streamMode == ZlibBaseStream.StreamMode.Reader)
				{
					return this._baseStream._z.TotalBytesIn + (long)this._baseStream._gzipHeaderByteCount;
				}
				return 0L;
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x0600713A RID: 28986 RVA: 0x00277BA4 File Offset: 0x00275DA4
		public override int Read(byte[] buffer, int offset, int count)
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException("GZipStream");
			}
			int result = this._baseStream.Read(buffer, offset, count);
			if (!this._firstReadDone)
			{
				this._firstReadDone = true;
				this.FileName = this._baseStream._GzipFileName;
				this.Comment = this._baseStream._GzipComment;
			}
			return result;
		}

		// Token: 0x0600713B RID: 28987 RVA: 0x000255BF File Offset: 0x000237BF
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600713C RID: 28988 RVA: 0x000255BF File Offset: 0x000237BF
		public override void SetLength(long value)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600713D RID: 28989 RVA: 0x00277C04 File Offset: 0x00275E04
		public override void Write(byte[] buffer, int offset, int count)
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException("GZipStream");
			}
			if (this._baseStream._streamMode == ZlibBaseStream.StreamMode.Undefined)
			{
				if (!this._baseStream._wantCompress)
				{
					throw new InvalidOperationException();
				}
				this._headerByteCount = this.EmitHeader();
			}
			this._baseStream.Write(buffer, offset, count);
		}

		// Token: 0x0600713E RID: 28990 RVA: 0x00277C64 File Offset: 0x00275E64
		private int EmitHeader()
		{
			byte[] array = (this.Comment == null) ? null : GZipStream.iso8859dash1.GetBytes(this.Comment);
			byte[] array2 = (this.FileName == null) ? null : GZipStream.iso8859dash1.GetBytes(this.FileName);
			int num = (this.Comment == null) ? 0 : (array.Length + 1);
			int num2 = (this.FileName == null) ? 0 : (array2.Length + 1);
			byte[] array3 = new byte[10 + num + num2];
			int num3 = 0;
			array3[num3++] = 31;
			array3[num3++] = 139;
			array3[num3++] = 8;
			byte b = 0;
			if (this.Comment != null)
			{
				b ^= 16;
			}
			if (this.FileName != null)
			{
				b ^= 8;
			}
			array3[num3++] = b;
			if (this.LastModified == null)
			{
				this.LastModified = new DateTime?(DateTime.Now);
			}
			Array.Copy(BitConverter.GetBytes((int)(this.LastModified.Value - GZipStream._unixEpoch).TotalSeconds), 0, array3, num3, 4);
			num3 += 4;
			array3[num3++] = 0;
			array3[num3++] = byte.MaxValue;
			if (num2 != 0)
			{
				Array.Copy(array2, 0, array3, num3, num2 - 1);
				num3 += num2 - 1;
				array3[num3++] = 0;
			}
			if (num != 0)
			{
				Array.Copy(array, 0, array3, num3, num - 1);
				num3 += num - 1;
				array3[num3++] = 0;
			}
			this._baseStream._stream.Write(array3, 0, array3.Length);
			return array3.Length;
		}

		// Token: 0x0600713F RID: 28991 RVA: 0x00277E00 File Offset: 0x00276000
		public static byte[] CompressString(string s)
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				Stream compressor = new GZipStream(memoryStream, CompressionMode.Compress, CompressionLevel.BestCompression);
				ZlibBaseStream.CompressString(s, compressor);
				result = memoryStream.ToArray();
			}
			return result;
		}

		// Token: 0x06007140 RID: 28992 RVA: 0x00277E48 File Offset: 0x00276048
		public static byte[] CompressBuffer(byte[] b)
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				Stream compressor = new GZipStream(memoryStream, CompressionMode.Compress, CompressionLevel.BestCompression);
				ZlibBaseStream.CompressBuffer(b, compressor);
				result = memoryStream.ToArray();
			}
			return result;
		}

		// Token: 0x06007141 RID: 28993 RVA: 0x00277E90 File Offset: 0x00276090
		public static string UncompressString(byte[] compressed)
		{
			string result;
			using (MemoryStream memoryStream = new MemoryStream(compressed))
			{
				Stream decompressor = new GZipStream(memoryStream, CompressionMode.Decompress);
				result = ZlibBaseStream.UncompressString(compressed, decompressor);
			}
			return result;
		}

		// Token: 0x06007142 RID: 28994 RVA: 0x00277ED4 File Offset: 0x002760D4
		public static byte[] UncompressBuffer(byte[] compressed)
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream(compressed))
			{
				Stream decompressor = new GZipStream(memoryStream, CompressionMode.Decompress);
				result = ZlibBaseStream.UncompressBuffer(compressed, decompressor);
			}
			return result;
		}

		// Token: 0x0400458D RID: 17805
		public DateTime? LastModified;

		// Token: 0x0400458E RID: 17806
		private int _headerByteCount;

		// Token: 0x0400458F RID: 17807
		internal ZlibBaseStream _baseStream;

		// Token: 0x04004590 RID: 17808
		private bool _disposed;

		// Token: 0x04004591 RID: 17809
		private bool _firstReadDone;

		// Token: 0x04004592 RID: 17810
		private string _FileName;

		// Token: 0x04004593 RID: 17811
		private string _Comment;

		// Token: 0x04004594 RID: 17812
		private int _Crc32;

		// Token: 0x04004595 RID: 17813
		internal static readonly DateTime _unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

		// Token: 0x04004596 RID: 17814
		internal static readonly Encoding iso8859dash1 = Encoding.GetEncoding("iso-8859-1");
	}
}
