using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000872 RID: 2162
	public class CompProperties_Milkable : CompProperties
	{
		// Token: 0x06003529 RID: 13609 RVA: 0x00122EC7 File Offset: 0x001210C7
		public CompProperties_Milkable()
		{
			this.compClass = typeof(CompMilkable);
		}

		// Token: 0x04001C77 RID: 7287
		public int milkIntervalDays;

		// Token: 0x04001C78 RID: 7288
		public int milkAmount = 1;

		// Token: 0x04001C79 RID: 7289
		public ThingDef milkDef;

		// Token: 0x04001C7A RID: 7290
		public bool milkFemaleOnly = true;
	}
}
