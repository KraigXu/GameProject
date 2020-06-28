using System;

namespace Verse
{
	// Token: 0x0200013C RID: 316
	public class LanguageWorker_Italian : LanguageWorker
	{
		// Token: 0x060008DE RID: 2270 RVA: 0x0002EBD0 File Offset: 0x0002CDD0
		public override string WithIndefiniteArticle(string str, Gender gender, bool plural = false, bool name = false)
		{
			if (name)
			{
				return str;
			}
			char c = str[0];
			char c2 = (str.Length >= 2) ? str[1] : '\0';
			if (gender == Gender.Female)
			{
				if (this.IsVowel(c))
				{
					return "un'" + str;
				}
				return "una " + str;
			}
			else
			{
				char c3 = char.ToLower(c);
				char c4 = char.ToLower(c2);
				if ((c == 's' || c == 'S') && !this.IsVowel(c2))
				{
					return "uno " + str;
				}
				if ((c3 == 'p' && c4 == 's') || (c3 == 'p' && c4 == 'n') || c3 == 'z' || c3 == 'x' || c3 == 'y' || (c3 == 'g' && c4 == 'n'))
				{
					return "uno " + str;
				}
				return "un " + str;
			}
		}

		// Token: 0x060008DF RID: 2271 RVA: 0x0002EC98 File Offset: 0x0002CE98
		public override string WithDefiniteArticle(string str, Gender gender, bool plural = false, bool name = false)
		{
			if (str.NullOrEmpty())
			{
				return str;
			}
			if (name)
			{
				return str;
			}
			char c = str[0];
			char ch = (str.Length >= 2) ? str[1] : '\0';
			if (gender == Gender.Female)
			{
				if (this.IsVowel(c))
				{
					return "l'" + str;
				}
				return "la " + str;
			}
			else
			{
				if (c == 'z' || c == 'Z')
				{
					return "lo " + str;
				}
				if ((c == 's' || c == 'S') && !this.IsVowel(ch))
				{
					return "lo " + str;
				}
				if (this.IsVowel(c))
				{
					return "l'" + str;
				}
				return "il " + str;
			}
		}

		// Token: 0x060008E0 RID: 2272 RVA: 0x0002ED4B File Offset: 0x0002CF4B
		public bool IsVowel(char ch)
		{
			return "aeiouAEIOU".IndexOf(ch) >= 0;
		}

		// Token: 0x060008E1 RID: 2273 RVA: 0x0002ED5E File Offset: 0x0002CF5E
		public override string OrdinalNumber(int number, Gender gender = Gender.None)
		{
			return number + "°";
		}

		// Token: 0x060008E2 RID: 2274 RVA: 0x0002ED70 File Offset: 0x0002CF70
		public override string Pluralize(string str, Gender gender, int count = -1)
		{
			if (str.NullOrEmpty())
			{
				return str;
			}
			char ch = str[str.Length - 1];
			if (!this.IsVowel(ch))
			{
				return str;
			}
			if (gender == Gender.Female)
			{
				return str.Substring(0, str.Length - 1) + "e";
			}
			return str.Substring(0, str.Length - 1) + "i";
		}
	}
}
