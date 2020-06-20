using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x020005D1 RID: 1489
	public class LordToil_DefendPoint : LordToil
	{
		// Token: 0x170007DE RID: 2014
		// (get) Token: 0x06002994 RID: 10644 RVA: 0x000F4B13 File Offset: 0x000F2D13
		protected LordToilData_DefendPoint Data
		{
			get
			{
				return (LordToilData_DefendPoint)this.data;
			}
		}

		// Token: 0x170007DF RID: 2015
		// (get) Token: 0x06002995 RID: 10645 RVA: 0x000F4B20 File Offset: 0x000F2D20
		public override IntVec3 FlagLoc
		{
			get
			{
				return this.Data.defendPoint;
			}
		}

		// Token: 0x170007E0 RID: 2016
		// (get) Token: 0x06002996 RID: 10646 RVA: 0x000F4B2D File Offset: 0x000F2D2D
		public override bool AllowSatisfyLongNeeds
		{
			get
			{
				return this.allowSatisfyLongNeeds;
			}
		}

		// Token: 0x06002997 RID: 10647 RVA: 0x000F4B35 File Offset: 0x000F2D35
		public LordToil_DefendPoint(bool canSatisfyLongNeeds = true)
		{
			this.allowSatisfyLongNeeds = canSatisfyLongNeeds;
			this.data = new LordToilData_DefendPoint();
		}

		// Token: 0x06002998 RID: 10648 RVA: 0x000F4B56 File Offset: 0x000F2D56
		public LordToil_DefendPoint(IntVec3 defendPoint, float defendRadius = 28f) : this(true)
		{
			this.Data.defendPoint = defendPoint;
			this.Data.defendRadius = defendRadius;
		}

		// Token: 0x06002999 RID: 10649 RVA: 0x000F4B78 File Offset: 0x000F2D78
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

		// Token: 0x0600299A RID: 10650 RVA: 0x000F4C34 File Offset: 0x000F2E34
		public void SetDefendPoint(IntVec3 defendPoint)
		{
			this.Data.defendPoint = defendPoint;
		}

		// Token: 0x040018F8 RID: 6392
		private bool allowSatisfyLongNeeds = true;
	}
}
