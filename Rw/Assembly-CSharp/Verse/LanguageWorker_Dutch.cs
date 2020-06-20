using System;

namespace Verse
{
	// Token: 0x02000137 RID: 311
	public class LanguageWorker_Dutch : LanguageWorker
	{
		// Token: 0x060008C1 RID: 2241 RVA: 0x0002DB41 File Offset: 0x0002BD41
		public override string WithIndefiniteArticle(string str, Gender gender, bool plural = false, bool name = false)
		{
			if (name)
			{
				return str;
			}
			if (plural)
			{
				return str;
			}
			return "een " + str;
		}

		// Token: 0x060008C2 RID: 2242 RVA: 0x0002DB59 File Offset: 0x0002BD59
		public override string WithDefiniteArticle(string str, Gender gender, bool plural = false, bool name = false)
		{
			if (name)
			{
				return str;
			}
			if (plural)
			{
				return "de " + str;
			}
			if (gender == Gender.Male || gender == Gender.Female)
			{
				return "de " + str;
			}
			return "het " + str;
		}
	}
}
