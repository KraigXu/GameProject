using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using RimWorld;
using UnityEngine;

namespace Verse
{
	
	public class ModContentPack
	{
		
		// (get) Token: 0x06000E23 RID: 3619 RVA: 0x000510D2 File Offset: 0x0004F2D2
		public string RootDir
		{
			get
			{
				return this.rootDirInt.FullName;
			}
		}

		
		// (get) Token: 0x06000E24 RID: 3620 RVA: 0x000510DF File Offset: 0x0004F2DF
		public string PackageId
		{
			get
			{
				return this.packageIdInt;
			}
		}

		
		// (get) Token: 0x06000E25 RID: 3621 RVA: 0x000510E7 File Offset: 0x0004F2E7
		public string PackageIdPlayerFacing
		{
			get
			{
				return this.packageIdPlayerFacingInt;
			}
		}

		
		// (get) Token: 0x06000E26 RID: 3622 RVA: 0x000510EF File Offset: 0x0004F2EF
		public string FolderName
		{
			get
			{
				return this.rootDirInt.Name;
			}
		}

		
		// (get) Token: 0x06000E27 RID: 3623 RVA: 0x000510FC File Offset: 0x0004F2FC
		public string Name
		{
			get
			{
				return this.nameInt;
			}
		}

		
		// (get) Token: 0x06000E28 RID: 3624 RVA: 0x00051104 File Offset: 0x0004F304
		public int OverwritePriority
		{
			get
			{
				if (!this.IsCoreMod)
				{
					return 1;
				}
				return 0;
			}
		}

		
		// (get) Token: 0x06000E29 RID: 3625 RVA: 0x00051111 File Offset: 0x0004F311
		public bool IsCoreMod
		{
			get
			{
				return this.PackageId == ModContentPack.CoreModPackageId;
			}
		}

		
		// (get) Token: 0x06000E2A RID: 3626 RVA: 0x00051123 File Offset: 0x0004F323
		public IEnumerable<Def> AllDefs
		{
			get
			{
				return this.defs;
			}
		}

		
		// (get) Token: 0x06000E2B RID: 3627 RVA: 0x0005112B File Offset: 0x0004F32B
		public IEnumerable<PatchOperation> Patches
		{
			get
			{
				if (this.patches == null)
				{
					this.LoadPatches();
				}
				return this.patches;
			}
		}

		
		public IEnumerable<string> AllAssetNamesInBundle(int index)
		{
			if (this.allAssetNamesInBundleCached == null)
			{
				this.allAssetNamesInBundleCached = new List<List<string>>();
				foreach (AssetBundle assetBundle in this.assetBundles.loadedAssetBundles)
				{
					this.allAssetNamesInBundleCached.Add(new List<string>(assetBundle.GetAllAssetNames()));
				}
			}
			return this.allAssetNamesInBundleCached[index];
		}

		
		[Obsolete("Only need this overload to not break mod compatibility.")]
		public ModContentPack(DirectoryInfo directory, string packageId, int loadOrder, string name) : this(directory, packageId, packageId, loadOrder, name)
		{
		}

		
		public ModContentPack(DirectoryInfo directory, string packageId, string packageIdPlayerFacing, int loadOrder, string name)
		{
			this.rootDirInt = directory;
			this.loadOrder = loadOrder;
			this.nameInt = name;
			this.packageIdInt = packageId.ToLower();
			this.packageIdPlayerFacingInt = packageIdPlayerFacing;
			this.audioClips = new ModContentHolder<AudioClip>(this);
			this.textures = new ModContentHolder<Texture2D>(this);
			this.strings = new ModContentHolder<string>(this);
			this.assetBundles = new ModAssetBundlesHandler(this);
			this.assemblies = new ModAssemblyHandler(this);
			this.InitLoadFolders();
		}

		
		public void ClearDestroy()
		{
			this.audioClips.ClearDestroy();
			this.textures.ClearDestroy();
			this.assetBundles.ClearDestroy();
			this.allAssetNamesInBundleCached = null;
		}

		
		public ModContentHolder<T> GetContentHolder<T>() where T : class
		{
			//if (typeof(T) == typeof(Texture2D))
			//{
			//	return (ModContentHolder<T>)this.textures;
			//}
			//if (typeof(T) == typeof(AudioClip))
			//{
			//	return (ModContentHolder<T>)this.audioClips;
			//}
			//if (typeof(T) == typeof(string))
			//{
			//	return (ModContentHolder<T>)this.strings;
			//}
			Log.Error("Mod lacks manager for asset type " + this.strings, false);
			return null;
		}

		
		private void ReloadContentInt()
		{
			DeepProfiler.Start("Reload audio clips");
			try
			{
				this.audioClips.ReloadAll();
			}
			finally
			{
				DeepProfiler.End();
			}
			DeepProfiler.Start("Reload textures");
			try
			{
				this.textures.ReloadAll();
			}
			finally
			{
				DeepProfiler.End();
			}
			DeepProfiler.Start("Reload strings");
			try
			{
				this.strings.ReloadAll();
			}
			finally
			{
				DeepProfiler.End();
			}
			DeepProfiler.Start("Reload asset bundles");
			try
			{
				this.assetBundles.ReloadAll();
				this.allAssetNamesInBundleCached = null;
			}
			finally
			{
				DeepProfiler.End();
			}
		}

		
		public void ReloadContent()
		{
			LongEventHandler.ExecuteWhenFinished(new Action(this.ReloadContentInt));
			this.assemblies.ReloadAll();
		}

		
		public IEnumerable<LoadableXmlAsset> LoadDefs()
		{
			if (this.defs.Count != 0)
			{
				Log.ErrorOnce("LoadDefs called with already existing def packages", 39029405, false);
			}
			DeepProfiler.Start("Load Assets");
			List<LoadableXmlAsset> list = DirectXmlLoader.XmlAssetsInModFolder(this, "Defs/", null).ToList<LoadableXmlAsset>();
			DeepProfiler.End();
			DeepProfiler.Start("Parse Assets");
			foreach (LoadableXmlAsset loadableXmlAsset in list)
			{
				yield return loadableXmlAsset;
			}
			List<LoadableXmlAsset>.Enumerator enumerator = default(List<LoadableXmlAsset>.Enumerator);
			DeepProfiler.End();
			yield break;
			yield break;
		}

		
		private void InitLoadFolders()
		{
			this.foldersToLoadDescendingOrder = new List<string>();
			ModMetaData modWithIdentifier = ModLister.GetModWithIdentifier(this.PackageId, false);
			if (((modWithIdentifier != null) ? modWithIdentifier.loadFolders : null) != null && modWithIdentifier.loadFolders.DefinedVersions().Count > 0)
			{
				List<LoadFolder> list = modWithIdentifier.LoadFoldersForVersion(VersionControl.CurrentVersionStringWithoutBuild);
				//if (list != null && list.Count > 0)
				//{
				//	this.<InitLoadFolders>g__AddFolders|45_0(list);
				//	return;
				//}
				//List<LoadFolder> list2 = modWithIdentifier.LoadFoldersForVersion("default");
				//if (list2 != null)
				//{
				//	this.<InitLoadFolders>g__AddFolders|45_0(list2);
				//	return;
				//}
				int num = VersionControl.CurrentVersion.Major;
				int num2 = VersionControl.CurrentVersion.Minor;
				//List<LoadFolder> list3;
				//do
				//{
				//	if (num2 == 0)
				//	{
				//		num--;
				//		num2 = 9;
				//	}
				//	else
				//	{
				//		num2--;
				//	}
				//	if (num < 1)
				//	{
				//		goto IL_D1;
				//	}
				//	list3 = modWithIdentifier.LoadFoldersForVersion(num + "." + num2);
				//}
				////while (list3 == null);
				////this.<InitLoadFolders>g__AddFolders|45_0(list3);
				////return;
				//IL_D1:
				Version version = new Version(0, 0);
				List<string> list4 = modWithIdentifier.loadFolders.DefinedVersions();
				for (int i = 0; i < list4.Count; i++)
				{
					Version version2;
					if (VersionControl.TryParseVersionString(list4[i], out version2) && version2 > version)
					{
						version = version2;
					}
				}
				if (version.Major > 0)
				{
					//this.<InitLoadFolders>g__AddFolders|45_0(modWithIdentifier.LoadFoldersForVersion(version.ToString()));
					return;
				}
			}
			if (this.foldersToLoadDescendingOrder.Count == 0)
			{
				string text = Path.Combine(this.RootDir, VersionControl.CurrentVersionStringWithoutBuild);
				if (Directory.Exists(text))
				{
					this.foldersToLoadDescendingOrder.Add(text);
				}
				else
				{
					Version version3 = new Version(0, 0);
					DirectoryInfo[] directories = this.rootDirInt.GetDirectories();
					for (int j = 0; j < directories.Length; j++)
					{
						Version version4;
						if (VersionControl.TryParseVersionString(directories[j].Name, out version4) && version4 > version3)
						{
							version3 = version4;
						}
					}
					if (version3.Major > 0)
					{
						this.foldersToLoadDescendingOrder.Add(Path.Combine(this.RootDir, version3.ToString()));
					}
				}
				string text2 = Path.Combine(this.RootDir, ModContentPack.CommonFolderName);
				if (Directory.Exists(text2))
				{
					this.foldersToLoadDescendingOrder.Add(text2);
				}
				this.foldersToLoadDescendingOrder.Add(this.RootDir);
			}
		}

		
		private void LoadPatches()
		{
			DeepProfiler.Start("Loading all patches");
			this.patches = new List<PatchOperation>();
			this.loadedAnyPatches = false;
			List<LoadableXmlAsset> list = DirectXmlLoader.XmlAssetsInModFolder(this, "Patches/", null).ToList<LoadableXmlAsset>();
			for (int i = 0; i < list.Count; i++)
			{
				XmlElement documentElement = list[i].xmlDoc.DocumentElement;
				if (documentElement.Name != "Patch")
				{
					Log.Error(string.Format("Unexpected document element in patch XML; got {0}, expected 'Patch'", documentElement.Name), false);
				}
				else
				{
					foreach (object obj in documentElement.ChildNodes)
					{
						XmlNode xmlNode = (XmlNode)obj;
						if (xmlNode.NodeType == XmlNodeType.Element)
						{
							if (xmlNode.Name != "Operation")
							{
								Log.Error(string.Format("Unexpected element in patch XML; got {0}, expected 'Operation'", xmlNode.Name), false);
							}
							else
							{
								PatchOperation patchOperation = DirectXmlToObject.ObjectFromXml<PatchOperation>(xmlNode, false);
								patchOperation.sourceFile = list[i].FullFilePath;
								this.patches.Add(patchOperation);
								this.loadedAnyPatches = true;
							}
						}
					}
				}
			}
			DeepProfiler.End();
		}

		
		public static Dictionary<string, FileInfo> GetAllFilesForMod(ModContentPack mod, string contentPath, Func<string, bool> validateExtension = null, List<string> foldersToLoadDebug = null)
		{
			List<string> list = foldersToLoadDebug ?? mod.foldersToLoadDescendingOrder;
			Dictionary<string, FileInfo> dictionary = new Dictionary<string, FileInfo>();
			for (int i = 0; i < list.Count; i++)
			{
				string text = list[i];
				DirectoryInfo directoryInfo = new DirectoryInfo(Path.Combine(text, contentPath));
				if (directoryInfo.Exists)
				{
					foreach (FileInfo fileInfo in directoryInfo.GetFiles("*.*", SearchOption.AllDirectories))
					{
						if (validateExtension == null || validateExtension(fileInfo.Extension))
						{
							string key = fileInfo.FullName.Substring(text.Length + 1);
							if (!dictionary.ContainsKey(key))
							{
								dictionary.Add(key, fileInfo);
							}
						}
					}
				}
			}
			return dictionary;
		}

		
		public static List<Tuple<string, FileInfo>> GetAllFilesForModPreserveOrder(ModContentPack mod, string contentPath, Func<string, bool> validateExtension = null, List<string> foldersToLoadDebug = null)
		{
			List<string> list = foldersToLoadDebug ?? mod.foldersToLoadDescendingOrder;
			List<Tuple<string, FileInfo>> list2 = new List<Tuple<string, FileInfo>>();
			for (int i = list.Count - 1; i >= 0; i--)
			{
				string text = list[i];
				DirectoryInfo directoryInfo = new DirectoryInfo(Path.Combine(text, contentPath));
				if (directoryInfo.Exists)
				{
					foreach (FileInfo fileInfo in directoryInfo.GetFiles("*.*", SearchOption.AllDirectories))
					{
						if (validateExtension == null || validateExtension(fileInfo.Extension))
						{
							string item = fileInfo.FullName.Substring(text.Length + 1);
							list2.Add(new Tuple<string, FileInfo>(item, fileInfo));
						}
					}
				}
			}
			HashSet<string> hashSet = new HashSet<string>();
			for (int k = list2.Count - 1; k >= 0; k--)
			{
				Tuple<string, FileInfo> tuple = list2[k];
				if (!hashSet.Contains(tuple.Item1))
				{
					hashSet.Add(tuple.Item1);
				}
				else
				{
					list2.RemoveAt(k);
				}
			}
			return list2;
		}

		
		public bool AnyContentLoaded()
		{
			if (this.textures.contentList != null && this.textures.contentList.Count != 0)
			{
				return true;
			}
			if (this.audioClips.contentList != null && this.audioClips.contentList.Count != 0)
			{
				return true;
			}
			if (this.strings.contentList != null && this.strings.contentList.Count != 0)
			{
				return true;
			}
			if (!this.assemblies.loadedAssemblies.NullOrEmpty<Assembly>())
			{
				return true;
			}
			if (!this.assetBundles.loadedAssetBundles.NullOrEmpty<AssetBundle>())
			{
				return true;
			}
			if (this.loadedAnyPatches)
			{
				return true;
			}
			if (this.AllDefs.Any<Def>())
			{
				return true;
			}
			foreach (string path in this.foldersToLoadDescendingOrder)
			{
				string path2 = Path.Combine(path, "Languages");
				if (Directory.Exists(path2) && Directory.EnumerateFiles(path2, "*", SearchOption.AllDirectories).Any<string>())
				{
					return true;
				}
			}
			return false;
		}

		
		public void ClearPatchesCache()
		{
			this.patches = null;
		}

		
		public void AddDef(Def def, string source = "Unknown")
		{
			def.modContentPack = this;
			def.fileName = source;
			this.defs.Add(def);
		}

		
		public override string ToString()
		{
			return this.PackageIdPlayerFacing;
		}

		
		private DirectoryInfo rootDirInt;

		
		public int loadOrder;

		
		private string nameInt;

		
		private string packageIdInt;

		
		private string packageIdPlayerFacingInt;

		
		private ModContentHolder<AudioClip> audioClips;

		
		private ModContentHolder<Texture2D> textures;

		
		private ModContentHolder<string> strings;

		
		public ModAssetBundlesHandler assetBundles;

		
		public ModAssemblyHandler assemblies;

		
		private List<PatchOperation> patches;

		
		private List<Def> defs = new List<Def>();

		
		private List<List<string>> allAssetNamesInBundleCached;

		
		public List<string> foldersToLoadDescendingOrder;

		
		private bool loadedAnyPatches;

		
		public static readonly string LudeonPackageIdAuthor = "ludeon";

		
		public static readonly string CoreModPackageId = "ludeon.rimworld";

		
		public static readonly string RoyaltyModPackageId = "ludeon.rimworld.royalty";

		
		public static readonly string CommonFolderName = "Common";
	}
}
