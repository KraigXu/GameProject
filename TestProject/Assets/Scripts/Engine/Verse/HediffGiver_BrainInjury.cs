using System;
using RimWorld;

namespace Verse
{
	
	public class HediffGiver_BrainInjury : HediffGiver
	{
		
		public override bool OnHediffAdded(Pawn pawn, Hediff hediff)
		{
			if (!(hediff is Hediff_Injury))
			{
				return false;
			}
			if (hediff.Part != pawn.health.hediffSet.GetBrain())
			{
				return false;
			}
			float num = hediff.Severity / hediff.Part.def.GetMaxHealth(pawn);
			if (Rand.Value < num * this.chancePerDamagePct && base.TryApply(pawn, null))
			{
				if ((pawn.Faction == Faction.OfPlayer || pawn.IsPrisonerOfColony) && !this.letter.NullOrEmpty())
				{
					Find.LetterStack.ReceiveLetter(this.letterLabel, this.letter.Formatted(pawn.Named("PAWN")).AdjustedFor(pawn, "PAWN", true), LetterDefOf.NegativeEvent, pawn, null, null, null, null);
				}
				return true;
			}
			return false;
		}

		
		public float chancePerDamagePct;

		
		public string letterLabel;

		
		public string letter;
	}
}
