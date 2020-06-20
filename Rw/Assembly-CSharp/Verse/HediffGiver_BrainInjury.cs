using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000282 RID: 642
	public class HediffGiver_BrainInjury : HediffGiver
	{
		// Token: 0x0600112F RID: 4399 RVA: 0x00060E20 File Offset: 0x0005F020
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

		// Token: 0x04000C59 RID: 3161
		public float chancePerDamagePct;

		// Token: 0x04000C5A RID: 3162
		public string letterLabel;

		// Token: 0x04000C5B RID: 3163
		public string letter;
	}
}
