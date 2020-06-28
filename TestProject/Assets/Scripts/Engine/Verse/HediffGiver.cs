using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x0200027D RID: 637
	public abstract class HediffGiver
	{
		// Token: 0x0600111B RID: 4379 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void OnIntervalPassed(Pawn pawn, Hediff cause)
		{
		}

		// Token: 0x0600111C RID: 4380 RVA: 0x00010306 File Offset: 0x0000E506
		public virtual bool OnHediffAdded(Pawn pawn, Hediff hediff)
		{
			return false;
		}

		// Token: 0x0600111D RID: 4381 RVA: 0x000607C6 File Offset: 0x0005E9C6
		public bool TryApply(Pawn pawn, List<Hediff> outAddedHediffs = null)
		{
			return (!pawn.IsQuestLodger() || this.allowOnLodgers) && HediffGiverUtility.TryApply(pawn, this.hediff, this.partsToAffect, this.canAffectAnyLivePart, this.countToAffect, outAddedHediffs);
		}

		// Token: 0x0600111E RID: 4382 RVA: 0x000607FC File Offset: 0x0005E9FC
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

		// Token: 0x0600111F RID: 4383 RVA: 0x00060938 File Offset: 0x0005EB38
		public virtual IEnumerable<string> ConfigErrors()
		{
			if (this.hediff == null)
			{
				yield return "hediff is null";
			}
			yield break;
		}

		// Token: 0x04000C4E RID: 3150
		[TranslationHandle]
		public HediffDef hediff;

		// Token: 0x04000C4F RID: 3151
		public List<BodyPartDef> partsToAffect;

		// Token: 0x04000C50 RID: 3152
		public bool canAffectAnyLivePart;

		// Token: 0x04000C51 RID: 3153
		public bool allowOnLodgers = true;

		// Token: 0x04000C52 RID: 3154
		public int countToAffect = 1;
	}
}
