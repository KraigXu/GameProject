using System;
using System.Collections.Generic;
using System.IO;
using RimWorld.IO;
using RuntimeAudioClipLoader;
using UnityEngine;

namespace Verse
{
	// Token: 0x020001F5 RID: 501
	public static class ModContentLoader<T> where T : class
	{
		// Token: 0x06000E1B RID: 3611 RVA: 0x00050C7C File Offset: 0x0004EE7C
		public static bool IsAcceptableExtension(string extension)
		{
			string[] array;
			if (typeof(T) == typeof(AudioClip))
			{
				array = ModContentLoader<T>.AcceptableExtensionsAudio;
			}
			else if (typeof(T) == typeof(Texture2D))
			{
				array = ModContentLoader<T>.AcceptableExtensionsTexture;
			}
			else
			{
				if (!(typeof(T) == typeof(string)))
				{
					Log.Error("Unknown content type " + typeof(T), false);
					return false;
				}
				array = ModContentLoader<T>.AcceptableExtensionsString;
			}
			foreach (string b in array)
			{
				if (extension.ToLower() == b)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000E1C RID: 3612 RVA: 0x00050D33 File Offset: 0x0004EF33
		public static IEnumerable<Pair<string, LoadedContentItem<T>>> LoadAllForMod(ModContentPack mod)
		{
			DeepProfiler.Start(string.Concat(new object[]
			{
				"Loading assets of type ",
				typeof(T),
				" for mod ",
				mod
			}));
			Dictionary<string, FileInfo> allFilesForMod = ModContentPack.GetAllFilesForMod(mod, GenFilePaths.ContentPath<T>(), new Func<string, bool>(ModContentLoader<T>.IsAcceptableExtension), null);
			foreach (KeyValuePair<string, FileInfo> keyValuePair in allFilesForMod)
			{
				LoadedContentItem<T> loadedContentItem = ModContentLoader<T>.LoadItem(keyValuePair.Value);
				if (loadedContentItem != null)
				{
					yield return new Pair<string, LoadedContentItem<T>>(keyValuePair.Key, loadedContentItem);
				}
			}
			Dictionary<string, FileInfo>.Enumerator enumerator = default(Dictionary<string, FileInfo>.Enumerator);
			DeepProfiler.End();
			yield break;
			yield break;
		}

		// Token: 0x06000E1D RID: 3613 RVA: 0x00050D44 File Offset: 0x0004EF44
		public static LoadedContentItem<T> LoadItem(VirtualFile file)
		{
			try
			{
				if (typeof(T) == typeof(string))
				{
					return new LoadedContentItem<T>(file, (T)((object)file.ReadAllText()), null);
				}
				if (typeof(T) == typeof(Texture2D))
				{
					return new LoadedContentItem<T>(file, (T)((object)ModContentLoader<T>.LoadTexture(file)), null);
				}
				if (typeof(T) == typeof(AudioClip))
				{
					if (Prefs.LogVerbose)
					{
						DeepProfiler.Start("Loading file " + file);
					}
					IDisposable extraDisposable = null;
					T t;
					try
					{
						bool doStream = ModContentLoader<T>.ShouldStreamAudioClipFromFile(file);
						Stream stream = file.CreateReadStream();
						try
						{
							t = (T)((object)Manager.Load(stream, ModContentLoader<T>.GetFormat(file.Name), file.Name, doStream, true, true));
						}
						catch (Exception)
						{
							stream.Dispose();
							throw;
						}
						extraDisposable = stream;
					}
					finally
					{
						if (Prefs.LogVerbose)
						{
							DeepProfiler.End();
						}
					}
					UnityEngine.Object @object = t as UnityEngine.Object;
					if (@object != null)
					{
						@object.name = Path.GetFileNameWithoutExtension(file.Name);
					}
					return new LoadedContentItem<T>(file, t, extraDisposable);
				}
			}
			catch (Exception ex)
			{
				Log.Error(string.Concat(new object[]
				{
					"Exception loading ",
					typeof(T),
					" from file.\nabsFilePath: ",
					file.FullPath,
					"\nException: ",
					ex.ToString()
				}), false);
			}
			if (typeof(T) == typeof(Texture2D))
			{
				return (LoadedContentItem<T>)new LoadedContentItem<Texture2D>(file, BaseContent.BadTex, null);
			}
			return null;
		}

		// Token: 0x06000E1E RID: 3614 RVA: 0x00050F40 File Offset: 0x0004F140
		private static AudioFormat GetFormat(string filename)
		{
			string extension = Path.GetExtension(filename);
			if (extension == ".ogg")
			{
				return AudioFormat.ogg;
			}
			if (extension == ".mp3")
			{
				return AudioFormat.mp3;
			}
			if (extension == ".aiff" || extension == ".aif" || extension == ".aifc")
			{
				return AudioFormat.aiff;
			}
			if (!(extension == ".wav"))
			{
				return AudioFormat.unknown;
			}
			return AudioFormat.wav;
		}

		// Token: 0x06000E1F RID: 3615 RVA: 0x00050FAD File Offset: 0x0004F1AD
		private static AudioType GetAudioTypeFromURI(string uri)
		{
			if (uri.EndsWith(".ogg"))
			{
				return AudioType.OGGVORBIS;
			}
			return AudioType.WAV;
		}

		// Token: 0x06000E20 RID: 3616 RVA: 0x00050FC1 File Offset: 0x0004F1C1
		private static bool ShouldStreamAudioClipFromFile(VirtualFile file)
		{
			return file is FilesystemFile && file.Exists && file.Length > 307200L;
		}

		// Token: 0x06000E21 RID: 3617 RVA: 0x00050FE4 File Offset: 0x0004F1E4
		private static Texture2D LoadTexture(VirtualFile file)
		{
			Texture2D texture2D = null;
			if (file.Exists)
			{
				byte[] data = file.ReadAllBytes();
				texture2D = new Texture2D(2, 2, TextureFormat.Alpha8, true);
				texture2D.LoadImage(data);
				texture2D.Compress(true);
				texture2D.name = Path.GetFileNameWithoutExtension(file.Name);
				texture2D.filterMode = FilterMode.Bilinear;
				texture2D.anisoLevel = 2;
				texture2D.Apply(true, true);
			}
			return texture2D;
		}

		// Token: 0x04000AAC RID: 2732
		private static string[] AcceptableExtensionsAudio = new string[]
		{
			".wav",
			".mp3",
			".ogg",
			".xm",
			".it",
			".mod",
			".s3m"
		};

		// Token: 0x04000AAD RID: 2733
		private static string[] AcceptableExtensionsTexture = new string[]
		{
			".png",
			".jpg",
			".jpeg",
			".psd"
		};

		// Token: 0x04000AAE RID: 2734
		private static string[] AcceptableExtensionsString = new string[]
		{
			".txt"
		};
	}
}
