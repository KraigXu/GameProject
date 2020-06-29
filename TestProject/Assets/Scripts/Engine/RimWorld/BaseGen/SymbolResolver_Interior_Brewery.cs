using System;
using Verse;

namespace RimWorld.BaseGen
{
	
	public class SymbolResolver_Interior_Brewery : SymbolResolver
	{
		
		// (get) Token: 0x0600659C RID: 26012 RVA: 0x00239117 File Offset: 0x00237317
		private float SpawnPassiveCoolerIfTemperatureAbove
		{
			get
			{
				return ThingDefOf.FermentingBarrel.GetCompProperties<CompProperties_TemperatureRuinable>().maxSafeTemperature;
			}
		}

		
		public override void Resolve(ResolveParams rp)
		{
			Map map = BaseGen.globalSettings.map;
			if (map.mapTemperature.OutdoorTemp > this.SpawnPassiveCoolerIfTemperatureAbove)
			{
				ResolveParams resolveParams = rp;
				resolveParams.singleThingDef = ThingDefOf.PassiveCooler;
				BaseGen.symbolStack.Push("edgeThing", resolveParams, null);
			}
			if (map.mapTemperature.OutdoorTemp < 7f)
			{
				ThingDef singleThingDef;
				if (rp.faction == null || rp.faction.def.techLevel >= TechLevel.Industrial)
				{
					singleThingDef = ThingDefOf.Heater;
				}
				else
				{
					singleThingDef = ThingDefOf.Campfire;
				}
				ResolveParams resolveParams2 = rp;
				resolveParams2.singleThingDef = singleThingDef;
				BaseGen.symbolStack.Push("edgeThing", resolveParams2, null);
			}
			BaseGen.symbolStack.Push("addWortToFermentingBarrels", rp, null);
			ResolveParams resolveParams3 = rp;
			resolveParams3.singleThingDef = ThingDefOf.FermentingBarrel;
			resolveParams3.thingRot = new Rot4?(Rand.Bool ? Rot4.North : Rot4.East);
			resolveParams3.fillWithThingsPadding = new int?(rp.fillWithThingsPadding ?? 1);
			BaseGen.symbolStack.Push("fillWithThings", resolveParams3, null);
		}

		
		private const float SpawnHeaterIfTemperatureBelow = 7f;
	}
}
