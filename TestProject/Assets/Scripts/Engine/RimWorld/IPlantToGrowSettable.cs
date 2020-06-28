using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000ABB RID: 2747
	public interface IPlantToGrowSettable
	{
		// Token: 0x17000B8A RID: 2954
		// (get) Token: 0x0600411B RID: 16667
		Map Map { get; }

		// Token: 0x17000B8B RID: 2955
		// (get) Token: 0x0600411C RID: 16668
		IEnumerable<IntVec3> Cells { get; }

		// Token: 0x0600411D RID: 16669
		ThingDef GetPlantDefToGrow();

		// Token: 0x0600411E RID: 16670
		void SetPlantDefToGrow(ThingDef plantDef);

		// Token: 0x0600411F RID: 16671
		bool CanAcceptSowNow();
	}
}
