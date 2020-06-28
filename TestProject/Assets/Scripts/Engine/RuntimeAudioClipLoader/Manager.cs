using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using UnityEngine;
using Verse;

namespace RuntimeAudioClipLoader
{
	// Token: 0x02000007 RID: 7
	[StaticConstructorOnStartup]
	public class Manager : MonoBehaviour
	{
		// Token: 0x06000034 RID: 52 RVA: 0x00002920 File Offset: 0x00000B20
		static Manager()
		{
			Manager.supportedFormats = Enum.GetNames(typeof(AudioFormat));
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00002980 File Offset: 0x00000B80
		public static AudioClip Load(string filePath, bool doStream = false, bool loadInBackground = true, bool useCache = true)
		{
			if (!Manager.IsSupportedFormat(filePath))
			{
				Debug.LogError("Could not load AudioClip at path '" + filePath + "' it's extensions marks unsupported format, supported formats are: " + string.Join(", ", Enum.GetNames(typeof(AudioFormat))));
				return null;
			}
			AudioClip audioClip = null;
			if (useCache && Manager.cache.TryGetValue(filePath, out audioClip) && audioClip)
			{
				return audioClip;
			}
			audioClip = Manager.Load(new StreamReader(filePath).BaseStream, Manager.GetAudioFormat(filePath), filePath, doStream, loadInBackground, true);
			if (useCache)
			{
				Manager.cache[filePath] = audioClip;
			}
			return audioClip;
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00002A10 File Offset: 0x00000C10
		public static AudioClip Load(Stream dataStream, AudioFormat audioFormat, string unityAudioClipName, bool doStream = false, bool loadInBackground = true, bool diposeDataStreamIfNotNeeded = true)
		{
			AudioClip audioClip = null;
			CustomAudioFileReader reader = null;
			try
			{
				reader = new CustomAudioFileReader(dataStream, audioFormat);
				Manager.AudioInstance audioInstance = new Manager.AudioInstance
				{
					reader = reader,
					samplesCount = (int)(reader.Length / (long)(reader.WaveFormat.BitsPerSample / 8))
				};
				if (doStream)
				{
					audioClip = AudioClip.Create(unityAudioClipName, audioInstance.samplesCount / audioInstance.channels, audioInstance.channels, audioInstance.sampleRate, doStream, delegate(float[] target)
					{
						reader.Read(target, 0, target.Length);
					}, delegate(int target)
					{
						reader.Seek((long)target, SeekOrigin.Begin);
					});
					audioInstance.audioClip = audioClip;
					Manager.SetAudioClipLoadType(audioInstance, AudioClipLoadType.Streaming);
					Manager.SetAudioClipLoadState(audioInstance, AudioDataLoadState.Loaded);
				}
				else
				{
					audioClip = AudioClip.Create(unityAudioClipName, audioInstance.samplesCount / audioInstance.channels, audioInstance.channels, audioInstance.sampleRate, doStream);
					audioInstance.audioClip = audioClip;
					if (diposeDataStreamIfNotNeeded)
					{
						audioInstance.streamToDisposeOnceDone = dataStream;
					}
					Manager.SetAudioClipLoadType(audioInstance, AudioClipLoadType.DecompressOnLoad);
					Manager.SetAudioClipLoadState(audioInstance, AudioDataLoadState.Loading);
					if (loadInBackground)
					{
						Queue<Manager.AudioInstance> obj = Manager.deferredLoadQueue;
						lock (obj)
						{
							Manager.deferredLoadQueue.Enqueue(audioInstance);
						}
						Manager.RunDeferredLoaderThread();
						Manager.EnsureInstanceExists();
					}
					else
					{
						audioInstance.dataToSet = new float[audioInstance.samplesCount];
						audioInstance.reader.Read(audioInstance.dataToSet, 0, audioInstance.dataToSet.Length);
						audioInstance.audioClip.SetData(audioInstance.dataToSet, 0);
						Manager.SetAudioClipLoadState(audioInstance, AudioDataLoadState.Loaded);
					}
				}
			}
			catch (Exception ex)
			{
				Manager.SetAudioClipLoadState(audioClip, AudioDataLoadState.Failed);
				Debug.LogError(string.Concat(new object[]
				{
					"Could not load AudioClip named '",
					unityAudioClipName,
					"', exception:",
					ex
				}));
			}
			return audioClip;
		}

		// Token: 0x06000037 RID: 55 RVA: 0x00002C0C File Offset: 0x00000E0C
		private static void RunDeferredLoaderThread()
		{
			if (Manager.deferredLoaderThread == null || !Manager.deferredLoaderThread.IsAlive)
			{
				Manager.deferredLoaderThread = new Thread(new ThreadStart(Manager.DeferredLoaderMain));
				Manager.deferredLoaderThread.IsBackground = true;
				Manager.deferredLoaderThread.Start();
			}
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00002C4C File Offset: 0x00000E4C
		private static void DeferredLoaderMain()
		{
			Manager.AudioInstance audioInstance = null;
			bool flag = true;
			long num = 100000L;
			while (flag || num > 0L)
			{
				num -= 1L;
				Queue<Manager.AudioInstance> obj = Manager.deferredLoadQueue;
				lock (obj)
				{
					flag = (Manager.deferredLoadQueue.Count > 0);
					if (!flag)
					{
						continue;
					}
					audioInstance = Manager.deferredLoadQueue.Dequeue();
				}
				num = 100000L;
				try
				{
					audioInstance.dataToSet = new float[audioInstance.samplesCount];
					audioInstance.reader.Read(audioInstance.dataToSet, 0, audioInstance.dataToSet.Length);
					audioInstance.reader.Close();
					audioInstance.reader.Dispose();
					if (audioInstance.streamToDisposeOnceDone != null)
					{
						audioInstance.streamToDisposeOnceDone.Close();
						audioInstance.streamToDisposeOnceDone.Dispose();
						audioInstance.streamToDisposeOnceDone = null;
					}
					obj = Manager.deferredSetDataQueue;
					lock (obj)
					{
						Manager.deferredSetDataQueue.Enqueue(audioInstance);
					}
				}
				catch (Exception exception)
				{
					Debug.LogException(exception);
					obj = Manager.deferredSetFail;
					lock (obj)
					{
						Manager.deferredSetFail.Enqueue(audioInstance);
					}
				}
			}
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00002DB8 File Offset: 0x00000FB8
		private void Update()
		{
			Manager.AudioInstance audioInstance = null;
			bool flag = true;
			Queue<Manager.AudioInstance> obj;
			while (flag)
			{
				obj = Manager.deferredSetDataQueue;
				lock (obj)
				{
					flag = (Manager.deferredSetDataQueue.Count > 0);
					if (!flag)
					{
						break;
					}
					audioInstance = Manager.deferredSetDataQueue.Dequeue();
				}
				audioInstance.audioClip.SetData(audioInstance.dataToSet, 0);
				Manager.SetAudioClipLoadState(audioInstance, AudioDataLoadState.Loaded);
				audioInstance.audioClip = null;
				audioInstance.dataToSet = null;
			}
			obj = Manager.deferredSetFail;
			lock (obj)
			{
				while (Manager.deferredSetFail.Count > 0)
				{
					audioInstance = Manager.deferredSetFail.Dequeue();
					Manager.SetAudioClipLoadState(audioInstance, AudioDataLoadState.Failed);
				}
			}
		}

		// Token: 0x0600003A RID: 58 RVA: 0x00002E94 File Offset: 0x00001094
		private static void EnsureInstanceExists()
		{
			if (!Manager.managerInstance)
			{
				Manager.managerInstance = new GameObject("Runtime AudioClip Loader Manger singleton instance");
				Manager.managerInstance.hideFlags = HideFlags.HideAndDontSave;
				Manager.managerInstance.AddComponent<Manager>();
			}
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00002EC8 File Offset: 0x000010C8
		public static void SetAudioClipLoadState(AudioClip audioClip, AudioDataLoadState newLoadState)
		{
			Manager.audioLoadState[audioClip] = newLoadState;
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00002ED8 File Offset: 0x000010D8
		public static AudioDataLoadState GetAudioClipLoadState(AudioClip audioClip)
		{
			AudioDataLoadState result = AudioDataLoadState.Failed;
			if (audioClip != null)
			{
				result = audioClip.loadState;
				Manager.audioLoadState.TryGetValue(audioClip, out result);
			}
			return result;
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00002F06 File Offset: 0x00001106
		public static void SetAudioClipLoadType(AudioClip audioClip, AudioClipLoadType newLoadType)
		{
			Manager.audioClipLoadType[audioClip] = newLoadType;
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00002F14 File Offset: 0x00001114
		public static AudioClipLoadType GetAudioClipLoadType(AudioClip audioClip)
		{
			AudioClipLoadType result = (AudioClipLoadType)(-1);
			if (audioClip != null)
			{
				result = audioClip.loadType;
				Manager.audioClipLoadType.TryGetValue(audioClip, out result);
			}
			return result;
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00002F42 File Offset: 0x00001142
		private static string GetExtension(string filePath)
		{
			return Path.GetExtension(filePath).Substring(1).ToLower();
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00002F55 File Offset: 0x00001155
		public static bool IsSupportedFormat(string filePath)
		{
			return Manager.supportedFormats.Contains(Manager.GetExtension(filePath));
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00002F68 File Offset: 0x00001168
		public static AudioFormat GetAudioFormat(string filePath)
		{
			AudioFormat result = AudioFormat.unknown;
			try
			{
				result = (AudioFormat)Enum.Parse(typeof(AudioFormat), Manager.GetExtension(filePath), true);
			}
			catch
			{
			}
			return result;
		}

		// Token: 0x06000042 RID: 66 RVA: 0x00002FAC File Offset: 0x000011AC
		public static void ClearCache()
		{
			Manager.cache.Clear();
		}

		// Token: 0x04000016 RID: 22
		private static readonly string[] supportedFormats;

		// Token: 0x04000017 RID: 23
		private static Dictionary<string, AudioClip> cache = new Dictionary<string, AudioClip>();

		// Token: 0x04000018 RID: 24
		private static Queue<Manager.AudioInstance> deferredLoadQueue = new Queue<Manager.AudioInstance>();

		// Token: 0x04000019 RID: 25
		private static Queue<Manager.AudioInstance> deferredSetDataQueue = new Queue<Manager.AudioInstance>();

		// Token: 0x0400001A RID: 26
		private static Queue<Manager.AudioInstance> deferredSetFail = new Queue<Manager.AudioInstance>();

		// Token: 0x0400001B RID: 27
		private static Thread deferredLoaderThread;

		// Token: 0x0400001C RID: 28
		private static GameObject managerInstance;

		// Token: 0x0400001D RID: 29
		private static Dictionary<AudioClip, AudioClipLoadType> audioClipLoadType = new Dictionary<AudioClip, AudioClipLoadType>();

		// Token: 0x0400001E RID: 30
		private static Dictionary<AudioClip, AudioDataLoadState> audioLoadState = new Dictionary<AudioClip, AudioDataLoadState>();

		// Token: 0x020012C7 RID: 4807
		private class AudioInstance
		{
			// Token: 0x1700135D RID: 4957
			// (get) Token: 0x06007213 RID: 29203 RVA: 0x0027E274 File Offset: 0x0027C474
			public int channels
			{
				get
				{
					return this.reader.WaveFormat.Channels;
				}
			}

			// Token: 0x1700135E RID: 4958
			// (get) Token: 0x06007214 RID: 29204 RVA: 0x0027E286 File Offset: 0x0027C486
			public int sampleRate
			{
				get
				{
					return this.reader.WaveFormat.SampleRate;
				}
			}

			// Token: 0x06007215 RID: 29205 RVA: 0x0027E298 File Offset: 0x0027C498
			public static implicit operator AudioClip(Manager.AudioInstance ai)
			{
				return ai.audioClip;
			}

			// Token: 0x040046AD RID: 18093
			public AudioClip audioClip;

			// Token: 0x040046AE RID: 18094
			public CustomAudioFileReader reader;

			// Token: 0x040046AF RID: 18095
			public float[] dataToSet;

			// Token: 0x040046B0 RID: 18096
			public int samplesCount;

			// Token: 0x040046B1 RID: 18097
			public Stream streamToDisposeOnceDone;
		}
	}
}
