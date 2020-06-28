using System;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200079B RID: 1947
	public class LordToil_LoadAndEnterTransporters : LordToil
	{
		// Token: 0x1700093D RID: 2365
		// (get) Token: 0x060032B5 RID: 12981 RVA: 0x00010306 File Offset: 0x0000E506
		public override bool AllowSatisfyLongNeeds
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060032B6 RID: 12982 RVA: 0x00119E24 File Offset: 0x00118024
		public LordToil_LoadAndEnterTransporters(int transportersGroup)
		{
			this.transportersGroup = transportersGroup;
		}

		// Token: 0x060032B7 RID: 12983 RVA: 0x00119E3C File Offset: 0x0011803C
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				PawnDuty pawnDuty = new PawnDuty(DutyDefOf.LoadAndEnterTransporters);
				pawnDuty.transportersGroup = this.transportersGroup;
				this.lord.ownedPawns[i].mindState.duty = pawnDuty;
			}
		}

		// Token: 0x04001B60 RID: 7008
		private int transportersGroup = -1;
	}
}
