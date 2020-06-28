using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000550 RID: 1360
	public class MentalState_BingingDrug : MentalState_Binging
	{
		// Token: 0x17000797 RID: 1943
		// (get) Token: 0x060026DC RID: 9948 RVA: 0x000E44EA File Offset: 0x000E26EA
		public override string InspectLine
		{
			get
			{
				return string.Format(base.InspectLine, this.chemical.label);
			}
		}

		// Token: 0x060026DD RID: 9949 RVA: 0x000E4502 File Offset: 0x000E2702
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ChemicalDef>(ref this.chemical, "chemical");
			Scribe_Values.Look<DrugCategory>(ref this.drugCategory, "drugCategory", DrugCategory.None, false);
		}

		// Token: 0x060026DE RID: 9950 RVA: 0x000E452C File Offset: 0x000E272C
		public override void PostStart(string reason)
		{
			base.PostStart(reason);
			this.ChooseRandomChemical();
			if (PawnUtility.ShouldSendNotificationAbout(this.pawn))
			{
				string str = "LetterLabelDrugBinge".Translate(this.chemical.label).CapitalizeFirst() + ": " + this.pawn.LabelShortCap;
				string text = "LetterDrugBinge".Translate(this.pawn.Label, this.chemical.label, this.pawn).CapitalizeFirst();
				if (!reason.NullOrEmpty())
				{
					text = text + "\n\n" + reason;
				}
				Find.LetterStack.ReceiveLetter(str, text, LetterDefOf.ThreatSmall, this.pawn, null, null, null, null);
			}
		}

		// Token: 0x060026DF RID: 9951 RVA: 0x000E461C File Offset: 0x000E281C
		public override void PostEnd()
		{
			base.PostEnd();
			if (PawnUtility.ShouldSendNotificationAbout(this.pawn))
			{
				Messages.Message("MessageNoLongerBingingOnDrug".Translate(this.pawn.LabelShort, this.chemical.label, this.pawn), this.pawn, MessageTypeDefOf.SituationResolved, true);
			}
		}

		// Token: 0x060026E0 RID: 9952 RVA: 0x000E468C File Offset: 0x000E288C
		private void ChooseRandomChemical()
		{
			MentalState_BingingDrug.addictions.Clear();
			List<Hediff> hediffs = this.pawn.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				Hediff_Addiction hediff_Addiction = hediffs[i] as Hediff_Addiction;
				if (hediff_Addiction != null && AddictionUtility.CanBingeOnNow(this.pawn, hediff_Addiction.Chemical, DrugCategory.Any))
				{
					MentalState_BingingDrug.addictions.Add(hediff_Addiction.Chemical);
				}
			}
			if (MentalState_BingingDrug.addictions.Count > 0)
			{
				this.chemical = MentalState_BingingDrug.addictions.RandomElement<ChemicalDef>();
				this.drugCategory = DrugCategory.Any;
				MentalState_BingingDrug.addictions.Clear();
				return;
			}
			this.chemical = (from x in DefDatabase<ChemicalDef>.AllDefsListForReading
			where AddictionUtility.CanBingeOnNow(this.pawn, x, this.def.drugCategory)
			select x).RandomElementWithFallback(null);
			if (this.chemical != null)
			{
				this.drugCategory = this.def.drugCategory;
				return;
			}
			this.chemical = (from x in DefDatabase<ChemicalDef>.AllDefsListForReading
			where AddictionUtility.CanBingeOnNow(this.pawn, x, DrugCategory.Any)
			select x).RandomElementWithFallback(null);
			if (this.chemical != null)
			{
				this.drugCategory = DrugCategory.Any;
				return;
			}
			this.chemical = DefDatabase<ChemicalDef>.AllDefsListForReading.RandomElement<ChemicalDef>();
			this.drugCategory = DrugCategory.Any;
		}

		// Token: 0x0400174B RID: 5963
		public ChemicalDef chemical;

		// Token: 0x0400174C RID: 5964
		public DrugCategory drugCategory;

		// Token: 0x0400174D RID: 5965
		private static List<ChemicalDef> addictions = new List<ChemicalDef>();
	}
}
