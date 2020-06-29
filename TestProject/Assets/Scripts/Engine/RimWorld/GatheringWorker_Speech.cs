using System;
using System.Linq;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	
	public class GatheringWorker_Speech : GatheringWorker
	{
		
		protected override LordJob CreateLordJob(IntVec3 spot, Pawn organizer)
		{
			return new LordJob_Joinable_Speech(spot, organizer, this.def);
		}

		
		public override bool CanExecute(Map map, Pawn organizer = null)
		{
			IntVec3 intVec;
			return organizer != null && this.TryFindGatherSpot(organizer, out intVec);
		}

		
		protected override bool TryFindGatherSpot(Pawn organizer, out IntVec3 spot)
		{
			Building_Throne building_Throne = RoyalTitleUtility.FindBestUsableThrone(organizer);
			if (building_Throne != null)
			{
				spot = building_Throne.InteractionCell;
				return true;
			}
			spot = IntVec3.Invalid;
			return false;
		}

		
		protected override void SendLetter(IntVec3 spot, Pawn organizer)
		{
			Find.LetterStack.ReceiveLetter(this.def.letterTitle, this.def.letterText.Formatted(organizer.Named("ORGANIZER")) + "\n\n" + GatheringWorker_Speech.OutcomeBreakdownForPawn(organizer), LetterDefOf.PositiveEvent, new TargetInfo(spot, organizer.Map, false), null, null, null, null);
		}

		
		public static string OutcomeBreakdownForPawn(Pawn organizer)
		{
			return "AbilitySpeechStatInfo".Translate(organizer.Named("ORGANIZER"), StatDefOf.SocialImpact.label) + ": " + organizer.GetStatValue(StatDefOf.SocialImpact, true).ToStringPercent() + "\n\n" + "AbilitySpeechPossibleOutcomes".Translate() + ":\n" + (from o in LordJob_Joinable_Speech.OutcomeChancesForPawn(organizer).Reverse<Tuple<ThoughtDef, float>>()
			select o.Item1.stages[0].LabelCap + " " + o.Item2.ToStringPercent()).ToLineList("  - ", false);
		}
	}
}
