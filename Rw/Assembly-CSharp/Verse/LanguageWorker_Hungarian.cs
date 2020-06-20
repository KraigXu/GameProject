using System;

namespace Verse
{
	// Token: 0x0200013B RID: 315
	public class LanguageWorker_Hungarian : LanguageWorker
	{
		// Token: 0x060008DA RID: 2266 RVA: 0x0002EB63 File Offset: 0x0002CD63
		public override string WithIndefiniteArticle(string str, Gender gender, bool plural = false, bool name = false)
		{
			if (name)
			{
				return str;
			}
			return "egy " + str;
		}

		// Token: 0x060008DB RID: 2267 RVA: 0x0002EB78 File Offset: 0x0002CD78
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
			char ch = str[0];
			if (this.IsVowel(ch))
			{
				return "az " + str;
			}
			return "a " + str;
		}

		// Token: 0x060008DC RID: 2268 RVA: 0x0002EBBD File Offset: 0x0002CDBD
		public bool IsVowel(char ch)
		{
			return "eéöőüűiíaáoóuúEÉÖŐÜŰIÍAÁOÓUÚ".IndexOf(ch) >= 0;
		}
	}
}
