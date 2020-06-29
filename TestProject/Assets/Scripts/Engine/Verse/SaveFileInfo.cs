using System;
using System.IO;
using RimWorld;
using UnityEngine;

namespace Verse
{
	
	public struct SaveFileInfo
	{
		
		
		public bool Valid
		{
			get
			{
				return this.gameVersion != null;
			}
		}

		
		
		public FileInfo FileInfo
		{
			get
			{
				return this.fileInfo;
			}
		}

		
		
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
