using System;

namespace Verse
{
	// Token: 0x02000138 RID: 312
	public class LanguageWorker_English : LanguageWorker
	{
		// Token: 0x060008C4 RID: 2244 RVA: 0x0002DB8F File Offset: 0x0002BD8F
		public override string WithIndefiniteArticle(string str, Gender gender, bool plural = false, bool name = false)
		{
			if (str.NullOrEmpty())
			{
				return "";
			}
			if (name)
			{
				return str;
			}
			if (plural)
			{
				return str;
			}
			return "a " + str;
		}

		// Token: 0x060008C5 RID: 2245 RVA: 0x0002DBB5 File Offset: 0x0002BDB5
		public override string WithDefiniteArticle(string str, Gender gender, bool plural = false, bool name = false)
		{
			if (str.NullOrEmpty())
			{
				return "";
			}
			if (name)
			{
				return str;
			}
			return "the " + str;
		}

		// Token: 0x060008C6 RID: 2246 RVA: 0x0002DBD8 File Offset: 0x0002BDD8
		public override string PostProcessed(string str)
		{
			str = base.PostProcessed(str);
			if (str.StartsWith("a ", StringComparison.OrdinalIgnoreCase) && str.Length >= 3 && (str.Substring(2) == "hour" || str[2] == 'a' || str[2] == 'e' || str[2] == 'i' || str[2] == 'o' || str[2] == 'u'))
			{
				str = str.Insert(1, "n");
			}
			str = str.Replace(" a a", " an a");
			str = str.Replace(" a e", " an e");
			str = str.Replace(" a i", " an i");
			str = str.Replace(" a o", " an o");
			str = str.Replace(" a u", " an u");
			str = str.Replace(" a hour", " an hour");
			str = str.Replace(" A a", " An a");
			str = str.Replace(" A e", " An e");
			str = str.Replace(" A i", " An i");
			str = str.Replace(" A o", " An o");
			str = str.Replace(" A u", " An u");
			str = str.Replace(" A hour", " An hour");
			str = str.Replace("\na a", "\nan a");
			str = str.Replace("\na e", "\nan e");
			str = str.Replace("\na i", "\nan i");
			str = str.Replace("\na o", "\nan o");
			str = str.Replace("\na u", "\nan u");
			str = str.Replace("\na hour", "\nan hour");
			str = str.Replace("\nA a", "\nAn a");
			str = str.Replace("\nA e", "\nAn e");
			str = str.Replace("\nA i", "\nAn i");
			str = str.Replace("\nA o", "\nAn o");
			str = str.Replace("\nA u", "\nAn u");
			str = str.Replace("\nA hour", "\nAn hour");
			return str;
		}

		// Token: 0x060008C7 RID: 2247 RVA: 0x0002DE10 File Offset: 0x0002C010
		public override string ToTitleCase(string str)
		{
			if (str.NullOrEmpty())
			{
				return str;
			}
			string[] array = str.MergeMultipleSpaces(false).Trim().Split(new char[]
			{
				' '
			});
			for (int i = 0; i < array.Length; i++)
			{
				string text = array[i];
				if ((i == 0 || i == array.Length - 1 || TitleCaseHelper.IsUppercaseTitleWord(text)) && !text.NullOrEmpty())
				{
					int num = text.FirstLetterBetweenTags();
					string str2 = (num == 0) ? text[num].ToString().ToUpper() : (text.Substring(0, num) + char.ToUpper(text[num]).ToString());
					string str3 = text.Substring(num + 1);
					array[i] = str2 + str3;
				}
			}
			return string.Join(" ", array);
		}

		// Token: 0x060008C8 RID: 2248 RVA: 0x0002DEDC File Offset: 0x0002C0DC
		public override string OrdinalNumber(int number, Gender gender = Gender.None)
		{
			int num = number % 10;
			if (number / 10 % 10 != 1)
			{
				if (num == 1)
				{
					return number + "st";
				}
				if (num == 2)
				{
					return number + "nd";
				}
				if (num == 3)
				{
					return number + "rd";
				}
			}
			return number + "th";
		}

		// Token: 0x060008C9 RID: 2249 RVA: 0x0002DF48 File Offset: 0x0002C148
		public override string Pluralize(string str, Gender gender, int count = -1)
		{
			if (str.NullOrEmpty() || str[str.Length - 1] == 's')
			{
				return str;
			}
			int num = (int)str[str.Length - 1];
			char c = (str.Length == 1) ? '\0' : str[str.Length - 2];
			bool flag = char.IsLetter(c) && "oaieuyOAIEUY".IndexOf(c) >= 0;
			bool flag2 = char.IsLetter(c) && !flag;
			if (num == 121 && flag2)
			{
				return str.Substring(0, str.Length - 1) + "ies";
			}
			return str + "s";
		}
	}
}
