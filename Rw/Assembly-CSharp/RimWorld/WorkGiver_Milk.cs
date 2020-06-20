using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200071E RID: 1822
	public class WorkGiver_Milk : WorkGiver_GatherAnimalBodyResources
	{
		// Token: 0x170008A7 RID: 2215
		// (get) Token: 0x06002FFC RID: 12284 RVA: 0x0010E3C9 File Offset: 0x0010C5C9
		protected override JobDef JobDef
		{
			get
			{
				return JobDefOf.Milk;
			}
		}

		// Token: 0x06002FFD RID: 12285 RVA: 0x000FA47D File Offset: 0x000F867D
		protected override CompHasGatherableBodyResource GetComp(Pawn animal)
		{
			return animal.TryGetComp<CompMilkable>();
		}
	}
}
