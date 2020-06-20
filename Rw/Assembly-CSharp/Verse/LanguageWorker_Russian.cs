using System;
using System.Text.RegularExpressions;

namespace Verse
{
	// Token: 0x02000141 RID: 321
	public class LanguageWorker_Russian : LanguageWorker
	{
		// Token: 0x060008F6 RID: 2294 RVA: 0x0002F2B6 File Offset: 0x0002D4B6
		public override string PostProcessedKeyedTranslation(string translation)
		{
			translation = base.PostProcessedKeyedTranslation(translation);
			return LanguageWorker_Russian.PostProcess(translation);
		}

		// Token: 0x060008F7 RID: 2295 RVA: 0x0002F2C7 File Offset: 0x0002D4C7
		public override string PostProcessed(string str)
		{
			str = base.PostProcessed(str);
			return LanguageWorker_Russian.PostProcess(str);
		}

		// Token: 0x060008F8 RID: 2296 RVA: 0x0002F2D8 File Offset: 0x0002D4D8
		private static string PostProcess(string translation)
		{
			return LanguageWorker_Russian._languageWorkerResolverRegex.Replace(translation, new MatchEvaluator(LanguageWorker_Russian.EvaluateResolver));
		}

		// Token: 0x060008F9 RID: 2297 RVA: 0x0002F2F4 File Offset: 0x0002D4F4
		private static string EvaluateResolver(Match match)
		{
			string value = match.Groups["resolverName"].Value;
			Group group = match.Groups["argument"];
			string[] array = new string[group.Captures.Count];
			for (int i = 0; i < group.Captures.Count; i++)
			{
				array[i] = group.Captures[i].Value.Trim();
			}
			LanguageWorker_Russian.IResolver resolverByKeyword = LanguageWorker_Russian.GetResolverByKeyword(value);
			if (resolverByKeyword == null)
			{
				return match.Value;
			}
			string text = resolverByKeyword.Resolve(array);
			if (text == null)
			{
				Log.ErrorOnce(string.Format("Error happened while resolving LW instruction: \"{0}\"", match.Value), match.Value.GetHashCode() ^ 374656432, false);
				return match.Value;
			}
			return text;
		}

		// Token: 0x060008FA RID: 2298 RVA: 0x0002F3BC File Offset: 0x0002D5BC
		private static LanguageWorker_Russian.IResolver GetResolverByKeyword(string keyword)
		{
			if (keyword == "Replace")
			{
				return LanguageWorker_Russian.replaceResolver;
			}
			if (!(keyword == "Number"))
			{
				return null;
			}
			return LanguageWorker_Russian.numberCaseResolver;
		}

		// Token: 0x060008FB RID: 2299 RVA: 0x0002F3E8 File Offset: 0x0002D5E8
		public override string Pluralize(string str, Gender gender, int count = -1)
		{
			if (str.NullOrEmpty())
			{
				return str;
			}
			char c = str[str.Length - 1];
			char c2 = (str.Length < 2) ? '\0' : str[str.Length - 2];
			if (gender != Gender.Male)
			{
				if (gender != Gender.Female)
				{
					if (gender == Gender.None)
					{
						if (c == 'o')
						{
							return str.Substring(0, str.Length - 1) + "a";
						}
						if (c == 'O')
						{
							return str.Substring(0, str.Length - 1) + "A";
						}
						if (c == 'e' || c == 'E')
						{
							char value = char.ToUpper(c2);
							if ("ГКХЖЧШЩЦ".IndexOf(value) >= 0)
							{
								if (c == 'e')
								{
									return str.Substring(0, str.Length - 1) + "a";
								}
								if (c == 'E')
								{
									return str.Substring(0, str.Length - 1) + "A";
								}
							}
							else
							{
								if (c == 'e')
								{
									return str.Substring(0, str.Length - 1) + "я";
								}
								if (c == 'E')
								{
									return str.Substring(0, str.Length - 1) + "Я";
								}
							}
						}
					}
				}
				else
				{
					if (c == 'я')
					{
						return str.Substring(0, str.Length - 1) + "и";
					}
					if (c == 'ь')
					{
						return str.Substring(0, str.Length - 1) + "и";
					}
					if (c == 'Я')
					{
						return str.Substring(0, str.Length - 1) + "И";
					}
					if (c == 'Ь')
					{
						return str.Substring(0, str.Length - 1) + "И";
					}
					if (c == 'a' || c == 'A')
					{
						char value2 = char.ToUpper(c2);
						if ("ГКХЖЧШЩ".IndexOf(value2) >= 0)
						{
							if (c == 'a')
							{
								return str.Substring(0, str.Length - 1) + "и";
							}
							return str.Substring(0, str.Length - 1) + "И";
						}
						else
						{
							if (c == 'a')
							{
								return str.Substring(0, str.Length - 1) + "ы";
							}
							return str.Substring(0, str.Length - 1) + "Ы";
						}
					}
				}
			}
			else
			{
				if (LanguageWorker_Russian.IsConsonant(c))
				{
					return str + "ы";
				}
				if (c == 'й')
				{
					return str.Substring(0, str.Length - 1) + "и";
				}
				if (c == 'ь')
				{
					return str.Substring(0, str.Length - 1) + "и";
				}
				if (c == 'Й')
				{
					return str.Substring(0, str.Length - 1) + "И";
				}
				if (c == 'Ь')
				{
					return str.Substring(0, str.Length - 1) + "И";
				}
			}
			return str;
		}

		// Token: 0x060008FC RID: 2300 RVA: 0x0002F6D8 File Offset: 0x0002D8D8
		private static bool IsConsonant(char ch)
		{
			return "бвгджзклмнпрстфхцчшщБВГДЖЗКЛМНПРСТФХЦЧШЩ".IndexOf(ch) >= 0;
		}

		// Token: 0x0400077B RID: 1915
		private static readonly LanguageWorker_Russian.ReplaceResolver replaceResolver = new LanguageWorker_Russian.ReplaceResolver();

		// Token: 0x0400077C RID: 1916
		private static readonly LanguageWorker_Russian.NumberCaseResolver numberCaseResolver = new LanguageWorker_Russian.NumberCaseResolver();

		// Token: 0x0400077D RID: 1917
		private static readonly Regex _languageWorkerResolverRegex = new Regex("\\^(?<resolverName>\\w+)\\(\\s*(?<argument>[^|]+?)\\s*(\\|\\s*(?<argument>[^|]+?)\\s*)*\\)\\^", RegexOptions.Compiled);

		// Token: 0x02001399 RID: 5017
		private interface IResolver
		{
			// Token: 0x060076C9 RID: 30409
			string Resolve(string[] arguments);
		}

		// Token: 0x0200139A RID: 5018
		private class ReplaceResolver : LanguageWorker_Russian.IResolver
		{
			// Token: 0x060076CA RID: 30410 RVA: 0x0028FA98 File Offset: 0x0028DC98
			public string Resolve(string[] arguments)
			{
				if (arguments.Length == 0)
				{
					return null;
				}
				string text = arguments[0];
				if (arguments.Length == 1)
				{
					return text;
				}
				for (int i = 1; i < arguments.Length; i++)
				{
					string input = arguments[i];
					Match match = LanguageWorker_Russian.ReplaceResolver._argumentRegex.Match(input);
					if (!match.Success)
					{
						return null;
					}
					string value = match.Groups["old"].Value;
					string value2 = match.Groups["new"].Value;
					if (value == text)
					{
						return value2;
					}
				}
				return text;
			}

			// Token: 0x04004A81 RID: 19073
			private static readonly Regex _argumentRegex = new Regex("'(?<old>[^']*?)'-'(?<new>[^']*?)'", RegexOptions.Compiled);
		}

		// Token: 0x0200139B RID: 5019
		private class NumberCaseResolver : LanguageWorker_Russian.IResolver
		{
			// Token: 0x060076CD RID: 30413 RVA: 0x0028FB30 File Offset: 0x0028DD30
			public string Resolve(string[] arguments)
			{
				if (arguments.Length != 4)
				{
					return null;
				}
				string text = arguments[0];
				Match match = LanguageWorker_Russian.NumberCaseResolver._numberRegex.Match(text);
				if (!match.Success)
				{
					return null;
				}
				bool success = match.Groups["frac"].Success;
				string value = match.Groups["floor"].Value;
				string formOne = arguments[1].Trim(new char[]
				{
					'\''
				});
				string text2 = arguments[2].Trim(new char[]
				{
					'\''
				});
				string formMany = arguments[3].Trim(new char[]
				{
					'\''
				});
				if (success)
				{
					return text2.Replace("#", text);
				}
				return LanguageWorker_Russian.NumberCaseResolver.GetFormForNumber(int.Parse(value), formOne, text2, formMany).Replace("#", text);
			}

			// Token: 0x060076CE RID: 30414 RVA: 0x0028FBF8 File Offset: 0x0028DDF8
			private static string GetFormForNumber(int number, string formOne, string formSeveral, string formMany)
			{
				int num = number % 10;
				if (number / 10 % 10 == 1)
				{
					return formMany;
				}
				if (num == 1)
				{
					return formOne;
				}
				if (num - 2 > 2)
				{
					return formMany;
				}
				return formSeveral;
			}

			// Token: 0x04004A82 RID: 19074
			private static readonly Regex _numberRegex = new Regex("(?<floor>[0-9]+)(\\.(?<frac>[0-9]+))?", RegexOptions.Compiled);
		}
	}
}
