using System;
using RimWorld;

namespace Verse.AI.Group
{
	
	public class LordToil_DefendPoint : LordToil
	{
		
		// (get) Token: 0x06002994 RID: 10644 RVA: 0x000F4B13 File Offset: 0x000F2D13
		protected LordToilData_DefendPoint Data
		{
			get
			{
				return (LordToilData_DefendPoint)this.data;
			}
		}

		
		// (get) Token: 0x06002995 RID: 10645 RVA: 0x000F4B20 File Offset: 0x000F2D20
		public override IntVec3 FlagLoc
		{
			get
			{
				return this.Data.defendPoint;
			}
		}

		
		// (get) Token: 0x06002996 RID: 10646 RVA: 0x000F4B2D File Offset: 0x000F2D2D
		public override bool AllowSatisfyLongNeeds
		{
			get
			{
				return this.allowSatisfyLongNeeds;
			}
		}

		
		public LordToil_DefendPoint(bool canSatisfyLongNeeds = true)
		{
			this.allowSatisfyLongNeeds = canSatisfyLongNeeds;
			this.data = new LordToilData_DefendPoint();
		}

		
		public LordToil_DefendPoint(IntVec3 defendPoint, float defendRadius = 28f) : this(true)
		{
			this.Data.defendPoint = defendPoint;
			this.Data.defendRadius = defendRadius;
		}

		
		public override void UpdateAllDuties()
		{
			LordToilData_DefendPoint data = this.Data;
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				Pawn pawn = this.lord.ownedPawns[i];
				pawn.mindState.duty = new PawnDuty(DutyDefOf.Defend, data.defendPoint, -1f);
				pawn.mindState.duty.focusSecond = data.defendPoint;
				pawn.mindState.duty.radius = ((pawn.kindDef.defendPointRadius >= 0f) ? pawn.kindDef.defendPointRadius : data.defendRadius);
			}
		}

		
		public void SetDefendPoint(IntVec3 defendPoint)
		{
			this.Data.defendPoint = defendPoint;
		}

		
		private bool allowSatisfyLongNeeds = true;
	}
}
