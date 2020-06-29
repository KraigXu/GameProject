using System;
using Verse;

namespace RimWorld
{
	
	public class CompDrug : ThingComp
	{
		
		
		public CompProperties_Drug Props
		{
			get
			{
				return (CompProperties_Drug)this.props;
			}
		}

		
		public override void PrePostIngested(Pawn ingester)
		{
			if (this.Props.Addictive && ingester.RaceProps.IsFlesh)
			{
				HediffDef addictionHediffDef = this.Props.chemical.addictionHediff;
				Hediff_Addiction hediff_Addiction = AddictionUtility.FindAddictionHediff(ingester, this.Props.chemical);
				Hediff hediff = AddictionUtility.FindToleranceHediff(ingester, this.Props.chemical);
				float num = (hediff != null) ? hediff.Severity : 0f;
				if (hediff_Addiction != null)
				{
					hediff_Addiction.Severity += this.Props.existingAddictionSeverityOffset;
				}
				else if (Rand.Value < this.Props.addictiveness && num >= this.Props.minToleranceToAddict)
				{
					ingester.health.AddHediff(this.Props.chemical.addictionHediff, null, null, null);
					if (PawnUtility.ShouldSendNotificationAbout(ingester))
					{
						Find.LetterStack.ReceiveLetter("LetterLabelNewlyAddicted".Translate(this.Props.chemical.label).CapitalizeFirst(), "LetterNewlyAddicted".Translate(ingester.LabelShort, this.Props.chemical.label, ingester.Named("PAWN")).AdjustedFor(ingester, "PAWN", true).CapitalizeFirst(), LetterDefOf.NegativeEvent, ingester, null, null, null, null);
					}
					AddictionUtility.CheckDrugAddictionTeachOpportunity(ingester);
				}
				if (addictionHediffDef.causesNeed != null)
				{
					Need need = ingester.needs.AllNeeds.Find((Need x) => x.def == addictionHediffDef.causesNeed);
					if (need != null)
					{
						float needLevelOffset = this.Props.needLevelOffset;
						AddictionUtility.ModifyChemicalEffectForToleranceAndBodySize(ingester, this.Props.chemical, ref needLevelOffset);
						need.CurLevel += needLevelOffset;
					}
				}
			}
		}

		
		public override void PostIngested(Pawn ingester)
		{
			if (this.Props.Addictive && ingester.RaceProps.IsFlesh)
			{
				Hediff firstHediffOfDef = ingester.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.DrugOverdose, false);
				float num = (firstHediffOfDef != null) ? firstHediffOfDef.Severity : 0f;
				if (num < 0.9f && Rand.Value < this.Props.largeOverdoseChance)
				{
					float num2 = Rand.Range(0.85f, 0.99f);
					HealthUtility.AdjustSeverity(ingester, HediffDefOf.DrugOverdose, num2 - num);
					if (ingester.Faction == Faction.OfPlayer)
					{
						Messages.Message("MessageAccidentalOverdose".Translate(ingester.Named("INGESTER"), this.parent.LabelNoCount, this.parent.Named("DRUG")), ingester, MessageTypeDefOf.NegativeHealthEvent, true);
					}
				}
				else
				{
					float num3 = this.Props.overdoseSeverityOffset.RandomInRange / ingester.BodySize;
					if (num3 > 0f)
					{
						HealthUtility.AdjustSeverity(ingester, HediffDefOf.DrugOverdose, num3);
					}
				}
			}
			if (this.Props.isCombatEnhancingDrug && !ingester.Dead)
			{
				ingester.mindState.lastTakeCombatEnhancingDrugTick = Find.TickManager.TicksGame;
			}
			if (ingester.drugs != null)
			{
				ingester.drugs.Notify_DrugIngested(this.parent);
			}
		}
	}
}
