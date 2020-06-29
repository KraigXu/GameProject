using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	
	public abstract class HediffGiver
	{
		
		public virtual void OnIntervalPassed(Pawn pawn, Hediff cause)
		{
		}

		
		public virtual bool OnHediffAdded(Pawn pawn, Hediff hediff)
		{
			return false;
		}

		
		public bool TryApply(Pawn pawn, List<Hediff> outAddedHediffs = null)
		{
			return (!pawn.IsQuestLodger() || this.allowOnLodgers) && HediffGiverUtility.TryApply(pawn, this.hediff, this.partsToAffect, this.canAffectAnyLivePart, this.countToAffect, outAddedHediffs);
		}

		
		protected void SendLetter(Pawn pawn, Hediff cause)
		{
			if (PawnUtility.ShouldSendNotificationAbout(pawn))
			{
				if (cause == null)
				{
					Find.LetterStack.ReceiveLetter("LetterHediffFromRandomHediffGiverLabel".Translate(pawn.LabelShort, this.hediff.LabelCap, pawn.Named("PAWN")).CapitalizeFirst(), "LetterHediffFromRandomHediffGiver".Translate(pawn.LabelShort, this.hediff.LabelCap, pawn.Named("PAWN")).CapitalizeFirst(), LetterDefOf.NegativeEvent, pawn, null, null, null, null);
					return;
				}
				Find.LetterStack.ReceiveLetter("LetterHealthComplicationsLabel".Translate(pawn.LabelShort, this.hediff.LabelCap, pawn.Named("PAWN")).CapitalizeFirst(), "LetterHealthComplications".Translate(pawn.LabelShort, this.hediff.LabelCap, cause.LabelCap, pawn.Named("PAWN")).CapitalizeFirst(), LetterDefOf.NegativeEvent, pawn, null, null, null, null);
			}
		}

		
		public virtual IEnumerable<string> ConfigErrors()
		{
			if (this.hediff == null)
			{
				yield return "hediff is null";
			}
			yield break;
		}

		
		[TranslationHandle]
		public HediffDef hediff;

		
		public List<BodyPartDef> partsToAffect;

		
		public bool canAffectAnyLivePart;

		
		public bool allowOnLodgers = true;

		
		public int countToAffect = 1;
	}
}
