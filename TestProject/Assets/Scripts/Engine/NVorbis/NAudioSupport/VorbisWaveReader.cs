using System;
using System.IO;
using NAudio.Wave;

namespace NVorbis.NAudioSupport
{
	
	internal class VorbisWaveReader : WaveStream, IDisposable, ISampleProvider, IWaveProvider
	{
		
		public VorbisWaveReader(string fileName)
		{
			this._reader = new VorbisReader(fileName);
			this._waveFormat = WaveFormat.CreateIeeeFloatWaveFormat(this._reader.SampleRate, this._reader.Channels);
		}

		
		public VorbisWaveReader(Stream sourceStream)
		{
			this._reader = new VorbisReader(sourceStream, false);
			this._waveFormat = WaveFormat.CreateIeeeFloatWaveFormat(this._reader.SampleRate, this._reader.Channels);
		}

		
		protected override void Dispose(bool disposing)
		{
			if (disposing && this._reader != null)
			{
				this._reader.Dispose();
				this._reader = null;
			}
			base.Dispose(disposing);
		}

		
		// (get) Token: 0x06000011 RID: 17 RVA: 0x000023DC File Offset: 0x000005DC
		public override WaveFormat WaveFormat
		{
			get
			{
				return this._waveFormat;
			}
		}

		
		// (get) Token: 0x06000012 RID: 18 RVA: 0x000023E4 File Offset: 0x000005E4
		public override long Length
		{
			get
			{
				return (long)(this._reader.TotalTime.TotalSeconds * (double)this._waveFormat.SampleRate * (double)this._waveFormat.Channels * 4.0);
			}
		}

		
		// (get) Token: 0x06000013 RID: 19 RVA: 0x0000242C File Offset: 0x0000062C
		// (set) Token: 0x06000014 RID: 20 RVA: 0x00002474 File Offset: 0x00000674
		public override long Position
		{
			get
			{
				return (long)(this._reader.DecodedTime.TotalMilliseconds * (double)this._reader.SampleRate * (double)this._reader.Channels * 4.0);
			}
			set
			{
				if (value < 0L || value > this.Length)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._reader.DecodedTime = TimeSpan.FromSeconds((double)value / (double)this._reader.SampleRate / (double)this._reader.Channels / 4.0);
			}
		}

		
		public override int Read(byte[] buffer, int offset, int count)
		{
			count /= 4;
			count -= count % this._reader.Channels;
			float[] array;
			if ((array = VorbisWaveReader._conversionBuffer) == null)
			{
				array = (VorbisWaveReader._conversionBuffer = new float[count]);
			}
			float[] array2 = array;
			if (array2.Length < count)
			{
				array2 = (VorbisWaveReader._conversionBuffer = new float[count]);
			}
			int num = this.Read(array2, 0, count) * 4;
			Buffer.BlockCopy(array2, 0, buffer, offset, num);
			return num;
		}

		
		public int Read(float[] buffer, int offset, int count)
		{
			return this._reader.ReadSamples(buffer, offset, count);
		}

		
		// (get) Token: 0x06000017 RID: 23 RVA: 0x00002543 File Offset: 0x00000743
		public bool IsParameterChange
		{
			get
			{
				return this._reader.IsParameterChange;
			}
		}

		
		public void ClearParameterChange()
		{
			this._reader.ClearParameterChange();
		}

		
		// (get) Token: 0x06000019 RID: 25 RVA: 0x0000255D File Offset: 0x0000075D
		public int StreamCount
		{
			get
			{
				return this._reader.StreamCount;
			}
		}

		
		// (get) Token: 0x0600001A RID: 26 RVA: 0x0000256A File Offset: 0x0000076A
		// (set) Token: 0x0600001B RID: 27 RVA: 0x00002572 File Offset: 0x00000772
		public int? NextStreamIndex { get; set; }

		
		public bool GetNextStreamIndex()
		{
			if (this.NextStreamIndex == null)
			{
				int streamCount = this._reader.StreamCount;
				if (this._reader.FindNextStream())
				{
					this.NextStreamIndex = new int?(streamCount);
					return true;
				}
			}
			return false;
		}

		
		// (get) Token: 0x0600001D RID: 29 RVA: 0x000025C1 File Offset: 0x000007C1
		// (set) Token: 0x0600001E RID: 30 RVA: 0x000025D0 File Offset: 0x000007D0
		public int CurrentStream
		{
			get
			{
				return this._reader.StreamIndex;
			}
			set
			{
				if (!this._reader.SwitchStreams(value))
				{
					throw new InvalidDataException("The selected stream is not a valid Vorbis stream!");
				}
				if (this.NextStreamIndex != null && value == this.NextStreamIndex.Value)
				{
					this.NextStreamIndex = null;
				}
			}
		}

		
		// (get) Token: 0x0600001F RID: 31 RVA: 0x00002626 File Offset: 0x00000826
		public int UpperBitrate
		{
			get
			{
				return this._reader.UpperBitrate;
			}
		}

		
		// (get) Token: 0x06000020 RID: 32 RVA: 0x00002633 File Offset: 0x00000833
		public int NominalBitrate
		{
			get
			{
				return this._reader.NominalBitrate;
			}
		}

		
		// (get) Token: 0x06000021 RID: 33 RVA: 0x00002640 File Offset: 0x00000840
		public int LowerBitrate
		{
			get
			{
				return this._reader.LowerBitrate;
			}
		}

		
		// (get) Token: 0x06000022 RID: 34 RVA: 0x0000264D File Offset: 0x0000084D
		public string Vendor
		{
			get
			{
				return this._reader.Vendor;
			}
		}

		
		// (get) Token: 0x06000023 RID: 35 RVA: 0x0000265A File Offset: 0x0000085A
		public string[] Comments
		{
			get
			{
				return this._reader.Comments;
			}
		}

		
		// (get) Token: 0x06000024 RID: 36 RVA: 0x00002667 File Offset: 0x00000867
		public long ContainerOverheadBits
		{
			get
			{
				return this._reader.ContainerOverheadBits;
			}
		}

		
		// (get) Token: 0x06000025 RID: 37 RVA: 0x00002674 File Offset: 0x00000874
		public IVorbisStreamStatus[] Stats
		{
			get
			{
				return this._reader.Stats;
			}
		}

		
		private VorbisReader _reader;

		
		private WaveFormat _waveFormat;

		
		[ThreadStatic]
		private static float[] _conversionBuffer;
	}
}
