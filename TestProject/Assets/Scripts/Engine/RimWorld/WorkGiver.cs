using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200071A RID: 1818
	public abstract class WorkGiver
	{
		// Token: 0x06002FDA RID: 12250 RVA: 0x00010306 File Offset: 0x0000E506
		public virtual bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return false;
		}

		// Token: 0x06002FDB RID: 12251 RVA: 0x00019EA1 File Offset: 0x000180A1
		public virtual Job NonScanJob(Pawn pawn)
		{
			return null;
		}

		// Token: 0x06002FDC RID: 12252 RVA: 0x0010DFBC File Offset: 0x0010C1BC
		public PawnCapacityDef MissingRequiredCapacity(Pawn pawn)
		{
			for (int i = 0; i < this.def.requiredCapacities.Count; i++)
			{
				if (!pawn.health.capacities.CapableOf(this.def.requiredCapacities[i]))
				{
					return this.def.requiredCapacities[i];
				}
			}
			return null;
		}

		// Token: 0x04001AD8 RID: 6872
		public WorkGiverDef def;
	}
}
