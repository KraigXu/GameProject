using System;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000792 RID: 1938
	public class LordToil_DefendHiveAggressively : LordToil_HiveRelated
	{
		// Token: 0x0600328C RID: 12940 RVA: 0x00119290 File Offset: 0x00117490
		public override void UpdateAllDuties()
		{
			base.FilterOutUnspawnedHives();
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				Hive hiveFor = base.GetHiveFor(this.lord.ownedPawns[i]);
				PawnDuty duty = new PawnDuty(DutyDefOf.DefendHiveAggressively, hiveFor, this.distToHiveToAttack);
				this.lord.ownedPawns[i].mindState.duty = duty;
			}
		}

		// Token: 0x04001B5C RID: 7004
		public float distToHiveToAttack = 40f;
	}
}
