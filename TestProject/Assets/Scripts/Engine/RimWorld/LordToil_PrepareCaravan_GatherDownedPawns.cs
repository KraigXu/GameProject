using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000786 RID: 1926
	public class LordToil_PrepareCaravan_GatherDownedPawns : LordToil
	{
		// Token: 0x1700091F RID: 2335
		// (get) Token: 0x06003257 RID: 12887 RVA: 0x0011854D File Offset: 0x0011674D
		public override float? CustomWakeThreshold
		{
			get
			{
				return new float?(0.5f);
			}
		}

		// Token: 0x17000920 RID: 2336
		// (get) Token: 0x06003258 RID: 12888 RVA: 0x00010306 File Offset: 0x0000E506
		public override bool AllowRestingInBed
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06003259 RID: 12889 RVA: 0x00118746 File Offset: 0x00116946
		public LordToil_PrepareCaravan_GatherDownedPawns(IntVec3 meetingPoint, IntVec3 exitSpot)
		{
			this.meetingPoint = meetingPoint;
			this.exitSpot = exitSpot;
		}

		// Token: 0x0600325A RID: 12890 RVA: 0x0011875C File Offset: 0x0011695C
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				Pawn pawn = this.lord.ownedPawns[i];
				if (pawn.IsColonist)
				{
					pawn.mindState.duty = new PawnDuty(DutyDefOf.PrepareCaravan_GatherDownedPawns, this.meetingPoint, this.exitSpot, -1f);
				}
				else
				{
					pawn.mindState.duty = new PawnDuty(DutyDefOf.PrepareCaravan_Wait, this.meetingPoint, -1f);
				}
			}
		}

		// Token: 0x0600325B RID: 12891 RVA: 0x001187F8 File Offset: 0x001169F8
		public override void LordToilTick()
		{
			if (Find.TickManager.TicksGame % 100 == 0)
			{
				bool flag = true;
				List<Pawn> downedPawns = ((LordJob_FormAndSendCaravan)this.lord.LordJob).downedPawns;
				for (int i = 0; i < downedPawns.Count; i++)
				{
					if (!JobGiver_PrepareCaravan_GatherDownedPawns.IsDownedPawnNearExitPoint(downedPawns[i], this.exitSpot))
					{
						flag = false;
						break;
					}
				}
				if (flag)
				{
					this.lord.ReceiveMemo("AllDownedPawnsGathered");
				}
			}
		}

		// Token: 0x04001B50 RID: 6992
		private IntVec3 meetingPoint;

		// Token: 0x04001B51 RID: 6993
		private IntVec3 exitSpot;
	}
}
