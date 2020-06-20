using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using RimWorld;

namespace Verse
{
	// Token: 0x02000131 RID: 305
	public static class LanguageReportGenerator
	{
		// Token: 0x0600088F RID: 2191 RVA: 0x0002C4D8 File Offset: 0x0002A6D8
		public static void SaveTranslationReport()
		{
			LoadedLanguage activeLanguage = LanguageDatabase.activeLanguage;
			LoadedLanguage defaultLanguage = LanguageDatabase.defaultLanguage;
			if (activeLanguage == defaultLanguage && !defaultLanguage.anyError)
			{
				Messages.Message("Please activate a non-English language to scan.", MessageTypeDefOf.RejectInput, false);
				return;
			}
			activeLanguage.LoadData();
			defaultLanguage.LoadData();
			LongEventHandler.QueueLongEvent(new Action(LanguageReportGenerator.DoSaveTranslationReport), "GeneratingTranslationReport", true, null, true);
		}

		// Token: 0x06000890 RID: 2192 RVA: 0x0002C534 File Offset: 0x0002A734
		private static void DoSaveTranslationReport()
		{
			LoadedLanguage activeLanguage = LanguageDatabase.activeLanguage;
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Translation report for " + activeLanguage);
			if (activeLanguage.defInjections.Any((DefInjectionPackage x) => x.usedOldRepSyntax))
			{
				stringBuilder.AppendLine();
				stringBuilder.AppendLine("Consider using <Something.Field.Example.Etc>translation</Something.Field.Example.Etc> def-injection syntax instead of <rep>.");
			}
			try
			{
				LanguageReportGenerator.AppendGeneralLoadErrors(stringBuilder);
			}
			catch (Exception arg)
			{
				Log.Error("Error while generating translation report (general load errors): " + arg, false);
			}
			try
			{
				LanguageReportGenerator.AppendDefInjectionsLoadErros(stringBuilder);
			}
			catch (Exception arg2)
			{
				Log.Error("Error while generating translation report (def-injections load errors): " + arg2, false);
			}
			try
			{
				LanguageReportGenerator.AppendBackstoriesLoadErrors(stringBuilder);
			}
			catch (Exception arg3)
			{
				Log.Error("Error while generating translation report (backstories load errors): " + arg3, false);
			}
			try
			{
				LanguageReportGenerator.AppendMissingKeyedTranslations(stringBuilder);
			}
			catch (Exception arg4)
			{
				Log.Error("Error while generating translation report (missing keyed translations): " + arg4, false);
			}
			List<string> list = new List<string>();
			try
			{
				LanguageReportGenerator.AppendMissingDefInjections(stringBuilder, list);
			}
			catch (Exception arg5)
			{
				Log.Error("Error while generating translation report (missing def-injections): " + arg5, false);
			}
			try
			{
				LanguageReportGenerator.AppendMissingBackstories(stringBuilder);
			}
			catch (Exception arg6)
			{
				Log.Error("Error while generating translation report (missing backstories): " + arg6, false);
			}
			try
			{
				LanguageReportGenerator.AppendUnnecessaryDefInjections(stringBuilder, list);
			}
			catch (Exception arg7)
			{
				Log.Error("Error while generating translation report (unnecessary def-injections): " + arg7, false);
			}
			try
			{
				LanguageReportGenerator.AppendRenamedDefInjections(stringBuilder);
			}
			catch (Exception arg8)
			{
				Log.Error("Error while generating translation report (renamed def-injections): " + arg8, false);
			}
			try
			{
				LanguageReportGenerator.AppendArgumentCountMismatches(stringBuilder);
			}
			catch (Exception arg9)
			{
				Log.Error("Error while generating translation report (argument count mismatches): " + arg9, false);
			}
			try
			{
				LanguageReportGenerator.AppendUnnecessaryKeyedTranslations(stringBuilder);
			}
			catch (Exception arg10)
			{
				Log.Error("Error while generating translation report (unnecessary keyed translations): " + arg10, false);
			}
			try
			{
				LanguageReportGenerator.AppendKeyedTranslationsMatchingEnglish(stringBuilder);
			}
			catch (Exception arg11)
			{
				Log.Error("Error while generating translation report (keyed translations matching English): " + arg11, false);
			}
			try
			{
				LanguageReportGenerator.AppendBackstoriesMatchingEnglish(stringBuilder);
			}
			catch (Exception arg12)
			{
				Log.Error("Error while generating translation report (backstories matching English): " + arg12, false);
			}
			try
			{
				LanguageReportGenerator.AppendDefInjectionsSyntaxSuggestions(stringBuilder);
			}
			catch (Exception arg13)
			{
				Log.Error("Error while generating translation report (def-injections syntax suggestions): " + arg13, false);
			}
			try
			{
				LanguageReportGenerator.AppendTKeySystemErrors(stringBuilder);
			}
			catch (Exception arg14)
			{
				Log.Error("Error while generating translation report (TKeySystem errors): " + arg14, false);
			}
			string text = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
			if (text.NullOrEmpty())
			{
				text = GenFilePaths.SaveDataFolderPath;
			}
			text = Path.Combine(text, "TranslationReport.txt");
			File.WriteAllText(text, stringBuilder.ToString());
			Messages.Message("MessageTranslationReportSaved".Translate(Path.GetFullPath(text)), MessageTypeDefOf.TaskCompletion, false);
		}

		// Token: 0x06000891 RID: 2193 RVA: 0x0002C858 File Offset: 0x0002AA58
		private static void AppendGeneralLoadErrors(StringBuilder sb)
		{
			LoadedLanguage activeLanguage = LanguageDatabase.activeLanguage;
			StringBuilder stringBuilder = new StringBuilder();
			int num = 0;
			foreach (string value in activeLanguage.loadErrors)
			{
				num++;
				stringBuilder.AppendLine(value);
			}
			sb.AppendLine();
			sb.AppendLine("========== General load errors (" + num + ") ==========");
			sb.Append(stringBuilder);
		}

		// Token: 0x06000892 RID: 2194 RVA: 0x0002C8E8 File Offset: 0x0002AAE8
		private static void AppendDefInjectionsLoadErros(StringBuilder sb)
		{
			LoadedLanguage activeLanguage = LanguageDatabase.activeLanguage;
			StringBuilder stringBuilder = new StringBuilder();
			int num = 0;
			foreach (DefInjectionPackage defInjectionPackage in activeLanguage.defInjections)
			{
				foreach (string value in defInjectionPackage.loadErrors)
				{
					num++;
					stringBuilder.AppendLine(value);
				}
			}
			sb.AppendLine();
			sb.AppendLine("========== Def-injected translations load errors (" + num + ") ==========");
			sb.Append(stringBuilder);
		}

		// Token: 0x06000893 RID: 2195 RVA: 0x0002C9B4 File Offset: 0x0002ABB4
		private static void AppendBackstoriesLoadErrors(StringBuilder sb)
		{
			LoadedLanguage activeLanguage = LanguageDatabase.activeLanguage;
			StringBuilder stringBuilder = new StringBuilder();
			int num = 0;
			foreach (string value in activeLanguage.backstoriesLoadErrors)
			{
				num++;
				stringBuilder.AppendLine(value);
			}
			sb.AppendLine();
			sb.AppendLine("========== Backstories load errors (" + num + ") ==========");
			sb.Append(stringBuilder);
		}

		// Token: 0x06000894 RID: 2196 RVA: 0x0002CA44 File Offset: 0x0002AC44
		private static void AppendMissingKeyedTranslations(StringBuilder sb)
		{
			LoadedLanguage activeLanguage = LanguageDatabase.activeLanguage;
			LoadedLanguage defaultLanguage = LanguageDatabase.defaultLanguage;
			if (activeLanguage == defaultLanguage)
			{
				return;
			}
			StringBuilder stringBuilder = new StringBuilder();
			int num = 0;
			foreach (KeyValuePair<string, LoadedLanguage.KeyedReplacement> keyValuePair in defaultLanguage.keyedReplacements)
			{
				if (!activeLanguage.HaveTextForKey(keyValuePair.Key, false))
				{
					string text = string.Concat(new string[]
					{
						keyValuePair.Key,
						" '",
						keyValuePair.Value.value.Replace("\n", "\\n"),
						"' (English file: ",
						defaultLanguage.GetKeySourceFileAndLine(keyValuePair.Key),
						")"
					});
					if (activeLanguage.HaveTextForKey(keyValuePair.Key, true))
					{
						text = text + " (placeholder exists in " + activeLanguage.GetKeySourceFileAndLine(keyValuePair.Key) + ")";
					}
					num++;
					stringBuilder.AppendLine(text);
				}
			}
			sb.AppendLine();
			sb.AppendLine("========== Missing keyed translations (" + num + ") ==========");
			sb.Append(stringBuilder);
		}

		// Token: 0x06000895 RID: 2197 RVA: 0x0002CB8C File Offset: 0x0002AD8C
		private static void AppendMissingDefInjections(StringBuilder sb, List<string> outUnnecessaryDefInjections)
		{
			LoadedLanguage activeLanguage = LanguageDatabase.activeLanguage;
			LoadedLanguage defaultLanguage = LanguageDatabase.defaultLanguage;
			if (activeLanguage == defaultLanguage)
			{
				return;
			}
			StringBuilder stringBuilder = new StringBuilder();
			int num = 0;
			foreach (DefInjectionPackage defInjectionPackage in activeLanguage.defInjections)
			{
				foreach (string str in defInjectionPackage.MissingInjections(outUnnecessaryDefInjections))
				{
					num++;
					stringBuilder.AppendLine(defInjectionPackage.defType.Name + ": " + str);
				}
			}
			sb.AppendLine();
			sb.AppendLine("========== Def-injected translations missing (" + num + ") ==========");
			sb.Append(stringBuilder);
		}

		// Token: 0x06000896 RID: 2198 RVA: 0x0002CC80 File Offset: 0x0002AE80
		private static void AppendMissingBackstories(StringBuilder sb)
		{
			LoadedLanguage activeLanguage = LanguageDatabase.activeLanguage;
			LoadedLanguage defaultLanguage = LanguageDatabase.defaultLanguage;
			if (activeLanguage == defaultLanguage)
			{
				return;
			}
			StringBuilder stringBuilder = new StringBuilder();
			int num = 0;
			foreach (string value in BackstoryTranslationUtility.MissingBackstoryTranslations(activeLanguage))
			{
				num++;
				stringBuilder.AppendLine(value);
			}
			sb.AppendLine();
			sb.AppendLine("========== Backstory translations missing (" + num + ") ==========");
			sb.Append(stringBuilder);
		}

		// Token: 0x06000897 RID: 2199 RVA: 0x0002CD20 File Offset: 0x0002AF20
		private static void AppendUnnecessaryDefInjections(StringBuilder sb, List<string> unnecessaryDefInjections)
		{
			StringBuilder stringBuilder = new StringBuilder();
			int num = 0;
			foreach (string value in unnecessaryDefInjections)
			{
				num++;
				stringBuilder.AppendLine(value);
			}
			sb.AppendLine();
			sb.AppendLine("========== Unnecessary def-injected translations (marked as NoTranslate) (" + num + ") ==========");
			sb.Append(stringBuilder);
		}

		// Token: 0x06000898 RID: 2200 RVA: 0x0002CDA8 File Offset: 0x0002AFA8
		private static void AppendRenamedDefInjections(StringBuilder sb)
		{
			LoadedLanguage activeLanguage = LanguageDatabase.activeLanguage;
			StringBuilder stringBuilder = new StringBuilder();
			int num = 0;
			foreach (DefInjectionPackage defInjectionPackage in activeLanguage.defInjections)
			{
				foreach (KeyValuePair<string, DefInjectionPackage.DefInjection> keyValuePair in defInjectionPackage.injections)
				{
					if (!(keyValuePair.Value.path == keyValuePair.Value.nonBackCompatiblePath))
					{
						string text = keyValuePair.Value.nonBackCompatiblePath.Split(new char[]
						{
							'.'
						})[0];
						string text2 = keyValuePair.Value.path.Split(new char[]
						{
							'.'
						})[0];
						if (text != text2)
						{
							stringBuilder.AppendLine(string.Concat(new string[]
							{
								"Def has been renamed: ",
								text,
								" -> ",
								text2,
								", translation ",
								keyValuePair.Value.nonBackCompatiblePath,
								" should be renamed as well."
							}));
						}
						else
						{
							stringBuilder.AppendLine("Translation " + keyValuePair.Value.nonBackCompatiblePath + " should be renamed to " + keyValuePair.Value.path);
						}
						num++;
					}
				}
			}
			sb.AppendLine();
			sb.AppendLine("========== Def-injected translations using old, renamed defs (fixed automatically but can break in the next RimWorld version) (" + num + ") =========");
			sb.Append(stringBuilder);
		}

		// Token: 0x06000899 RID: 2201 RVA: 0x0002CF78 File Offset: 0x0002B178
		private static void AppendArgumentCountMismatches(StringBuilder sb)
		{
			LoadedLanguage activeLanguage = LanguageDatabase.activeLanguage;
			LoadedLanguage defaultLanguage = LanguageDatabase.defaultLanguage;
			if (activeLanguage == defaultLanguage)
			{
				return;
			}
			StringBuilder stringBuilder = new StringBuilder();
			int num = 0;
			foreach (string text in defaultLanguage.keyedReplacements.Keys.Intersect(activeLanguage.keyedReplacements.Keys))
			{
				if (!activeLanguage.keyedReplacements[text].isPlaceholder && !LanguageReportGenerator.SameSimpleGrammarResolverSymbols(defaultLanguage.keyedReplacements[text].value, activeLanguage.keyedReplacements[text].value))
				{
					num++;
					stringBuilder.AppendLine(string.Format("{0} ({1})\n  - '{2}'\n  - '{3}'", new object[]
					{
						text,
						activeLanguage.GetKeySourceFileAndLine(text),
						defaultLanguage.keyedReplacements[text].value.Replace("\n", "\\n"),
						activeLanguage.keyedReplacements[text].value.Replace("\n", "\\n")
					}));
				}
			}
			sb.AppendLine();
			sb.AppendLine("========== Argument count mismatches (may or may not be incorrect) (" + num + ") ==========");
			sb.Append(stringBuilder);
		}

		// Token: 0x0600089A RID: 2202 RVA: 0x0002D0D8 File Offset: 0x0002B2D8
		private static void AppendUnnecessaryKeyedTranslations(StringBuilder sb)
		{
			LoadedLanguage activeLanguage = LanguageDatabase.activeLanguage;
			LoadedLanguage defaultLanguage = LanguageDatabase.defaultLanguage;
			if (activeLanguage == defaultLanguage)
			{
				return;
			}
			StringBuilder stringBuilder = new StringBuilder();
			int num = 0;
			foreach (KeyValuePair<string, LoadedLanguage.KeyedReplacement> keyValuePair in activeLanguage.keyedReplacements)
			{
				if (!defaultLanguage.HaveTextForKey(keyValuePair.Key, false))
				{
					num++;
					stringBuilder.AppendLine(string.Concat(new string[]
					{
						keyValuePair.Key,
						" '",
						keyValuePair.Value.value.Replace("\n", "\\n"),
						"' (",
						activeLanguage.GetKeySourceFileAndLine(keyValuePair.Key),
						")"
					}));
				}
			}
			sb.AppendLine();
			sb.AppendLine("========== Unnecessary keyed translations (will never be used) (" + num + ") ==========");
			sb.Append(stringBuilder);
		}

		// Token: 0x0600089B RID: 2203 RVA: 0x0002D1E4 File Offset: 0x0002B3E4
		private static void AppendKeyedTranslationsMatchingEnglish(StringBuilder sb)
		{
			LoadedLanguage activeLanguage = LanguageDatabase.activeLanguage;
			LoadedLanguage defaultLanguage = LanguageDatabase.defaultLanguage;
			if (activeLanguage == defaultLanguage)
			{
				return;
			}
			StringBuilder stringBuilder = new StringBuilder();
			int num = 0;
			foreach (KeyValuePair<string, LoadedLanguage.KeyedReplacement> keyValuePair in activeLanguage.keyedReplacements)
			{
				TaggedString taggedString;
				if (!keyValuePair.Value.isPlaceholder && defaultLanguage.TryGetTextFromKey(keyValuePair.Key, out taggedString) && keyValuePair.Value.value == taggedString)
				{
					num++;
					stringBuilder.AppendLine(string.Concat(new string[]
					{
						keyValuePair.Key,
						" '",
						keyValuePair.Value.value.Replace("\n", "\\n"),
						"' (",
						activeLanguage.GetKeySourceFileAndLine(keyValuePair.Key),
						")"
					}));
				}
			}
			sb.AppendLine();
			sb.AppendLine("========== Keyed translations matching English (maybe ok) (" + num + ") ==========");
			sb.Append(stringBuilder);
		}

		// Token: 0x0600089C RID: 2204 RVA: 0x0002D324 File Offset: 0x0002B524
		private static void AppendBackstoriesMatchingEnglish(StringBuilder sb)
		{
			LoadedLanguage activeLanguage = LanguageDatabase.activeLanguage;
			LoadedLanguage defaultLanguage = LanguageDatabase.defaultLanguage;
			if (activeLanguage == defaultLanguage)
			{
				return;
			}
			StringBuilder stringBuilder = new StringBuilder();
			int num = 0;
			foreach (string value in BackstoryTranslationUtility.BackstoryTranslationsMatchingEnglish(activeLanguage))
			{
				num++;
				stringBuilder.AppendLine(value);
			}
			sb.AppendLine();
			sb.AppendLine("========== Backstory translations matching English (maybe ok) (" + num + ") ==========");
			sb.Append(stringBuilder);
		}

		// Token: 0x0600089D RID: 2205 RVA: 0x0002D3C4 File Offset: 0x0002B5C4
		private static void AppendDefInjectionsSyntaxSuggestions(StringBuilder sb)
		{
			LoadedLanguage activeLanguage = LanguageDatabase.activeLanguage;
			StringBuilder stringBuilder = new StringBuilder();
			int num = 0;
			foreach (DefInjectionPackage defInjectionPackage in activeLanguage.defInjections)
			{
				foreach (string value in defInjectionPackage.loadSyntaxSuggestions)
				{
					num++;
					stringBuilder.AppendLine(value);
				}
			}
			if (num == 0)
			{
				return;
			}
			sb.AppendLine();
			sb.AppendLine("========== Def-injected translations syntax suggestions (" + num + ") ==========");
			sb.Append(stringBuilder);
		}

		// Token: 0x0600089E RID: 2206 RVA: 0x0002D494 File Offset: 0x0002B694
		private static void AppendTKeySystemErrors(StringBuilder sb)
		{
			if (TKeySystem.loadErrors.Count == 0)
			{
				return;
			}
			sb.AppendLine();
			sb.AppendLine("========== TKey system errors (" + TKeySystem.loadErrors.Count + ") ==========");
			sb.Append(string.Join("\r\n", TKeySystem.loadErrors));
		}

		// Token: 0x0600089F RID: 2207 RVA: 0x0002D4F0 File Offset: 0x0002B6F0
		public static bool SameSimpleGrammarResolverSymbols(string str1, string str2)
		{
			LanguageReportGenerator.tmpStr1Symbols.Clear();
			LanguageReportGenerator.tmpStr2Symbols.Clear();
			LanguageReportGenerator.CalculateSimpleGrammarResolverSymbols(str1, LanguageReportGenerator.tmpStr1Symbols);
			LanguageReportGenerator.CalculateSimpleGrammarResolverSymbols(str2, LanguageReportGenerator.tmpStr2Symbols);
			for (int i = 0; i < LanguageReportGenerator.tmpStr1Symbols.Count; i++)
			{
				if (!LanguageReportGenerator.tmpStr2Symbols.Contains(LanguageReportGenerator.tmpStr1Symbols[i]))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060008A0 RID: 2208 RVA: 0x0002D558 File Offset: 0x0002B758
		private static void CalculateSimpleGrammarResolverSymbols(string str, List<string> outSymbols)
		{
			outSymbols.Clear();
			for (int i = 0; i < str.Length; i++)
			{
				if (str[i] == '{')
				{
					LanguageReportGenerator.tmpSymbol.Length = 0;
					bool flag = false;
					bool flag2 = false;
					bool flag3 = false;
					for (i++; i < str.Length; i++)
					{
						char c = str[i];
						if (c == '}')
						{
							flag = true;
							break;
						}
						if (c == '_')
						{
							flag2 = true;
						}
						else if (c == '?')
						{
							flag3 = true;
						}
						else if (!flag2 && !flag3)
						{
							LanguageReportGenerator.tmpSymbol.Append(c);
						}
					}
					if (flag)
					{
						outSymbols.Add(LanguageReportGenerator.tmpSymbol.ToString().Trim());
					}
				}
			}
		}

		// Token: 0x04000768 RID: 1896
		private const string FileName = "TranslationReport.txt";

		// Token: 0x04000769 RID: 1897
		private static List<string> tmpStr1Symbols = new List<string>();

		// Token: 0x0400076A RID: 1898
		private static List<string> tmpStr2Symbols = new List<string>();

		// Token: 0x0400076B RID: 1899
		private static StringBuilder tmpSymbol = new StringBuilder();
	}
}
