using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	
	public class LordToil_PrepareCaravan_Leave : LordToil
	{
		
		// (get) Token: 0x06003266 RID: 12902 RVA: 0x00010306 File Offset: 0x0000E506
		public override bool AllowSatisfyLongNeeds
		{
			get
			{
				return false;
			}
		}

		
		// (get) Token: 0x06003267 RID: 12903 RVA: 0x0011854D File Offset: 0x0011674D
		public override float? CustomWakeThreshold
		{
			get
			{
				return new float?(0.5f);
			}
		}

		
		// (get) Token: 0x06003268 RID: 12904 RVA: 0x00010306 File Offset: 0x0000E506
		public override bool AllowRestingInBed
		{
			get
			{
				return false;
			}
		}

		
		// (get) Token: 0x06003269 RID: 12905 RVA: 0x00010306 File Offset: 0x0000E506
		public override bool AllowSelfTend
		{
			get
			{
				return false;
			}
		}

		
		public LordToil_PrepareCaravan_Leave(IntVec3 exitSpot)
		{
			this.exitSpot = exitSpot;
		}

		
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				Pawn pawn = this.lord.ownedPawns[i];
				pawn.mindState.duty = new PawnDuty(DutyDefOf.TravelOrWait, this.exitSpot, -1f);
				pawn.mindState.duty.locomotion = LocomotionUrgency.Jog;
			}
		}

		
		public override void LordToilTick()
		{
			if (Find.TickManager.TicksGame % 100 == 0)
			{
				GatherAnimalsAndSlavesForCaravanUtility.CheckArrived(this.lord, this.lord.ownedPawns, this.exitSpot, "ReadyToExitMap", (Pawn x) => true, null);
			}
		}

		
		private IntVec3 exitSpot;
	}
}
