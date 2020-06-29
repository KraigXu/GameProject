using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Verse
{
	
	public class ModAssetBundlesHandler
	{
		
		public ModAssetBundlesHandler(ModContentPack mod)
		{
			this.mod = mod;
		}

		
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

		
		private ModContentPack mod;

		
		public List<AssetBundle> loadedAssetBundles = new List<AssetBundle>();

		
		public static readonly string[] TextureExtensions = new string[]
		{
			".png",
			".jpg",
			".jpeg",
			".psd"
		};

		
		public static readonly string[] AudioClipExtensions = new string[]
		{
			".wav",
			".mp3"
		};
	}
}
