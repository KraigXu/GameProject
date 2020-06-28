using System;

namespace RimWorld.BaseGen
{
	// Token: 0x020010DF RID: 4319
	public class SymbolResolver_Interior_PrisonCell : SymbolResolver
	{
		// Token: 0x060065A3 RID: 26019 RVA: 0x002392F8 File Offset: 0x002374F8
		public override void Resolve(ResolveParams rp)
		{
			ThingSetMakerParams value = default(ThingSetMakerParams);
			value.techLevel = new TechLevel?((rp.faction != null) ? rp.faction.def.techLevel : TechLevel.Spacer);
			ResolveParams resolveParams = rp;
			resolveParams.thingSetMakerDef = ThingSetMakerDefOf.MapGen_PrisonCellStockpile;
			resolveParams.thingSetMakerParams = new ThingSetMakerParams?(value);
			resolveParams.innerStockpileSize = new int?(3);
			BaseGen.symbolStack.Push("innerStockpile", resolveParams, null);
			InteriorSymbolResolverUtility.PushBedroomHeatersCoolersAndLightSourcesSymbols(rp, false);
			BaseGen.symbolStack.Push("prisonerBed", rp, null);
		}

		// Token: 0x04003DD9 RID: 15833
		private const int FoodStockpileSize = 3;
	}
}
