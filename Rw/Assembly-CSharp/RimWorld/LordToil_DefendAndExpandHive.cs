using System;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000790 RID: 1936
	public class LordToil_DefendAndExpandHive : LordToil_HiveRelated
	{
		// Token: 0x06003287 RID: 12935 RVA: 0x00119190 File Offset: 0x00117390
		public override void UpdateAllDuties()
		{
			base.FilterOutUnspawnedHives();
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				Hive hiveFor = base.GetHiveFor(this.lord.ownedPawns[i]);
				PawnDuty duty = new PawnDuty(DutyDefOf.DefendAndExpandHive, hiveFor, this.distToHiveToAttack);
				this.lord.ownedPawns[i].mindState.duty = duty;
			}
		}

		// Token: 0x04001B5A RID: 7002
		public float distToHiveToAttack = 10f;
	}
}
