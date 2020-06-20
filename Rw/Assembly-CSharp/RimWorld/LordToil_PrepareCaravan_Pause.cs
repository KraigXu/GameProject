using System;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200078A RID: 1930
	public class LordToil_PrepareCaravan_Pause : LordToil
	{
		// Token: 0x0600326D RID: 12909 RVA: 0x00118C28 File Offset: 0x00116E28
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				this.lord.ownedPawns[i].mindState.duty = new PawnDuty(DutyDefOf.PrepareCaravan_Pause);
			}
		}
	}
}
