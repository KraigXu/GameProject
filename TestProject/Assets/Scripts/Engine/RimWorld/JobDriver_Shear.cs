using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000624 RID: 1572
	public class JobDriver_Shear : JobDriver_GatherAnimalBodyResources
	{
		// Token: 0x1700081F RID: 2079
		// (get) Token: 0x06002B06 RID: 11014 RVA: 0x000FA48D File Offset: 0x000F868D
		protected override float WorkTotal
		{
			get
			{
				return 1700f;
			}
		}

		// Token: 0x06002B07 RID: 11015 RVA: 0x000FA494 File Offset: 0x000F8694
		protected override CompHasGatherableBodyResource GetComp(Pawn animal)
		{
			return animal.TryGetComp<CompShearable>();
		}
	}
}
