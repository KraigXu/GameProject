using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000886 RID: 2182
	public class CompProperties_Schedule : CompProperties
	{
		// Token: 0x06003548 RID: 13640 RVA: 0x00123248 File Offset: 0x00121448
		public CompProperties_Schedule()
		{
			this.compClass = typeof(CompSchedule);
		}

		// Token: 0x04001CB0 RID: 7344
		public float startTime;

		// Token: 0x04001CB1 RID: 7345
		public float endTime = 1f;

		// Token: 0x04001CB2 RID: 7346
		[MustTranslate]
		public string offMessage;
	}
}
