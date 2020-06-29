using System;
using RimWorld;

namespace Verse.AI.Group
{
	
	public class LordToil_Travel : LordToil
	{
		
		// (get) Token: 0x060029B4 RID: 10676 RVA: 0x000F4F9C File Offset: 0x000F319C
		public override IntVec3 FlagLoc
		{
			get
			{
				return this.Data.dest;
			}
		}

		
		// (get) Token: 0x060029B5 RID: 10677 RVA: 0x000F4FA9 File Offset: 0x000F31A9
		private LordToilData_Travel Data
		{
			get
			{
				return (LordToilData_Travel)this.data;
			}
		}

		
		// (get) Token: 0x060029B6 RID: 10678 RVA: 0x00010306 File Offset: 0x0000E506
		public override bool AllowSatisfyLongNeeds
		{
			get
			{
				return false;
			}
		}

		
		// (get) Token: 0x060029B7 RID: 10679 RVA: 0x000F4FB6 File Offset: 0x000F31B6
		protected virtual float AllArrivedCheckRadius
		{
			get
			{
				return 10f;
			}
		}

		
		public LordToil_Travel(IntVec3 dest)
		{
			this.data = new LordToilData_Travel();
			this.Data.dest = dest;
		}

		
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

		
		public bool HasDestination()
		{
			return this.Data.dest.IsValid;
		}

		
		public void SetDestination(IntVec3 dest)
		{
			this.Data.dest = dest;
		}

		
		public Danger maxDanger;
	}
}
