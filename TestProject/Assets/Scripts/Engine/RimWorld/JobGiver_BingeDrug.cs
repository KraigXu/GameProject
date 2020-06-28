using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020006A4 RID: 1700
	public class JobGiver_BingeDrug : JobGiver_Binge
	{
		// Token: 0x06002E0E RID: 11790 RVA: 0x00103298 File Offset: 0x00101498
		protected override int IngestInterval(Pawn pawn)
		{
			ChemicalDef chemical = this.GetChemical(pawn);
			int num = 600;
			if (chemical == ChemicalDefOf.Alcohol)
			{
				Hediff firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.AlcoholHigh, false);
				if (firstHediffOfDef != null)
				{
					num = (int)((float)num * JobGiver_BingeDrug.IngestIntervalFactorCurve_Drunkness.Evaluate(firstHediffOfDef.Severity));
				}
			}
			else
			{
				Hediff firstHediffOfDef2 = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.DrugOverdose, false);
				if (firstHediffOfDef2 != null)
				{
					num = (int)((float)num * JobGiver_BingeDrug.IngestIntervalFactorCurve_DrugOverdose.Evaluate(firstHediffOfDef2.Severity));
				}
			}
			return num;
		}

		// Token: 0x06002E0F RID: 11791 RVA: 0x0010331C File Offset: 0x0010151C
		protected override Thing BestIngestTarget(Pawn pawn)
		{
			ChemicalDef chemical = this.GetChemical(pawn);
			DrugCategory drugCategory = this.GetDrugCategory(pawn);
			if (chemical == null)
			{
				Log.ErrorOnce("Tried to binge on null chemical.", 1393746152, false);
				return null;
			}
			Hediff overdose = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.DrugOverdose, false);
			Predicate<Thing> validator = delegate(Thing t)
			{
				if (!this.IgnoreForbid(pawn) && t.IsForbidden(pawn))
				{
					return false;
				}
				if (!pawn.CanReserve(t, 1, -1, null, false))
				{
					return false;
				}
				CompDrug compDrug = t.TryGetComp<CompDrug>();
				return compDrug.Props.chemical == chemical && (overdose == null || !compDrug.Props.CanCauseOverdose || overdose.Severity + compDrug.Props.overdoseSeverityOffset.max < 0.786f) && (pawn.Position.InHorDistOf(t.Position, 60f) || t.Position.Roofed(t.Map) || pawn.Map.areaManager.Home[t.Position] || t.GetSlotGroup() != null) && t.def.ingestible.drugCategory.IncludedIn(drugCategory);
			};
			return GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForGroup(ThingRequestGroup.Drug), PathEndMode.OnCell, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false), 9999f, validator, null, 0, -1, false, RegionType.Set_Passable, false);
		}

		// Token: 0x06002E10 RID: 11792 RVA: 0x001033E6 File Offset: 0x001015E6
		private ChemicalDef GetChemical(Pawn pawn)
		{
			return ((MentalState_BingingDrug)pawn.MentalState).chemical;
		}

		// Token: 0x06002E11 RID: 11793 RVA: 0x001033F8 File Offset: 0x001015F8
		private DrugCategory GetDrugCategory(Pawn pawn)
		{
			return ((MentalState_BingingDrug)pawn.MentalState).drugCategory;
		}

		// Token: 0x04001A54 RID: 6740
		private const int BaseIngestInterval = 600;

		// Token: 0x04001A55 RID: 6741
		private const float OverdoseSeverityToAvoid = 0.786f;

		// Token: 0x04001A56 RID: 6742
		private static readonly SimpleCurve IngestIntervalFactorCurve_Drunkness = new SimpleCurve
		{
			{
				new CurvePoint(0f, 1f),
				true
			},
			{
				new CurvePoint(1f, 4f),
				true
			}
		};

		// Token: 0x04001A57 RID: 6743
		private static readonly SimpleCurve IngestIntervalFactorCurve_DrugOverdose = new SimpleCurve
		{
			{
				new CurvePoint(0f, 1f),
				true
			},
			{
				new CurvePoint(1f, 5f),
				true
			}
		};
	}
}
