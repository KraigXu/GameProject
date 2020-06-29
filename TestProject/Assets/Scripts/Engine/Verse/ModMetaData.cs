using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using RimWorld;
using Steamworks;
using UnityEngine;
using Verse.Steam;

namespace Verse
{
	
	public class ModMetaData : WorkshopUploadable
	{
		
		
		public Texture2D PreviewImage
		{
			get
			{
				if (this.previewImageWasLoaded)
				{
					return this.previewImage;
				}
				if (File.Exists(this.PreviewImagePath))
				{
					this.previewImage = new Texture2D(0, 0);
					this.previewImage.LoadImage(File.ReadAllBytes(this.PreviewImagePath));
				}
				this.previewImageWasLoaded = true;
				return this.previewImage;
			}
		}

		
		
		public string FolderName
		{
			get
			{
				return this.RootDir.Name;
			}
		}

		
		
		public DirectoryInfo RootDir
		{
			get
			{
				return this.rootDirInt;
			}
		}

		
		
		public bool IsCoreMod
		{
			get
			{
				return this.SamePackageId(ModContentPack.CoreModPackageId, false);
			}
		}

		
		
		
		public bool Active
		{
			get
			{
				return ModsConfig.IsActive(this);
			}
			set
			{
				ModsConfig.SetActive(this, value);
			}
		}

		
		
		public bool VersionCompatible
		{
			get
			{
				if (this.IsCoreMod)
				{
					return true;
				}
				return this.meta.SupportedVersions.Any((System.Version v) => VersionControl.IsCompatible(v));
			}
		}

		
		
		public bool MadeForNewerVersion
		{
			get
			{
				if (this.VersionCompatible)
				{
					return false;
				}
				return this.meta.SupportedVersions.Any((System.Version v) => v.Major > VersionControl.CurrentMajor || (v.Major == VersionControl.CurrentMajor && v.Minor > VersionControl.CurrentMinor));
			}
		}

		
		
		public ExpansionDef Expansion
		{
			get
			{
				return ModLister.GetExpansionWithIdentifier(this.PackageId);
			}
		}

		
		
		public string Name
		{
			get
			{
				ExpansionDef expansion = this.Expansion;
				if (expansion == null)
				{
					return this.meta.name;
				}
				return expansion.label;
			}
		}

		
		
		public string Description
		{
			get
			{
				if (this.descriptionCached == null)
				{
					ExpansionDef expansionWithIdentifier = ModLister.GetExpansionWithIdentifier(this.PackageId);
					this.descriptionCached = ((expansionWithIdentifier != null) ? expansionWithIdentifier.description : this.meta.description);
				}
				return this.descriptionCached;
			}
		}

		
		
		public string Author
		{
			get
			{
				return this.meta.author;
			}
		}

		
		
		public string Url
		{
			get
			{
				return this.meta.url;
			}
		}

		
		
		public int SteamAppId
		{
			get
			{
				return this.meta.steamAppId;
			}
		}

		
		
		[Obsolete("Deprecated, will be removed in the future. Use SupportedVersions instead")]
		public string TargetVersion
		{
			get
			{
				if (this.SupportedVersionsReadOnly.Count == 0)
				{
					return "Unknown";
				}
				System.Version version = this.meta.SupportedVersions[0];
				return version.Major + "." + version.Minor;
			}
		}

		
		
		public List<System.Version> SupportedVersionsReadOnly
		{
			get
			{
				return this.meta.SupportedVersions;
			}
		}

		
		
		IEnumerable<System.Version> WorkshopUploadable.SupportedVersions
		{
			get
			{
				return this.SupportedVersionsReadOnly;
			}
		}

		
		
		public string PreviewImagePath
		{
			get
			{
				return string.Concat(new string[]
				{
					this.rootDirInt.FullName,
					Path.DirectorySeparatorChar.ToString(),
					"About",
					Path.DirectorySeparatorChar.ToString(),
					"Preview.png"
				});
			}
		}

		
		
		public bool Official
		{
			get
			{
				return this.IsCoreMod || this.Source == ContentSource.OfficialModsFolder;
			}
		}

		
		
		public ContentSource Source
		{
			get
			{
				return this.source;
			}
		}

		
		
		public string PackageId
		{
			get
			{
				if (!this.appendPackageIdSteamPostfix)
				{
					return this.packageIdLowerCase;
				}
				return this.packageIdLowerCase + ModMetaData.SteamModPostfix;
			}
		}

		
		
		public string PackageIdNonUnique
		{
			get
			{
				return this.packageIdLowerCase;
			}
		}

		
		
		public string PackageIdPlayerFacing
		{
			get
			{
				return this.meta.packageId;
			}
		}

		
		
		public List<ModDependency> Dependencies
		{
			get
			{
				return this.meta.modDependencies;
			}
		}

		
		
		public List<string> LoadBefore
		{
			get
			{
				return this.meta.loadBefore;
			}
		}

		
		
		public List<string> LoadAfter
		{
			get
			{
				return this.meta.loadAfter;
			}
		}

		
		
		public List<string> IncompatibleWith
		{
			get
			{
				return this.meta.incompatibleWith;
			}
		}

		
		public List<string> UnsatisfiedDependencies()
		{
			this.unsatisfiedDepsList.Clear();
			for (int i = 0; i < this.Dependencies.Count; i++)
			{
				ModDependency modDependency = this.Dependencies[i];
				if (!modDependency.IsSatisfied)
				{
					this.unsatisfiedDepsList.Add(modDependency.displayName);
				}
			}
			return this.unsatisfiedDepsList;
		}

		
		
		
		public bool HadIncorrectlyFormattedVersionInMetadata { get; private set; }

		
		
		
		public bool HadIncorrectlyFormattedPackageId { get; private set; }

		
		public ModMetaData(string localAbsPath, bool official = false)
		{
			this.rootDirInt = new DirectoryInfo(localAbsPath);
			this.source = (official ? ContentSource.OfficialModsFolder : ContentSource.ModsFolder);
			this.Init();
		}

		
		public ModMetaData(WorkshopItem workshopItem)
		{
			this.rootDirInt = workshopItem.Directory;
			this.source = ContentSource.SteamWorkshop;
			this.Init();
		}

		
		public void UnsetPreviewImage()
		{
			this.previewImage = null;
		}

		
		public bool SamePackageId(string otherPackageId, bool ignorePostfix = false)
		{
			if (this.PackageId == null)
			{
				return false;
			}
			if (ignorePostfix)
			{
				return this.packageIdLowerCase.Equals(otherPackageId, StringComparison.CurrentCultureIgnoreCase);
			}
			return this.PackageId.Equals(otherPackageId, StringComparison.CurrentCultureIgnoreCase);
		}

		
		public List<LoadFolder> LoadFoldersForVersion(string version)
		{
			ModLoadFolders modLoadFolders = this.loadFolders;
			if (modLoadFolders == null)
			{
				return null;
			}
			return modLoadFolders.FoldersForVersion(version);
		}

		
		private void Init()
		{
			this.meta = DirectXmlLoader.ItemFromXmlFile<ModMetaData.ModMetaDataInternal>(string.Concat(new string[]
			{
				this.RootDir.FullName,
				Path.DirectorySeparatorChar.ToString(),
				"About",
				Path.DirectorySeparatorChar.ToString(),
				"About.xml"
			}), true);
			this.loadFolders = DirectXmlLoader.ItemFromXmlFile<ModLoadFolders>(this.RootDir.FullName + Path.DirectorySeparatorChar.ToString() + "LoadFolders.xml", true);
			bool shouldLogIssues = ModLister.ShouldLogIssues;
			this.HadIncorrectlyFormattedVersionInMetadata = !this.meta.TryParseSupportedVersions(!this.OnSteamWorkshop && shouldLogIssues);
			if (this.meta.name.NullOrEmpty())
			{
				if (this.OnSteamWorkshop)
				{
					this.meta.name = "Workshop mod " + this.FolderName;
				}
				else
				{
					this.meta.name = this.FolderName;
				}
			}
			this.HadIncorrectlyFormattedPackageId = !this.meta.TryParsePackageId(this.Official, !this.OnSteamWorkshop && shouldLogIssues);
			this.packageIdLowerCase = this.meta.packageId.ToLower();
			this.meta.InitVersionedData();
			this.meta.ValidateDependencies_NewTmp(shouldLogIssues);
			string publishedFileIdPath = this.PublishedFileIdPath;
			ulong value;
			if (File.Exists(this.PublishedFileIdPath) && ulong.TryParse(File.ReadAllText(publishedFileIdPath), out value))
			{
				this.publishedFileIdInt = new PublishedFileId_t(value);
			}
		}

		
		internal void DeleteContent()
		{
			this.rootDirInt.Delete(true);
			ModLister.RebuildModList();
		}

		
		
		public bool OnSteamWorkshop
		{
			get
			{
				return this.source == ContentSource.SteamWorkshop;
			}
		}

		
		
		private string PublishedFileIdPath
		{
			get
			{
				return string.Concat(new string[]
				{
					this.rootDirInt.FullName,
					Path.DirectorySeparatorChar.ToString(),
					"About",
					Path.DirectorySeparatorChar.ToString(),
					"PublishedFileId.txt"
				});
			}
		}

		
		public void PrepareForWorkshopUpload()
		{
		}

		
		public bool CanToUploadToWorkshop()
		{
			return !this.Official && this.Source == ContentSource.ModsFolder && !this.GetWorkshopItemHook().MayHaveAuthorNotCurrentUser;
		}

		
		public PublishedFileId_t GetPublishedFileId()
		{
			return this.publishedFileIdInt;
		}

		
		public void SetPublishedFileId(PublishedFileId_t newPfid)
		{
			if (this.publishedFileIdInt == newPfid)
			{
				return;
			}
			this.publishedFileIdInt = newPfid;
			File.WriteAllText(this.PublishedFileIdPath, newPfid.ToString());
		}

		
		public string GetWorkshopName()
		{
			return this.Name;
		}

		
		public string GetWorkshopDescription()
		{
			return this.Description;
		}

		
		public string GetWorkshopPreviewImagePath()
		{
			return this.PreviewImagePath;
		}

		
		public IList<string> GetWorkshopTags()
		{
			return new List<string>
			{
				"Mod"
			};
		}

		
		public DirectoryInfo GetWorkshopUploadDirectory()
		{
			return this.RootDir;
		}

		
		public WorkshopItemHook GetWorkshopItemHook()
		{
			if (this.workshopHookInt == null)
			{
				this.workshopHookInt = new WorkshopItemHook(this);
			}
			return this.workshopHookInt;
		}

		
		public IEnumerable<ModRequirement> GetRequirements()
		{
			int num;
			for (int i = 0; i < this.Dependencies.Count; i = num + 1)
			{
				yield return this.Dependencies[i];
				num = i;
			}
			for (int i = 0; i < this.meta.incompatibleWith.Count; i = num + 1)
			{
				ModMetaData modWithIdentifier = ModLister.GetModWithIdentifier(this.meta.incompatibleWith[i], false);
				if (modWithIdentifier != null)
				{
					yield return new ModIncompatibility
					{
						packageId = modWithIdentifier.PackageIdPlayerFacing,
						displayName = modWithIdentifier.Name
					};
				}
				num = i;
			}
			yield break;
		}

		
		public override int GetHashCode()
		{
			return this.PackageId.GetHashCode();
		}

		
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"[",
				this.PackageIdPlayerFacing,
				"|",
				this.Name,
				"]"
			});
		}

		
		public string ToStringLong()
		{
			return this.PackageIdPlayerFacing + "(" + this.RootDir.ToString() + ")";
		}

		
		private DirectoryInfo rootDirInt;

		
		private ContentSource source;

		
		private Texture2D previewImage;

		
		private bool previewImageWasLoaded;

		
		public bool enabled = true;

		
		private ModMetaData.ModMetaDataInternal meta = new ModMetaData.ModMetaDataInternal();

		
		public ModLoadFolders loadFolders;

		
		private WorkshopItemHook workshopHookInt;

		
		private PublishedFileId_t publishedFileIdInt = PublishedFileId_t.Invalid;

		
		public bool appendPackageIdSteamPostfix;

		
		private string packageIdLowerCase;

		
		private string descriptionCached;

		
		private const string AboutFolderName = "About";

		
		public static readonly string SteamModPostfix = "_steam";

		
		private List<string> unsatisfiedDepsList = new List<string>();

		
		private class ModMetaDataInternal
		{
			
			
			
			public List<System.Version> SupportedVersions { get; private set; }

			
			private bool TryParseVersion(string str, bool logIssues = true)
			{
				System.Version version;
				if (!VersionControl.TryParseVersionString(str, out version))
				{
					if (logIssues)
					{
						Log.Error(string.Concat(new string[]
						{
							"Unable to parse version string on mod ",
							this.name,
							" from ",
							this.author,
							" \"",
							str,
							"\""
						}), false);
					}
					return false;
				}
				this.SupportedVersions.Add(version);
				if (!VersionControl.IsWellFormattedVersionString(str))
				{
					if (logIssues)
					{
						Log.Warning(string.Concat(new string[]
						{
							"Malformed (correct format is Major.Minor) version string on mod ",
							this.name,
							" from ",
							this.author,
							" \"",
							str,
							"\" - parsed as \"",
							version.Major.ToString(),
							".",
							version.Minor.ToString(),
							"\""
						}), false);
					}
					return false;
				}
				return true;
			}

			
			public bool TryParseSupportedVersions(bool logIssues = true)
			{
				if (this.targetVersion != null && logIssues)
				{
					Log.Warning("Mod " + this.name + ": targetVersion field is obsolete, use supportedVersions instead.", false);
				}
				bool flag = false;
				this.SupportedVersions = new List<System.Version>();
				if (this.packageId.ToLower() == ModContentPack.CoreModPackageId)
				{
					this.SupportedVersions.Add(VersionControl.CurrentVersion);
				}
				else if (this.supportedVersions == null)
				{
					if (logIssues)
					{
						Log.Warning("Mod " + this.name + " is missing supported versions list in About.xml! (example: <supportedVersions><li>1.0</li></supportedVersions>)", false);
					}
					flag = true;
				}
				else if (this.supportedVersions.Count == 0)
				{
					if (logIssues)
					{
						Log.Error("Mod " + this.name + ": <supportedVersions> in mod About.xml must specify at least one version.", false);
					}
					flag = true;
				}
				else
				{
					for (int i = 0; i < this.supportedVersions.Count; i++)
					{
						flag |= !this.TryParseVersion(this.supportedVersions[i], logIssues);
					}
				}
				this.SupportedVersions = this.SupportedVersions.OrderBy(delegate(System.Version v)
				{
					if (!VersionControl.IsCompatible(v))
					{
						return 100;
					}
					return -100;
				}).ThenByDescending((System.Version v) => v.Major).ThenByDescending((System.Version v) => v.Minor).Distinct<System.Version>().ToList<System.Version>();
				return !flag;
			}

			
			public bool TryParsePackageId(bool isOfficial, bool logIssues = true)
			{
				bool flag = false;
				if (this.packageId.NullOrEmpty())
				{
					string text = "none";
					if (!this.description.NullOrEmpty())
					{
						text = GenText.StableStringHash(this.description).ToString().Replace("-", "");
						text = text.Substring(0, Math.Min(3, text.Length));
					}
					this.packageId = this.ConvertToASCII(this.author + text) + "." + this.ConvertToASCII(this.name);
					if (logIssues)
					{
						Log.Warning("Mod " + this.name + " is missing packageId in About.xml! (example: <packageId>AuthorName.ModName.Specific</packageId>)", false);
					}
					flag = true;
				}
				if (!ModMetaData.ModMetaDataInternal.PackageIdFormatRegex.IsMatch(this.packageId))
				{
					if (logIssues)
					{
						Log.Warning(string.Concat(new string[]
						{
							"Mod ",
							this.name,
							" <packageId> (",
							this.packageId,
							") is not in valid format."
						}), false);
					}
					flag = true;
				}
				if (!isOfficial && this.packageId.ToLower().Contains(ModContentPack.LudeonPackageIdAuthor))
				{
					if (logIssues)
					{
						Log.Warning("Mod " + this.name + " <packageId> contains word \"Ludeon\", which is reserved for official content.", false);
					}
					flag = true;
				}
				return !flag;
			}

			
			private string ConvertToASCII(string part)
			{
				StringBuilder stringBuilder = new StringBuilder("");
				foreach (char c in part)
				{
					if (!char.IsLetterOrDigit(c) || c >= '\u0080')
					{
						//c = c % '\u0019' + 'A';
					}
					stringBuilder.Append(c);
				}
				return stringBuilder.ToString();
			}

			
			[Obsolete("Only need this overload to not break mod compatibility.")]
			public void ValidateDependencies()
			{
				this.ValidateDependencies_NewTmp(true);
			}

			
			public void ValidateDependencies_NewTmp(bool logIssues = true)
			{
				for (int i = this.modDependencies.Count - 1; i >= 0; i--)
				{
					bool flag = false;
					ModDependency modDependency = this.modDependencies[i];
					if (modDependency.packageId.NullOrEmpty())
					{
						if (logIssues)
						{
							Log.Warning("Mod " + this.name + " has a dependency with no <packageId> specified.", false);
						}
						flag = true;
					}
					else if (!ModMetaData.ModMetaDataInternal.PackageIdFormatRegex.IsMatch(modDependency.packageId))
					{
						if (logIssues)
						{
							Log.Warning("Mod " + this.name + " has a dependency with invalid <packageId>: " + modDependency.packageId, false);
						}
						flag = true;
					}
					if (modDependency.displayName.NullOrEmpty())
					{
						if (logIssues)
						{
							Log.Warning(string.Concat(new string[]
							{
								"Mod ",
								this.name,
								" has a dependency (",
								modDependency.packageId,
								") with empty display name."
							}), false);
						}
						flag = true;
					}
					if (modDependency.downloadUrl.NullOrEmpty() && modDependency.steamWorkshopUrl.NullOrEmpty() && !modDependency.packageId.ToLower().Contains(ModContentPack.LudeonPackageIdAuthor))
					{
						if (logIssues)
						{
							Log.Warning(string.Concat(new string[]
							{
								"Mod ",
								this.name,
								" dependency (",
								modDependency.packageId,
								") needs to have <downloadUrl> and/or <steamWorkshopUrl> specified."
							}), false);
						}
						flag = true;
					}
					if (flag)
					{
						this.modDependencies.Remove(modDependency);
					}
				}
			}

			
			public void InitVersionedData()
			{
				string currentVersionStringWithoutBuild = VersionControl.CurrentVersionStringWithoutBuild;
				ModMetaData.VersionedData<string> versionedData = this.descriptionsByVersion;
				string text = (versionedData != null) ? versionedData.GetItemForVersion(currentVersionStringWithoutBuild) : null;
				if (text != null)
				{
					this.description = text;
				}
				ModMetaData.VersionedData<List<ModDependency>> versionedData2 = this.modDependenciesByVersion;
				List<ModDependency> list = (versionedData2 != null) ? versionedData2.GetItemForVersion(currentVersionStringWithoutBuild) : null;
				if (list != null)
				{
					this.modDependencies = list;
				}
				ModMetaData.VersionedData<List<string>> versionedData3 = this.loadBeforeByVersion;
				List<string> list2 = (versionedData3 != null) ? versionedData3.GetItemForVersion(currentVersionStringWithoutBuild) : null;
				if (list2 != null)
				{
					this.loadBefore = list2;
				}
				ModMetaData.VersionedData<List<string>> versionedData4 = this.loadAfterByVersion;
				List<string> list3 = (versionedData4 != null) ? versionedData4.GetItemForVersion(currentVersionStringWithoutBuild) : null;
				if (list3 != null)
				{
					this.loadAfter = list3;
				}
				ModMetaData.VersionedData<List<string>> versionedData5 = this.incompatibleWithByVersion;
				List<string> list4 = (versionedData5 != null) ? versionedData5.GetItemForVersion(currentVersionStringWithoutBuild) : null;
				if (list4 != null)
				{
					this.incompatibleWith = list4;
				}
			}

			
			public string packageId = "";

			
			public string name = "";

			
			public string author = "Anonymous";

			
			public string url = "";

			
			public string description = "No description provided.";

			
			public int steamAppId;

			
			public List<string> supportedVersions;

			
			[Unsaved(true)]
			private string targetVersion;

			
			public List<ModDependency> modDependencies = new List<ModDependency>();

			
			public List<string> loadBefore = new List<string>();

			
			public List<string> loadAfter = new List<string>();

			
			public List<string> incompatibleWith = new List<string>();

			
			private ModMetaData.VersionedData<string> descriptionsByVersion;

			
			private ModMetaData.VersionedData<List<ModDependency>> modDependenciesByVersion;

			
			private ModMetaData.VersionedData<List<string>> loadBeforeByVersion;

			
			private ModMetaData.VersionedData<List<string>> loadAfterByVersion;

			
			private ModMetaData.VersionedData<List<string>> incompatibleWithByVersion;

			
			public static readonly Regex PackageIdFormatRegex = new Regex("(?=.{1,60}$)^(?!\\.)(?=.*?[.])(?!.*([.])\\1+)[a-zA-Z0-9.]{1,}[a-zA-Z0-9]{1}$");
		}

		
		private class VersionedData<T> where T : class
		{
			
			public void LoadDataFromXmlCustom(XmlNode xmlRoot)
			{
				foreach (object obj in xmlRoot.ChildNodes)
				{
					XmlNode xmlNode = (XmlNode)obj;
					if (!(xmlNode is XmlComment))
					{
						string text = xmlNode.Name.ToLower();
						if (text.StartsWith("v"))
						{
							text = text.Substring(1);
						}
						if (!this.itemForVersion.ContainsKey(text))
						{
							this.itemForVersion[text] = ((typeof(T) == typeof(string)) ? ((T)((object)xmlNode.FirstChild.Value)) : DirectXmlToObject.ObjectFromXml<T>(xmlNode, false));
						}
						else
						{
							Log.Warning("More than one value for a same version of " + typeof(T).Name + " named " + xmlRoot.Name, false);
						}
					}
				}
			}

			
			public T GetItemForVersion(string ver)
			{
				if (this.itemForVersion.ContainsKey(ver))
				{
					return this.itemForVersion[ver];
				}
				return default(T);
			}

			
			private Dictionary<string, T> itemForVersion = new Dictionary<string, T>();
		}
	}
}
