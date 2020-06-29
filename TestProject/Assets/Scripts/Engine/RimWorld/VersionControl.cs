using System;
using System.Reflection;
using UnityEngine;
using Verse;
using Verse.Steam;

namespace RimWorld
{
	
	public static class VersionControl
	{
		
		// (get) Token: 0x06002A4B RID: 10827 RVA: 0x000F6C04 File Offset: 0x000F4E04
		public static Version CurrentVersion
		{
			get
			{
				return VersionControl.version;
			}
		}

		
		// (get) Token: 0x06002A4C RID: 10828 RVA: 0x000F6C0B File Offset: 0x000F4E0B
		public static string CurrentVersionString
		{
			get
			{
				return VersionControl.versionString;
			}
		}

		
		// (get) Token: 0x06002A4D RID: 10829 RVA: 0x000F6C12 File Offset: 0x000F4E12
		public static string CurrentVersionStringWithoutBuild
		{
			get
			{
				return VersionControl.versionStringWithoutBuild;
			}
		}

		
		// (get) Token: 0x06002A4E RID: 10830 RVA: 0x000F6C19 File Offset: 0x000F4E19
		public static string CurrentVersionStringWithRev
		{
			get
			{
				return VersionControl.versionStringWithRev;
			}
		}

		
		// (get) Token: 0x06002A4F RID: 10831 RVA: 0x000F6C20 File Offset: 0x000F4E20
		public static int CurrentMajor
		{
			get
			{
				return VersionControl.version.Major;
			}
		}

		
		// (get) Token: 0x06002A50 RID: 10832 RVA: 0x000F6C2C File Offset: 0x000F4E2C
		public static int CurrentMinor
		{
			get
			{
				return VersionControl.version.Minor;
			}
		}

		
		// (get) Token: 0x06002A51 RID: 10833 RVA: 0x000F6C38 File Offset: 0x000F4E38
		public static int CurrentBuild
		{
			get
			{
				return VersionControl.version.Build;
			}
		}

		
		// (get) Token: 0x06002A52 RID: 10834 RVA: 0x000F6C44 File Offset: 0x000F4E44
		public static int CurrentRevision
		{
			get
			{
				return VersionControl.version.Revision;
			}
		}

		
		// (get) Token: 0x06002A53 RID: 10835 RVA: 0x000F6C50 File Offset: 0x000F4E50
		public static DateTime CurrentBuildDate
		{
			get
			{
				return VersionControl.buildDate;
			}
		}

		
		static VersionControl()
		{
			Version version = Assembly.GetExecutingAssembly().GetName().Version;
			VersionControl.buildDate = new DateTime(2000, 1, 1).AddDays((double)version.Build);
			int build = version.Build - 4805;
			int revision = version.Revision * 2 / 60;
			VersionControl.version = new Version(version.Major, version.Minor, build, revision);
			VersionControl.versionStringWithRev = string.Concat(new object[]
			{
				VersionControl.version.Major,
				".",
				VersionControl.version.Minor,
				".",
				VersionControl.version.Build,
				" rev",
				VersionControl.version.Revision
			});
			VersionControl.versionString = string.Concat(new object[]
			{
				VersionControl.version.Major,
				".",
				VersionControl.version.Minor,
				".",
				VersionControl.version.Build
			});
			VersionControl.versionStringWithoutBuild = VersionControl.version.Major + "." + VersionControl.version.Minor;
		}

		
		public static void DrawInfoInCorner()
		{
			Text.Font = GameFont.Small;
			GUI.color = new Color(1f, 1f, 1f, 0.5f);
			string text = "VersionIndicator".Translate(VersionControl.versionString);
			string versionExtraInfo = VersionControl.GetVersionExtraInfo();
			if (!versionExtraInfo.NullOrEmpty())
			{
				text = text + " (" + versionExtraInfo + ")";
			}
			text += "\n" + "CompiledOn".Translate(VersionControl.buildDate.ToString("MMM d yyyy"));
			if (SteamManager.Initialized)
			{
				text += "\n" + "LoggedIntoSteamAs".Translate(SteamUtility.SteamPersonaName);
			}
			Rect rect = new Rect(10f, 10f, 330f, Text.CalcHeight(text, 330f));
			Widgets.Label(rect, text);
			GUI.color = Color.white;
			LatestVersionGetter component = Current.Root.gameObject.GetComponent<LatestVersionGetter>();
			Rect rect2 = new Rect(10f, rect.yMax - 5f, 330f, 999f);
			component.DrawAt(rect2);
		}

		
		private static string GetVersionExtraInfo()
		{
			string text = "";
			if (UnityData.Is32BitBuild)
			{
				text += "32-bit";
			}
			else if (UnityData.Is64BitBuild)
			{
				text += "64-bit";
			}
			return text;
		}

		
		public static void LogVersionNumber()
		{
			Log.Message("RimWorld " + VersionControl.versionStringWithRev, false);
		}

		
		public static bool IsCompatible(Version v)
		{
			return v.Major == VersionControl.CurrentMajor && v.Minor == VersionControl.CurrentMinor;
		}

		
		public static bool TryParseVersionString(string str, out Version version)
		{
			version = null;
			if (str == null)
			{
				return false;
			}
			string[] array = str.Split(new char[]
			{
				'.'
			});
			if (array.Length < 2)
			{
				return false;
			}
			for (int i = 0; i < 2; i++)
			{
				int num;
				if (!int.TryParse(array[i], out num))
				{
					return false;
				}
				if (num < 0)
				{
					return false;
				}
			}
			version = new Version(int.Parse(array[0]), int.Parse(array[1]));
			return true;
		}

		
		public static int BuildFromVersionString(string str)
		{
			str = VersionControl.VersionStringWithoutRev(str);
			int result = 0;
			string[] array = str.Split(new char[]
			{
				'.'
			});
			if (array.Length < 3 || !int.TryParse(array[2], out result))
			{
				Log.Warning("Could not get build from version string " + str, false);
			}
			return result;
		}

		
		public static int MinorFromVersionString(string str)
		{
			str = VersionControl.VersionStringWithoutRev(str);
			int result = 0;
			string[] array = str.Split(new char[]
			{
				'.'
			});
			if (array.Length < 2 || !int.TryParse(array[1], out result))
			{
				Log.Warning("Could not get minor version from version string " + str, false);
			}
			return result;
		}

		
		public static int MajorFromVersionString(string str)
		{
			str = VersionControl.VersionStringWithoutRev(str);
			int result = 0;
			if (!int.TryParse(str.Split(new char[]
			{
				'.'
			})[0], out result))
			{
				Log.Warning("Could not get major version from version string " + str, false);
			}
			return result;
		}

		
		public static string VersionStringWithoutRev(string str)
		{
			return str.Split(new char[]
			{
				' '
			})[0];
		}

		
		public static Version VersionFromString(string str)
		{
			if (str.NullOrEmpty())
			{
				throw new ArgumentException("str");
			}
			string[] array = str.Split(new char[]
			{
				'.'
			});
			if (array.Length > 3)
			{
				throw new ArgumentException("str");
			}
			int major = 0;
			int minor = 0;
			int build = 0;
			for (int i = 0; i < 3; i++)
			{
				int num;
				if (!int.TryParse(array[i], out num))
				{
					throw new ArgumentException("str");
				}
				if (num < 0)
				{
					throw new ArgumentException("str");
				}
				switch (i)
				{
				case 0:
					major = num;
					break;
				case 1:
					minor = num;
					break;
				case 2:
					build = num;
					break;
				}
			}
			return new Version(major, minor, build);
		}

		
		public static bool IsWellFormattedVersionString(string str)
		{
			string[] array = str.Split(new char[]
			{
				'.'
			});
			if (array.Length != 2)
			{
				return false;
			}
			for (int i = 0; i < 2; i++)
			{
				int num;
				if (!int.TryParse(array[i], out num))
				{
					return false;
				}
				if (num < 0)
				{
					return false;
				}
			}
			return true;
		}

		
		private static Version version;

		
		private static string versionStringWithoutBuild;

		
		private static string versionString;

		
		private static string versionStringWithRev;

		
		private static DateTime buildDate;
	}
}
