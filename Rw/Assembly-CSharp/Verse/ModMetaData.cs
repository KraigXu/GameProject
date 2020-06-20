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
	// Token: 0x020001F8 RID: 504
	public class ModMetaData : WorkshopUploadable
	{
		// Token: 0x170002C0 RID: 704
		// (get) Token: 0x06000E4E RID: 3662 RVA: 0x00052278 File Offset: 0x00050478
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

		// Token: 0x170002C1 RID: 705
		// (get) Token: 0x06000E4F RID: 3663 RVA: 0x000522D2 File Offset: 0x000504D2
		public string FolderName
		{
			get
			{
				return this.RootDir.Name;
			}
		}

		// Token: 0x170002C2 RID: 706
		// (get) Token: 0x06000E50 RID: 3664 RVA: 0x000522DF File Offset: 0x000504DF
		public DirectoryInfo RootDir
		{
			get
			{
				return this.rootDirInt;
			}
		}

		// Token: 0x170002C3 RID: 707
		// (get) Token: 0x06000E51 RID: 3665 RVA: 0x000522E7 File Offset: 0x000504E7
		public bool IsCoreMod
		{
			get
			{
				return this.SamePackageId(ModContentPack.CoreModPackageId, false);
			}
		}

		// Token: 0x170002C4 RID: 708
		// (get) Token: 0x06000E52 RID: 3666 RVA: 0x000522F5 File Offset: 0x000504F5
		// (set) Token: 0x06000E53 RID: 3667 RVA: 0x000522FD File Offset: 0x000504FD
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

		// Token: 0x170002C5 RID: 709
		// (get) Token: 0x06000E54 RID: 3668 RVA: 0x00052306 File Offset: 0x00050506
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

		// Token: 0x170002C6 RID: 710
		// (get) Token: 0x06000E55 RID: 3669 RVA: 0x00052341 File Offset: 0x00050541
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

		// Token: 0x170002C7 RID: 711
		// (get) Token: 0x06000E56 RID: 3670 RVA: 0x0005237C File Offset: 0x0005057C
		public ExpansionDef Expansion
		{
			get
			{
				return ModLister.GetExpansionWithIdentifier(this.PackageId);
			}
		}

		// Token: 0x170002C8 RID: 712
		// (get) Token: 0x06000E57 RID: 3671 RVA: 0x0005238C File Offset: 0x0005058C
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

		// Token: 0x170002C9 RID: 713
		// (get) Token: 0x06000E58 RID: 3672 RVA: 0x000523B8 File Offset: 0x000505B8
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

		// Token: 0x170002CA RID: 714
		// (get) Token: 0x06000E59 RID: 3673 RVA: 0x000523FB File Offset: 0x000505FB
		public string Author
		{
			get
			{
				return this.meta.author;
			}
		}

		// Token: 0x170002CB RID: 715
		// (get) Token: 0x06000E5A RID: 3674 RVA: 0x00052408 File Offset: 0x00050608
		public string Url
		{
			get
			{
				return this.meta.url;
			}
		}

		// Token: 0x170002CC RID: 716
		// (get) Token: 0x06000E5B RID: 3675 RVA: 0x00052415 File Offset: 0x00050615
		public int SteamAppId
		{
			get
			{
				return this.meta.steamAppId;
			}
		}

		// Token: 0x170002CD RID: 717
		// (get) Token: 0x06000E5C RID: 3676 RVA: 0x00052424 File Offset: 0x00050624
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

		// Token: 0x170002CE RID: 718
		// (get) Token: 0x06000E5D RID: 3677 RVA: 0x00052476 File Offset: 0x00050676
		public List<System.Version> SupportedVersionsReadOnly
		{
			get
			{
				return this.meta.SupportedVersions;
			}
		}

		// Token: 0x170002CF RID: 719
		// (get) Token: 0x06000E5E RID: 3678 RVA: 0x00052483 File Offset: 0x00050683
		IEnumerable<System.Version> WorkshopUploadable.SupportedVersions
		{
			get
			{
				return this.SupportedVersionsReadOnly;
			}
		}

		// Token: 0x170002D0 RID: 720
		// (get) Token: 0x06000E5F RID: 3679 RVA: 0x0005248C File Offset: 0x0005068C
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

		// Token: 0x170002D1 RID: 721
		// (get) Token: 0x06000E60 RID: 3680 RVA: 0x000524E2 File Offset: 0x000506E2
		public bool Official
		{
			get
			{
				return this.IsCoreMod || this.Source == ContentSource.OfficialModsFolder;
			}
		}

		// Token: 0x170002D2 RID: 722
		// (get) Token: 0x06000E61 RID: 3681 RVA: 0x000524F7 File Offset: 0x000506F7
		public ContentSource Source
		{
			get
			{
				return this.source;
			}
		}

		// Token: 0x170002D3 RID: 723
		// (get) Token: 0x06000E62 RID: 3682 RVA: 0x000524FF File Offset: 0x000506FF
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

		// Token: 0x170002D4 RID: 724
		// (get) Token: 0x06000E63 RID: 3683 RVA: 0x00052520 File Offset: 0x00050720
		public string PackageIdNonUnique
		{
			get
			{
				return this.packageIdLowerCase;
			}
		}

		// Token: 0x170002D5 RID: 725
		// (get) Token: 0x06000E64 RID: 3684 RVA: 0x00052528 File Offset: 0x00050728
		public string PackageIdPlayerFacing
		{
			get
			{
				return this.meta.packageId;
			}
		}

		// Token: 0x170002D6 RID: 726
		// (get) Token: 0x06000E65 RID: 3685 RVA: 0x00052535 File Offset: 0x00050735
		public List<ModDependency> Dependencies
		{
			get
			{
				return this.meta.modDependencies;
			}
		}

		// Token: 0x170002D7 RID: 727
		// (get) Token: 0x06000E66 RID: 3686 RVA: 0x00052542 File Offset: 0x00050742
		public List<string> LoadBefore
		{
			get
			{
				return this.meta.loadBefore;
			}
		}

		// Token: 0x170002D8 RID: 728
		// (get) Token: 0x06000E67 RID: 3687 RVA: 0x0005254F File Offset: 0x0005074F
		public List<string> LoadAfter
		{
			get
			{
				return this.meta.loadAfter;
			}
		}

		// Token: 0x170002D9 RID: 729
		// (get) Token: 0x06000E68 RID: 3688 RVA: 0x0005255C File Offset: 0x0005075C
		public List<string> IncompatibleWith
		{
			get
			{
				return this.meta.incompatibleWith;
			}
		}

		// Token: 0x06000E69 RID: 3689 RVA: 0x0005256C File Offset: 0x0005076C
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

		// Token: 0x170002DA RID: 730
		// (get) Token: 0x06000E6A RID: 3690 RVA: 0x000525C6 File Offset: 0x000507C6
		// (set) Token: 0x06000E6B RID: 3691 RVA: 0x000525CE File Offset: 0x000507CE
		public bool HadIncorrectlyFormattedVersionInMetadata { get; private set; }

		// Token: 0x170002DB RID: 731
		// (get) Token: 0x06000E6C RID: 3692 RVA: 0x000525D7 File Offset: 0x000507D7
		// (set) Token: 0x06000E6D RID: 3693 RVA: 0x000525DF File Offset: 0x000507DF
		public bool HadIncorrectlyFormattedPackageId { get; private set; }

		// Token: 0x06000E6E RID: 3694 RVA: 0x000525E8 File Offset: 0x000507E8
		public ModMetaData(string localAbsPath, bool official = false)
		{
			this.rootDirInt = new DirectoryInfo(localAbsPath);
			this.source = (official ? ContentSource.OfficialModsFolder : ContentSource.ModsFolder);
			this.Init();
		}

		// Token: 0x06000E6F RID: 3695 RVA: 0x00052644 File Offset: 0x00050844
		public ModMetaData(WorkshopItem workshopItem)
		{
			this.rootDirInt = workshopItem.Directory;
			this.source = ContentSource.SteamWorkshop;
			this.Init();
		}

		// Token: 0x06000E70 RID: 3696 RVA: 0x00052698 File Offset: 0x00050898
		public void UnsetPreviewImage()
		{
			this.previewImage = null;
		}

		// Token: 0x06000E71 RID: 3697 RVA: 0x000526A1 File Offset: 0x000508A1
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

		// Token: 0x06000E72 RID: 3698 RVA: 0x000526CB File Offset: 0x000508CB
		public List<LoadFolder> LoadFoldersForVersion(string version)
		{
			ModLoadFolders modLoadFolders = this.loadFolders;
			if (modLoadFolders == null)
			{
				return null;
			}
			return modLoadFolders.FoldersForVersion(version);
		}

		// Token: 0x06000E73 RID: 3699 RVA: 0x000526E0 File Offset: 0x000508E0
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

		// Token: 0x06000E74 RID: 3700 RVA: 0x0005285E File Offset: 0x00050A5E
		internal void DeleteContent()
		{
			this.rootDirInt.Delete(true);
			ModLister.RebuildModList();
		}

		// Token: 0x170002DC RID: 732
		// (get) Token: 0x06000E75 RID: 3701 RVA: 0x00052871 File Offset: 0x00050A71
		public bool OnSteamWorkshop
		{
			get
			{
				return this.source == ContentSource.SteamWorkshop;
			}
		}

		// Token: 0x170002DD RID: 733
		// (get) Token: 0x06000E76 RID: 3702 RVA: 0x0005287C File Offset: 0x00050A7C
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

		// Token: 0x06000E77 RID: 3703 RVA: 0x00002681 File Offset: 0x00000881
		public void PrepareForWorkshopUpload()
		{
		}

		// Token: 0x06000E78 RID: 3704 RVA: 0x000528D2 File Offset: 0x00050AD2
		public bool CanToUploadToWorkshop()
		{
			return !this.Official && this.Source == ContentSource.ModsFolder && !this.GetWorkshopItemHook().MayHaveAuthorNotCurrentUser;
		}

		// Token: 0x06000E79 RID: 3705 RVA: 0x000528F9 File Offset: 0x00050AF9
		public PublishedFileId_t GetPublishedFileId()
		{
			return this.publishedFileIdInt;
		}

		// Token: 0x06000E7A RID: 3706 RVA: 0x00052901 File Offset: 0x00050B01
		public void SetPublishedFileId(PublishedFileId_t newPfid)
		{
			if (this.publishedFileIdInt == newPfid)
			{
				return;
			}
			this.publishedFileIdInt = newPfid;
			File.WriteAllText(this.PublishedFileIdPath, newPfid.ToString());
		}

		// Token: 0x06000E7B RID: 3707 RVA: 0x00052931 File Offset: 0x00050B31
		public string GetWorkshopName()
		{
			return this.Name;
		}

		// Token: 0x06000E7C RID: 3708 RVA: 0x00052939 File Offset: 0x00050B39
		public string GetWorkshopDescription()
		{
			return this.Description;
		}

		// Token: 0x06000E7D RID: 3709 RVA: 0x00052941 File Offset: 0x00050B41
		public string GetWorkshopPreviewImagePath()
		{
			return this.PreviewImagePath;
		}

		// Token: 0x06000E7E RID: 3710 RVA: 0x00052949 File Offset: 0x00050B49
		public IList<string> GetWorkshopTags()
		{
			return new List<string>
			{
				"Mod"
			};
		}

		// Token: 0x06000E7F RID: 3711 RVA: 0x0005295B File Offset: 0x00050B5B
		public DirectoryInfo GetWorkshopUploadDirectory()
		{
			return this.RootDir;
		}

		// Token: 0x06000E80 RID: 3712 RVA: 0x00052963 File Offset: 0x00050B63
		public WorkshopItemHook GetWorkshopItemHook()
		{
			if (this.workshopHookInt == null)
			{
				this.workshopHookInt = new WorkshopItemHook(this);
			}
			return this.workshopHookInt;
		}

		// Token: 0x06000E81 RID: 3713 RVA: 0x0005297F File Offset: 0x00050B7F
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

		// Token: 0x06000E82 RID: 3714 RVA: 0x0005298F File Offset: 0x00050B8F
		public override int GetHashCode()
		{
			return this.PackageId.GetHashCode();
		}

		// Token: 0x06000E83 RID: 3715 RVA: 0x0005299C File Offset: 0x00050B9C
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

		// Token: 0x06000E84 RID: 3716 RVA: 0x000529D3 File Offset: 0x00050BD3
		public string ToStringLong()
		{
			return this.PackageIdPlayerFacing + "(" + this.RootDir.ToString() + ")";
		}

		// Token: 0x04000AC8 RID: 2760
		private DirectoryInfo rootDirInt;

		// Token: 0x04000AC9 RID: 2761
		private ContentSource source;

		// Token: 0x04000ACA RID: 2762
		private Texture2D previewImage;

		// Token: 0x04000ACB RID: 2763
		private bool previewImageWasLoaded;

		// Token: 0x04000ACC RID: 2764
		public bool enabled = true;

		// Token: 0x04000ACD RID: 2765
		private ModMetaData.ModMetaDataInternal meta = new ModMetaData.ModMetaDataInternal();

		// Token: 0x04000ACE RID: 2766
		public ModLoadFolders loadFolders;

		// Token: 0x04000ACF RID: 2767
		private WorkshopItemHook workshopHookInt;

		// Token: 0x04000AD0 RID: 2768
		private PublishedFileId_t publishedFileIdInt = PublishedFileId_t.Invalid;

		// Token: 0x04000AD1 RID: 2769
		public bool appendPackageIdSteamPostfix;

		// Token: 0x04000AD2 RID: 2770
		private string packageIdLowerCase;

		// Token: 0x04000AD3 RID: 2771
		private string descriptionCached;

		// Token: 0x04000AD4 RID: 2772
		private const string AboutFolderName = "About";

		// Token: 0x04000AD5 RID: 2773
		public static readonly string SteamModPostfix = "_steam";

		// Token: 0x04000AD6 RID: 2774
		private List<string> unsatisfiedDepsList = new List<string>();

		// Token: 0x0200140C RID: 5132
		private class ModMetaDataInternal
		{
			// Token: 0x1700147A RID: 5242
			// (get) Token: 0x060078F1 RID: 30961 RVA: 0x00294D51 File Offset: 0x00292F51
			// (set) Token: 0x060078F2 RID: 30962 RVA: 0x00294D59 File Offset: 0x00292F59
			public List<System.Version> SupportedVersions { get; private set; }

			// Token: 0x060078F3 RID: 30963 RVA: 0x00294D64 File Offset: 0x00292F64
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

			// Token: 0x060078F4 RID: 30964 RVA: 0x00294E60 File Offset: 0x00293060
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

			// Token: 0x060078F5 RID: 30965 RVA: 0x00294FDC File Offset: 0x002931DC
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

			// Token: 0x060078F6 RID: 30966 RVA: 0x00295124 File Offset: 0x00293324
			private string ConvertToASCII(string part)
			{
				StringBuilder stringBuilder = new StringBuilder("");
				foreach (char c in part)
				{
					if (!char.IsLetterOrDigit(c) || c >= '\u0080')
					{
						c = c % '\u0019' + 'A';
					}
					stringBuilder.Append(c);
				}
				return stringBuilder.ToString();
			}

			// Token: 0x060078F7 RID: 30967 RVA: 0x0029517C File Offset: 0x0029337C
			[Obsolete("Only need this overload to not break mod compatibility.")]
			public void ValidateDependencies()
			{
				this.ValidateDependencies_NewTmp(true);
			}

			// Token: 0x060078F8 RID: 30968 RVA: 0x00295188 File Offset: 0x00293388
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

			// Token: 0x060078F9 RID: 30969 RVA: 0x002952F8 File Offset: 0x002934F8
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

			// Token: 0x04004C36 RID: 19510
			public string packageId = "";

			// Token: 0x04004C37 RID: 19511
			public string name = "";

			// Token: 0x04004C38 RID: 19512
			public string author = "Anonymous";

			// Token: 0x04004C39 RID: 19513
			public string url = "";

			// Token: 0x04004C3A RID: 19514
			public string description = "No description provided.";

			// Token: 0x04004C3B RID: 19515
			public int steamAppId;

			// Token: 0x04004C3C RID: 19516
			public List<string> supportedVersions;

			// Token: 0x04004C3D RID: 19517
			[Unsaved(true)]
			private string targetVersion;

			// Token: 0x04004C3E RID: 19518
			public List<ModDependency> modDependencies = new List<ModDependency>();

			// Token: 0x04004C3F RID: 19519
			public List<string> loadBefore = new List<string>();

			// Token: 0x04004C40 RID: 19520
			public List<string> loadAfter = new List<string>();

			// Token: 0x04004C41 RID: 19521
			public List<string> incompatibleWith = new List<string>();

			// Token: 0x04004C42 RID: 19522
			private ModMetaData.VersionedData<string> descriptionsByVersion;

			// Token: 0x04004C43 RID: 19523
			private ModMetaData.VersionedData<List<ModDependency>> modDependenciesByVersion;

			// Token: 0x04004C44 RID: 19524
			private ModMetaData.VersionedData<List<string>> loadBeforeByVersion;

			// Token: 0x04004C45 RID: 19525
			private ModMetaData.VersionedData<List<string>> loadAfterByVersion;

			// Token: 0x04004C46 RID: 19526
			private ModMetaData.VersionedData<List<string>> incompatibleWithByVersion;

			// Token: 0x04004C47 RID: 19527
			public static readonly Regex PackageIdFormatRegex = new Regex("(?=.{1,60}$)^(?!\\.)(?=.*?[.])(?!.*([.])\\1+)[a-zA-Z0-9.]{1,}[a-zA-Z0-9]{1}$");
		}

		// Token: 0x0200140D RID: 5133
		private class VersionedData<T> where T : class
		{
			// Token: 0x060078FC RID: 30972 RVA: 0x00295430 File Offset: 0x00293630
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

			// Token: 0x060078FD RID: 30973 RVA: 0x00295530 File Offset: 0x00293730
			public T GetItemForVersion(string ver)
			{
				if (this.itemForVersion.ContainsKey(ver))
				{
					return this.itemForVersion[ver];
				}
				return default(T);
			}

			// Token: 0x04004C49 RID: 19529
			private Dictionary<string, T> itemForVersion = new Dictionary<string, T>();
		}
	}
}
