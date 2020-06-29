using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	
	public abstract class LordJob_Joinable_Gathering : LordJob_VoluntarilyJoinable
	{
		
		
		public Pawn Organizer
		{
			get
			{
				return this.organizer;
			}
		}

		
		public LordJob_Joinable_Gathering()
		{
		}

		
		public LordJob_Joinable_Gathering(IntVec3 spot, Pawn organizer, GatheringDef gatheringDef)
		{
			this.spot = spot;
			this.organizer = organizer;
			this.gatheringDef = gatheringDef;
		}

		
		protected abstract LordToil CreateGatheringToil(IntVec3 spot, Pawn organizer, GatheringDef gatheringDef);

		
		protected virtual bool ShouldBeCalledOff()
		{
			return !GatheringsUtility.PawnCanStartOrContinueGathering(this.organizer) || !GatheringsUtility.AcceptableGameConditionsToContinueGathering(base.Map);
		}

		
		public override float VoluntaryJoinPriorityFor(Pawn p)
		{
			if (!this.IsInvited(p))
			{
				return 0f;
			}
			if (!GatheringsUtility.ShouldPawnKeepGathering(p, this.gatheringDef))
			{
				return 0f;
			}
			if (this.spot.IsForbidden(p))
			{
				return 0f;
			}
			if (!this.lord.ownedPawns.Contains(p) && this.IsGatheringAboutToEnd())
			{
				return 0f;
			}
			return VoluntarilyJoinableLordJobJoinPriorities.SocialGathering;
		}

		
		public override void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.spot, "spot", default(IntVec3), false);
			Scribe_References.Look<Pawn>(ref this.organizer, "organizer", false);
			Scribe_Defs.Look<GatheringDef>(ref this.gatheringDef, "gatheringDef");
		}

		
		private bool IsGatheringAboutToEnd()
		{
			return this.timeoutTrigger.TicksLeft < 1200;
		}

		
		private bool IsInvited(Pawn p)
		{
			return this.lord.faction != null && p.Faction == this.lord.faction;
		}

		
		protected IntVec3 spot;

		
		protected Pawn organizer;

		
		protected GatheringDef gatheringDef;

		
		protected Trigger_TicksPassed timeoutTrigger;
	}
}
