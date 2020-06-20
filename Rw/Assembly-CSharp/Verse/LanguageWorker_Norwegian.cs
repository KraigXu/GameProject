using System;

namespace Verse
{
	// Token: 0x0200013E RID: 318
	public class LanguageWorker_Norwegian : LanguageWorker
	{
		// Token: 0x060008EC RID: 2284 RVA: 0x0002F0E2 File Offset: 0x0002D2E2
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
			return "et " + str;
		}

		// Token: 0x060008ED RID: 2285 RVA: 0x0002F10C File Offset: 0x0002D30C
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
			char c = str[str.Length - 1];
			if (gender == Gender.Male || gender == Gender.Female)
			{
				if (c == 'e')
				{
					return str + "n";
				}
				return str + "en";
			}
			else
			{
				if (c == 'e')
				{
					return str + "t";
				}
				return str + "et";
			}
		}
	}
}
