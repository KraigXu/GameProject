using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020007A7 RID: 1959
	public abstract class LordToil_Gathering : LordToil
	{
		// Token: 0x060032F0 RID: 13040 RVA: 0x0011B05C File Offset: 0x0011925C
		public LordToil_Gathering(IntVec3 spot, GatheringDef gatheringDef)
		{
			this.spot = spot;
			this.gatheringDef = gatheringDef;
		}

		// Token: 0x060032F1 RID: 13041 RVA: 0x0011B072 File Offset: 0x00119272
		public override ThinkTreeDutyHook VoluntaryJoinDutyHookFor(Pawn p)
		{
			return this.gatheringDef.duty.hook;
		}

		// Token: 0x060032F2 RID: 13042 RVA: 0x0011B084 File Offset: 0x00119284
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				this.lord.ownedPawns[i].mindState.duty = new PawnDuty(this.gatheringDef.duty, this.spot, -1f);
			}
		}

		// Token: 0x04001B75 RID: 7029
		protected IntVec3 spot;

		// Token: 0x04001B76 RID: 7030
		protected GatheringDef gatheringDef;
	}
}
