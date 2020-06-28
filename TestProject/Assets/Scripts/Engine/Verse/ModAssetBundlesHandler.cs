using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Verse
{
	// Token: 0x020001F2 RID: 498
	public class ModAssetBundlesHandler
	{
		// Token: 0x06000E10 RID: 3600 RVA: 0x00050888 File Offset: 0x0004EA88
		public ModAssetBundlesHandler(ModContentPack mod)
		{
			this.mod = mod;
		}

		// Token: 0x06000E11 RID: 3601 RVA: 0x000508A4 File Offset: 0x0004EAA4
		public void ReloadAll()
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(Path.Combine(this.mod.RootDir, "AssetBundles"));
			if (!directoryInfo.Exists)
			{
				return;
			}
			foreach (FileInfo fileInfo in directoryInfo.GetFiles("*", SearchOption.AllDirectories))
			{
				if (fileInfo.Extension.NullOrEmpty())
				{
					AssetBundle assetBundle = AssetBundle.LoadFromFile(fileInfo.FullName);
					if (assetBundle != null)
					{
						this.loadedAssetBundles.Add(assetBundle);
					}
					else
					{
						Log.Error("Could not load asset bundle at " + fileInfo.FullName, false);
					}
				}
			}
		}

		// Token: 0x06000E12 RID: 3602 RVA: 0x0005093D File Offset: 0x0004EB3D
		public void ClearDestroy()
		{
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				for (int i = 0; i < this.loadedAssetBundles.Count; i++)
				{
					this.loadedAssetBundles[i].Unload(true);
				}
				this.loadedAssetBundles.Clear();
			});
		}

		// Token: 0x04000AA2 RID: 2722
		private ModContentPack mod;

		// Token: 0x04000AA3 RID: 2723
		public List<AssetBundle> loadedAssetBundles = new List<AssetBundle>();

		// Token: 0x04000AA4 RID: 2724
		public static readonly string[] TextureExtensions = new string[]
		{
			".png",
			".jpg",
			".jpeg",
			".psd"
		};

		// Token: 0x04000AA5 RID: 2725
		public static readonly string[] AudioClipExtensions = new string[]
		{
			".wav",
			".mp3"
		};
	}
}
