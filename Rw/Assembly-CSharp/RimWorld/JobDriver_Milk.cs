using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000623 RID: 1571
	public class JobDriver_Milk : JobDriver_GatherAnimalBodyResources
	{
		// Token: 0x1700081E RID: 2078
		// (get) Token: 0x06002B03 RID: 11011 RVA: 0x000FA476 File Offset: 0x000F8676
		protected override float WorkTotal
		{
			get
			{
				return 400f;
			}
		}

		// Token: 0x06002B04 RID: 11012 RVA: 0x000FA47D File Offset: 0x000F867D
		protected override CompHasGatherableBodyResource GetComp(Pawn animal)
		{
			return animal.TryGetComp<CompMilkable>();
		}
	}
}
