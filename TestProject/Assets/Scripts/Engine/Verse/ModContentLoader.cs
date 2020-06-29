using System;
using System.Collections.Generic;
using System.IO;
using RimWorld.IO;
using RuntimeAudioClipLoader;
using UnityEngine;

namespace Verse
{
	
	public static class ModContentLoader<T> where T : class
	{
		
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

		
		private static AudioType GetAudioTypeFromURI(string uri)
		{
			if (uri.EndsWith(".ogg"))
			{
				return AudioType.OGGVORBIS;
			}
			return AudioType.WAV;
		}

		
		private static bool ShouldStreamAudioClipFromFile(VirtualFile file)
		{
			return file is FilesystemFile && file.Exists && file.Length > 307200L;
		}

		
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

		
		private static string[] AcceptableExtensionsTexture = new string[]
		{
			".png",
			".jpg",
			".jpeg",
			".psd"
		};

		
		private static string[] AcceptableExtensionsString = new string[]
		{
			".txt"
		};
	}
}
