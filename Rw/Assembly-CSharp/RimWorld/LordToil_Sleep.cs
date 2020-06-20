using System;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020007A1 RID: 1953
	public class LordToil_Sleep : LordToil
	{
		// Token: 0x060032DC RID: 13020 RVA: 0x0011AE48 File Offset: 0x00119048
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				this.lord.ownedPawns[i].mindState.duty = new PawnDuty(DutyDefOf.SleepForever);
			}
		}
	}
}
