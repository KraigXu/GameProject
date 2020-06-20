using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200077C RID: 1916
	public class LordJob_Joinable_Concert : LordJob_Joinable_Party
	{
		// Token: 0x1700090D RID: 2317
		// (get) Token: 0x06003204 RID: 12804 RVA: 0x00116E84 File Offset: 0x00115084
		protected override ThoughtDef AttendeeThought
		{
			get
			{
				return ThoughtDefOf.AttendedConcert;
			}
		}

		// Token: 0x1700090E RID: 2318
		// (get) Token: 0x06003205 RID: 12805 RVA: 0x00116E8B File Offset: 0x0011508B
		protected override TaleDef AttendeeTale
		{
			get
			{
				return TaleDefOf.AttendedConcert;
			}
		}

		// Token: 0x1700090F RID: 2319
		// (get) Token: 0x06003206 RID: 12806 RVA: 0x00116E92 File Offset: 0x00115092
		protected override ThoughtDef OrganizerThought
		{
			get
			{
				return ThoughtDefOf.HeldConcert;
			}
		}

		// Token: 0x17000910 RID: 2320
		// (get) Token: 0x06003207 RID: 12807 RVA: 0x00116E99 File Offset: 0x00115099
		protected override TaleDef OrganizerTale
		{
			get
			{
				return TaleDefOf.HeldConcert;
			}
		}

		// Token: 0x06003208 RID: 12808 RVA: 0x00116EA0 File Offset: 0x001150A0
		public LordJob_Joinable_Concert()
		{
		}

		// Token: 0x06003209 RID: 12809 RVA: 0x00116EA8 File Offset: 0x001150A8
		public LordJob_Joinable_Concert(IntVec3 spot, Pawn organizer, GatheringDef gatheringDef) : base(spot, organizer, gatheringDef)
		{
		}

		// Token: 0x0600320A RID: 12810 RVA: 0x00116EB3 File Offset: 0x001150B3
		public override string GetReport(Pawn pawn)
		{
			if (pawn != this.organizer)
			{
				return "LordReportAttendingConcert".Translate();
			}
			return "LordReportHoldingConcert".Translate();
		}

		// Token: 0x0600320B RID: 12811 RVA: 0x00116EDD File Offset: 0x001150DD
		protected override LordToil CreateGatheringToil(IntVec3 spot, Pawn organizer, GatheringDef gatheringDef)
		{
			return new LordToil_Concert(spot, organizer, gatheringDef, 3.5E-05f);
		}

		// Token: 0x0600320C RID: 12812 RVA: 0x00116EEC File Offset: 0x001150EC
		protected override Trigger_TicksPassed GetTimeoutTrigger()
		{
			return new Trigger_TicksPassedAfterConditionMet(base.DurationTicks, () => GatheringsUtility.InGatheringArea(this.organizer.Position, this.spot, this.organizer.Map), 60);
		}
	}
}
