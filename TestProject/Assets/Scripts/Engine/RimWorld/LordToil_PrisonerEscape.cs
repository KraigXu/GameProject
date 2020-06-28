using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200079E RID: 1950
	public class LordToil_PrisonerEscape : LordToil_Travel
	{
		// Token: 0x17000940 RID: 2368
		// (get) Token: 0x060032C0 RID: 12992 RVA: 0x0011A040 File Offset: 0x00118240
		public override IntVec3 FlagLoc
		{
			get
			{
				return this.Data.dest;
			}
		}

		// Token: 0x17000941 RID: 2369
		// (get) Token: 0x060032C1 RID: 12993 RVA: 0x000F4FA9 File Offset: 0x000F31A9
		private LordToilData_Travel Data
		{
			get
			{
				return (LordToilData_Travel)this.data;
			}
		}

		// Token: 0x17000942 RID: 2370
		// (get) Token: 0x060032C2 RID: 12994 RVA: 0x00010306 File Offset: 0x0000E506
		public override bool AllowSatisfyLongNeeds
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000943 RID: 2371
		// (get) Token: 0x060032C3 RID: 12995 RVA: 0x0011A04D File Offset: 0x0011824D
		protected override float AllArrivedCheckRadius
		{
			get
			{
				return 14f;
			}
		}

		// Token: 0x060032C4 RID: 12996 RVA: 0x0011A054 File Offset: 0x00118254
		public LordToil_PrisonerEscape(IntVec3 dest, int sapperThingID) : base(dest)
		{
			this.sapperThingID = sapperThingID;
		}

		// Token: 0x060032C5 RID: 12997 RVA: 0x0011A064 File Offset: 0x00118264
		public override void UpdateAllDuties()
		{
			LordToilData_Travel data = this.Data;
			Pawn leader = this.GetLeader();
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				Pawn pawn = this.lord.ownedPawns[i];
				if (this.IsSapper(pawn))
				{
					pawn.mindState.duty = new PawnDuty(DutyDefOf.PrisonerEscapeSapper, data.dest, -1f);
				}
				else if (leader == null || pawn == leader)
				{
					pawn.mindState.duty = new PawnDuty(DutyDefOf.PrisonerEscape, data.dest, -1f);
				}
				else
				{
					pawn.mindState.duty = new PawnDuty(DutyDefOf.PrisonerEscape, leader, 10f);
				}
			}
		}

		// Token: 0x060032C6 RID: 12998 RVA: 0x0011A130 File Offset: 0x00118330
		public override void LordToilTick()
		{
			base.LordToilTick();
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				this.lord.ownedPawns[i].guilt.Notify_Guilty();
			}
		}

		// Token: 0x060032C7 RID: 12999 RVA: 0x0011A17C File Offset: 0x0011837C
		private Pawn GetLeader()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				if (!this.lord.ownedPawns[i].Downed && this.IsSapper(this.lord.ownedPawns[i]))
				{
					return this.lord.ownedPawns[i];
				}
			}
			for (int j = 0; j < this.lord.ownedPawns.Count; j++)
			{
				if (!this.lord.ownedPawns[j].Downed)
				{
					return this.lord.ownedPawns[j];
				}
			}
			return null;
		}

		// Token: 0x060032C8 RID: 13000 RVA: 0x0011A22D File Offset: 0x0011842D
		private bool IsSapper(Pawn p)
		{
			return p.thingIDNumber == this.sapperThingID;
		}

		// Token: 0x04001B61 RID: 7009
		private int sapperThingID;
	}
}
