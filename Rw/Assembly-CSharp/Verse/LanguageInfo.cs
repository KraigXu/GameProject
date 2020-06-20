using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000130 RID: 304
	public class LanguageInfo
	{
		// Token: 0x04000763 RID: 1891
		public string friendlyNameNative;

		// Token: 0x04000764 RID: 1892
		public string friendlyNameEnglish;

		// Token: 0x04000765 RID: 1893
		public bool canBeTiny = true;

		// Token: 0x04000766 RID: 1894
		public List<CreditsEntry> credits = new List<CreditsEntry>();

		// Token: 0x04000767 RID: 1895
		public Type languageWorkerClass = typeof(LanguageWorker_Default);
	}
}
