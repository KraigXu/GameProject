using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	
	public abstract class GatheringWorker
	{
		
		public virtual bool CanExecute(Map map, Pawn organizer = null)
		{
			if (organizer == null)
			{
				organizer = this.FindOrganizer(map);
			}
			IntVec3 intVec;
			return organizer != null && this.TryFindGatherSpot(organizer, out intVec) && GatheringsUtility.PawnCanStartOrContinueGathering(organizer);
		}

		
		public virtual bool TryExecute(Map map, Pawn organizer = null)
		{
			if (organizer == null)
			{
				organizer = this.FindOrganizer(map);
			}
			if (organizer == null)
			{
				return false;
			}
			IntVec3 spot;
			if (!this.TryFindGatherSpot(organizer, out spot))
			{
				return false;
			}
			LordJob lordJob = this.CreateLordJob(spot, organizer);
			Faction faction = organizer.Faction;
			LordJob lordJob2 = lordJob;
			Map map2 = organizer.Map;
			//object startingPawns;
			//if (!lordJob.OrganizerIsStartingPawn)
			//{
			//	startingPawns = null;
			//}
			//else
			//{
			//	(startingPawns = new Pawn[1])[0] = organizer;
			//}
			//LordMaker.MakeNewLord(faction, lordJob2, map2, startingPawns);
			this.SendLetter(spot, organizer);
			return true;
		}

		
		protected virtual void SendLetter(IntVec3 spot, Pawn organizer)
		{
			Find.LetterStack.ReceiveLetter(this.def.letterTitle, this.def.letterText.Formatted(organizer.Named("ORGANIZER")), LetterDefOf.PositiveEvent, new TargetInfo(spot, organizer.Map, false), null, null, null, null);
		}

		
		protected virtual Pawn FindOrganizer(Map map)
		{
			return GatheringsUtility.FindRandomGatheringOrganizer(Faction.OfPlayer, map, this.def);
		}

		
		protected abstract bool TryFindGatherSpot(Pawn organizer, out IntVec3 spot);

		
		protected abstract LordJob CreateLordJob(IntVec3 spot, Pawn organizer);

		
		public GatheringDef def;
	}
}
