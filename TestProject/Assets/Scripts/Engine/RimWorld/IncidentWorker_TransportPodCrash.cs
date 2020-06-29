using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public class IncidentWorker_TransportPodCrash : IncidentWorker
	{
		
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			List<Thing> things = ThingSetMakerDefOf.RefugeePod.root.Generate();
			IntVec3 intVec = DropCellFinder.RandomDropSpot(map);
			Pawn pawn = ThingUtility.FindPawn(things);
			pawn.guest.getRescuedThoughtOnUndownedBecauseOfPlayer = true;
			TaggedString baseLetterLabel = "LetterLabelRefugeePodCrash".Translate();
			TaggedString taggedString = "RefugeePodCrash".Translate(pawn.Named("PAWN")).AdjustedFor(pawn, "PAWN", true);
			taggedString += "\n\n";
			if (pawn.Faction == null)
			{
				taggedString += "RefugeePodCrash_Factionless".Translate(pawn.Named("PAWN")).AdjustedFor(pawn, "PAWN", true);
			}
			else if (pawn.Faction.HostileTo(Faction.OfPlayer))
			{
				taggedString += "RefugeePodCrash_Hostile".Translate(pawn.Named("PAWN")).AdjustedFor(pawn, "PAWN", true);
			}
			else
			{
				taggedString += "RefugeePodCrash_NonHostile".Translate(pawn.Named("PAWN")).AdjustedFor(pawn, "PAWN", true);
			}
			PawnRelationUtility.TryAppendRelationsWithColonistsInfo(ref taggedString, ref baseLetterLabel, pawn);
			base.SendStandardLetter(baseLetterLabel, taggedString, LetterDefOf.NeutralEvent, parms, new TargetInfo(intVec, map, false), Array.Empty<NamedArgument>());
			ActiveDropPodInfo activeDropPodInfo = new ActiveDropPodInfo();
			activeDropPodInfo.innerContainer.TryAddRangeOrTransfer(things, true, false);
			activeDropPodInfo.openDelay = 180;
			activeDropPodInfo.leaveSlag = true;
			DropPodUtility.MakeDropPodAt(intVec, map, activeDropPodInfo);
			return true;
		}
	}
}
