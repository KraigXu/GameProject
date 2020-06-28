using System;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000683 RID: 1667
	public class JobDriver_PlantCut : JobDriver_PlantWork
	{
		// Token: 0x06002D5D RID: 11613 RVA: 0x000FFDFE File Offset: 0x000FDFFE
		protected override void Init()
		{
			if (base.Plant.def.plant.harvestedThingDef != null && base.Plant.CanYieldNow())
			{
				this.xpPerTick = 0.085f;
				return;
			}
			this.xpPerTick = 0f;
		}

		// Token: 0x06002D5E RID: 11614 RVA: 0x000FFE3B File Offset: 0x000FE03B
		protected override Toil PlantWorkDoneToil()
		{
			return Toils_Interact.DestroyThing(TargetIndex.A);
		}
	}
}
