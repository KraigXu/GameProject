using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200071F RID: 1823
	public class WorkGiver_Shear : WorkGiver_GatherAnimalBodyResources
	{
		// Token: 0x170008A8 RID: 2216
		// (get) Token: 0x06002FFF RID: 12287 RVA: 0x0010E3D8 File Offset: 0x0010C5D8
		protected override JobDef JobDef
		{
			get
			{
				return JobDefOf.Shear;
			}
		}

		// Token: 0x06003000 RID: 12288 RVA: 0x000FA494 File Offset: 0x000F8694
		protected override CompHasGatherableBodyResource GetComp(Pawn animal)
		{
			return animal.TryGetComp<CompShearable>();
		}
	}
}
