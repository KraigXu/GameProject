using System;

namespace Verse
{
	// Token: 0x02000140 RID: 320
	public class LanguageWorker_Romanian : LanguageWorker
	{
		// Token: 0x060008F2 RID: 2290 RVA: 0x0002F1E9 File Offset: 0x0002D3E9
		public override string WithIndefiniteArticle(string str, Gender gender, bool plural = false, bool name = false)
		{
			if (name)
			{
				return str;
			}
			if (!plural)
			{
				return ((gender == Gender.Female) ? "a " : "un ") + str;
			}
			if (gender != Gender.Male)
			{
				return str + "e";
			}
			return str + "i";
		}

		// Token: 0x060008F3 RID: 2291 RVA: 0x0002F228 File Offset: 0x0002D428
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
			char ch = str[str.Length - 1];
			if (plural)
			{
				if (gender != Gender.Male)
				{
					return str + "e";
				}
				return str + "i";
			}
			else
			{
				if (!this.IsVowel(ch))
				{
					return str + "ul";
				}
				if (gender == Gender.Male)
				{
					return str + "le";
				}
				return str + "a";
			}
		}

		// Token: 0x060008F4 RID: 2292 RVA: 0x0002F2A3 File Offset: 0x0002D4A3
		public bool IsVowel(char ch)
		{
			return "aeiouâîAEIOUÂÎ".IndexOf(ch) >= 0;
		}
	}
}
