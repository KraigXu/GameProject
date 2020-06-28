using System;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000682 RID: 1666
	public class JobDriver_PlantHarvest : JobDriver_PlantWork
	{
		// Token: 0x06002D5A RID: 11610 RVA: 0x000FFDDC File Offset: 0x000FDFDC
		protected override void Init()
		{
			this.xpPerTick = 0.085f;
		}

		// Token: 0x06002D5B RID: 11611 RVA: 0x000FFDE9 File Offset: 0x000FDFE9
		protected override Toil PlantWorkDoneToil()
		{
			return Toils_General.RemoveDesignationsOnThing(TargetIndex.A, DesignationDefOf.HarvestPlant);
		}
	}
}
