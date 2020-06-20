using System;

namespace Verse
{
	// Token: 0x02000143 RID: 323
	public class LanguageWorker_Swedish : LanguageWorker
	{
		// Token: 0x06000905 RID: 2309 RVA: 0x0002F856 File Offset: 0x0002DA56
		public override string WithIndefiniteArticle(string str, Gender gender, bool plural = false, bool name = false)
		{
			if (name)
			{
				return str;
			}
			if (gender == Gender.Male || gender == Gender.Female)
			{
				return "en " + str;
			}
			return "ett " + str;
		}

		// Token: 0x06000906 RID: 2310 RVA: 0x0002F880 File Offset: 0x0002DA80
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
			if (gender == Gender.Male || gender == Gender.Female)
			{
				if (this.IsVowel(ch))
				{
					return str + "n";
				}
				return str + "en";
			}
			else
			{
				if (this.IsVowel(ch))
				{
					return str + "t";
				}
				return str + "et";
			}
		}

		// Token: 0x06000907 RID: 2311 RVA: 0x0002F8F5 File Offset: 0x0002DAF5
		public bool IsVowel(char ch)
		{
			return "aeiouyåäöAEIOUYÅÄÖ".IndexOf(ch) >= 0;
		}
	}
}
