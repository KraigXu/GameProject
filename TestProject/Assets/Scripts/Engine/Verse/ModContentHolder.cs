using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Verse
{
	
	public class ModContentHolder<T> where T : class
	{
		
		public ModContentHolder(ModContentPack mod)
		{
			this.mod = mod;
		}

		
		public void ClearDestroy()
		{
			if (typeof(UnityEngine.Object).IsAssignableFrom(typeof(T)))
			{
				foreach (T localObj2 in this.contentList.Values)
				{
					T localObj = localObj2;
					LongEventHandler.ExecuteWhenFinished(delegate
					{
						UnityEngine.Object.Destroy((UnityEngine.Object)((object)localObj));
					});
				}
			}
			for (int i = 0; i < this.extraDisposables.Count; i++)
			{
				this.extraDisposables[i].Dispose();
			}
			this.extraDisposables.Clear();
			this.contentList.Clear();
		}

		
		public void ReloadAll()
		{
			foreach (Pair<string, LoadedContentItem<T>> pair in ModContentLoader<T>.LoadAllForMod(this.mod))
			{
				string text = pair.First;
				text = text.Replace('\\', '/');
				string text2 = GenFilePaths.ContentPath<T>();
				if (text.StartsWith(text2))
				{
					text = text.Substring(text2.Length);
				}
				if (text.EndsWith(Path.GetExtension(text)))
				{
					text = text.Substring(0, text.Length - Path.GetExtension(text).Length);
				}
				if (this.contentList.ContainsKey(text))
				{
					Log.Warning(string.Concat(new object[]
					{
						"Tried to load duplicate ",
						typeof(T),
						" with path: ",
						pair.Second.internalFile,
						" and internal path: ",
						text
					}), false);
				}
				else
				{
					this.contentList.Add(text, pair.Second.contentItem);
					if (pair.Second.extraDisposable != null)
					{
						this.extraDisposables.Add(pair.Second.extraDisposable);
					}
				}
			}
		}

		
		public T Get(string path)
		{
			T result;
			if (this.contentList.TryGetValue(path, out result))
			{
				return result;
			}
			return default(T);
		}

		
		public IEnumerable<T> GetAllUnderPath(string pathRoot)
		{
			foreach (KeyValuePair<string, T> keyValuePair in this.contentList)
			{
				if (keyValuePair.Key.StartsWith(pathRoot))
				{
					yield return keyValuePair.Value;
				}
			}
			Dictionary<string, T>.Enumerator enumerator = default(Dictionary<string, T>.Enumerator);
			yield break;
			yield break;
		}

		
		private ModContentPack mod;

		
		public Dictionary<string, T> contentList = new Dictionary<string, T>();

		
		public List<IDisposable> extraDisposables = new List<IDisposable>();
	}
}
