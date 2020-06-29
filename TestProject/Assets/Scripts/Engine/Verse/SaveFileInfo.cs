using System;
using System.IO;
using RimWorld;
using UnityEngine;

namespace Verse
{
	
	public struct SaveFileInfo
	{
		
		// (get) Token: 0x0600140A RID: 5130 RVA: 0x00074832 File Offset: 0x00072A32
		public bool Valid
		{
			get
			{
				return this.gameVersion != null;
			}
		}

		
		// (get) Token: 0x0600140B RID: 5131 RVA: 0x0007483D File Offset: 0x00072A3D
		public FileInfo FileInfo
		{
			get
			{
				return this.fileInfo;
			}
		}

		
		// (get) Token: 0x0600140C RID: 5132 RVA: 0x00074845 File Offset: 0x00072A45
		public string GameVersion
		{
			get
			{
				if (!this.Valid)
				{
					return "???";
				}
				return this.gameVersion;
			}
		}

		
		// (get) Token: 0x0600140D RID: 5133 RVA: 0x0007485C File Offset: 0x00072A5C
		public Color VersionColor
		{
			get
			{
				if (!this.Valid)
				{
					return ColoredText.RedReadable;
				}
				if (VersionControl.MajorFromVersionString(this.gameVersion) == VersionControl.CurrentMajor && VersionControl.MinorFromVersionString(this.gameVersion) == VersionControl.CurrentMinor)
				{
					return SaveFileInfo.UnimportantTextColor;
				}
				if (BackCompatibility.IsSaveCompatibleWith(this.gameVersion))
				{
					return Color.yellow;
				}
				return ColoredText.RedReadable;
			}
		}

		
		// (get) Token: 0x0600140E RID: 5134 RVA: 0x000748BC File Offset: 0x00072ABC
		public TipSignal CompatibilityTip
		{
			get
			{
				if (!this.Valid)
				{
					return "SaveIsUnknownFormat".Translate();
				}
				if ((VersionControl.MajorFromVersionString(this.gameVersion) != VersionControl.CurrentMajor || VersionControl.MinorFromVersionString(this.gameVersion) != VersionControl.CurrentMinor) && !BackCompatibility.IsSaveCompatibleWith(this.gameVersion))
				{
					return "SaveIsFromDifferentGameVersion".Translate(VersionControl.CurrentVersionString, this.gameVersion);
				}
				if (VersionControl.BuildFromVersionString(this.gameVersion) != VersionControl.CurrentBuild)
				{
					return "SaveIsFromDifferentGameBuild".Translate(VersionControl.CurrentVersionString, this.gameVersion);
				}
				return "SaveIsFromThisGameBuild".Translate();
			}
		}

		
		public SaveFileInfo(FileInfo fileInfo)
		{
			this.fileInfo = fileInfo;
			this.gameVersion = ScribeMetaHeaderUtility.GameVersionOf(fileInfo);
		}

		
		private FileInfo fileInfo;

		
		private string gameVersion;

		
		public static readonly Color UnimportantTextColor = new Color(1f, 1f, 1f, 0.5f);
	}
}
