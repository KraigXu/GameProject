using System;
using System.Collections.Generic;
using System.Text;

namespace Verse
{
	// Token: 0x02000148 RID: 328
	public static class Translator
	{
		// Token: 0x0600093B RID: 2363 RVA: 0x00032D84 File Offset: 0x00030F84
		public static bool CanTranslate(this string key)
		{
			return LanguageDatabase.activeLanguage.HaveTextForKey(key, false);
		}

		// Token: 0x0600093C RID: 2364 RVA: 0x00032D94 File Offset: 0x00030F94
		public static TaggedString TranslateWithBackup(this string key, TaggedString backupKey)
		{
			TaggedString result;
			if (key.TryTranslate(out result))
			{
				return result;
			}
			if (backupKey.TryTranslate(out result))
			{
				return result;
			}
			return key.Translate();
		}

		// Token: 0x0600093D RID: 2365 RVA: 0x00032DC4 File Offset: 0x00030FC4
		public static bool TryTranslate(this string key, out TaggedString result)
		{
			if (key.NullOrEmpty())
			{
				result = key;
				return false;
			}
			if (LanguageDatabase.activeLanguage == null)
			{
				Log.Error("No active language! Cannot translate from key " + key + ".", false);
				result = key;
				return true;
			}
			if (LanguageDatabase.activeLanguage.TryGetTextFromKey(key, out result))
			{
				return true;
			}
			result = key;
			return false;
		}

		// Token: 0x0600093E RID: 2366 RVA: 0x00032E2F File Offset: 0x0003102F
		public static string TranslateSimple(this string key)
		{
			return key.Translate();
		}

		// Token: 0x0600093F RID: 2367 RVA: 0x00032E3C File Offset: 0x0003103C
		public static TaggedString Translate(this string key)
		{
			TaggedString taggedString;
			if (key.TryTranslate(out taggedString))
			{
				return taggedString;
			}
			LanguageDatabase.defaultLanguage.TryGetTextFromKey(key, out taggedString);
			if (Prefs.DevMode)
			{
				taggedString = Translator.PseudoTranslated(taggedString);
			}
			return taggedString;
		}

		// Token: 0x06000940 RID: 2368 RVA: 0x00032E7C File Offset: 0x0003107C
		[Obsolete("Use TranslatorFormattedStringExtensions")]
		public static string Translate(this string key, params object[] args)
		{
			if (key.NullOrEmpty())
			{
				return key;
			}
			if (LanguageDatabase.activeLanguage == null)
			{
				Log.Error("No active language! Cannot translate from key " + key + ".", false);
				return key;
			}
			TaggedString taggedString;
			if (!LanguageDatabase.activeLanguage.TryGetTextFromKey(key, out taggedString))
			{
				LanguageDatabase.defaultLanguage.TryGetTextFromKey(key, out taggedString);
				if (Prefs.DevMode)
				{
					taggedString = Translator.PseudoTranslated(taggedString);
				}
			}
			string result = taggedString;
			try
			{
				result = string.Format(taggedString, args);
			}
			catch (Exception arg)
			{
				Log.ErrorOnce("Exception translating '" + taggedString + "': " + arg, Gen.HashCombineInt(key.GetHashCode(), 394878901), false);
			}
			return result;
		}

		// Token: 0x06000941 RID: 2369 RVA: 0x00032F48 File Offset: 0x00031148
		public static bool TryGetTranslatedStringsForFile(string fileName, out List<string> stringList)
		{
			if (!LanguageDatabase.activeLanguage.TryGetStringsFromFile(fileName, out stringList) && !LanguageDatabase.defaultLanguage.TryGetStringsFromFile(fileName, out stringList))
			{
				Log.Error("No string files for " + fileName + ".", false);
				return false;
			}
			return true;
		}

		// Token: 0x06000942 RID: 2370 RVA: 0x00032F80 File Offset: 0x00031180
		private static string PseudoTranslated(string original)
		{
			if (original == null)
			{
				return null;
			}
			if (!Prefs.DevMode)
			{
				return original;
			}
			bool flag = false;
			StringBuilder stringBuilder = new StringBuilder();
			foreach (char c in original)
			{
				if (c == '{')
				{
					flag = true;
					stringBuilder.Append(c);
				}
				else if (c == '}')
				{
					flag = false;
					stringBuilder.Append(c);
				}
				else if (!flag)
				{
					string value;
					switch (c)
					{
					case 'a':
						value = "à";
						break;
					case 'b':
						value = "þ";
						break;
					case 'c':
						value = "ç";
						break;
					case 'd':
						value = "ð";
						break;
					case 'e':
						value = "è";
						break;
					case 'f':
						value = "Ƒ";
						break;
					case 'g':
						value = "ğ";
						break;
					case 'h':
						value = "ĥ";
						break;
					case 'i':
						value = "ì";
						break;
					case 'j':
						value = "ĵ";
						break;
					case 'k':
						value = "к";
						break;
					case 'l':
						value = "ſ";
						break;
					case 'm':
						value = "ṁ";
						break;
					case 'n':
						value = "ƞ";
						break;
					case 'o':
						value = "ò";
						break;
					case 'p':
						value = "ṗ";
						break;
					case 'q':
						value = "q";
						break;
					case 'r':
						value = "ṟ";
						break;
					case 's':
						value = "ș";
						break;
					case 't':
						value = "ṭ";
						break;
					case 'u':
						value = "ù";
						break;
					case 'v':
						value = "ṽ";
						break;
					case 'w':
						value = "ẅ";
						break;
					case 'x':
						value = "ẋ";
						break;
					case 'y':
						value = "ý";
						break;
					case 'z':
						value = "ž";
						break;
					default:
						value = (c.ToString() ?? "");
						break;
					}
					stringBuilder.Append(value);
				}
				else
				{
					stringBuilder.Append(c);
				}
			}
			return stringBuilder.ToString();
		}
	}
}
