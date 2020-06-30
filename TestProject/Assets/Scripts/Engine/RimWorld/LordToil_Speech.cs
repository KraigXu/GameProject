using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	
	public class LordToil_Speech : LordToil_Gathering
	{
		
		
		public LordToilData_Speech Data
		{
			get
			{
				return (LordToilData_Speech)this.data;
			}
		}

		
		public LordToil_Speech(IntVec3 spot, GatheringDef gatheringDef, Pawn organizer) : base(spot, gatheringDef)
		{
			this.organizer = organizer;
			this.data = new LordToilData_Speech();
		}

		
		public override void Init()
		{
			base.Init();
			//this.Data.spectateRect = CellRect.CenteredOn(this.spot, 0);
			//Rot4 rotation = this.spot.GetFirstThing(this.organizer.MapHeld).Rotation;
			//SpectateRectSide asSpectateSide = rotation.Opposite.AsSpectateSide;
			//this.Data.spectateRectAllowedSides = (SpectateRectSide.All & ~asSpectateSide);
			//this.Data.spectateRectPreferredSide = rotation.AsSpectateSide;
		}

		
		public override ThinkTreeDutyHook VoluntaryJoinDutyHookFor(Pawn p)
		{
			if (p == this.organizer)
			{
				return DutyDefOf.GiveSpeech.hook;
			}
			return DutyDefOf.Spectate.hook;
		}

		
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				Pawn pawn = this.lord.ownedPawns[i];
				if (pawn == this.organizer)
				{
					Building_Throne firstThing = this.spot.GetFirstThing<Building_Throne>(base.Map);
					pawn.mindState.duty = new PawnDuty(DutyDefOf.GiveSpeech, this.spot, firstThing, -1f);
					pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
				}
				else
				{
					PawnDuty pawnDuty = new PawnDuty(DutyDefOf.Spectate);
					pawnDuty.spectateRect = this.Data.spectateRect;
					pawnDuty.spectateRectAllowedSides = this.Data.spectateRectAllowedSides;
					pawnDuty.spectateRectPreferredSide = this.Data.spectateRectPreferredSide;
					pawn.mindState.duty = pawnDuty;
				}
			}
		}

		
		public Pawn organizer;
	}
}
