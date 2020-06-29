using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	
	public class LordToil_PrepareCaravan_GatherDownedPawns : LordToil
	{
		
		// (get) Token: 0x06003257 RID: 12887 RVA: 0x0011854D File Offset: 0x0011674D
		public override float? CustomWakeThreshold
		{
			get
			{
				return new float?(0.5f);
			}
		}

		
		// (get) Token: 0x06003258 RID: 12888 RVA: 0x00010306 File Offset: 0x0000E506
		public override bool AllowRestingInBed
		{
			get
			{
				return false;
			}
		}

		
		public LordToil_PrepareCaravan_GatherDownedPawns(IntVec3 meetingPoint, IntVec3 exitSpot)
		{
			this.meetingPoint = meetingPoint;
			this.exitSpot = exitSpot;
		}

		
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

		
		private IntVec3 meetingPoint;

		
		private IntVec3 exitSpot;
	}
}
