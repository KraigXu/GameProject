using System;
using RimWorld;

namespace Verse.AI.Group
{
	
	public class LordToil_ExitMap : LordToil
	{
		
		// (get) Token: 0x060029A2 RID: 10658 RVA: 0x00010306 File Offset: 0x0000E506
		public override bool AllowSatisfyLongNeeds
		{
			get
			{
				return false;
			}
		}

		
		// (get) Token: 0x060029A3 RID: 10659 RVA: 0x00010306 File Offset: 0x0000E506
		public override bool AllowSelfTend
		{
			get
			{
				return false;
			}
		}

		
		// (get) Token: 0x060029A4 RID: 10660 RVA: 0x000F4D3D File Offset: 0x000F2F3D
		public virtual DutyDef ExitDuty
		{
			get
			{
				return DutyDefOf.ExitMapBest;
			}
		}

		
		// (get) Token: 0x060029A5 RID: 10661 RVA: 0x000F4D44 File Offset: 0x000F2F44
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
