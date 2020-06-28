using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000866 RID: 2150
	public class CompProperties_CausesGameCondition : CompProperties
	{
		// Token: 0x06003507 RID: 13575 RVA: 0x00122768 File Offset: 0x00120968
		public CompProperties_CausesGameCondition()
		{
			this.compClass = typeof(CompCauseGameCondition);
		}

		// Token: 0x04001C3B RID: 7227
		public GameConditionDef conditionDef;

		// Token: 0x04001C3C RID: 7228
		public int worldRange;

		// Token: 0x04001C3D RID: 7229
		public bool preventConditionStacking = true;
	}
}
