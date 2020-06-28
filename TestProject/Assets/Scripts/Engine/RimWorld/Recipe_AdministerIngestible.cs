using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000AF2 RID: 2802
	public class Recipe_AdministerIngestible : Recipe_Surgery
	{
		// Token: 0x06004238 RID: 16952 RVA: 0x00161AB8 File Offset: 0x0015FCB8
		public override void ApplyOnPawn(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
		{
			float num = ingredients[0].Ingested(pawn, (pawn.needs != null && pawn.needs.food != null) ? pawn.needs.food.NutritionWanted : 0f);
			if (!pawn.Dead)
			{
				pawn.needs.food.CurLevel += num;
			}
			if (pawn.needs.mood != null)
			{
				if (pawn.IsTeetotaler() && ingredients[0].def.IsNonMedicalDrug)
				{
					pawn.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.ForcedMeToTakeDrugs, billDoer);
					return;
				}
				if (pawn.IsProsthophobe() && ingredients[0].def == ThingDefOf.Luciferium)
				{
					pawn.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.ForcedMeToTakeLuciferium, billDoer);
				}
			}
		}

		// Token: 0x06004239 RID: 16953 RVA: 0x00002681 File Offset: 0x00000881
		public override void ConsumeIngredient(Thing ingredient, RecipeDef recipe, Map map)
		{
		}

		// Token: 0x0600423A RID: 16954 RVA: 0x00161BA8 File Offset: 0x0015FDA8
		public override bool IsViolationOnPawn(Pawn pawn, BodyPartRecord part, Faction billDoerFaction)
		{
			if (pawn.Faction == billDoerFaction)
			{
				return false;
			}
			ThingDef thingDef = this.recipe.ingredients[0].filter.AllowedThingDefs.First<ThingDef>();
			if (thingDef.IsNonMedicalDrug)
			{
				foreach (CompProperties compProperties in thingDef.comps)
				{
					CompProperties_Drug compProperties_Drug = compProperties as CompProperties_Drug;
					if (compProperties_Drug != null && compProperties_Drug.chemical != null && compProperties_Drug.chemical.addictionHediff != null && pawn.health.hediffSet.HasHediff(compProperties_Drug.chemical.addictionHediff, false))
					{
						return false;
					}
				}
			}
			return thingDef.IsNonMedicalDrug;
		}

		// Token: 0x0600423B RID: 16955 RVA: 0x00161C70 File Offset: 0x0015FE70
		public override string GetLabelWhenUsedOn(Pawn pawn, BodyPartRecord part)
		{
			if (pawn.IsTeetotaler() && this.recipe.ingredients[0].filter.BestThingRequest.singleDef.IsNonMedicalDrug)
			{
				return base.GetLabelWhenUsedOn(pawn, part) + " (" + "TeetotalerUnhappy".Translate() + ")";
			}
			if (pawn.IsProsthophobe() && this.recipe.ingredients[0].filter.BestThingRequest.singleDef == ThingDefOf.Luciferium)
			{
				return base.GetLabelWhenUsedOn(pawn, part) + " (" + "ProsthophobeUnhappy".Translate() + ")";
			}
			return base.GetLabelWhenUsedOn(pawn, part);
		}
	}
}
