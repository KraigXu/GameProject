using System;
using System.IO;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using NVorbis.NAudioSupport;
using UnityEngine;

namespace RuntimeAudioClipLoader
{
	
	internal class CustomAudioFileReader : WaveStream, ISampleProvider
	{
		
		public CustomAudioFileReader(Stream stream, AudioFormat format)
		{
			this.lockObject = new object();
			this.CreateReaderStream(stream, format);
			this.sourceBytesPerSample = this.readerStream.WaveFormat.BitsPerSample / 8 * this.readerStream.WaveFormat.Channels;
			this.sampleChannel = new SampleChannel(this.readerStream, false);
			this.destBytesPerSample = 4 * this.sampleChannel.WaveFormat.Channels;
			this.length = this.SourceToDest(this.readerStream.Length);
		}

		
		private void CreateReaderStream(Stream stream, AudioFormat format)
		{
			if (format == AudioFormat.wav)
			{
				this.readerStream = new WaveFileReader(stream);
				if (this.readerStream.WaveFormat.Encoding != WaveFormatEncoding.Pcm && this.readerStream.WaveFormat.Encoding != WaveFormatEncoding.IeeeFloat)
				{
					this.readerStream = WaveFormatConversionStream.CreatePcmStream(this.readerStream);
					this.readerStream = new BlockAlignReductionStream(this.readerStream);
					return;
				}
			}
			else
			{
				if (format == AudioFormat.mp3)
				{
					this.readerStream = new Mp3FileReader(stream);
					return;
				}
				if (format == AudioFormat.aiff)
				{
					this.readerStream = new AiffFileReader(stream);
					return;
				}
				if (format == AudioFormat.ogg)
				{
					this.readerStream = new VorbisWaveReader(stream);
					return;
				}
				Debug.LogWarning("Audio format " + format + " is not supported");
			}
		}

		
		// (get) Token: 0x06000029 RID: 41 RVA: 0x000027C9 File Offset: 0x000009C9
		public override WaveFormat WaveFormat
		{
			get
			{
				return this.sampleChannel.WaveFormat;
			}
		}

		
		// (get) Token: 0x0600002A RID: 42 RVA: 0x000027D6 File Offset: 0x000009D6
		public override long Length
		{
			get
			{
				return this.length;
			}
		}

		
		// (get) Token: 0x0600002B RID: 43 RVA: 0x000027DE File Offset: 0x000009DE
		// (set) Token: 0x0600002C RID: 44 RVA: 0x000027F4 File Offset: 0x000009F4
		public override long Position
		{
			get
			{
				return this.SourceToDest(this.readerStream.Position);
			}
			set
			{
				object obj = this.lockObject;
				lock (obj)
				{
					this.readerStream.Position = this.DestToSource(value);
				}
			}
		}

		
		public override int Read(byte[] buffer, int offset, int count)
		{
			WaveBuffer waveBuffer = new WaveBuffer(buffer);
			int count2 = count / 4;
			return this.Read(waveBuffer.FloatBuffer, offset / 4, count2) * 4;
		}

		
		public int Read(float[] buffer, int offset, int count)
		{
			object obj = this.lockObject;
			int result;
			lock (obj)
			{
				result = this.sampleChannel.Read(buffer, offset, count);
			}
			return result;
		}

		
		// (get) Token: 0x0600002F RID: 47 RVA: 0x000028B8 File Offset: 0x00000AB8
		// (set) Token: 0x06000030 RID: 48 RVA: 0x000028C5 File Offset: 0x00000AC5
		public float Volume
		{
			get
			{
				return this.sampleChannel.Volume;
			}
			set
			{
				this.sampleChannel.Volume = value;
			}
		}

		
		private long SourceToDest(long sourceBytes)
		{
			return (long)this.destBytesPerSample * (sourceBytes / (long)this.sourceBytesPerSample);
		}

		
		private long DestToSource(long destBytes)
		{
			return (long)this.sourceBytesPerSample * (destBytes / (long)this.destBytesPerSample);
		}

		
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.readerStream != null)
			{
				this.readerStream.Dispose();
				this.readerStream = null;
			}
			base.Dispose(disposing);
		}

		
		private WaveStream readerStream;

		
		private readonly SampleChannel sampleChannel;

		
		private readonly int destBytesPerSample;

		
		private readonly int sourceBytesPerSample;

		
		private readonly long length;

		
		private readonly object lockObject;
	}
}
