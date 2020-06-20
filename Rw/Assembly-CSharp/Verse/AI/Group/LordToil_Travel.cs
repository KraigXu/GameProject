using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x020005DA RID: 1498
	public class LordToil_Travel : LordToil
	{
		// Token: 0x170007EB RID: 2027
		// (get) Token: 0x060029B4 RID: 10676 RVA: 0x000F4F9C File Offset: 0x000F319C
		public override IntVec3 FlagLoc
		{
			get
			{
				return this.Data.dest;
			}
		}

		// Token: 0x170007EC RID: 2028
		// (get) Token: 0x060029B5 RID: 10677 RVA: 0x000F4FA9 File Offset: 0x000F31A9
		private LordToilData_Travel Data
		{
			get
			{
				return (LordToilData_Travel)this.data;
			}
		}

		// Token: 0x170007ED RID: 2029
		// (get) Token: 0x060029B6 RID: 10678 RVA: 0x00010306 File Offset: 0x0000E506
		public override bool AllowSatisfyLongNeeds
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170007EE RID: 2030
		// (get) Token: 0x060029B7 RID: 10679 RVA: 0x000F4FB6 File Offset: 0x000F31B6
		protected virtual float AllArrivedCheckRadius
		{
			get
			{
				return 10f;
			}
		}

		// Token: 0x060029B8 RID: 10680 RVA: 0x000F4FBD File Offset: 0x000F31BD
		public LordToil_Travel(IntVec3 dest)
		{
			this.data = new LordToilData_Travel();
			this.Data.dest = dest;
		}

		// Token: 0x060029B9 RID: 10681 RVA: 0x000F4FDC File Offset: 0x000F31DC
		public override void UpdateAllDuties()
		{
			LordToilData_Travel data = this.Data;
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				PawnDuty pawnDuty = new PawnDuty(DutyDefOf.TravelOrLeave, data.dest, -1f);
				pawnDuty.maxDanger = this.maxDanger;
				this.lord.ownedPawns[i].mindState.duty = pawnDuty;
			}
		}

		// Token: 0x060029BA RID: 10682 RVA: 0x000F5050 File Offset: 0x000F3250
		public override void LordToilTick()
		{
			if (Find.TickManager.TicksGame % 205 == 0)
			{
				LordToilData_Travel data = this.Data;
				bool flag = true;
				for (int i = 0; i < this.lord.ownedPawns.Count; i++)
				{
					Pawn pawn = this.lord.ownedPawns[i];
					if (!pawn.Position.InHorDistOf(data.dest, this.AllArrivedCheckRadius) || !pawn.CanReach(data.dest, PathEndMode.ClosestTouch, Danger.Deadly, false, TraverseMode.ByPawn))
					{
						flag = false;
						break;
					}
				}
				if (flag)
				{
					this.lord.ReceiveMemo("TravelArrived");
				}
			}
		}

		// Token: 0x060029BB RID: 10683 RVA: 0x000F50F3 File Offset: 0x000F32F3
		public bool HasDestination()
		{
			return this.Data.dest.IsValid;
		}

		// Token: 0x060029BC RID: 10684 RVA: 0x000F5105 File Offset: 0x000F3305
		public void SetDestination(IntVec3 dest)
		{
			this.Data.dest = dest;
		}

		// Token: 0x04001902 RID: 6402
		public Danger maxDanger;
	}
}
