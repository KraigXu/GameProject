using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000CDA RID: 3290
	public static class ThingSetMakerUtility
	{
		// Token: 0x06004FC0 RID: 20416 RVA: 0x001AE828 File Offset: 0x001ACA28
		public static void Reset()
		{
			ThingSetMakerUtility.allGeneratableItems.Clear();
			foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefs)
			{
				if (ThingSetMakerUtility.CanGenerate(thingDef))
				{
					ThingSetMakerUtility.allGeneratableItems.Add(thingDef);
				}
			}
			ThingSetMaker_Meteorite.Reset();
		}

		// Token: 0x06004FC1 RID: 20417 RVA: 0x001AE890 File Offset: 0x001ACA90
		public static bool CanGenerate(ThingDef thingDef)
		{
			return (thingDef.category == ThingCategory.Item || thingDef.Minifiable) && (thingDef.category != ThingCategory.Item || thingDef.EverHaulable) && !thingDef.isUnfinishedThing && !thingDef.IsCorpse && thingDef.PlayerAcquirable && thingDef.graphicData != null && !typeof(MinifiedThing).IsAssignableFrom(thingDef.thingClass);
		}

		// Token: 0x06004FC2 RID: 20418 RVA: 0x001AE8FC File Offset: 0x001ACAFC
		public static IEnumerable<ThingDef> GetAllowedThingDefs(ThingSetMakerParams parms)
		{
			TechLevel techLevel = parms.techLevel ?? TechLevel.Undefined;
			IEnumerable<ThingDef> source = parms.filter.AllowedThingDefs;
			if (techLevel != TechLevel.Undefined)
			{
				source = from x in source
				where x.techLevel <= techLevel
				select x;
			}
			RoyalTitleDef highestTitle = null;
			if (parms.makingFaction != null && parms.makingFaction.def.HasRoyalTitles)
			{
				foreach (Pawn pawn in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_Colonists)
				{
					RoyalTitleDef royalTitleDef = (pawn.royalty != null) ? pawn.royalty.GetCurrentTitle(parms.makingFaction) : null;
					if (royalTitleDef != null && (highestTitle == null || royalTitleDef.seniority > highestTitle.seniority))
					{
						highestTitle = royalTitleDef;
					}
				}
			}
			source = source.Where(delegate(ThingDef x)
			{
				CompProperties_Techprint compProperties = x.GetCompProperties<CompProperties_Techprint>();
				if (compProperties != null)
				{
					if (parms.makingFaction != null && !compProperties.project.heldByFactionCategoryTags.Contains(parms.makingFaction.def.categoryTag))
					{
						return false;
					}
					if (compProperties.project.IsFinished || compProperties.project.TechprintRequirementMet)
					{
						return false;
					}
				}
				if (parms.makingFaction != null && parms.makingFaction.def.HasRoyalTitles)
				{
					RoyalTitleDef minTitleToUse = ThingRequiringRoyalPermissionUtility.GetMinTitleToUse(x, parms.makingFaction, 0);
					if (minTitleToUse != null && (highestTitle == null || highestTitle.seniority < minTitleToUse.seniority))
					{
						return false;
					}
				}
				return true;
			});
			return source.Where(delegate(ThingDef x)
			{
				if (ThingSetMakerUtility.CanGenerate(x))
				{
					if (parms.maxThingMarketValue != null)
					{
						float baseMarketValue = x.BaseMarketValue;
						float? maxThingMarketValue = parms.maxThingMarketValue;
						if (!(baseMarketValue <= maxThingMarketValue.GetValueOrDefault() & maxThingMarketValue != null))
						{
							return false;
						}
					}
					return parms.validator == null || parms.validator(x);
				}
				return false;
			});
		}

		// Token: 0x06004FC3 RID: 20419 RVA: 0x001AEA48 File Offset: 0x001ACC48
		public static void AssignQuality(Thing thing, QualityGenerator? qualityGenerator)
		{
			CompQuality compQuality = thing.TryGetComp<CompQuality>();
			if (compQuality != null)
			{
				QualityCategory q = QualityUtility.GenerateQuality(qualityGenerator ?? QualityGenerator.BaseGen);
				compQuality.SetQuality(q, ArtGenerationContext.Outsider);
			}
		}

		// Token: 0x06004FC4 RID: 20420 RVA: 0x001AEA84 File Offset: 0x001ACC84
		public static bool IsDerpAndDisallowed(ThingDef thing, ThingDef stuff, QualityGenerator? qualityGenerator)
		{
			QualityGenerator? qualityGenerator2 = qualityGenerator;
			QualityGenerator qualityGenerator3 = QualityGenerator.Gift;
			if (!(qualityGenerator2.GetValueOrDefault() == qualityGenerator3 & qualityGenerator2 != null))
			{
				qualityGenerator2 = qualityGenerator;
				qualityGenerator3 = QualityGenerator.Reward;
				if (!(qualityGenerator2.GetValueOrDefault() == qualityGenerator3 & qualityGenerator2 != null))
				{
					qualityGenerator2 = qualityGenerator;
					qualityGenerator3 = QualityGenerator.Super;
					if (!(qualityGenerator2.GetValueOrDefault() == qualityGenerator3 & qualityGenerator2 != null))
					{
						return false;
					}
				}
			}
			return PawnWeaponGenerator.IsDerpWeapon(thing, stuff) || PawnApparelGenerator.IsDerpApparel(thing, stuff);
		}

		// Token: 0x06004FC5 RID: 20421 RVA: 0x001AEAF0 File Offset: 0x001ACCF0
		public static float AdjustedBigCategoriesSelectionWeight(ThingDef d, int numMeats, int numLeathers)
		{
			float num = 1f;
			if (d.IsMeat)
			{
				num *= Mathf.Min(5f / (float)numMeats, 1f);
			}
			if (d.IsLeather)
			{
				num *= Mathf.Min(5f / (float)numLeathers, 1f);
			}
			return num;
		}

		// Token: 0x06004FC6 RID: 20422 RVA: 0x001AEB40 File Offset: 0x001ACD40
		public static bool PossibleToWeighNoMoreThan(ThingDef t, float maxMass, IEnumerable<ThingDef> allowedStuff)
		{
			if (maxMass == 3.40282347E+38f || t.category == ThingCategory.Pawn)
			{
				return true;
			}
			if (maxMass < 0f)
			{
				return false;
			}
			if (t.MadeFromStuff)
			{
				foreach (ThingDef stuff in allowedStuff)
				{
					if (t.GetStatValueAbstract(StatDefOf.Mass, stuff) <= maxMass)
					{
						return true;
					}
				}
				return false;
			}
			return t.GetStatValueAbstract(StatDefOf.Mass, null) <= maxMass;
		}

		// Token: 0x06004FC7 RID: 20423 RVA: 0x001AEBD0 File Offset: 0x001ACDD0
		public static bool TryGetRandomThingWhichCanWeighNoMoreThan(IEnumerable<ThingDef> candidates, TechLevel stuffTechLevel, float maxMass, QualityGenerator? qualityGenerator, out ThingStuffPair thingStuffPair)
		{
			ThingDef thingDef;
			if (!(from x in candidates
			where ThingSetMakerUtility.PossibleToWeighNoMoreThan(x, maxMass, GenStuff.AllowedStuffsFor(x, stuffTechLevel))
			select x).TryRandomElement(out thingDef))
			{
				thingStuffPair = default(ThingStuffPair);
				return false;
			}
			ThingDef stuff;
			if (thingDef.MadeFromStuff)
			{
				if (!(from x in GenStuff.AllowedStuffsFor(thingDef, stuffTechLevel)
				where thingDef.GetStatValueAbstract(StatDefOf.Mass, x) <= maxMass && !ThingSetMakerUtility.IsDerpAndDisallowed(thingDef, x, qualityGenerator)
				select x).TryRandomElementByWeight((ThingDef x) => x.stuffProps.commonality, out stuff))
				{
					thingStuffPair = default(ThingStuffPair);
					return false;
				}
			}
			else
			{
				stuff = null;
			}
			thingStuffPair = new ThingStuffPair(thingDef, stuff, 1f);
			return true;
		}

		// Token: 0x06004FC8 RID: 20424 RVA: 0x001AECA0 File Offset: 0x001ACEA0
		public static bool PossibleToWeighNoMoreThan(IEnumerable<ThingDef> candidates, TechLevel stuffTechLevel, float maxMass, int count)
		{
			if (maxMass == 3.40282347E+38f || count <= 0)
			{
				return true;
			}
			if (maxMass < 0f)
			{
				return false;
			}
			float num = float.MaxValue;
			foreach (ThingDef thingDef in candidates)
			{
				num = Mathf.Min(num, ThingSetMakerUtility.GetMinMass(thingDef, stuffTechLevel));
			}
			return num <= maxMass * (float)count;
		}

		// Token: 0x06004FC9 RID: 20425 RVA: 0x001AED18 File Offset: 0x001ACF18
		public static float GetMinMass(ThingDef thingDef, TechLevel stuffTechLevel)
		{
			float num = float.MaxValue;
			if (thingDef.MadeFromStuff)
			{
				using (IEnumerator<ThingDef> enumerator = GenStuff.AllowedStuffsFor(thingDef, stuffTechLevel).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ThingDef thingDef2 = enumerator.Current;
						if (thingDef2.stuffProps.commonality > 0f)
						{
							num = Mathf.Min(num, thingDef.GetStatValueAbstract(StatDefOf.Mass, thingDef2));
						}
					}
					return num;
				}
			}
			num = Mathf.Min(num, thingDef.GetStatValueAbstract(StatDefOf.Mass, null));
			return num;
		}

		// Token: 0x06004FCA RID: 20426 RVA: 0x001AEDA8 File Offset: 0x001ACFA8
		public static float GetMinMarketValue(ThingDef thingDef, TechLevel stuffTechLevel)
		{
			float num = float.MaxValue;
			if (thingDef.MadeFromStuff)
			{
				using (IEnumerator<ThingDef> enumerator = GenStuff.AllowedStuffsFor(thingDef, stuffTechLevel).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ThingDef thingDef2 = enumerator.Current;
						if (thingDef2.stuffProps.commonality > 0f)
						{
							num = Mathf.Min(num, StatDefOf.MarketValue.Worker.GetValue(StatRequest.For(thingDef, thingDef2, QualityCategory.Awful), true));
						}
					}
					return num;
				}
			}
			num = Mathf.Min(num, StatDefOf.MarketValue.Worker.GetValue(StatRequest.For(thingDef, null, QualityCategory.Awful), true));
			return num;
		}

		// Token: 0x04002CA3 RID: 11427
		public static List<ThingDef> allGeneratableItems = new List<ThingDef>();
	}
}
