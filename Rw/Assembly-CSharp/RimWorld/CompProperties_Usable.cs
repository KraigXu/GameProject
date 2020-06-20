using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200087E RID: 2174
	public class CompProperties_Usable : CompProperties
	{
		// Token: 0x06003540 RID: 13632 RVA: 0x00123154 File Offset: 0x00121354
		public CompProperties_Usable()
		{
			this.compClass = typeof(CompUsable);
		}

		// Token: 0x04001CA7 RID: 7335
		public JobDef useJob;

		// Token: 0x04001CA8 RID: 7336
		[MustTranslate]
		public string useLabel;

		// Token: 0x04001CA9 RID: 7337
		public int useDuration = 100;
	}
}
