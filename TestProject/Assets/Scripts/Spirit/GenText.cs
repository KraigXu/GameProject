using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using RimWorld;
using UnityEngine;
using Verse.Grammar;

namespace Verse
{
	public static class GenText
	{

		private const int SaveNameMaxLength = 40;

		private const char DegreeSymbol = '°';

		private static StringBuilder tmpSb = new StringBuilder();

		private static StringBuilder tmpSbForCapitalizedSentences = new StringBuilder();

		private static StringBuilder tmpStringBuilder = new StringBuilder();

		private static string[] eventTypesCached;

		public static string Possessive(this Pawn p)
		{
			if (p.gender == Gender.Female)
			{
				return "Proher".Translate();
			}
			return "Prohis".Translate();
		}

		// Token: 0x0600025B RID: 603 RVA: 0x0000BC4E File Offset: 0x00009E4E
		public static string PossessiveCap(this Pawn p)
		{
			if (p.gender == Gender.Female)
			{
				return "ProherCap".Translate();
			}
			return "ProhisCap".Translate();
		}

		// Token: 0x0600025C RID: 604 RVA: 0x0000BC78 File Offset: 0x00009E78
		public static string ProObj(this Pawn p)
		{
			if (p.gender == Gender.Female)
			{
				return "ProherObj".Translate();
			}
			return "ProhimObj".Translate();
		}

		// Token: 0x0600025D RID: 605 RVA: 0x0000BCA2 File Offset: 0x00009EA2
		public static string ProObjCap(this Pawn p)
		{
			if (p.gender == Gender.Female)
			{
				return "ProherObjCap".Translate();
			}
			return "ProhimObjCap".Translate();
		}

		// Token: 0x0600025E RID: 606 RVA: 0x0000BCCC File Offset: 0x00009ECC
		public static string ProSubj(this Pawn p)
		{
			if (p.gender == Gender.Female)
			{
				return "Proshe".Translate();
			}
			return "Prohe".Translate();
		}

		// Token: 0x0600025F RID: 607 RVA: 0x0000BCF6 File Offset: 0x00009EF6
		public static string ProSubjCap(this Pawn p)
		{
			if (p.gender == Gender.Female)
			{
				return "ProsheCap".Translate();
			}
			return "ProheCap".Translate();
		}

		// Token: 0x06000260 RID: 608 RVA: 0x0000BD20 File Offset: 0x00009F20
		public static string AdjustedFor(this string text, Pawn p, string pawnSymbol = "PAWN", bool addRelationInfoSymbol = true)
		{
			GrammarRequest request = default(GrammarRequest);
			request.Includes.Add(RulePackDefOf.DynamicWrapper);
			request.Rules.Add(new Rule_String("RULE", text));
			request.Rules.AddRange(GrammarUtility.RulesForPawn(pawnSymbol, p, null, addRelationInfoSymbol, true));
			return GrammarResolver.Resolve("r_root", request, null, false, null, null, null, true);
		}

		// Token: 0x06000261 RID: 609 RVA: 0x0000BD84 File Offset: 0x00009F84
		public static string AdjustedForKeys(this string text, List<string> outErrors = null, bool resolveKeys = true)
		{
			if (outErrors != null)
			{
				outErrors.Clear();
			}
			if (text.NullOrEmpty())
			{
				return text;
			}
			int num = 0;
			for (; ; )
			{
				num++;
				if (num > 500000)
				{
					break;
				}
				int num2 = text.IndexOf("{Key:");
				if (num2 < 0)
				{
					return text;
				}
				int num3 = num2;
				while (text[num3] != '}')
				{
					num3++;
					if (num3 >= text.Length)
					{
						goto Block_6;
					}
				}
				string text2 = text.Substring(num2 + 5, num3 - (num2 + 5));
				KeyBindingDef namedSilentFail = DefDatabase<KeyBindingDef>.GetNamedSilentFail(text2);
				string text3 = text.Substring(0, num2);
				if (namedSilentFail != null)
				{
					if (resolveKeys)
					{
						text3 += namedSilentFail.MainKeyLabel;
					}
					else
					{
						text3 += "placeholder";
					}
				}
				else
				{
					text3 += "error";
					if (outErrors != null)
					{
						string text4 = "Could not find key '" + text2 + "'";
						string text5 = BackCompatibility.BackCompatibleDefName(typeof(KeyBindingDef), text2, false, null);
						if (text5 != text2)
						{
							text4 = text4 + " (hint: it was renamed to '" + text5 + "')";
						}
						outErrors.Add(text4);
					}
				}
				text3 += text.Substring(num3 + 1);
				text = text3;
			}
			Log.Error("Too many iterations.", false);
			if (outErrors != null)
			{
				outErrors.Add("The parsed string caused an infinite loop");
				return text;
			}
			return text;
		Block_6:
			if (outErrors != null)
			{
				outErrors.Add("Mismatched braces");
			}
			return text;
		}

		// Token: 0x06000262 RID: 610 RVA: 0x0000BED6 File Offset: 0x0000A0D6
		public static string LabelIndefinite(this Pawn pawn)
		{
			if (pawn.Name != null && !pawn.Name.Numerical)
			{
				return pawn.LabelShort;
			}
			return pawn.KindLabelIndefinite();
		}

		// Token: 0x06000263 RID: 611 RVA: 0x0000BEFA File Offset: 0x0000A0FA
		public static string LabelDefinite(this Pawn pawn)
		{
			if (pawn.Name != null && !pawn.Name.Numerical)
			{
				return pawn.LabelShort;
			}
			return pawn.KindLabelDefinite();
		}

		// Token: 0x06000264 RID: 612 RVA: 0x0000BF1E File Offset: 0x0000A11E
		public static string KindLabelIndefinite(this Pawn pawn)
		{
			return Find.ActiveLanguageWorker.WithIndefiniteArticlePostProcessed(pawn.KindLabel, pawn.gender, false, false);
		}

		// Token: 0x06000265 RID: 613 RVA: 0x0000BF38 File Offset: 0x0000A138
		public static string KindLabelDefinite(this Pawn pawn)
		{
			return Find.ActiveLanguageWorker.WithDefiniteArticlePostProcessed(pawn.KindLabel, pawn.gender, false, false);
		}

		// Token: 0x06000266 RID: 614 RVA: 0x0000BF54 File Offset: 0x0000A154
		public static string RandomSeedString()
		{
			return GrammarResolver.Resolve("r_seed", new GrammarRequest
			{
				Includes =
				{
					RulePackDefOf.SeedGenerator
				}
			}, null, false, null, null, null, true).ToLower();
		}

		// Token: 0x06000267 RID: 615 RVA: 0x0000BF90 File Offset: 0x0000A190
		public static string Shorten(this string s)
		{
			if (s.NullOrEmpty() || s.Length <= 4)
			{
				return s;
			}
			s = s.Trim();
			string[] array = s.Split(new char[]
			{
				' '
			});
			string text = "";
			for (int i = 0; i < array.Length; i++)
			{
				if (i > 0)
				{
					text += " ";
				}
				if (array[i].Length > 2)
				{
					text = text + array[i].Substring(0, 1) + array[i].Substring(1, array[i].Length - 2).WithoutVowels() + array[i].Substring(array[i].Length - 1, 1);
				}
			}
			return text;
		}

		// Token: 0x06000268 RID: 616 RVA: 0x0000C038 File Offset: 0x0000A238
		private static string WithoutVowels(this string s)
		{
			string vowels = "aeiouy";
			return new string((from c in s
							   where !vowels.Contains(c)
							   select c).ToArray<char>());
		}

		// Token: 0x06000269 RID: 617 RVA: 0x0000C074 File Offset: 0x0000A274
		public static string MarchingEllipsis(float offset = 0f)
		{
			switch (Mathf.FloorToInt(Time.realtimeSinceStartup + offset) % 3)
			{
				case 0:
					return ".";
				case 1:
					return "..";
				case 2:
					return "...";
				default:
					throw new Exception();
			}
		}

		// Token: 0x0600026A RID: 618 RVA: 0x0000C0BB File Offset: 0x0000A2BB
		public static void SetTextSizeToFit(string text, Rect r)
		{
			Text.Font = GameFont.Small;
			if (Text.CalcHeight(text, r.width) > r.height)
			{
				Text.Font = GameFont.Tiny;
			}
		}

		// Token: 0x0600026B RID: 619 RVA: 0x0000C0DF File Offset: 0x0000A2DF
		public static string TrimEndNewlines(this string s)
		{
			return s.TrimEnd(new char[]
			{
				'\r',
				'\n'
			});
		}

		// Token: 0x0600026C RID: 620 RVA: 0x0000C0F7 File Offset: 0x0000A2F7
		public static string Indented(this string s, string indentation = "    ")
		{
			if (s.NullOrEmpty())
			{
				return s;
			}
			return indentation + s.Replace("\r", "").Replace("\n", "\n" + indentation);
		}

		// Token: 0x0600026D RID: 621 RVA: 0x0000C130 File Offset: 0x0000A330
		public static string ReplaceFirst(this string source, string key, string replacement)
		{
			int num = source.IndexOf(key);
			if (num < 0)
			{
				return source;
			}
			return source.Substring(0, num) + replacement + source.Substring(num + key.Length);
		}

		// Token: 0x0600026E RID: 622 RVA: 0x0000C168 File Offset: 0x0000A368
		public static int StableStringHash(string str)
		{
			if (str == null)
			{
				return 0;
			}
			int num = 23;
			int length = str.Length;
			for (int i = 0; i < length; i++)
			{
				num = num * 31 + (int)str[i];
			}
			return num;
		}

		// Token: 0x0600026F RID: 623 RVA: 0x0000C1A0 File Offset: 0x0000A3A0
		public static string StringFromEnumerable(IEnumerable source)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (object obj in source)
			{
				stringBuilder.AppendLine("• " + obj.ToString());
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06000270 RID: 624 RVA: 0x0000C20C File Offset: 0x0000A40C
		public static IEnumerable<string> LinesFromString(string text)
		{
			string[] separator = new string[]
			{
				"\r\n",
				"\n"
			};
			string[] array = text.Split(separator, StringSplitOptions.RemoveEmptyEntries);
			for (int i = 0; i < array.Length; i++)
			{
				string text2 = array[i].Trim();
				if (!text2.StartsWith("//"))
				{
					text2 = text2.Split(new string[]
					{
						"//"
					}, StringSplitOptions.None)[0];
					if (text2.Length != 0)
					{
						yield return text2;
					}
				}
			}
			array = null;
			yield break;
		}

		// Token: 0x06000271 RID: 625 RVA: 0x0000C21C File Offset: 0x0000A41C
		public static string GetInvalidFilenameCharacters()
		{
			return new string(Path.GetInvalidFileNameChars()) + "/\\{}<>:*|!@#$%^&*?";
		}

		// Token: 0x06000272 RID: 626 RVA: 0x0000C232 File Offset: 0x0000A432
		public static bool IsValidFilename(string str)
		{
			return str.Length <= 40 && !new Regex("[" + Regex.Escape(GenText.GetInvalidFilenameCharacters()) + "]").IsMatch(str);
		}

		// Token: 0x06000273 RID: 627 RVA: 0x0000C267 File Offset: 0x0000A467
		public static string SanitizeFilename(string str)
		{
			return string.Join("_", str.Split(GenText.GetInvalidFilenameCharacters().ToArray<char>(), StringSplitOptions.RemoveEmptyEntries)).TrimEnd(new char[]
			{
				'.'
			});
		}

		// Token: 0x06000274 RID: 628 RVA: 0x0000C294 File Offset: 0x0000A494
		public static bool NullOrEmpty(this string str)
		{
			return string.IsNullOrEmpty(str);
		}

		// Token: 0x06000275 RID: 629 RVA: 0x0000C29C File Offset: 0x0000A49C
		public static string SplitCamelCase(string Str)
		{
			return Regex.Replace(Str, "(\\B[A-Z]+?(?=[A-Z][^A-Z])|\\B[A-Z]+?(?=[^A-Z]))", " $1");
		}

		// Token: 0x06000276 RID: 630 RVA: 0x0000C2B0 File Offset: 0x0000A4B0
		public static string CapitalizedNoSpaces(string s)
		{
			string[] array = s.Split(new char[]
			{
				' '
			});
			StringBuilder stringBuilder = new StringBuilder();
			foreach (string text in array)
			{
				if (text.Length > 0)
				{
					stringBuilder.Append(char.ToUpper(text[0]));
				}
				if (text.Length > 1)
				{
					stringBuilder.Append(text.Substring(1));
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06000277 RID: 631 RVA: 0x0000C320 File Offset: 0x0000A520
		public static string RemoveNonAlphanumeric(string s)
		{
			GenText.tmpSb.Length = 0;
			for (int i = 0; i < s.Length; i++)
			{
				if (char.IsLetterOrDigit(s[i]))
				{
					GenText.tmpSb.Append(s[i]);
				}
			}
			return GenText.tmpSb.ToString();
		}

		// Token: 0x06000278 RID: 632 RVA: 0x0000C373 File Offset: 0x0000A573
		public static bool EqualsIgnoreCase(this string A, string B)
		{
			return string.Compare(A, B, true) == 0;
		}

		// Token: 0x06000279 RID: 633 RVA: 0x0000C380 File Offset: 0x0000A580
		public static string WithoutByteOrderMark(this string str)
		{
			return str.Trim().Trim(new char[]
			{
				'﻿'
			});
		}

		// Token: 0x0600027A RID: 634 RVA: 0x0000C39C File Offset: 0x0000A59C
		public static bool NamesOverlap(string A, string B)
		{
			A = A.ToLower();
			B = B.ToLower();
			string[] array = A.Split(new char[]
			{
				' '
			});
			string[] source = B.Split(new char[]
			{
				' '
			});
			foreach (string text in array)
			{
				if (TitleCaseHelper.IsUppercaseTitleWord(text) && source.Contains(text))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600027B RID: 635 RVA: 0x0000C408 File Offset: 0x0000A608
		public static int FirstLetterBetweenTags(this string str)
		{
			int num = 0;
			if (str[num] == '<' && str.IndexOf('>') > num && num < str.Length - 1 && str[num + 1] != '/')
			{
				num = str.IndexOf('>') + 1;
			}
			return num;
		}

		// Token: 0x0600027C RID: 636 RVA: 0x0000C454 File Offset: 0x0000A654
		public static string CapitalizeFirst(this string str)
		{
			if (str.NullOrEmpty())
			{
				return str;
			}
			if (char.IsUpper(str[0]))
			{
				return str;
			}
			if (str.Length == 1)
			{
				return str.ToUpper();
			}
			int num = str.FirstLetterBetweenTags();
			if (num == 0)
			{
				return char.ToUpper(str[num]).ToString() + str.Substring(num + 1);
			}
			return str.Substring(0, num) + char.ToUpper(str[num]).ToString() + str.Substring(num + 1);
		}

		// Token: 0x0600027D RID: 637 RVA: 0x0000C4E2 File Offset: 0x0000A6E2
		public static string CapitalizeFirst(this string str, Def possibleDef)
		{
			if (possibleDef != null && str == possibleDef.label)
			{
				return possibleDef.LabelCap;
			}
			return str.CapitalizeFirst();
		}

		// Token: 0x0600027E RID: 638 RVA: 0x0000C508 File Offset: 0x0000A708
		public static string UncapitalizeFirst(this string str)
		{
			if (str.NullOrEmpty())
			{
				return str;
			}
			if (char.IsLower(str[0]))
			{
				return str;
			}
			if (str.Length == 1)
			{
				return str.ToLower();
			}
			int num = str.FirstLetterBetweenTags();
			if (num == 0)
			{
				return char.ToLower(str[num]).ToString() + str.Substring(num + 1);
			}
			return str.Substring(0, num) + char.ToLower(str[num]).ToString() + str.Substring(num + 1);
		}

		// Token: 0x0600027F RID: 639 RVA: 0x0000C598 File Offset: 0x0000A798
		public static string ToNewsCase(string str)
		{
			string[] array = str.Split(new char[]
			{
				' '
			});
			for (int i = 0; i < array.Length; i++)
			{
				string text = array[i];
				if (text.Length >= 2)
				{
					if (i == 0)
					{
						array[i] = text[0].ToString().ToUpper() + text.Substring(1);
					}
					else
					{
						array[i] = text.ToLower();
					}
				}
			}
			return string.Join(" ", array);
		}

		// Token: 0x06000280 RID: 640 RVA: 0x0000C60E File Offset: 0x0000A80E
		public static string ToTitleCaseSmart(string str)
		{
			return Find.ActiveLanguageWorker.ToTitleCase(str);
		}

		// Token: 0x06000281 RID: 641 RVA: 0x0000C61C File Offset: 0x0000A81C
		public static string CapitalizeSentences(string input, bool capitalizeFirstSentence = true)
		{
			if (input.NullOrEmpty())
			{
				return input;
			}
			if (input.Length != 1)
			{
				bool flag = capitalizeFirstSentence;
				bool flag2 = false;
				bool flag3 = false;
				GenText.tmpSbForCapitalizedSentences.Length = 0;
				for (int i = 0; i < input.Length; i++)
				{
					if (flag && char.IsLetterOrDigit(input[i]) && !flag2 && !flag3)
					{
						GenText.tmpSbForCapitalizedSentences.Append(char.ToUpper(input[i]));
						flag = false;
					}
					else
					{
						GenText.tmpSbForCapitalizedSentences.Append(input[i]);
					}
					if (input[i] == '\r' || input[i] == '\n' || (input[i] == '.' && i < input.Length - 1 && !char.IsLetter(input[i + 1])) || input[i] == '!' || input[i] == '?' || input[i] == ':')
					{
						flag = true;
					}
					else if (input[i] == '<' && i < input.Length - 1 && input[i + 1] != '/')
					{
						flag2 = true;
					}
					else if (flag2 && input[i] == '>')
					{
						flag2 = false;
					}
					else if (input[i] == '{')
					{
						flag3 = true;
						flag = false;
					}
					else if (flag3 && input[i] == '}')
					{
						flag3 = false;
						flag = false;
					}
				}
				return GenText.tmpSbForCapitalizedSentences.ToString();
			}
			if (capitalizeFirstSentence)
			{
				return input.ToUpper();
			}
			return input;
		}

		// Token: 0x06000282 RID: 642 RVA: 0x0000C60E File Offset: 0x0000A80E
		public static string CapitalizeAsTitle(string str)
		{
			return Find.ActiveLanguageWorker.ToTitleCase(str);
		}

		// Token: 0x06000283 RID: 643 RVA: 0x0000C77C File Offset: 0x0000A97C
		public static string ToCommaList(this IEnumerable<string> items, bool useAnd = false)
		{
			if (items == null)
			{
				return "";
			}
			string text = null;
			string text2 = null;
			int num = 0;
			StringBuilder stringBuilder = new StringBuilder();
			IList<string> list = items as IList<string>;
			if (list != null)
			{
				for (int i = 0; i < list.Count; i++)
				{
					string text3 = list[i];
					if (!text3.NullOrEmpty())
					{
						if (text2 == null)
						{
							text2 = text3;
						}
						if (text != null)
						{
							stringBuilder.Append(text + ", ");
						}
						text = text3;
						num++;
					}
				}
			}
			else
			{
				foreach (string text4 in items)
				{
					if (!text4.NullOrEmpty())
					{
						if (text2 == null)
						{
							text2 = text4;
						}
						if (text != null)
						{
							stringBuilder.Append(text + ", ");
						}
						text = text4;
						num++;
					}
				}
			}
			if (num == 0)
			{
				return "NoneLower".Translate();
			}
			if (num == 1)
			{
				return text;
			}
			if (!useAnd)
			{
				stringBuilder.Append(text);
				return stringBuilder.ToString();
			}
			if (num == 2)
			{
				return "ToCommaListAnd".Translate(text2, text);
			}
			stringBuilder.Remove(stringBuilder.Length - 2, 2);
			return "ToCommaListAnd".Translate(stringBuilder.ToString(), text);
		}

		// Token: 0x06000284 RID: 644 RVA: 0x0000C8D8 File Offset: 0x0000AAD8
		public static TaggedString ToClauseSequence(this List<string> entries)
		{
			switch (entries.Count)
			{
				case 0:
					return "None".Translate() + ".";
				case 1:
					return entries[0] + ".";
				case 2:
					return "ClauseSequence2".Translate(entries[0], entries[1]);
				case 3:
					return "ClauseSequence3".Translate(entries[0], entries[1], entries[2]);
				case 4:
					return "ClauseSequence4".Translate(entries[0], entries[1], entries[2], entries[3]);
				case 5:
					return "ClauseSequence5".Translate(entries[0], entries[1], entries[2], entries[3], entries[4]);
				case 6:
					return "ClauseSequence6".Translate(entries[0], entries[1], entries[2], entries[3], entries[4], entries[5]);
				case 7:
					return "ClauseSequence7".Translate(entries[0], entries[1], entries[2], entries[3], entries[4], entries[5], entries[6]);
				case 8:
					return "ClauseSequence8".Translate(entries[0], entries[1], entries[2], entries[3], entries[4], entries[5], entries[6], entries[7]);
				default:
					return entries.ToCommaList(true);
			}
		}

		// Token: 0x06000285 RID: 645 RVA: 0x0000CB44 File Offset: 0x0000AD44
		public static string ToLineList(this IList<string> entries, string prefix = null)
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < entries.Count; i++)
			{
				if (i != 0)
				{
					stringBuilder.Append("\n");
				}
				if (prefix != null)
				{
					stringBuilder.Append(prefix);
				}
				stringBuilder.Append(entries[i]);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06000286 RID: 646 RVA: 0x0000CB98 File Offset: 0x0000AD98
		public static string ToLineList(this IEnumerable<string> entries, string prefix = null, bool capitalizeItems = false)
		{
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = true;
			foreach (string text in entries)
			{
				if (!flag)
				{
					stringBuilder.Append("\n");
				}
				if (prefix != null)
				{
					stringBuilder.Append(prefix);
				}
				stringBuilder.Append(capitalizeItems ? text.CapitalizeFirst() : text);
				flag = false;
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06000287 RID: 647 RVA: 0x0000CC18 File Offset: 0x0000AE18
		public static string ToSpaceList(IEnumerable<string> entries)
		{
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = true;
			foreach (string value in entries)
			{
				if (!flag)
				{
					stringBuilder.Append(" ");
				}
				stringBuilder.Append(value);
				flag = false;
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06000288 RID: 648 RVA: 0x0000CC80 File Offset: 0x0000AE80
		public static string ToCamelCase(string str)
		{
			string text = "";
			foreach (string str2 in str.Split(new char[]
			{
				' '
			}))
			{
				if (text.Length == 0)
				{
					text += str2.UncapitalizeFirst();
				}
				else
				{
					text += str2.CapitalizeFirst();
				}
			}
			return text;
		}

		// Token: 0x06000289 RID: 649 RVA: 0x0000CCDC File Offset: 0x0000AEDC
		public static string Truncate(this string str, float width, Dictionary<string, string> cache = null)
		{
			string text;
			if (cache != null && cache.TryGetValue(str, out text))
			{
				return text;
			}
			if (Text.CalcSize(str).x <= width)
			{
				if (cache != null)
				{
					cache.Add(str, str);
				}
				return str;
			}
			text = str;
			do
			{
				text = text.Substring(0, text.Length - 1);
			}
			while (text.Length > 0 && Text.CalcSize(text + "...").x > width);
			text += "...";
			if (cache != null)
			{
				cache.Add(str, text);
			}
			return text;
		}

		// Token: 0x0600028A RID: 650 RVA: 0x0000CD60 File Offset: 0x0000AF60
		public static TaggedString Truncate(this TaggedString str, float width, Dictionary<string, TaggedString> cache = null)
		{
			TaggedString taggedString;
			if (cache != null && cache.TryGetValue(str.RawText, out taggedString))
			{
				return taggedString;
			}
			if (Text.CalcSize(str.RawText.StripTags()).x < width)
			{
				if (cache != null)
				{
					cache.Add(str.RawText, str);
				}
				return str;
			}
			taggedString = str;
			do
			{
				taggedString = taggedString.RawText.Substring(0, taggedString.RawText.Length - 1);
			}
			while (taggedString.RawText.StripTags().Length > 0 && Text.CalcSize(taggedString.RawText.StripTags() + "...").x > width);
			taggedString += "...";
			if (cache != null)
			{
				cache.Add(str.RawText, str);
			}
			return taggedString;
		}

		// Token: 0x0600028B RID: 651 RVA: 0x0000CE28 File Offset: 0x0000B028
		public static string TruncateHeight(this string str, float width, float height, Dictionary<string, string> cache = null)
		{
			string text;
			if (cache != null && cache.TryGetValue(str, out text))
			{
				return text;
			}
			if (Text.CalcHeight(str, width) <= height)
			{
				if (cache != null)
				{
					cache.Add(str, str);
				}
				return str;
			}
			text = str;
			do
			{
				text = text.Substring(0, text.Length - 1);
			}
			while (text.Length > 0 && Text.CalcHeight(text + "...", width) > height);
			text += "...";
			if (cache != null)
			{
				cache.Add(str, text);
			}
			return text;
		}

		// Token: 0x0600028C RID: 652 RVA: 0x0000CEA4 File Offset: 0x0000B0A4
		public static string Flatten(this string str)
		{
			if (str.NullOrEmpty())
			{
				return str;
			}
			if (str.Contains("\n"))
			{
				str = str.Replace("\n", " ");
			}
			if (str.Contains("\r"))
			{
				str = str.Replace("\r", "");
			}
			str = str.MergeMultipleSpaces(false);
			return str.Trim(new char[]
			{
				' ',
				'\n',
				'\r',
				'\t'
			});
		}

		// Token: 0x0600028D RID: 653 RVA: 0x0000CF1C File Offset: 0x0000B11C
		public static string MergeMultipleSpaces(this string str, bool leaveMultipleSpacesAtLineBeginning = true)
		{
			if (str.NullOrEmpty())
			{
				return str;
			}
			if (!str.Contains("  "))
			{
				return str;
			}
			bool flag = true;
			GenText.tmpStringBuilder.Length = 0;
			for (int i = 0; i < str.Length; i++)
			{
				if (str[i] == '\r' || str[i] == '\n')
				{
					flag = true;
				}
				if ((leaveMultipleSpacesAtLineBeginning && flag) || str[i] != ' ' || i == 0 || str[i - 1] != ' ')
				{
					GenText.tmpStringBuilder.Append(str[i]);
				}
				if (!char.IsWhiteSpace(str[i]))
				{
					flag = false;
				}
			}
			return GenText.tmpStringBuilder.ToString();
		}

		// Token: 0x0600028E RID: 654 RVA: 0x0000CFC4 File Offset: 0x0000B1C4
		public static string TrimmedToLength(this string str, int length)
		{
			if (str == null || str.Length <= length)
			{
				return str;
			}
			return str.Substring(0, length);
		}

		// Token: 0x0600028F RID: 655 RVA: 0x0000CFDC File Offset: 0x0000B1DC
		public static bool ContainsEmptyLines(string str)
		{
			return str.NullOrEmpty() || (str[0] == '\n' || str[0] == '\r') || (str[str.Length - 1] == '\n' || str[str.Length - 1] == '\r') || (str.Contains("\n\n") || str.Contains("\r\n\r\n") || str.Contains("\r\r"));
		}

		// Token: 0x06000290 RID: 656 RVA: 0x0000D05C File Offset: 0x0000B25C
		public static string ToStringByStyle(this float f, ToStringStyle style, ToStringNumberSense numberSense = ToStringNumberSense.Absolute)
		{
			if (style == ToStringStyle.Temperature && numberSense == ToStringNumberSense.Offset)
			{
				style = ToStringStyle.TemperatureOffset;
			}
			if (numberSense == ToStringNumberSense.Factor)
			{
				if (f >= 10f)
				{
					style = ToStringStyle.FloatMaxTwo;
				}
				else
				{
					style = ToStringStyle.PercentZero;
				}
			}
			string text;
			switch (style)
			{
				case ToStringStyle.Integer:
					text = Mathf.RoundToInt(f).ToString();
					break;
				case ToStringStyle.FloatOne:
					text = f.ToString("F1");
					break;
				case ToStringStyle.FloatTwo:
					text = f.ToString("F2");
					break;
				case ToStringStyle.FloatThree:
					text = f.ToString("F3");
					break;
				case ToStringStyle.FloatMaxOne:
					text = f.ToString("0.#");
					break;
				case ToStringStyle.FloatMaxTwo:
					text = f.ToString("0.##");
					break;
				case ToStringStyle.FloatMaxThree:
					text = f.ToString("0.###");
					break;
				case ToStringStyle.FloatTwoOrThree:
					text = f.ToString((f == 0f || Mathf.Abs(f) >= 0.01f) ? "F2" : "F3");
					break;
				case ToStringStyle.PercentZero:
					text = f.ToStringPercent();
					break;
				case ToStringStyle.PercentOne:
					text = f.ToStringPercent("F1");
					break;
				case ToStringStyle.PercentTwo:
					text = f.ToStringPercent("F2");
					break;
				case ToStringStyle.Temperature:
					text = f.ToStringTemperature("F1");
					break;
				case ToStringStyle.TemperatureOffset:
					text = f.ToStringTemperatureOffset("F1");
					break;
				case ToStringStyle.WorkAmount:
					text = f.ToStringWorkAmount();
					break;
				case ToStringStyle.Money:
					text = f.ToStringMoney(null);
					break;
				default:
					Log.Error("Unknown ToStringStyle " + style, false);
					text = f.ToString();
					break;
			}
			if (numberSense == ToStringNumberSense.Offset)
			{
				if (f >= 0f)
				{
					text = "+" + text;
				}
			}
			else if (numberSense == ToStringNumberSense.Factor)
			{
				text = "x" + text;
			}
			return text;
		}

		// Token: 0x06000291 RID: 657 RVA: 0x0000D21C File Offset: 0x0000B41C
		public static string ToStringDecimalIfSmall(this float f)
		{
			if (Mathf.Abs(f) < 1f)
			{
				return Math.Round((double)f, 2).ToString("0.##");
			}
			if (Mathf.Abs(f) < 10f)
			{
				return Math.Round((double)f, 1).ToString("0.#");
			}
			return Mathf.RoundToInt(f).ToStringCached();
		}

		// Token: 0x06000292 RID: 658 RVA: 0x0000D27A File Offset: 0x0000B47A
		public static string ToStringPercent(this float f)
		{
			return (f * 100f).ToStringDecimalIfSmall() + "%";
		}

		// Token: 0x06000293 RID: 659 RVA: 0x0000D294 File Offset: 0x0000B494
		public static string ToStringPercent(this float f, string format)
		{
			return ((f + 1E-05f) * 100f).ToString(format) + "%";
		}

		// Token: 0x06000294 RID: 660 RVA: 0x0000D2C4 File Offset: 0x0000B4C4
		public static string ToStringMoney(this float f, string format = null)
		{
			if (format == null)
			{
				if (f >= 10f || f == 0f)
				{
					format = "F0";
				}
				else
				{
					format = "F2";
				}
			}
			return "MoneyFormat".Translate(f.ToString(format));
		}

		// Token: 0x06000295 RID: 661 RVA: 0x0000D310 File Offset: 0x0000B510
		public static string ToStringMoneyOffset(this float f, string format = null)
		{
			string text = f.ToStringMoney(format);
			if (f > 0f && text != "$0")
			{
				return "+" + text;
			}
			return text;
		}

		// Token: 0x06000296 RID: 662 RVA: 0x0000D347 File Offset: 0x0000B547
		public static string ToStringWithSign(this int i)
		{
			return i.ToString("+#;-#;0");
		}

		// Token: 0x06000297 RID: 663 RVA: 0x0000D355 File Offset: 0x0000B555
		public static string ToStringWithSign(this float f, string format = "0.##")
		{
			if (f > 0f)
			{
				return "+" + f.ToString(format);
			}
			return f.ToString(format);
		}

		// Token: 0x06000298 RID: 664 RVA: 0x0000D37C File Offset: 0x0000B57C
		public static string ToStringKilobytes(this int bytes, string format = "F2")
		{
			return ((float)bytes / 1024f).ToString(format) + "Kb";
		}

		// Token: 0x06000299 RID: 665 RVA: 0x0000D3A4 File Offset: 0x0000B5A4
		public static string ToStringYesNo(this bool b)
		{
			return b ? "Yes".Translate() : "No".Translate();
		}

		// Token: 0x0600029A RID: 666 RVA: 0x0000D3C4 File Offset: 0x0000B5C4
		public static string ToStringLongitude(this float longitude)
		{
			bool flag = longitude < 0f;
			if (flag)
			{
				longitude = -longitude;
			}
			return longitude.ToString("F2") + "°" + (flag ? "W" : "E");
		}

		// Token: 0x0600029B RID: 667 RVA: 0x0000D408 File Offset: 0x0000B608
		public static string ToStringLatitude(this float latitude)
		{
			bool flag = latitude < 0f;
			if (flag)
			{
				latitude = -latitude;
			}
			return latitude.ToString("F2") + "°" + (flag ? "S" : "N");
		}

		// Token: 0x0600029C RID: 668 RVA: 0x0000D44C File Offset: 0x0000B64C
		public static string ToStringMass(this float mass)
		{
			if (mass == 0f)
			{
				return "0 g";
			}
			float num = Mathf.Abs(mass);
			if (num >= 100f)
			{
				return mass.ToString("F0") + " kg";
			}
			if (num >= 10f)
			{
				return mass.ToString("0.#") + " kg";
			}
			if (num >= 0.1f)
			{
				return mass.ToString("0.##") + " kg";
			}
			float num2 = mass * 1000f;
			if (num >= 0.01f)
			{
				return num2.ToString("F0") + " g";
			}
			if (num >= 0.001f)
			{
				return num2.ToString("0.#") + " g";
			}
			return num2.ToString("0.##") + " g";
		}

		// Token: 0x0600029D RID: 669 RVA: 0x0000D528 File Offset: 0x0000B728
		public static string ToStringMassOffset(this float mass)
		{
			string text = mass.ToStringMass();
			if (mass > 0f)
			{
				return "+" + text;
			}
			return text;
		}

		// Token: 0x0600029E RID: 670 RVA: 0x0000D551 File Offset: 0x0000B751
		public static string ToStringSign(this float val)
		{
			if (val >= 0f)
			{
				return "+";
			}
			return "";
		}

		// Token: 0x0600029F RID: 671 RVA: 0x0000D568 File Offset: 0x0000B768
		public static string ToStringEnsureThreshold(this float value, float threshold, int decimalPlaces)
		{
			if (value > threshold && Math.Round((double)value, decimalPlaces) <= Math.Round((double)threshold, decimalPlaces))
			{
				return (value + 1f / Mathf.Pow(10f, (float)decimalPlaces)).ToString("F" + decimalPlaces);
			}
			return value.ToString("F" + decimalPlaces);
		}

		// Token: 0x060002A0 RID: 672 RVA: 0x0000D5CF File Offset: 0x0000B7CF
		public static string ToStringTemperature(this float celsiusTemp, string format = "F1")
		{
			celsiusTemp = GenTemperature.CelsiusTo(celsiusTemp, Prefs.TemperatureMode);
			return celsiusTemp.ToStringTemperatureRaw(format);
		}

		// Token: 0x060002A1 RID: 673 RVA: 0x0000D5E5 File Offset: 0x0000B7E5
		public static string ToStringTemperatureOffset(this float celsiusTemp, string format = "F1")
		{
			celsiusTemp = GenTemperature.CelsiusToOffset(celsiusTemp, Prefs.TemperatureMode);
			return celsiusTemp.ToStringTemperatureRaw(format);
		}

		// Token: 0x060002A2 RID: 674 RVA: 0x0000D5FC File Offset: 0x0000B7FC
		public static string ToStringTemperatureRaw(this float temp, string format = "F1")
		{
			switch (Prefs.TemperatureMode)
			{
				case TemperatureDisplayMode.Celsius:
					return temp.ToString(format) + "C";
				case TemperatureDisplayMode.Fahrenheit:
					return temp.ToString(format) + "F";
				case TemperatureDisplayMode.Kelvin:
					return temp.ToString(format) + "K";
				default:
					throw new InvalidOperationException();
			}
		}

		// Token: 0x060002A3 RID: 675 RVA: 0x0000D664 File Offset: 0x0000B864
		public static string ToStringTwoDigits(this Vector2 v)
		{
			return string.Concat(new string[]
			{
				"(",
				v.x.ToString("F2"),
				", ",
				v.y.ToString("F2"),
				")"
			});
		}

		// Token: 0x060002A4 RID: 676 RVA: 0x0000D6BC File Offset: 0x0000B8BC
		public static string ToStringWorkAmount(this float workAmount)
		{
			return Mathf.CeilToInt(workAmount / 60f).ToString();
		}

		// Token: 0x060002A5 RID: 677 RVA: 0x0000D6E0 File Offset: 0x0000B8E0
		public static string ToStringBytes(this int b, string format = "F2")
		{
			return ((float)b / 8f / 1024f).ToString(format) + "kb";
		}

		// Token: 0x060002A6 RID: 678 RVA: 0x0000D710 File Offset: 0x0000B910
		public static string ToStringBytes(this uint b, string format = "F2")
		{
			return (b / 8f / 1024f).ToString(format) + "kb";
		}

		// Token: 0x060002A7 RID: 679 RVA: 0x0000D740 File Offset: 0x0000B940
		public static string ToStringBytes(this long b, string format = "F2")
		{
			return ((float)b / 8f / 1024f).ToString(format) + "kb";
		}

		// Token: 0x060002A8 RID: 680 RVA: 0x0000D770 File Offset: 0x0000B970
		public static string ToStringBytes(this ulong b, string format = "F2")
		{
			return (b / 8f / 1024f).ToString(format) + "kb";
		}

		// Token: 0x060002A9 RID: 681 RVA: 0x0000D7A0 File Offset: 0x0000B9A0
		public static string ToStringReadable(this KeyCode k)
		{
			if (k <= KeyCode.BackQuote)
			{
				switch (k)
				{
					case KeyCode.Backspace:
						return "Bksp";
					case KeyCode.Tab:
					case (KeyCode)10:
					case (KeyCode)11:
					case (KeyCode)14:
					case (KeyCode)15:
					case (KeyCode)16:
					case (KeyCode)17:
					case (KeyCode)18:
					case KeyCode.Pause:
					case (KeyCode)20:
					case (KeyCode)21:
					case (KeyCode)22:
					case (KeyCode)23:
					case (KeyCode)24:
					case (KeyCode)25:
					case (KeyCode)26:
					case (KeyCode)28:
					case (KeyCode)29:
					case (KeyCode)30:
					case (KeyCode)31:
					case KeyCode.Space:
					case KeyCode.Percent:
					case KeyCode.Equals:
						break;
					case KeyCode.Clear:
						return "Clr";
					case KeyCode.Return:
						return "Ent";
					case KeyCode.Escape:
						return "Esc";
					case KeyCode.Exclaim:
						return "!";
					case KeyCode.DoubleQuote:
						return "\"";
					case KeyCode.Hash:
						return "#";
					case KeyCode.Dollar:
						return "$";
					case KeyCode.Ampersand:
						return "&";
					case KeyCode.Quote:
						return "'";
					case KeyCode.LeftParen:
						return "(";
					case KeyCode.RightParen:
						return ")";
					case KeyCode.Asterisk:
						return "*";
					case KeyCode.Plus:
						return "+";
					case KeyCode.Comma:
						return ",";
					case KeyCode.Minus:
						return "-";
					case KeyCode.Period:
						return ".";
					case KeyCode.Slash:
						return "/";
					case KeyCode.Alpha0:
						return "0";
					case KeyCode.Alpha1:
						return "1";
					case KeyCode.Alpha2:
						return "2";
					case KeyCode.Alpha3:
						return "3";
					case KeyCode.Alpha4:
						return "4";
					case KeyCode.Alpha5:
						return "5";
					case KeyCode.Alpha6:
						return "6";
					case KeyCode.Alpha7:
						return "7";
					case KeyCode.Alpha8:
						return "8";
					case KeyCode.Alpha9:
						return "9";
					case KeyCode.Colon:
						return ":";
					case KeyCode.Semicolon:
						return ";";
					case KeyCode.Less:
						return "<";
					case KeyCode.Greater:
						return ">";
					case KeyCode.Question:
						return "?";
					case KeyCode.At:
						return "@";
					default:
						switch (k)
						{
							case KeyCode.LeftBracket:
								return "[";
							case KeyCode.Backslash:
								return "\\";
							case KeyCode.RightBracket:
								return "]";
							case KeyCode.Caret:
								return "^";
							case KeyCode.Underscore:
								return "_";
							case KeyCode.BackQuote:
								return "`";
						}
						break;
				}
			}
			else
			{
				if (k == KeyCode.Delete)
				{
					return "Del";
				}
				switch (k)
				{
					case KeyCode.Keypad0:
						return "Kp0";
					case KeyCode.Keypad1:
						return "Kp1";
					case KeyCode.Keypad2:
						return "Kp2";
					case KeyCode.Keypad3:
						return "Kp3";
					case KeyCode.Keypad4:
						return "Kp4";
					case KeyCode.Keypad5:
						return "Kp5";
					case KeyCode.Keypad6:
						return "Kp6";
					case KeyCode.Keypad7:
						return "Kp7";
					case KeyCode.Keypad8:
						return "Kp8";
					case KeyCode.Keypad9:
						return "Kp9";
					case KeyCode.KeypadPeriod:
						return "Kp.";
					case KeyCode.KeypadDivide:
						return "Kp/";
					case KeyCode.KeypadMultiply:
						return "Kp*";
					case KeyCode.KeypadMinus:
						return "Kp-";
					case KeyCode.KeypadPlus:
						return "Kp+";
					case KeyCode.KeypadEnter:
						return "KpEnt";
					case KeyCode.KeypadEquals:
						return "Kp=";
					case KeyCode.UpArrow:
						return "Up";
					case KeyCode.DownArrow:
						return "Down";
					case KeyCode.RightArrow:
						return "Right";
					case KeyCode.LeftArrow:
						return "Left";
					case KeyCode.Insert:
						return "Ins";
					case KeyCode.Home:
						return "Home";
					case KeyCode.End:
						return "End";
					case KeyCode.PageUp:
						return "PgUp";
					case KeyCode.PageDown:
						return "PgDn";
					case KeyCode.Numlock:
						return "NumL";
					case KeyCode.CapsLock:
						return "CapL";
					case KeyCode.ScrollLock:
						return "ScrL";
					case KeyCode.RightShift:
						return "RShf";
					case KeyCode.LeftShift:
						return "LShf";
					case KeyCode.RightControl:
						return "RCtrl";
					case KeyCode.LeftControl:
						return "LCtrl";
					case KeyCode.RightAlt:
						return "RAlt";
					case KeyCode.LeftAlt:
						return "LAlt";
					case KeyCode.RightCommand:
						return "Appl";
					case KeyCode.LeftCommand:
						return "Cmd";
					case KeyCode.LeftWindows:
						return "Win";
					case KeyCode.RightWindows:
						return "Win";
					case KeyCode.AltGr:
						return "AltGr";
					case KeyCode.Help:
						return "Help";
					case KeyCode.Print:
						return "Prnt";
					case KeyCode.SysReq:
						return "SysReq";
					case KeyCode.Break:
						return "Brk";
					case KeyCode.Menu:
						return "Menu";
				}
			}
			return k.ToString();
		}

		// Token: 0x060002AA RID: 682 RVA: 0x0000DBF1 File Offset: 0x0000BDF1
		public static void AppendWithComma(this StringBuilder sb, string text)
		{
			sb.AppendWithSeparator(text, ", ");
		}

		// Token: 0x060002AB RID: 683 RVA: 0x0000DBFF File Offset: 0x0000BDFF
		public static void AppendInNewLine(this StringBuilder sb, string text)
		{
			sb.AppendWithSeparator(text, "\n");
		}

		// Token: 0x060002AC RID: 684 RVA: 0x0000DC0D File Offset: 0x0000BE0D
		public static void AppendWithSeparator(this StringBuilder sb, string text, string separator)
		{
			if (text.NullOrEmpty())
			{
				return;
			}
			if (sb.Length > 0)
			{
				sb.Append(separator);
			}
			sb.Append(text);
		}

		// Token: 0x060002AD RID: 685 RVA: 0x0000DC34 File Offset: 0x0000BE34
		public static string WordWrapAt(this string text, float length)
		{
			Text.Font = GameFont.Medium;
			if (text.GetWidthCached() < length)
			{
				return text;
			}
			IEnumerable<Pair<char, int>> source = from p in text.Select((char c, int idx) => new Pair<char, int>(c, idx))
												  where p.First == ' '
												  select p;
			if (!source.Any<Pair<char, int>>())
			{
				return text;
			}
			Pair<char, int> pair = source.MinBy((Pair<char, int> p) => Mathf.Abs(text.Substring(0, p.Second).GetWidthCached() - text.Substring(p.Second + 1).GetWidthCached()));
			return text.Substring(0, pair.Second) + "\n" + text.Substring(pair.Second + 1);
		}

		// Token: 0x060002AE RID: 686 RVA: 0x0000DD0C File Offset: 0x0000BF0C
		public static string EventTypeToStringCached(EventType eventType)
		{
			if (GenText.eventTypesCached == null)
			{
				int num = 0;
				foreach (object obj in Enum.GetValues(typeof(EventType)))
				{
					num = Mathf.Max(num, (int)obj);
				}
				GenText.eventTypesCached = new string[num + 1];
				foreach (object obj2 in Enum.GetValues(typeof(EventType)))
				{
					GenText.eventTypesCached[(int)obj2] = obj2.ToString();
				}
			}
			if (eventType >= EventType.MouseDown && eventType < (EventType)GenText.eventTypesCached.Length)
			{
				return GenText.eventTypesCached[(int)eventType];
			}
			return "Unknown";
		}

		// Token: 0x060002AF RID: 687 RVA: 0x0000DE00 File Offset: 0x0000C000
		public static string FieldsToString<T>(T obj)
		{
			if (obj == null)
			{
				return "";
			}
			StringBuilder stringBuilder = new StringBuilder();
			foreach (FieldInfo fieldInfo in typeof(T).GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
			{
				if (stringBuilder.Length != 0)
				{
					stringBuilder.Append(", ");
				}
				stringBuilder.Append(fieldInfo.Name);
				stringBuilder.Append("=");
				object value = fieldInfo.GetValue(obj);
				if (value == null)
				{
					stringBuilder.Append("null");
				}
				else
				{
					stringBuilder.Append(value.ToString());
				}
			}
			return stringBuilder.ToString();
		}

	}
}
