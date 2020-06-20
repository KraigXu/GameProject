using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x020005D5 RID: 1493
	public class LordToil_ExitMap : LordToil
	{
		// Token: 0x170007E2 RID: 2018
		// (get) Token: 0x060029A2 RID: 10658 RVA: 0x00010306 File Offset: 0x0000E506
		public override bool AllowSatisfyLongNeeds
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170007E3 RID: 2019
		// (get) Token: 0x060029A3 RID: 10659 RVA: 0x00010306 File Offset: 0x0000E506
		public override bool AllowSelfTend
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170007E4 RID: 2020
		// (get) Token: 0x060029A4 RID: 10660 RVA: 0x000F4D3D File Offset: 0x000F2F3D
		public virtual DutyDef ExitDuty
		{
			get
			{
				return DutyDefOf.ExitMapBest;
			}
		}

		// Token: 0x170007E5 RID: 2021
		// (get) Token: 0x060029A5 RID: 10661 RVA: 0x000F4D44 File Offset: 0x000F2F44
		protected LordToilData_ExitMap Data
		{
			get
			{
				return (LordToilData_ExitMap)this.data;
			}
		}

		// Token: 0x060029A6 RID: 10662 RVA: 0x000F4D51 File Offset: 0x000F2F51
		public LordToil_ExitMap(LocomotionUrgency locomotion = LocomotionUrgency.None, bool canDig = false, bool interruptCurrentJob = false)
		{
			this.data = new LordToilData_ExitMap();
			this.Data.locomotion = locomotion;
			this.Data.canDig = canDig;
			this.Data.interruptCurrentJob = interruptCurrentJob;
		}

		// Token: 0x060029A7 RID: 10663 RVA: 0x000F4D88 File Offset: 0x000F2F88
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
