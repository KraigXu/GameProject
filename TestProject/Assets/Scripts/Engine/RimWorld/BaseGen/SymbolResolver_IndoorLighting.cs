using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020010CD RID: 4301
	public class SymbolResolver_IndoorLighting : SymbolResolver
	{
		// Token: 0x0600656C RID: 25964 RVA: 0x00237530 File Offset: 0x00235730
		public override void Resolve(ResolveParams rp)
		{
			Map map = BaseGen.globalSettings.map;
			ThingDef thingDef;
			if (rp.faction == null || rp.faction.def.techLevel >= TechLevel.Industrial)
			{
				thingDef = ThingDefOf.StandingLamp;
			}
			else if (map.mapTemperature.OutdoorTemp > 18f)
			{
				thingDef = null;
			}
			else
			{
				thingDef = ThingDefOf.TorchLamp;
			}
			if (thingDef != null)
			{
				ResolveParams resolveParams = rp;
				resolveParams.singleThingDef = thingDef;
				BaseGen.symbolStack.Push("edgeThing", resolveParams, null);
			}
		}

		// Token: 0x04003DC4 RID: 15812
		private const float NeverSpawnTorchesIfTemperatureAbove = 18f;
	}
}
