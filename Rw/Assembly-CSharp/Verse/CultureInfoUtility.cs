using System;
using System.Globalization;
using System.Threading;

namespace Verse
{
	// Token: 0x0200002E RID: 46
	public static class CultureInfoUtility
	{
		// Token: 0x060002E9 RID: 745 RVA: 0x0000F0BF File Offset: 0x0000D2BF
		public static void EnsureEnglish()
		{
			if (Thread.CurrentThread.CurrentCulture.Name != "en-US")
			{
				Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");
			}
		}

		// Token: 0x04000090 RID: 144
		private const string EnglishCulture = "en-US";
	}
}
