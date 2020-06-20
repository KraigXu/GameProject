using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D99 RID: 3481
	public class CompProperties_Initiatable : CompProperties
	{
		// Token: 0x060054A3 RID: 21667 RVA: 0x001C3850 File Offset: 0x001C1A50
		public CompProperties_Initiatable()
		{
			this.compClass = typeof(CompInitiatable);
		}

		// Token: 0x04002E7E RID: 11902
		public int initiationDelayTicks;
	}
}
