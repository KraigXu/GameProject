using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using RimWorld;
using UnityEngine;

namespace Verse
{
	
	public static class ColoredText
	{
		
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

		
		public static void ClearCache()
		{
			ColoredText.cache.Clear();
		}

		
		public static TaggedString ApplyTag(this string s, TagType tagType, string arg = null)
		{
			if (arg == null)
			{
				return string.Format("(*{0}){1}(/{0})", tagType.ToString(), s);
			}
			return string.Format("(*{0}={1}){2}(/{0})", tagType.ToString(), arg, s);
		}

		
		public static TaggedString ApplyTag(this string s, Faction faction)
		{
			if (faction == null)
			{
				return s;
			}
			return s.ApplyTag(TagType.Faction, faction.GetUniqueLoadID());
		}

		
		public static string StripTags(this string s)
		{
			if (s.NullOrEmpty() || (s.IndexOf("(*") < 0 && s.IndexOf('<') < 0))
			{
				return s;
			}
			s = ColoredText.XMLRegex.Replace(s, string.Empty);
			return ColoredText.TagRegex.Replace(s, string.Empty);
		}

		
		public static string ResolveTags(this string str)
		{
			return ColoredText.Resolve(str);
		}

		
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

		
		public static string Colorize(this string s, Color color)
		{
			return string.Format("<color=#{0}>{1}</color>", ColorUtility.ToHtmlStringRGBA(color), s);
		}

		
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

		
		private static T ParseEnum<T>(string value, bool ignoreCase = true)
		{
			if (Enum.IsDefined(typeof(T), value))
			{
				return (T)((object)Enum.Parse(typeof(T), value, ignoreCase));
			}
			return default(T);
		}

		
		public static void AppendTagged(this StringBuilder sb, TaggedString taggedString)
		{
			sb.Append(taggedString.Resolve());
		}

		
		public static void AppendLineTagged(this StringBuilder sb, TaggedString taggedString)
		{
			sb.AppendLine(taggedString.Resolve());
		}

		
		public static TaggedString ToTaggedString(this StringBuilder sb)
		{
			return new TaggedString(sb.ToString());
		}

		
		private static StringBuilder resultBuffer = new StringBuilder();

		
		private static StringBuilder tagBuffer = new StringBuilder();

		
		private static StringBuilder argBuffer = new StringBuilder();

		
		private static Dictionary<string, string> cache = new Dictionary<string, string>();

		
		private static ColoredText.CaptureStage capStage = ColoredText.CaptureStage.Result;

		
		private static Regex DaysRegex;

		
		private static Regex HoursRegex;

		
		private static Regex SecondsRegex;

		
		private static Regex ColonistCountRegex;

		
		public static readonly Color RedReadable = new Color(1f, 0.2f, 0.2f);

		
		public static readonly Color NameColor = GenColor.FromHex("d09b61");

		
		public static readonly Color CurrencyColor = GenColor.FromHex("dbb40c");

		
		public static readonly Color DateTimeColor = GenColor.FromHex("87f6f6");

		
		public static readonly Color FactionColor_Ally = GenColor.FromHex("00ff00");

		
		public static readonly Color FactionColor_Hostile = ColoredText.RedReadable;

		
		public static readonly Color FactionColor_Neutral = GenColor.FromHex("00bfff");

		
		public static readonly Color WarningColor = GenColor.FromHex("ff0000");

		
		public static readonly Color ColonistCountColor = GenColor.FromHex("dcffaf");

		
		private static readonly Regex CurrencyRegex = new Regex("\\$\\d+\\.?\\d*");

		
		private static readonly Regex TagRegex = new Regex("\\([\\*\\/][^\\)]*\\)");

		
		private static readonly Regex XMLRegex = new Regex("<[^>]*>");

		
		private const string Digits = "\\d+\\.?\\d*";

		
		private const string Replacement = "$&";

		
		private const string TagStartString = "(*";

		
		private const char TagStartChar = '(';

		
		private const char TagEndChar = ')';

		
		private enum CaptureStage
		{
			
			Tag,
			
			Arg,
			
			Result
		}
	}
}
