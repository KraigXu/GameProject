using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020006A6 RID: 1702
	public class JobGiver_SatisfyChemicalNeed : ThinkNode_JobGiver
	{
		// Token: 0x06002E17 RID: 11799 RVA: 0x001034BB File Offset: 0x001016BB
		public override float GetPriority(Pawn pawn)
		{
			if (pawn.needs.AllNeeds.Any((Need x) => this.ShouldSatisfy(x)))
			{
				return 9.25f;
			}
			return 0f;
		}

		// Token: 0x06002E18 RID: 11800 RVA: 0x001034E8 File Offset: 0x001016E8
		protected override Job TryGiveJob(Pawn pawn)
		{
			JobGiver_SatisfyChemicalNeed.tmpChemicalNeeds.Clear();
			List<Need> allNeeds = pawn.needs.AllNeeds;
			for (int i = 0; i < allNeeds.Count; i++)
			{
				if (this.ShouldSatisfy(allNeeds[i]))
				{
					JobGiver_SatisfyChemicalNeed.tmpChemicalNeeds.Add((Need_Chemical)allNeeds[i]);
				}
			}
			if (!JobGiver_SatisfyChemicalNeed.tmpChemicalNeeds.Any<Need_Chemical>())
			{
				return null;
			}
			JobGiver_SatisfyChemicalNeed.tmpChemicalNeeds.SortBy((Need_Chemical x) => x.CurLevel);
			for (int j = 0; j < JobGiver_SatisfyChemicalNeed.tmpChemicalNeeds.Count; j++)
			{
				Thing thing = this.FindDrugFor(pawn, JobGiver_SatisfyChemicalNeed.tmpChemicalNeeds[j]);
				if (thing != null)
				{
					JobGiver_SatisfyChemicalNeed.tmpChemicalNeeds.Clear();
					return DrugAIUtility.IngestAndTakeToInventoryJob(thing, pawn, 1);
				}
			}
			JobGiver_SatisfyChemicalNeed.tmpChemicalNeeds.Clear();
			return null;
		}

		// Token: 0x06002E19 RID: 11801 RVA: 0x001035C4 File Offset: 0x001017C4
		private bool ShouldSatisfy(Need need)
		{
			Need_Chemical need_Chemical = need as Need_Chemical;
			return need_Chemical != null && need_Chemical.CurCategory <= DrugDesireCategory.Desire;
		}

		// Token: 0x06002E1A RID: 11802 RVA: 0x001035E8 File Offset: 0x001017E8
		private Thing FindDrugFor(Pawn pawn, Need_Chemical need)
		{
			Hediff_Addiction addictionHediff = need.AddictionHediff;
			if (addictionHediff == null)
			{
				return null;
			}
			ThingOwner<Thing> innerContainer = pawn.inventory.innerContainer;
			for (int i = 0; i < innerContainer.Count; i++)
			{
				if (this.DrugValidator(pawn, addictionHediff, innerContainer[i]))
				{
					return innerContainer[i];
				}
			}
			return GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForGroup(ThingRequestGroup.Drug), PathEndMode.ClosestTouch, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false), 9999f, (Thing x) => this.DrugValidator(pawn, addictionHediff, x), null, 0, -1, false, RegionType.Set_Passable, false);
		}

		// Token: 0x06002E1B RID: 11803 RVA: 0x001036AC File Offset: 0x001018AC
		private bool DrugValidator(Pawn pawn, Hediff_Addiction addiction, Thing drug)
		{
			if (!drug.def.IsDrug)
			{
				return false;
			}
			if (drug.Spawned)
			{
				if (drug.IsForbidden(pawn))
				{
					return false;
				}
				if (!pawn.CanReserve(drug, 1, -1, null, false))
				{
					return false;
				}
				if (!drug.IsSociallyProper(pawn))
				{
					return false;
				}
			}
			CompDrug compDrug = drug.TryGetComp<CompDrug>();
			return compDrug != null && compDrug.Props.chemical != null && compDrug.Props.chemical.addictionHediff == addiction.def && (pawn.drugs == null || pawn.drugs.CurrentPolicy[drug.def].allowedForAddiction || pawn.story == null || pawn.story.traits.DegreeOfTrait(TraitDefOf.DrugDesire) > 0 || (pawn.InMentalState && pawn.MentalStateDef.ignoreDrugPolicy));
		}

		// Token: 0x04001A59 RID: 6745
		private static List<Need_Chemical> tmpChemicalNeeds = new List<Need_Chemical>();
	}
}
