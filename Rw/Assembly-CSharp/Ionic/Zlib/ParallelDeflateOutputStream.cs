using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using Ionic.Crc;

namespace Ionic.Zlib
{
	// Token: 0x020012B4 RID: 4788
	public class ParallelDeflateOutputStream : Stream
	{
		// Token: 0x06007165 RID: 29029 RVA: 0x0027B43E File Offset: 0x0027963E
		public ParallelDeflateOutputStream(Stream stream) : this(stream, CompressionLevel.Default, CompressionStrategy.Default, false)
		{
		}

		// Token: 0x06007166 RID: 29030 RVA: 0x0027B44A File Offset: 0x0027964A
		public ParallelDeflateOutputStream(Stream stream, CompressionLevel level) : this(stream, level, CompressionStrategy.Default, false)
		{
		}

		// Token: 0x06007167 RID: 29031 RVA: 0x0027B456 File Offset: 0x00279656
		public ParallelDeflateOutputStream(Stream stream, bool leaveOpen) : this(stream, CompressionLevel.Default, CompressionStrategy.Default, leaveOpen)
		{
		}

		// Token: 0x06007168 RID: 29032 RVA: 0x0027B462 File Offset: 0x00279662
		public ParallelDeflateOutputStream(Stream stream, CompressionLevel level, bool leaveOpen) : this(stream, CompressionLevel.Default, CompressionStrategy.Default, leaveOpen)
		{
		}

		// Token: 0x06007169 RID: 29033 RVA: 0x0027B470 File Offset: 0x00279670
		public ParallelDeflateOutputStream(Stream stream, CompressionLevel level, CompressionStrategy strategy, bool leaveOpen)
		{
			this._outStream = stream;
			this._compressLevel = level;
			this.Strategy = strategy;
			this._leaveOpen = leaveOpen;
			this.MaxBufferPairs = 16;
		}

		// Token: 0x17001336 RID: 4918
		// (get) Token: 0x0600716A RID: 29034 RVA: 0x0027B4DF File Offset: 0x002796DF
		// (set) Token: 0x0600716B RID: 29035 RVA: 0x0027B4E7 File Offset: 0x002796E7
		public CompressionStrategy Strategy { get; private set; }

		// Token: 0x17001337 RID: 4919
		// (get) Token: 0x0600716C RID: 29036 RVA: 0x0027B4F0 File Offset: 0x002796F0
		// (set) Token: 0x0600716D RID: 29037 RVA: 0x0027B4F8 File Offset: 0x002796F8
		public int MaxBufferPairs
		{
			get
			{
				return this._maxBufferPairs;
			}
			set
			{
				if (value < 4)
				{
					throw new ArgumentException("MaxBufferPairs", "Value must be 4 or greater.");
				}
				this._maxBufferPairs = value;
			}
		}

		// Token: 0x17001338 RID: 4920
		// (get) Token: 0x0600716E RID: 29038 RVA: 0x0027B515 File Offset: 0x00279715
		// (set) Token: 0x0600716F RID: 29039 RVA: 0x0027B51D File Offset: 0x0027971D
		public int BufferSize
		{
			get
			{
				return this._bufferSize;
			}
			set
			{
				if (value < 1024)
				{
					throw new ArgumentOutOfRangeException("BufferSize", "BufferSize must be greater than 1024 bytes");
				}
				this._bufferSize = value;
			}
		}

		// Token: 0x17001339 RID: 4921
		// (get) Token: 0x06007170 RID: 29040 RVA: 0x0027B53E File Offset: 0x0027973E
		public int Crc32
		{
			get
			{
				return this._Crc32;
			}
		}

		// Token: 0x1700133A RID: 4922
		// (get) Token: 0x06007171 RID: 29041 RVA: 0x0027B546 File Offset: 0x00279746
		public long BytesProcessed
		{
			get
			{
				return this._totalBytesProcessed;
			}
		}

		// Token: 0x06007172 RID: 29042 RVA: 0x0027B550 File Offset: 0x00279750
		private void _InitializePoolOfWorkItems()
		{
			this._toWrite = new Queue<int>();
			this._toFill = new Queue<int>();
			this._pool = new List<WorkItem>();
			int num = ParallelDeflateOutputStream.BufferPairsPerCore * Environment.ProcessorCount;
			num = Math.Min(num, this._maxBufferPairs);
			for (int i = 0; i < num; i++)
			{
				this._pool.Add(new WorkItem(this._bufferSize, this._compressLevel, this.Strategy, i));
				this._toFill.Enqueue(i);
			}
			this._newlyCompressedBlob = new AutoResetEvent(false);
			this._runningCrc = new CRC32();
			this._currentlyFilling = -1;
			this._lastFilled = -1;
			this._lastWritten = -1;
			this._latestCompressed = -1;
		}

		// Token: 0x06007173 RID: 29043 RVA: 0x0027B608 File Offset: 0x00279808
		public override void Write(byte[] buffer, int offset, int count)
		{
			bool mustWait = false;
			if (this._isClosed)
			{
				throw new InvalidOperationException();
			}
			if (this._pendingException != null)
			{
				this._handlingException = true;
				Exception pendingException = this._pendingException;
				this._pendingException = null;
				throw pendingException;
			}
			if (count == 0)
			{
				return;
			}
			if (!this._firstWriteDone)
			{
				this._InitializePoolOfWorkItems();
				this._firstWriteDone = true;
			}
			for (;;)
			{
				this.EmitPendingBuffers(false, mustWait);
				mustWait = false;
				int num;
				if (this._currentlyFilling >= 0)
				{
					num = this._currentlyFilling;
					goto IL_98;
				}
				if (this._toFill.Count != 0)
				{
					num = this._toFill.Dequeue();
					this._lastFilled++;
					goto IL_98;
				}
				mustWait = true;
				IL_145:
				if (count <= 0)
				{
					return;
				}
				continue;
				IL_98:
				WorkItem workItem = this._pool[num];
				int num2 = (workItem.buffer.Length - workItem.inputBytesAvailable > count) ? count : (workItem.buffer.Length - workItem.inputBytesAvailable);
				workItem.ordinal = this._lastFilled;
				Buffer.BlockCopy(buffer, offset, workItem.buffer, workItem.inputBytesAvailable, num2);
				count -= num2;
				offset += num2;
				workItem.inputBytesAvailable += num2;
				if (workItem.inputBytesAvailable == workItem.buffer.Length)
				{
					if (!ThreadPool.QueueUserWorkItem(new WaitCallback(this._DeflateOne), workItem))
					{
						break;
					}
					this._currentlyFilling = -1;
				}
				else
				{
					this._currentlyFilling = num;
				}
				goto IL_145;
			}
			throw new Exception("Cannot enqueue workitem");
		}

		// Token: 0x06007174 RID: 29044 RVA: 0x0027B764 File Offset: 0x00279964
		private void _FlushFinish()
		{
			byte[] array = new byte[128];
			ZlibCodec zlibCodec = new ZlibCodec();
			int num = zlibCodec.InitializeDeflate(this._compressLevel, false);
			zlibCodec.InputBuffer = null;
			zlibCodec.NextIn = 0;
			zlibCodec.AvailableBytesIn = 0;
			zlibCodec.OutputBuffer = array;
			zlibCodec.NextOut = 0;
			zlibCodec.AvailableBytesOut = array.Length;
			num = zlibCodec.Deflate(FlushType.Finish);
			if (num != 1 && num != 0)
			{
				throw new Exception("deflating: " + zlibCodec.Message);
			}
			if (array.Length - zlibCodec.AvailableBytesOut > 0)
			{
				this._outStream.Write(array, 0, array.Length - zlibCodec.AvailableBytesOut);
			}
			zlibCodec.EndDeflate();
			this._Crc32 = this._runningCrc.Crc32Result;
		}

		// Token: 0x06007175 RID: 29045 RVA: 0x0027B820 File Offset: 0x00279A20
		private void _Flush(bool lastInput)
		{
			if (this._isClosed)
			{
				throw new InvalidOperationException();
			}
			if (this.emitting)
			{
				return;
			}
			if (this._currentlyFilling >= 0)
			{
				WorkItem wi = this._pool[this._currentlyFilling];
				this._DeflateOne(wi);
				this._currentlyFilling = -1;
			}
			if (lastInput)
			{
				this.EmitPendingBuffers(true, false);
				this._FlushFinish();
				return;
			}
			this.EmitPendingBuffers(false, false);
		}

		// Token: 0x06007176 RID: 29046 RVA: 0x0027B887 File Offset: 0x00279A87
		public override void Flush()
		{
			if (this._pendingException != null)
			{
				this._handlingException = true;
				Exception pendingException = this._pendingException;
				this._pendingException = null;
				throw pendingException;
			}
			if (this._handlingException)
			{
				return;
			}
			this._Flush(false);
		}

		// Token: 0x06007177 RID: 29047 RVA: 0x0027B8BC File Offset: 0x00279ABC
		public override void Close()
		{
			if (this._pendingException != null)
			{
				this._handlingException = true;
				Exception pendingException = this._pendingException;
				this._pendingException = null;
				throw pendingException;
			}
			if (this._handlingException)
			{
				return;
			}
			if (this._isClosed)
			{
				return;
			}
			this._Flush(true);
			if (!this._leaveOpen)
			{
				this._outStream.Close();
			}
			this._isClosed = true;
		}

		// Token: 0x06007178 RID: 29048 RVA: 0x0027B91F File Offset: 0x00279B1F
		public new void Dispose()
		{
			this.Close();
			this._pool = null;
			this.Dispose(true);
		}

		// Token: 0x06007179 RID: 29049 RVA: 0x0027B935 File Offset: 0x00279B35
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
		}

		// Token: 0x0600717A RID: 29050 RVA: 0x0027B940 File Offset: 0x00279B40
		public void Reset(Stream stream)
		{
			if (!this._firstWriteDone)
			{
				return;
			}
			this._toWrite.Clear();
			this._toFill.Clear();
			foreach (WorkItem workItem in this._pool)
			{
				this._toFill.Enqueue(workItem.index);
				workItem.ordinal = -1;
			}
			this._firstWriteDone = false;
			this._totalBytesProcessed = 0L;
			this._runningCrc = new CRC32();
			this._isClosed = false;
			this._currentlyFilling = -1;
			this._lastFilled = -1;
			this._lastWritten = -1;
			this._latestCompressed = -1;
			this._outStream = stream;
		}

		// Token: 0x0600717B RID: 29051 RVA: 0x0027BA08 File Offset: 0x00279C08
		private void EmitPendingBuffers(bool doAll, bool mustWait)
		{
			if (this.emitting)
			{
				return;
			}
			this.emitting = true;
			if (doAll || mustWait)
			{
				this._newlyCompressedBlob.WaitOne();
			}
			do
			{
				int num = -1;
				int num2 = doAll ? 200 : (mustWait ? -1 : 0);
				int num3 = -1;
				do
				{
					if (Monitor.TryEnter(this._toWrite, num2))
					{
						num3 = -1;
						try
						{
							if (this._toWrite.Count > 0)
							{
								num3 = this._toWrite.Dequeue();
							}
						}
						finally
						{
							Monitor.Exit(this._toWrite);
						}
						if (num3 >= 0)
						{
							WorkItem workItem = this._pool[num3];
							if (workItem.ordinal != this._lastWritten + 1)
							{
								Queue<int> toWrite = this._toWrite;
								lock (toWrite)
								{
									this._toWrite.Enqueue(num3);
								}
								if (num == num3)
								{
									this._newlyCompressedBlob.WaitOne();
									num = -1;
								}
								else if (num == -1)
								{
									num = num3;
								}
							}
							else
							{
								num = -1;
								this._outStream.Write(workItem.compressed, 0, workItem.compressedBytesAvailable);
								this._runningCrc.Combine(workItem.crc, workItem.inputBytesAvailable);
								this._totalBytesProcessed += (long)workItem.inputBytesAvailable;
								workItem.inputBytesAvailable = 0;
								this._lastWritten = workItem.ordinal;
								this._toFill.Enqueue(workItem.index);
								if (num2 == -1)
								{
									num2 = 0;
								}
							}
						}
					}
					else
					{
						num3 = -1;
					}
				}
				while (num3 >= 0);
			}
			while (doAll && this._lastWritten != this._latestCompressed);
			this.emitting = false;
		}

		// Token: 0x0600717C RID: 29052 RVA: 0x0027BBA8 File Offset: 0x00279DA8
		private void _DeflateOne(object wi)
		{
			WorkItem workItem = (WorkItem)wi;
			try
			{
				CRC32 crc = new CRC32();
				crc.SlurpBlock(workItem.buffer, 0, workItem.inputBytesAvailable);
				this.DeflateOneSegment(workItem);
				workItem.crc = crc.Crc32Result;
				object obj = this._latestLock;
				lock (obj)
				{
					if (workItem.ordinal > this._latestCompressed)
					{
						this._latestCompressed = workItem.ordinal;
					}
				}
				Queue<int> toWrite = this._toWrite;
				lock (toWrite)
				{
					this._toWrite.Enqueue(workItem.index);
				}
				this._newlyCompressedBlob.Set();
			}
			catch (Exception pendingException)
			{
				object obj = this._eLock;
				lock (obj)
				{
					if (this._pendingException != null)
					{
						this._pendingException = pendingException;
					}
				}
			}
		}

		// Token: 0x0600717D RID: 29053 RVA: 0x0027BCC8 File Offset: 0x00279EC8
		private bool DeflateOneSegment(WorkItem workitem)
		{
			ZlibCodec compressor = workitem.compressor;
			compressor.ResetDeflate();
			compressor.NextIn = 0;
			compressor.AvailableBytesIn = workitem.inputBytesAvailable;
			compressor.NextOut = 0;
			compressor.AvailableBytesOut = workitem.compressed.Length;
			do
			{
				compressor.Deflate(FlushType.None);
			}
			while (compressor.AvailableBytesIn > 0 || compressor.AvailableBytesOut == 0);
			compressor.Deflate(FlushType.Sync);
			workitem.compressedBytesAvailable = (int)compressor.TotalBytesOut;
			return true;
		}

		// Token: 0x0600717E RID: 29054 RVA: 0x0027BD3C File Offset: 0x00279F3C
		[Conditional("Trace")]
		private void TraceOutput(ParallelDeflateOutputStream.TraceBits bits, string format, params object[] varParams)
		{
			if ((bits & this._DesiredTrace) != ParallelDeflateOutputStream.TraceBits.None)
			{
				object outputLock = this._outputLock;
				lock (outputLock)
				{
					int hashCode = Thread.CurrentThread.GetHashCode();
					Console.ForegroundColor = hashCode % 8 + ConsoleColor.DarkGray;
					Console.Write("{0:000} PDOS ", hashCode);
					Console.WriteLine(format, varParams);
					Console.ResetColor();
				}
			}
		}

		// Token: 0x1700133B RID: 4923
		// (get) Token: 0x0600717F RID: 29055 RVA: 0x00010306 File Offset: 0x0000E506
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700133C RID: 4924
		// (get) Token: 0x06007180 RID: 29056 RVA: 0x00010306 File Offset: 0x0000E506
		public override bool CanRead
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700133D RID: 4925
		// (get) Token: 0x06007181 RID: 29057 RVA: 0x0027BDB4 File Offset: 0x00279FB4
		public override bool CanWrite
		{
			get
			{
				return this._outStream.CanWrite;
			}
		}

		// Token: 0x1700133E RID: 4926
		// (get) Token: 0x06007182 RID: 29058 RVA: 0x0027BDC1 File Offset: 0x00279FC1
		public override long Length
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x1700133F RID: 4927
		// (get) Token: 0x06007183 RID: 29059 RVA: 0x0027BDC8 File Offset: 0x00279FC8
		// (set) Token: 0x06007184 RID: 29060 RVA: 0x0027BDC1 File Offset: 0x00279FC1
		public override long Position
		{
			get
			{
				return this._outStream.Position;
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x06007185 RID: 29061 RVA: 0x0027BDC1 File Offset: 0x00279FC1
		public override int Read(byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06007186 RID: 29062 RVA: 0x0027BDC1 File Offset: 0x00279FC1
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06007187 RID: 29063 RVA: 0x0027BDC1 File Offset: 0x00279FC1
		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x040045F3 RID: 17907
		private static readonly int IO_BUFFER_SIZE_DEFAULT = 65536;

		// Token: 0x040045F4 RID: 17908
		private static readonly int BufferPairsPerCore = 4;

		// Token: 0x040045F5 RID: 17909
		private List<WorkItem> _pool;

		// Token: 0x040045F6 RID: 17910
		private bool _leaveOpen;

		// Token: 0x040045F7 RID: 17911
		private bool emitting;

		// Token: 0x040045F8 RID: 17912
		private Stream _outStream;

		// Token: 0x040045F9 RID: 17913
		private int _maxBufferPairs;

		// Token: 0x040045FA RID: 17914
		private int _bufferSize = ParallelDeflateOutputStream.IO_BUFFER_SIZE_DEFAULT;

		// Token: 0x040045FB RID: 17915
		private AutoResetEvent _newlyCompressedBlob;

		// Token: 0x040045FC RID: 17916
		private object _outputLock = new object();

		// Token: 0x040045FD RID: 17917
		private bool _isClosed;

		// Token: 0x040045FE RID: 17918
		private bool _firstWriteDone;

		// Token: 0x040045FF RID: 17919
		private int _currentlyFilling;

		// Token: 0x04004600 RID: 17920
		private int _lastFilled;

		// Token: 0x04004601 RID: 17921
		private int _lastWritten;

		// Token: 0x04004602 RID: 17922
		private int _latestCompressed;

		// Token: 0x04004603 RID: 17923
		private int _Crc32;

		// Token: 0x04004604 RID: 17924
		private CRC32 _runningCrc;

		// Token: 0x04004605 RID: 17925
		private object _latestLock = new object();

		// Token: 0x04004606 RID: 17926
		private Queue<int> _toWrite;

		// Token: 0x04004607 RID: 17927
		private Queue<int> _toFill;

		// Token: 0x04004608 RID: 17928
		private long _totalBytesProcessed;

		// Token: 0x04004609 RID: 17929
		private CompressionLevel _compressLevel;

		// Token: 0x0400460A RID: 17930
		private volatile Exception _pendingException;

		// Token: 0x0400460B RID: 17931
		private bool _handlingException;

		// Token: 0x0400460C RID: 17932
		private object _eLock = new object();

		// Token: 0x0400460D RID: 17933
		private ParallelDeflateOutputStream.TraceBits _DesiredTrace = ParallelDeflateOutputStream.TraceBits.EmitLock | ParallelDeflateOutputStream.TraceBits.EmitEnter | ParallelDeflateOutputStream.TraceBits.EmitBegin | ParallelDeflateOutputStream.TraceBits.EmitDone | ParallelDeflateOutputStream.TraceBits.EmitSkip | ParallelDeflateOutputStream.TraceBits.Session | ParallelDeflateOutputStream.TraceBits.Compress | ParallelDeflateOutputStream.TraceBits.WriteEnter | ParallelDeflateOutputStream.TraceBits.WriteTake;

		// Token: 0x02002066 RID: 8294
		[Flags]
		private enum TraceBits : uint
		{
			// Token: 0x0400798E RID: 31118
			None = 0u,
			// Token: 0x0400798F RID: 31119
			NotUsed1 = 1u,
			// Token: 0x04007990 RID: 31120
			EmitLock = 2u,
			// Token: 0x04007991 RID: 31121
			EmitEnter = 4u,
			// Token: 0x04007992 RID: 31122
			EmitBegin = 8u,
			// Token: 0x04007993 RID: 31123
			EmitDone = 16u,
			// Token: 0x04007994 RID: 31124
			EmitSkip = 32u,
			// Token: 0x04007995 RID: 31125
			EmitAll = 58u,
			// Token: 0x04007996 RID: 31126
			Flush = 64u,
			// Token: 0x04007997 RID: 31127
			Lifecycle = 128u,
			// Token: 0x04007998 RID: 31128
			Session = 256u,
			// Token: 0x04007999 RID: 31129
			Synch = 512u,
			// Token: 0x0400799A RID: 31130
			Instance = 1024u,
			// Token: 0x0400799B RID: 31131
			Compress = 2048u,
			// Token: 0x0400799C RID: 31132
			Write = 4096u,
			// Token: 0x0400799D RID: 31133
			WriteEnter = 8192u,
			// Token: 0x0400799E RID: 31134
			WriteTake = 16384u,
			// Token: 0x0400799F RID: 31135
			All = 4294967295u
		}
	}
}
