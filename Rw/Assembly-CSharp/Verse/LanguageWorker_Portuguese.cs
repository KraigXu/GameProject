using System;

namespace Verse
{
	// Token: 0x0200013F RID: 319
	public class LanguageWorker_Portuguese : LanguageWorker
	{
		// Token: 0x060008EF RID: 2287 RVA: 0x0002F179 File Offset: 0x0002D379
		public override string WithIndefiniteArticle(string str, Gender gender, bool plural = false, bool name = false)
		{
			if (name)
			{
				return str;
			}
			if (plural)
			{
				return ((gender == Gender.Female) ? "umas " : "uns ") + str;
			}
			return ((gender == Gender.Female) ? "uma " : "um ") + str;
		}

		// Token: 0x060008F0 RID: 2288 RVA: 0x0002F1B1 File Offset: 0x0002D3B1
		public override string WithDefiniteArticle(string str, Gender gender, bool plural = false, bool name = false)
		{
			if (name)
			{
				return str;
			}
			if (plural)
			{
				return ((gender == Gender.Female) ? "as " : "os ") + str;
			}
			return ((gender == Gender.Female) ? "a " : "o ") + str;
		}
	}
}
