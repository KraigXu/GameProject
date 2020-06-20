using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000560 RID: 1376
	public class MentalStateWorker
	{
		// Token: 0x06002722 RID: 10018 RVA: 0x000E506C File Offset: 0x000E326C
		public virtual bool StateCanOccur(Pawn pawn)
		{
			if (!this.def.unspawnedCanDo && !pawn.Spawned)
			{
				return false;
			}
			if (!this.def.prisonersCanDo && pawn.HostFaction != null)
			{
				return false;
			}
			if (this.def.colonistsOnly && pawn.Faction != Faction.OfPlayer)
			{
				return false;
			}
			for (int i = 0; i < this.def.requiredCapacities.Count; i++)
			{
				if (!pawn.health.capacities.CapableOf(this.def.requiredCapacities[i]))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0400175A RID: 5978
		public MentalStateDef def;
	}
}
