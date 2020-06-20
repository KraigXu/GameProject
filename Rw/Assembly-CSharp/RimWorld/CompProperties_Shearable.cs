using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000878 RID: 2168
	public class CompProperties_Shearable : CompProperties
	{
		// Token: 0x0600353A RID: 13626 RVA: 0x00123047 File Offset: 0x00121247
		public CompProperties_Shearable()
		{
			this.compClass = typeof(CompShearable);
		}

		// Token: 0x04001C8D RID: 7309
		public int shearIntervalDays;

		// Token: 0x04001C8E RID: 7310
		public int woolAmount = 1;

		// Token: 0x04001C8F RID: 7311
		public ThingDef woolDef;
	}
}
