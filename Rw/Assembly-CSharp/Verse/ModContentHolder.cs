using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Verse
{
	// Token: 0x020001F3 RID: 499
	public class ModContentHolder<T> where T : class
	{
		// Token: 0x06000E15 RID: 3605 RVA: 0x000509E4 File Offset: 0x0004EBE4
		public ModContentHolder(ModContentPack mod)
		{
			this.mod = mod;
		}

		// Token: 0x06000E16 RID: 3606 RVA: 0x00050A0C File Offset: 0x0004EC0C
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

		// Token: 0x06000E17 RID: 3607 RVA: 0x00050AD4 File Offset: 0x0004ECD4
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

		// Token: 0x06000E18 RID: 3608 RVA: 0x00050C20 File Offset: 0x0004EE20
		public T Get(string path)
		{
			T result;
			if (this.contentList.TryGetValue(path, out result))
			{
				return result;
			}
			return default(T);
		}

		// Token: 0x06000E19 RID: 3609 RVA: 0x00050C48 File Offset: 0x0004EE48
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

		// Token: 0x04000AA6 RID: 2726
		private ModContentPack mod;

		// Token: 0x04000AA7 RID: 2727
		public Dictionary<string, T> contentList = new Dictionary<string, T>();

		// Token: 0x04000AA8 RID: 2728
		public List<IDisposable> extraDisposables = new List<IDisposable>();
	}
}
