using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	
	public class LordJob_Joinable_Concert : LordJob_Joinable_Party
	{
		
		
		protected override ThoughtDef AttendeeThought
		{
			get
			{
				return ThoughtDefOf.AttendedConcert;
			}
		}

		
		
		protected override TaleDef AttendeeTale
		{
			get
			{
				return TaleDefOf.AttendedConcert;
			}
		}

		
		
		protected override ThoughtDef OrganizerThought
		{
			get
			{
				return ThoughtDefOf.HeldConcert;
			}
		}

		
		
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
