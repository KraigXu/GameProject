using System;
using RimWorld;

namespace Verse.AI.Group
{
	
	public class LordToil_ExitMap : LordToil
	{
		
		
		public override bool AllowSatisfyLongNeeds
		{
			get
			{
				return false;
			}
		}

		
		
		public override bool AllowSelfTend
		{
			get
			{
				return false;
			}
		}

		
		
		public virtual DutyDef ExitDuty
		{
			get
			{
				return DutyDefOf.ExitMapBest;
			}
		}

		
		
		protected LordToilData_ExitMap Data
		{
			get
			{
				return (LordToilData_ExitMap)this.data;
			}
		}

		
		public LordToil_ExitMap(LocomotionUrgency locomotion = LocomotionUrgency.None, bool canDig = false, bool interruptCurrentJob = false)
		{
			this.data = new LordToilData_ExitMap();
			this.Data.locomotion = locomotion;
			this.Data.canDig = canDig;
			this.Data.interruptCurrentJob = interruptCurrentJob;
		}

		
		public override void UpdateAllDuties()
		{
			LordToilData_ExitMap data = this.Data;
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				PawnDuty pawnDuty = new PawnDuty(this.ExitDuty);
				pawnDuty.locomotion = data.locomotion;
				pawnDuty.canDig = data.canDig;
				Pawn pawn = this.lord.ownedPawns[i];
				pawn.mindState.duty = pawnDuty;
				if (this.Data.interruptCurrentJob && pawn.jobs.curJob != null)
				{
					pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
				}
			}
		}
	}
}
