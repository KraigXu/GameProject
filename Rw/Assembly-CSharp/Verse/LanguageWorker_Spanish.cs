using System;

namespace Verse
{
	// Token: 0x02000142 RID: 322
	public class LanguageWorker_Spanish : LanguageWorker
	{
		// Token: 0x060008FF RID: 2303 RVA: 0x0002F711 File Offset: 0x0002D911
		public override string WithIndefiniteArticle(string str, Gender gender, bool plural = false, bool name = false)
		{
			if (name)
			{
				return str;
			}
			return ((gender == Gender.Female) ? "una " : "un ") + str;
		}

		// Token: 0x06000900 RID: 2304 RVA: 0x0002F72F File Offset: 0x0002D92F
		public override string WithDefiniteArticle(string str, Gender gender, bool plural = false, bool name = false)
		{
			if (name)
			{
				return str;
			}
			return ((gender == Gender.Female) ? "la " : "el ") + str;
		}

		// Token: 0x06000901 RID: 2305 RVA: 0x0002F74D File Offset: 0x0002D94D
		public override string OrdinalNumber(int number, Gender gender = Gender.None)
		{
			return number + ".º";
		}

		// Token: 0x06000902 RID: 2306 RVA: 0x0002F760 File Offset: 0x0002D960
		public override string Pluralize(string str, Gender gender, int count = -1)
		{
			if (str.NullOrEmpty())
			{
				return str;
			}
			char c = str[str.Length - 1];
			char c2 = (str.Length >= 2) ? str[str.Length - 2] : '\0';
			if (this.IsVowel(c))
			{
				if (str == "sí")
				{
					return "síes";
				}
				if (c == 'í' || c == 'ú' || c == 'Í' || c == 'Ú')
				{
					return str + "es";
				}
				return str + "s";
			}
			else
			{
				if ((c == 'y' || c == 'Y') && this.IsVowel(c2))
				{
					return str + "es";
				}
				if ("lrndzjsxLRNDZJSX".IndexOf(c) >= 0 || (c == 'h' && c2 == 'c'))
				{
					return str + "es";
				}
				return str + "s";
			}
		}

		// Token: 0x06000903 RID: 2307 RVA: 0x0002F843 File Offset: 0x0002DA43
		public bool IsVowel(char ch)
		{
			return "aeiouáéíóúAEIOUÁÉÍÓÚ".IndexOf(ch) >= 0;
		}
	}
}
