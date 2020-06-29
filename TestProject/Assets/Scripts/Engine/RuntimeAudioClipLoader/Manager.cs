﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using UnityEngine;
using Verse;

namespace RuntimeAudioClipLoader
{
	
	[StaticConstructorOnStartup]
	public class Manager : MonoBehaviour
	{
		
		static Manager()
		{
			Manager.supportedFormats = Enum.GetNames(typeof(AudioFormat));
		}

		
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

		
		private static void RunDeferredLoaderThread()
		{
			if (Manager.deferredLoaderThread == null || !Manager.deferredLoaderThread.IsAlive)
			{
				Manager.deferredLoaderThread = new Thread(new ThreadStart(Manager.DeferredLoaderMain));
				Manager.deferredLoaderThread.IsBackground = true;
				Manager.deferredLoaderThread.Start();
			}
		}

		
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

		
		private static void EnsureInstanceExists()
		{
			if (!Manager.managerInstance)
			{
				Manager.managerInstance = new GameObject("Runtime AudioClip Loader Manger singleton instance");
				Manager.managerInstance.hideFlags = HideFlags.HideAndDontSave;
				Manager.managerInstance.AddComponent<Manager>();
			}
		}

		
		public static void SetAudioClipLoadState(AudioClip audioClip, AudioDataLoadState newLoadState)
		{
			Manager.audioLoadState[audioClip] = newLoadState;
		}

		
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

		
		public static void SetAudioClipLoadType(AudioClip audioClip, AudioClipLoadType newLoadType)
		{
			Manager.audioClipLoadType[audioClip] = newLoadType;
		}

		
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

		
		private static string GetExtension(string filePath)
		{
			return Path.GetExtension(filePath).Substring(1).ToLower();
		}

		
		public static bool IsSupportedFormat(string filePath)
		{
			return Manager.supportedFormats.Contains(Manager.GetExtension(filePath));
		}

		
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

		
		public static void ClearCache()
		{
			Manager.cache.Clear();
		}

		
		private static readonly string[] supportedFormats;

		
		private static Dictionary<string, AudioClip> cache = new Dictionary<string, AudioClip>();

		
		private static Queue<Manager.AudioInstance> deferredLoadQueue = new Queue<Manager.AudioInstance>();

		
		private static Queue<Manager.AudioInstance> deferredSetDataQueue = new Queue<Manager.AudioInstance>();

		
		private static Queue<Manager.AudioInstance> deferredSetFail = new Queue<Manager.AudioInstance>();

		
		private static Thread deferredLoaderThread;

		
		private static GameObject managerInstance;

		
		private static Dictionary<AudioClip, AudioClipLoadType> audioClipLoadType = new Dictionary<AudioClip, AudioClipLoadType>();

		
		private static Dictionary<AudioClip, AudioDataLoadState> audioLoadState = new Dictionary<AudioClip, AudioDataLoadState>();

		
		private class AudioInstance
		{
			
			// (get) Token: 0x06007213 RID: 29203 RVA: 0x0027E274 File Offset: 0x0027C474
			public int channels
			{
				get
				{
					return this.reader.WaveFormat.Channels;
				}
			}

			
			// (get) Token: 0x06007214 RID: 29204 RVA: 0x0027E286 File Offset: 0x0027C486
			public int sampleRate
			{
				get
				{
					return this.reader.WaveFormat.SampleRate;
				}
			}

			
			public static implicit operator AudioClip(Manager.AudioInstance ai)
			{
				return ai.audioClip;
			}

			
			public AudioClip audioClip;

			
			public CustomAudioFileReader reader;

			
			public float[] dataToSet;

			
			public int samplesCount;

			
			public Stream streamToDisposeOnceDone;
		}
	}
}
