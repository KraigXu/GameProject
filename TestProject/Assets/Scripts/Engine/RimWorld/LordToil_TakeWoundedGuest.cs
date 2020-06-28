using System;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020007A5 RID: 1957
	public class LordToil_TakeWoundedGuest : LordToil
	{
		// Token: 0x1700094E RID: 2382
		// (get) Token: 0x060032EA RID: 13034 RVA: 0x00010306 File Offset: 0x0000E506
		public override bool AllowSatisfyLongNeeds
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700094F RID: 2383
		// (get) Token: 0x060032EB RID: 13035 RVA: 0x00010306 File Offset: 0x0000E506
		public override bool AllowSelfTend
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060032EC RID: 13036 RVA: 0x0011AFF4 File Offset: 0x001191F4
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				this.lord.ownedPawns[i].mindState.duty = new PawnDuty(DutyDefOf.TakeWoundedGuest);
			}
		}
	}
}
