using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	
	public class LordJob_Joinable_Concert : LordJob_Joinable_Party
	{
		
		// (get) Token: 0x06003204 RID: 12804 RVA: 0x00116E84 File Offset: 0x00115084
		protected override ThoughtDef AttendeeThought
		{
			get
			{
				return ThoughtDefOf.AttendedConcert;
			}
		}

		
		// (get) Token: 0x06003205 RID: 12805 RVA: 0x00116E8B File Offset: 0x0011508B
		protected override TaleDef AttendeeTale
		{
			get
			{
				return TaleDefOf.AttendedConcert;
			}
		}

		
		// (get) Token: 0x06003206 RID: 12806 RVA: 0x00116E92 File Offset: 0x00115092
		protected override ThoughtDef OrganizerThought
		{
			get
			{
				return ThoughtDefOf.HeldConcert;
			}
		}

		
		// (get) Token: 0x06003207 RID: 12807 RVA: 0x00116E99 File Offset: 0x00115099
		protected override TaleDef OrganizerTale
		{
			get
			{
				return TaleDefOf.HeldConcert;
			}
		}

		
		public LordJob_Joinable_Concert()
		{
		}

		
		public LordJob_Joinable_Concert(IntVec3 spot, Pawn organizer, GatheringDef gatheringDef) : base(spot, organizer, gatheringDef)
		{
		}

		
		public override string GetReport(Pawn pawn)
		{
			if (pawn != this.organizer)
			{
				return "LordReportAttendingConcert".Translate();
			}
			return "LordReportHoldingConcert".Translate();
		}

		
		protected override LordToil CreateGatheringToil(IntVec3 spot, Pawn organizer, GatheringDef gatheringDef)
		{
			return new LordToil_Concert(spot, organizer, gatheringDef, 3.5E-05f);
		}

		
		protected override Trigger_TicksPassed GetTimeoutTrigger()
		{
			return new Trigger_TicksPassedAfterConditionMet(base.DurationTicks, () => GatheringsUtility.InGatheringArea(this.organizer.Position, this.spot, this.organizer.Map), 60);
		}
	}
}
