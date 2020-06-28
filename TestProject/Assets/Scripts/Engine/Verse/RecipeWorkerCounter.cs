using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x0200009E RID: 158
	public class RecipeWorkerCounter
	{
		// Token: 0x06000518 RID: 1304 RVA: 0x00019A88 File Offset: 0x00017C88
		public virtual bool CanCountProducts(Bill_Production bill)
		{
			return this.recipe.specialProducts == null && this.recipe.products != null && this.recipe.products.Count == 1;
		}

		// Token: 0x06000519 RID: 1305 RVA: 0x00019ABC File Offset: 0x00017CBC
		public virtual int CountProducts(Bill_Production bill)
		{
			ThingDefCountClass thingDefCountClass = this.recipe.products[0];
			ThingDef thingDef = thingDefCountClass.thingDef;
			if (thingDefCountClass.thingDef.CountAsResource && !bill.includeEquipped && (bill.includeTainted || !thingDefCountClass.thingDef.IsApparel || !thingDefCountClass.thingDef.apparel.careIfWornByCorpse) && bill.includeFromZone == null && bill.hpRange.min == 0f && bill.hpRange.max == 1f && bill.qualityRange.min == QualityCategory.Awful && bill.qualityRange.max == QualityCategory.Legendary && !bill.limitToAllowedStuff)
			{
				return bill.Map.resourceCounter.GetCount(thingDefCountClass.thingDef) + this.GetCarriedCount(bill, thingDef);
			}
			int num = 0;
			if (bill.includeFromZone == null)
			{
				num = this.CountValidThings(bill.Map.listerThings.ThingsOfDef(thingDefCountClass.thingDef), bill, thingDef);
				if (thingDefCountClass.thingDef.Minifiable)
				{
					List<Thing> list = bill.Map.listerThings.ThingsInGroup(ThingRequestGroup.MinifiedThing);
					for (int i = 0; i < list.Count; i++)
					{
						MinifiedThing minifiedThing = (MinifiedThing)list[i];
						if (this.CountValidThing(minifiedThing.InnerThing, bill, thingDef))
						{
							num += minifiedThing.stackCount * minifiedThing.InnerThing.stackCount;
						}
					}
				}
				num += this.GetCarriedCount(bill, thingDef);
			}
			else
			{
				foreach (Thing outerThing in bill.includeFromZone.AllContainedThings)
				{
					Thing innerIfMinified = outerThing.GetInnerIfMinified();
					if (this.CountValidThing(innerIfMinified, bill, thingDef))
					{
						num += innerIfMinified.stackCount;
					}
				}
			}
			if (bill.includeEquipped)
			{
				foreach (Pawn pawn in bill.Map.mapPawns.FreeColonistsSpawned)
				{
					List<ThingWithComps> allEquipmentListForReading = pawn.equipment.AllEquipmentListForReading;
					for (int j = 0; j < allEquipmentListForReading.Count; j++)
					{
						if (this.CountValidThing(allEquipmentListForReading[j], bill, thingDef))
						{
							num += allEquipmentListForReading[j].stackCount;
						}
					}
					List<Apparel> wornApparel = pawn.apparel.WornApparel;
					for (int k = 0; k < wornApparel.Count; k++)
					{
						if (this.CountValidThing(wornApparel[k], bill, thingDef))
						{
							num += wornApparel[k].stackCount;
						}
					}
					ThingOwner directlyHeldThings = pawn.inventory.GetDirectlyHeldThings();
					for (int l = 0; l < directlyHeldThings.Count; l++)
					{
						if (this.CountValidThing(directlyHeldThings[l], bill, thingDef))
						{
							num += directlyHeldThings[l].stackCount;
						}
					}
				}
			}
			return num;
		}

		// Token: 0x0600051A RID: 1306 RVA: 0x00019DC8 File Offset: 0x00017FC8
		public int CountValidThings(List<Thing> things, Bill_Production bill, ThingDef def)
		{
			int num = 0;
			for (int i = 0; i < things.Count; i++)
			{
				if (this.CountValidThing(things[i], bill, def))
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x0600051B RID: 1307 RVA: 0x00019E00 File Offset: 0x00018000
		public bool CountValidThing(Thing thing, Bill_Production bill, ThingDef def)
		{
			ThingDef def2 = thing.def;
			if (def2 != def)
			{
				return false;
			}
			if (!bill.includeTainted && def2.IsApparel && ((Apparel)thing).WornByCorpse)
			{
				return false;
			}
			if (thing.def.useHitPoints && !bill.hpRange.IncludesEpsilon((float)thing.HitPoints / (float)thing.MaxHitPoints))
			{
				return false;
			}
			CompQuality compQuality = thing.TryGetComp<CompQuality>();
			return (compQuality == null || bill.qualityRange.Includes(compQuality.Quality)) && (!bill.limitToAllowedStuff || bill.ingredientFilter.Allows(thing.Stuff));
		}

		// Token: 0x0600051C RID: 1308 RVA: 0x00019EA1 File Offset: 0x000180A1
		public virtual string ProductsDescription(Bill_Production bill)
		{
			return null;
		}

		// Token: 0x0600051D RID: 1309 RVA: 0x00019EA4 File Offset: 0x000180A4
		public virtual bool CanPossiblyStoreInStockpile(Bill_Production bill, Zone_Stockpile stockpile)
		{
			return !this.CanCountProducts(bill) || stockpile.GetStoreSettings().AllowedToAccept(this.recipe.products[0].thingDef);
		}

		// Token: 0x0600051E RID: 1310 RVA: 0x00019ED4 File Offset: 0x000180D4
		private int GetCarriedCount(Bill_Production bill, ThingDef prodDef)
		{
			int num = 0;
			foreach (Pawn pawn in bill.Map.mapPawns.FreeColonistsSpawned)
			{
				Thing thing = pawn.carryTracker.CarriedThing;
				if (thing != null)
				{
					int stackCount = thing.stackCount;
					thing = thing.GetInnerIfMinified();
					if (this.CountValidThing(thing, bill, prodDef))
					{
						num += stackCount;
					}
				}
			}
			return num;
		}

		// Token: 0x04000301 RID: 769
		public RecipeDef recipe;
	}
}
