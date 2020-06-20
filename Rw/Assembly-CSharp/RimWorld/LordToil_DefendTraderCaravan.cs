using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000793 RID: 1939
	internal class LordToil_DefendTraderCaravan : LordToil_DefendPoint
	{
		// Token: 0x17000931 RID: 2353
		// (get) Token: 0x0600328D RID: 12941 RVA: 0x00010306 File Offset: 0x0000E506
		public override bool AllowSatisfyLongNeeds
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000932 RID: 2354
		// (get) Token: 0x0600328E RID: 12942 RVA: 0x0011854D File Offset: 0x0011674D
		public override float? CustomWakeThreshold
		{
			get
			{
				return new float?(0.5f);
			}
		}

		// Token: 0x0600328F RID: 12943 RVA: 0x00119309 File Offset: 0x00117509
		public LordToil_DefendTraderCaravan() : base(true)
		{
		}

		// Token: 0x06003290 RID: 12944 RVA: 0x00119312 File Offset: 0x00117512
		public LordToil_DefendTraderCaravan(IntVec3 defendPoint) : base(defendPoint, 28f)
		{
		}

		// Token: 0x06003291 RID: 12945 RVA: 0x00119320 File Offset: 0x00117520
		public override void UpdateAllDuties()
		{
			LordToilData_DefendPoint data = base.Data;
			Pawn pawn = TraderCaravanUtility.FindTrader(this.lord);
			if (pawn != null)
			{
				pawn.mindState.duty = new PawnDuty(DutyDefOf.Defend, data.defendPoint, data.defendRadius);
				for (int i = 0; i < this.lord.ownedPawns.Count; i++)
				{
					Pawn pawn2 = this.lord.ownedPawns[i];
					switch (pawn2.GetTraderCaravanRole())
					{
					case TraderCaravanRole.Carrier:
						pawn2.mindState.duty = new PawnDuty(DutyDefOf.Follow, pawn, 5f);
						pawn2.mindState.duty.locomotion = LocomotionUrgency.Walk;
						break;
					case TraderCaravanRole.Guard:
						pawn2.mindState.duty = new PawnDuty(DutyDefOf.Defend, data.defendPoint, data.defendRadius);
						break;
					case TraderCaravanRole.Chattel:
						pawn2.mindState.duty = new PawnDuty(DutyDefOf.Escort, pawn, 5f);
						pawn2.mindState.duty.locomotion = LocomotionUrgency.Walk;
						break;
					}
				}
				return;
			}
		}
	}
}
