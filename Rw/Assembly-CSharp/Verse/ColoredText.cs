using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000328 RID: 808
	public static class ColoredText
	{
		// Token: 0x0600179F RID: 6047 RVA: 0x00085DD8 File Offset: 0x00083FD8
		public static void ResetStaticData()
		{
			ColoredText.DaysRegex = new Regex(string.Format("PeriodDays".Translate(), "\\d+\\.?\\d*"));
			ColoredText.HoursRegex = new Regex(string.Format("PeriodHours".Translate(), "\\d+\\.?\\d*"));
			ColoredText.SecondsRegex = new Regex(string.Format("PeriodSeconds".Translate(), "\\d+\\.?\\d*"));
			string str = string.Concat(new string[]
			{
				"(",
				FactionDefOf.PlayerColony.pawnSingular,
				"|",
				FactionDefOf.PlayerColony.pawnsPlural,
				")"
			});
			ColoredText.ColonistCountRegex = new Regex("\\d+\\.?\\d* " + str);
		}

		// Token: 0x060017A0 RID: 6048 RVA: 0x00085EA1 File Offset: 0x000840A1
		public static void ClearCache()
		{
			ColoredText.cache.Clear();
		}

		// Token: 0x060017A1 RID: 6049 RVA: 0x00085EB0 File Offset: 0x000840B0
		public static TaggedString ApplyTag(this string s, TagType tagType, string arg = null)
		{
			if (arg == null)
			{
				return string.Format("(*{0}){1}(/{0})", tagType.ToString(), s);
			}
			return string.Format("(*{0}={1}){2}(/{0})", tagType.ToString(), arg, s);
		}

		// Token: 0x060017A2 RID: 6050 RVA: 0x00085EFC File Offset: 0x000840FC
		public static TaggedString ApplyTag(this string s, Faction faction)
		{
			if (faction == null)
			{
				return s;
			}
			return s.ApplyTag(TagType.Faction, faction.GetUniqueLoadID());
		}

		// Token: 0x060017A3 RID: 6051 RVA: 0x00085F18 File Offset: 0x00084118
		public static string StripTags(this string s)
		{
			if (s.NullOrEmpty() || (s.IndexOf("(*") < 0 && s.IndexOf('<') < 0))
			{
				return s;
			}
			s = ColoredText.XMLRegex.Replace(s, string.Empty);
			return ColoredText.TagRegex.Replace(s, string.Empty);
		}

		// Token: 0x060017A4 RID: 6052 RVA: 0x00085F6A File Offset: 0x0008416A
		public static string ResolveTags(this string str)
		{
			return ColoredText.Resolve(str);
		}

		// Token: 0x060017A5 RID: 6053 RVA: 0x00085F78 File Offset: 0x00084178
		public static string Resolve(TaggedString taggedStr)
		{
			if (taggedStr == null)
			{
				return null;
			}
			string rawText = taggedStr.RawText;
			if (rawText == null)
			{
				return rawText;
			}
			string text;
			if (ColoredText.cache.TryGetValue(rawText, out text))
			{
				return ColoredText.cache[rawText];
			}
			ColoredText.resultBuffer.Length = 0;
			if (rawText.IndexOf("(*") < 0)
			{
				ColoredText.resultBuffer.Append(rawText);
			}
			else
			{
				for (int i = 0; i < rawText.Length; i++)
				{
					char c = rawText[i];
					if (c == '(' && i < rawText.Length - 1 && rawText[i + 1] == '*' && rawText.IndexOf(')', i) > i + 1)
					{
						bool flag = false;
						int num = i;
						ColoredText.tagBuffer.Length = 0;
						ColoredText.argBuffer.Length = 0;
						ColoredText.capStage = ColoredText.CaptureStage.Tag;
						for (i += 2; i < rawText.Length; i++)
						{
							char c2 = rawText[i];
							if (c2 == ')')
							{
								ColoredText.capStage = ColoredText.CaptureStage.Result;
								if (flag)
								{
									string value = rawText.Substring(num, i - num + 1).SwapTagWithColor(ColoredText.tagBuffer.ToString(), ColoredText.argBuffer.ToString());
									ColoredText.resultBuffer.Append(value);
									break;
								}
							}
							else if (c2 == '/')
							{
								flag = true;
							}
							if (ColoredText.capStage == ColoredText.CaptureStage.Arg)
							{
								ColoredText.argBuffer.Append(c2);
							}
							if (!flag && c2 == '=')
							{
								ColoredText.capStage = ColoredText.CaptureStage.Arg;
							}
							if (ColoredText.capStage == ColoredText.CaptureStage.Tag)
							{
								ColoredText.tagBuffer.Append(c2);
							}
						}
						if (!flag)
						{
							ColoredText.resultBuffer.Append(c);
							i = num + 1;
						}
					}
					else
					{
						ColoredText.resultBuffer.Append(c);
					}
				}
			}
			string text2 = ColoredText.resultBuffer.ToString();
			text2 = ColoredText.CurrencyRegex.Replace(text2, "$&".Colorize(ColoredText.CurrencyColor));
			text2 = ColoredText.DaysRegex.Replace(text2, "$&".Colorize(ColoredText.DateTimeColor));
			text2 = ColoredText.HoursRegex.Replace(text2, "$&".Colorize(ColoredText.DateTimeColor));
			text2 = ColoredText.SecondsRegex.Replace(text2, "$&".Colorize(ColoredText.DateTimeColor));
			text2 = ColoredText.ColonistCountRegex.Replace(text2, "$&".Colorize(ColoredText.ColonistCountColor));
			ColoredText.cache.Add(rawText, text2);
			return text2;
		}

		// Token: 0x060017A6 RID: 6054 RVA: 0x000861CB File Offset: 0x000843CB
		public static string Colorize(this string s, Color color)
		{
			return string.Format("<color=#{0}>{1}</color>", ColorUtility.ToHtmlStringRGBA(color), s);
		}

		// Token: 0x060017A7 RID: 6055 RVA: 0x000861E0 File Offset: 0x000843E0
		private static string SwapTagWithColor(this string str, string tag, string arg)
		{
			TagType tagType = ColoredText.ParseEnum<TagType>(tag.CapitalizeFirst(), true);
			string text = str.StripTags();
			switch (tagType)
			{
			case TagType.Undefined:
				return str;
			case TagType.Name:
				return text.Colorize(ColoredText.NameColor);
			case TagType.Faction:
			{
				if (arg.NullOrEmpty())
				{
					return text;
				}
				Faction faction = Find.FactionManager.AllFactions.ToList<Faction>().Find((Faction x) => x.GetUniqueLoadID() == arg);
				if (faction == null)
				{
					Log.Error("No faction found with UniqueLoadID '" + arg + "'", false);
				}
				return text.Colorize(ColoredText.GetFactionRelationColor(faction));
			}
			case TagType.Settlement:
			{
				if (arg.NullOrEmpty())
				{
					return text;
				}
				Faction faction2 = Find.FactionManager.AllFactionsVisible.ToList<Faction>().Find((Faction x) => x.GetUniqueLoadID() == arg);
				if (faction2 == null)
				{
					Log.Error("No faction found with UniqueLoadID '" + arg + "'", false);
				}
				if (faction2 == null)
				{
					return text;
				}
				return text.Colorize(faction2.Color);
			}
			case TagType.DateTime:
				return text.Colorize(ColoredText.DateTimeColor);
			case TagType.ColonistCount:
				return text.Colorize(ColoredText.ColonistCountColor);
			default:
				Log.Error("Invalid tag '" + tag + "'", false);
				return text;
			}
		}

		// Token: 0x060017A8 RID: 6056 RVA: 0x00086330 File Offset: 0x00084530
		private static Color GetFactionRelationColor(Faction faction)
		{
			if (faction == null)
			{
				return Color.white;
			}
			if (faction.IsPlayer)
			{
				return faction.Color;
			}
			switch (faction.RelationKindWith(Faction.OfPlayer))
			{
			case FactionRelationKind.Hostile:
				return ColoredText.FactionColor_Hostile;
			case FactionRelationKind.Neutral:
				return ColoredText.FactionColor_Neutral;
			case FactionRelationKind.Ally:
				return ColoredText.FactionColor_Ally;
			default:
				return faction.Color;
			}
		}

		// Token: 0x060017A9 RID: 6057 RVA: 0x00086390 File Offset: 0x00084590
		private static T ParseEnum<T>(string value, bool ignoreCase = true)
		{
			if (Enum.IsDefined(typeof(T), value))
			{
				return (T)((object)Enum.Parse(typeof(T), value, ignoreCase));
			}
			return default(T);
		}

		// Token: 0x060017AA RID: 6058 RVA: 0x000863CF File Offset: 0x000845CF
		public static void AppendTagged(this StringBuilder sb, TaggedString taggedString)
		{
			sb.Append(taggedString.Resolve());
		}

		// Token: 0x060017AB RID: 6059 RVA: 0x000863DF File Offset: 0x000845DF
		public static void AppendLineTagged(this StringBuilder sb, TaggedString taggedString)
		{
			sb.AppendLine(taggedString.Resolve());
		}

		// Token: 0x060017AC RID: 6060 RVA: 0x000863EF File Offset: 0x000845EF
		public static TaggedString ToTaggedString(this StringBuilder sb)
		{
			return new TaggedString(sb.ToString());
		}

		// Token: 0x04000EC5 RID: 3781
		private static StringBuilder resultBuffer = new StringBuilder();

		// Token: 0x04000EC6 RID: 3782
		private static StringBuilder tagBuffer = new StringBuilder();

		// Token: 0x04000EC7 RID: 3783
		private static StringBuilder argBuffer = new StringBuilder();

		// Token: 0x04000EC8 RID: 3784
		private static Dictionary<string, string> cache = new Dictionary<string, string>();

		// Token: 0x04000EC9 RID: 3785
		private static ColoredText.CaptureStage capStage = ColoredText.CaptureStage.Result;

		// Token: 0x04000ECA RID: 3786
		private static Regex DaysRegex;

		// Token: 0x04000ECB RID: 3787
		private static Regex HoursRegex;

		// Token: 0x04000ECC RID: 3788
		private static Regex SecondsRegex;

		// Token: 0x04000ECD RID: 3789
		private static Regex ColonistCountRegex;

		// Token: 0x04000ECE RID: 3790
		public static readonly Color RedReadable = new Color(1f, 0.2f, 0.2f);

		// Token: 0x04000ECF RID: 3791
		public static readonly Color NameColor = GenColor.FromHex("d09b61");

		// Token: 0x04000ED0 RID: 3792
		public static readonly Color CurrencyColor = GenColor.FromHex("dbb40c");

		// Token: 0x04000ED1 RID: 3793
		public static readonly Color DateTimeColor = GenColor.FromHex("87f6f6");

		// Token: 0x04000ED2 RID: 3794
		public static readonly Color FactionColor_Ally = GenColor.FromHex("00ff00");

		// Token: 0x04000ED3 RID: 3795
		public static readonly Color FactionColor_Hostile = ColoredText.RedReadable;

		// Token: 0x04000ED4 RID: 3796
		public static readonly Color FactionColor_Neutral = GenColor.FromHex("00bfff");

		// Token: 0x04000ED5 RID: 3797
		public static readonly Color WarningColor = GenColor.FromHex("ff0000");

		// Token: 0x04000ED6 RID: 3798
		public static readonly Color ColonistCountColor = GenColor.FromHex("dcffaf");

		// Token: 0x04000ED7 RID: 3799
		private static readonly Regex CurrencyRegex = new Regex("\\$\\d+\\.?\\d*");

		// Token: 0x04000ED8 RID: 3800
		private static readonly Regex TagRegex = new Regex("\\([\\*\\/][^\\)]*\\)");

		// Token: 0x04000ED9 RID: 3801
		private static readonly Regex XMLRegex = new Regex("<[^>]*>");

		// Token: 0x04000EDA RID: 3802
		private const string Digits = "\\d+\\.?\\d*";

		// Token: 0x04000EDB RID: 3803
		private const string Replacement = "$&";

		// Token: 0x04000EDC RID: 3804
		private const string TagStartString = "(*";

		// Token: 0x04000EDD RID: 3805
		private const char TagStartChar = '(';

		// Token: 0x04000EDE RID: 3806
		private const char TagEndChar = ')';

		// Token: 0x020014BA RID: 5306
		private enum CaptureStage
		{
			// Token: 0x04004E8B RID: 20107
			Tag,
			// Token: 0x04004E8C RID: 20108
			Arg,
			// Token: 0x04004E8D RID: 20109
			Result
		}
	}
}
