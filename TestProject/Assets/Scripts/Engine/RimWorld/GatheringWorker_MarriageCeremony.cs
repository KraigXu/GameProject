using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	
	public class GatheringWorker_MarriageCeremony : GatheringWorker
	{
		
		private static void FindFiancees(Pawn organizer, out Pawn firstFiance, out Pawn secondFiance)
		{
			firstFiance = organizer;
			secondFiance = organizer.relations.GetFirstDirectRelationPawn(PawnRelationDefOf.Fiance, null);
		}

		
		protected override LordJob CreateLordJob(IntVec3 spot, Pawn organizer)
		{
			Pawn firstPawn;
			Pawn secondPawn;
			GatheringWorker_MarriageCeremony.FindFiancees(organizer, out firstPawn, out secondPawn);
			return new LordJob_Joinable_MarriageCeremony(firstPawn, secondPawn, spot);
		}

		
		protected override bool TryFindGatherSpot(Pawn organizer, out IntVec3 spot)
		{
			Pawn firstFiance;
			Pawn secondFiance;
			GatheringWorker_MarriageCeremony.FindFiancees(organizer, out firstFiance, out secondFiance);
			return RCellFinder.TryFindMarriageSite(firstFiance, secondFiance, out spot);
		}

		
		protected override void SendLetter(IntVec3 spot, Pawn organizer)
		{
			Pawn pawn;
			Pawn pawn2;
			GatheringWorker_MarriageCeremony.FindFiancees(organizer, out pawn, out pawn2);
			Messages.Message("MessageNewMarriageCeremony".Translate(pawn.LabelShort, pawn2.LabelShort, pawn.Named("PAWN1"), pawn2.Named("PAWN2")), new TargetInfo(spot, pawn.Map, false), MessageTypeDefOf.PositiveEvent, true);
		}

		
		public override bool CanExecute(Map map, Pawn organizer = null)
		{
			if (organizer != null)
			{
				Pawn pawn;
				Pawn pawn2;
				GatheringWorker_MarriageCeremony.FindFiancees(organizer, out pawn, out pawn2);
				if (!GatheringsUtility.PawnCanStartOrContinueGathering(pawn) || !GatheringsUtility.PawnCanStartOrContinueGathering(pawn2))
				{
					return false;
				}
			}
			return base.CanExecute(map, organizer);
		}
	}
}
