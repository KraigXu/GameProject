using System;

namespace Verse
{
	// Token: 0x02000144 RID: 324
	public class LanguageWorker_Turkish : LanguageWorker
	{
		// Token: 0x06000909 RID: 2313 RVA: 0x0002F908 File Offset: 0x0002DB08
		public override string WithIndefiniteArticle(string str, Gender gender, bool plural = false, bool name = false)
		{
			if (name)
			{
				return str;
			}
			return str + " bir";
		}

		// Token: 0x0600090A RID: 2314 RVA: 0x0002D90A File Offset: 0x0002BB0A
		public override string WithDefiniteArticle(string str, Gender gender, bool plural = false, bool name = false)
		{
			return str;
		}
	}
}
