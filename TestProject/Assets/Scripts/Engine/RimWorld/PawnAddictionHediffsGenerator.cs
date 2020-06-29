using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	
	public static class PawnAddictionHediffsGenerator
	{
		
		public static void GenerateAddictionsAndTolerancesFor(Pawn pawn)
		{
			if (!pawn.RaceProps.IsFlesh || !pawn.RaceProps.Humanlike)
			{
				return;
			}
			if (pawn.IsTeetotaler())
			{
				return;
			}
			PawnAddictionHediffsGenerator.allDrugs.Clear();
			int num = 0;
			while (num < 3 && Rand.Value < pawn.kindDef.chemicalAddictionChance)
			{
				if (!PawnAddictionHediffsGenerator.allDrugs.Any<ThingDef>())
				{
					PawnAddictionHediffsGenerator.allDrugs.AddRange(from x in DefDatabase<ThingDef>.AllDefsListForReading
					where x.category == ThingCategory.Item && x.GetCompProperties<CompProperties_Drug>() != null
					select x);
				}
				IEnumerable<ChemicalDef> allDefsListForReading = DefDatabase<ChemicalDef>.AllDefsListForReading;
				Func<ChemicalDef, bool> predicate = ((ChemicalDef x) => PawnAddictionHediffsGenerator.PossibleWithTechLevel(x, pawn.Faction) && !AddictionUtility.IsAddicted(pawn, x));

				ChemicalDef chemicalDef;
				if (!allDefsListForReading.Where(predicate).TryRandomElement(out chemicalDef))
				{
					break;
				}
				Hediff hediff = HediffMaker.MakeHediff(chemicalDef.addictionHediff, pawn, null);
				hediff.Severity = PawnAddictionHediffsGenerator.GeneratedAddictionSeverityRange.RandomInRange;
				pawn.health.AddHediff(hediff, null, null, null);
				if (chemicalDef.toleranceHediff != null && Rand.Value < chemicalDef.onGeneratedAddictedToleranceChance)
				{
					Hediff hediff2 = HediffMaker.MakeHediff(chemicalDef.toleranceHediff, pawn, null);
					hediff2.Severity = PawnAddictionHediffsGenerator.GeneratedToleranceSeverityRange.RandomInRange;
					pawn.health.AddHediff(hediff2, null, null, null);
				}
				if (chemicalDef.onGeneratedAddictedEvents != null)
				{
					foreach (HediffGiver_Event hediffGiver_Event in chemicalDef.onGeneratedAddictedEvents)
					{
						hediffGiver_Event.EventOccurred(pawn);
					}
				}
				PawnAddictionHediffsGenerator.DoIngestionOutcomeDoers(pawn, chemicalDef);
				num++;
			}
		}

		
		private static bool PossibleWithTechLevel(ChemicalDef chemical, Faction faction)
		{
			return faction == null || PawnAddictionHediffsGenerator.allDrugs.Any((ThingDef x) => x.GetCompProperties<CompProperties_Drug>().chemical == chemical && x.techLevel <= faction.def.techLevel);
		}

		
		private static void DoIngestionOutcomeDoers(Pawn pawn, ChemicalDef chemical)
		{
			for (int i = 0; i < PawnAddictionHediffsGenerator.allDrugs.Count; i++)
			{
				if (PawnAddictionHediffsGenerator.allDrugs[i].GetCompProperties<CompProperties_Drug>().chemical == chemical)
				{
					List<IngestionOutcomeDoer> outcomeDoers = PawnAddictionHediffsGenerator.allDrugs[i].ingestible.outcomeDoers;
					for (int j = 0; j < outcomeDoers.Count; j++)
					{
						if (outcomeDoers[j].doToGeneratedPawnIfAddicted)
						{
							outcomeDoers[j].DoIngestionOutcome(pawn, null);
						}
					}
				}
			}
		}

		
		private static List<ThingDef> allDrugs = new List<ThingDef>();

		
		private const int MaxAddictions = 3;

		
		private static readonly FloatRange GeneratedAddictionSeverityRange = new FloatRange(0.6f, 1f);

		
		private static readonly FloatRange GeneratedToleranceSeverityRange = new FloatRange(0.1f, 0.9f);
	}
}
