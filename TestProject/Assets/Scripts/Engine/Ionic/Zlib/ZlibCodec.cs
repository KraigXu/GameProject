using System;
using System.Runtime.InteropServices;

namespace Ionic.Zlib
{
	// Token: 0x020012C1 RID: 4801
	[Guid("ebc25cf6-9120-4283-b972-0e5520d0000D")]
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	public sealed class ZlibCodec
	{
		// Token: 0x17001349 RID: 4937
		// (get) Token: 0x060071B6 RID: 29110 RVA: 0x0027D3A4 File Offset: 0x0027B5A4
		public int Adler32
		{
			get
			{
				return (int)this._Adler32;
			}
		}

		// Token: 0x060071B7 RID: 29111 RVA: 0x0027D3AC File Offset: 0x0027B5AC
		public ZlibCodec()
		{
		}

		// Token: 0x060071B8 RID: 29112 RVA: 0x0027D3C4 File Offset: 0x0027B5C4
		public ZlibCodec(CompressionMode mode)
		{
			if (mode == CompressionMode.Compress)
			{
				if (this.InitializeDeflate() != 0)
				{
					throw new ZlibException("Cannot initialize for deflate.");
				}
			}
			else
			{
				if (mode != CompressionMode.Decompress)
				{
					throw new ZlibException("Invalid ZlibStreamFlavor.");
				}
				if (this.InitializeInflate() != 0)
				{
					throw new ZlibException("Cannot initialize for inflate.");
				}
			}
		}

		// Token: 0x060071B9 RID: 29113 RVA: 0x0027D41E File Offset: 0x0027B61E
		public int InitializeInflate()
		{
			return this.InitializeInflate(this.WindowBits);
		}

		// Token: 0x060071BA RID: 29114 RVA: 0x0027D42C File Offset: 0x0027B62C
		public int InitializeInflate(bool expectRfc1950Header)
		{
			return this.InitializeInflate(this.WindowBits, expectRfc1950Header);
		}

		// Token: 0x060071BB RID: 29115 RVA: 0x0027D43B File Offset: 0x0027B63B
		public int InitializeInflate(int windowBits)
		{
			this.WindowBits = windowBits;
			return this.InitializeInflate(windowBits, true);
		}

		// Token: 0x060071BC RID: 29116 RVA: 0x0027D44C File Offset: 0x0027B64C
		public int InitializeInflate(int windowBits, bool expectRfc1950Header)
		{
			this.WindowBits = windowBits;
			if (this.dstate != null)
			{
				throw new ZlibException("You may not call InitializeInflate() after calling InitializeDeflate().");
			}
			this.istate = new InflateManager(expectRfc1950Header);
			return this.istate.Initialize(this, windowBits);
		}

		// Token: 0x060071BD RID: 29117 RVA: 0x0027D481 File Offset: 0x0027B681
		public int Inflate(FlushType flush)
		{
			if (this.istate == null)
			{
				throw new ZlibException("No Inflate State!");
			}
			return this.istate.Inflate(flush);
		}

		// Token: 0x060071BE RID: 29118 RVA: 0x0027D4A2 File Offset: 0x0027B6A2
		public int EndInflate()
		{
			if (this.istate == null)
			{
				throw new ZlibException("No Inflate State!");
			}
			int result = this.istate.End();
			this.istate = null;
			return result;
		}

		// Token: 0x060071BF RID: 29119 RVA: 0x0027D4C9 File Offset: 0x0027B6C9
		public int SyncInflate()
		{
			if (this.istate == null)
			{
				throw new ZlibException("No Inflate State!");
			}
			return this.istate.Sync();
		}

		// Token: 0x060071C0 RID: 29120 RVA: 0x0027D4E9 File Offset: 0x0027B6E9
		public int InitializeDeflate()
		{
			return this._InternalInitializeDeflate(true);
		}

		// Token: 0x060071C1 RID: 29121 RVA: 0x0027D4F2 File Offset: 0x0027B6F2
		public int InitializeDeflate(CompressionLevel level)
		{
			this.CompressLevel = level;
			return this._InternalInitializeDeflate(true);
		}

		// Token: 0x060071C2 RID: 29122 RVA: 0x0027D502 File Offset: 0x0027B702
		public int InitializeDeflate(CompressionLevel level, bool wantRfc1950Header)
		{
			this.CompressLevel = level;
			return this._InternalInitializeDeflate(wantRfc1950Header);
		}

		// Token: 0x060071C3 RID: 29123 RVA: 0x0027D512 File Offset: 0x0027B712
		public int InitializeDeflate(CompressionLevel level, int bits)
		{
			this.CompressLevel = level;
			this.WindowBits = bits;
			return this._InternalInitializeDeflate(true);
		}

		// Token: 0x060071C4 RID: 29124 RVA: 0x0027D529 File Offset: 0x0027B729
		public int InitializeDeflate(CompressionLevel level, int bits, bool wantRfc1950Header)
		{
			this.CompressLevel = level;
			this.WindowBits = bits;
			return this._InternalInitializeDeflate(wantRfc1950Header);
		}

		// Token: 0x060071C5 RID: 29125 RVA: 0x0027D540 File Offset: 0x0027B740
		private int _InternalInitializeDeflate(bool wantRfc1950Header)
		{
			if (this.istate != null)
			{
				throw new ZlibException("You may not call InitializeDeflate() after calling InitializeInflate().");
			}
			this.dstate = new DeflateManager();
			this.dstate.WantRfc1950HeaderBytes = wantRfc1950Header;
			return this.dstate.Initialize(this, this.CompressLevel, this.WindowBits, this.Strategy);
		}

		// Token: 0x060071C6 RID: 29126 RVA: 0x0027D595 File Offset: 0x0027B795
		public int Deflate(FlushType flush)
		{
			if (this.dstate == null)
			{
				throw new ZlibException("No Deflate State!");
			}
			return this.dstate.Deflate(flush);
		}

		// Token: 0x060071C7 RID: 29127 RVA: 0x0027D5B6 File Offset: 0x0027B7B6
		public int EndDeflate()
		{
			if (this.dstate == null)
			{
				throw new ZlibException("No Deflate State!");
			}
			this.dstate = null;
			return 0;
		}

		// Token: 0x060071C8 RID: 29128 RVA: 0x0027D5D3 File Offset: 0x0027B7D3
		public void ResetDeflate()
		{
			if (this.dstate == null)
			{
				throw new ZlibException("No Deflate State!");
			}
			this.dstate.Reset();
		}

		// Token: 0x060071C9 RID: 29129 RVA: 0x0027D5F3 File Offset: 0x0027B7F3
		public int SetDeflateParams(CompressionLevel level, CompressionStrategy strategy)
		{
			if (this.dstate == null)
			{
				throw new ZlibException("No Deflate State!");
			}
			return this.dstate.SetParams(level, strategy);
		}

		// Token: 0x060071CA RID: 29130 RVA: 0x0027D615 File Offset: 0x0027B815
		public int SetDictionary(byte[] dictionary)
		{
			if (this.istate != null)
			{
				return this.istate.SetDictionary(dictionary);
			}
			if (this.dstate != null)
			{
				return this.dstate.SetDictionary(dictionary);
			}
			throw new ZlibException("No Inflate or Deflate state!");
		}

		// Token: 0x060071CB RID: 29131 RVA: 0x0027D64C File Offset: 0x0027B84C
		internal void flush_pending()
		{
			int num = this.dstate.pendingCount;
			if (num > this.AvailableBytesOut)
			{
				num = this.AvailableBytesOut;
			}
			if (num == 0)
			{
				return;
			}
			if (this.dstate.pending.Length <= this.dstate.nextPending || this.OutputBuffer.Length <= this.NextOut || this.dstate.pending.Length < this.dstate.nextPending + num || this.OutputBuffer.Length < this.NextOut + num)
			{
				throw new ZlibException(string.Format("Invalid State. (pending.Length={0}, pendingCount={1})", this.dstate.pending.Length, this.dstate.pendingCount));
			}
			Array.Copy(this.dstate.pending, this.dstate.nextPending, this.OutputBuffer, this.NextOut, num);
			this.NextOut += num;
			this.dstate.nextPending += num;
			this.TotalBytesOut += (long)num;
			this.AvailableBytesOut -= num;
			this.dstate.pendingCount -= num;
			if (this.dstate.pendingCount == 0)
			{
				this.dstate.nextPending = 0;
			}
		}

		// Token: 0x060071CC RID: 29132 RVA: 0x0027D798 File Offset: 0x0027B998
		internal int read_buf(byte[] buf, int start, int size)
		{
			int num = this.AvailableBytesIn;
			if (num > size)
			{
				num = size;
			}
			if (num == 0)
			{
				return 0;
			}
			this.AvailableBytesIn -= num;
			if (this.dstate.WantRfc1950HeaderBytes)
			{
				this._Adler32 = Adler.Adler32(this._Adler32, this.InputBuffer, this.NextIn, num);
			}
			Array.Copy(this.InputBuffer, this.NextIn, buf, start, num);
			this.NextIn += num;
			this.TotalBytesIn += (long)num;
			return num;
		}

		// Token: 0x04004664 RID: 18020
		public byte[] InputBuffer;

		// Token: 0x04004665 RID: 18021
		public int NextIn;

		// Token: 0x04004666 RID: 18022
		public int AvailableBytesIn;

		// Token: 0x04004667 RID: 18023
		public long TotalBytesIn;

		// Token: 0x04004668 RID: 18024
		public byte[] OutputBuffer;

		// Token: 0x04004669 RID: 18025
		public int NextOut;

		// Token: 0x0400466A RID: 18026
		public int AvailableBytesOut;

		// Token: 0x0400466B RID: 18027
		public long TotalBytesOut;

		// Token: 0x0400466C RID: 18028
		public string Message;

		// Token: 0x0400466D RID: 18029
		internal DeflateManager dstate;

		// Token: 0x0400466E RID: 18030
		internal InflateManager istate;

		// Token: 0x0400466F RID: 18031
		internal uint _Adler32;

		// Token: 0x04004670 RID: 18032
		public CompressionLevel CompressLevel = CompressionLevel.Default;

		// Token: 0x04004671 RID: 18033
		public int WindowBits = 15;

		// Token: 0x04004672 RID: 18034
		public CompressionStrategy Strategy;
	}
}
