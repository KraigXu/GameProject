﻿using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x020005D3 RID: 1491
	public class LordToil_DefendSelf : LordToil
	{
		// Token: 0x0600299D RID: 10653 RVA: 0x000F4CA0 File Offset: 0x000F2EA0
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				this.lord.ownedPawns[i].mindState.duty = new PawnDuty(DutyDefOf.Defend, this.lord.ownedPawns[i].Position, -1f);
				this.lord.ownedPawns[i].mindState.duty.radius = 28f;
			}
		}
	}
}
