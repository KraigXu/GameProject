using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020010D8 RID: 4312
	public static class InteriorSymbolResolverUtility
	{
		// Token: 0x06006594 RID: 26004 RVA: 0x00238DFC File Offset: 0x00236FFC
		public static void PushBedroomHeatersCoolersAndLightSourcesSymbols(ResolveParams rp, bool hasToSpawnLightSource = true)
		{
			Map map = BaseGen.globalSettings.map;
			if (map.mapTemperature.OutdoorTemp > 22f)
			{
				ResolveParams resolveParams = rp;
				resolveParams.singleThingDef = ThingDefOf.PassiveCooler;
				BaseGen.symbolStack.Push("edgeThing", resolveParams, null);
			}
			bool flag = false;
			if (map.mapTemperature.OutdoorTemp < 3f)
			{
				ThingDef singleThingDef;
				if (rp.faction == null || rp.faction.def.techLevel >= TechLevel.Industrial)
				{
					singleThingDef = ThingDefOf.Heater;
				}
				else
				{
					singleThingDef = ((map.mapTemperature.OutdoorTemp < -20f) ? ThingDefOf.Campfire : ThingDefOf.TorchLamp);
					flag = true;
				}
				int num = (map.mapTemperature.OutdoorTemp < -45f) ? 2 : 1;
				for (int i = 0; i < num; i++)
				{
					ResolveParams resolveParams2 = rp;
					resolveParams2.singleThingDef = singleThingDef;
					BaseGen.symbolStack.Push("edgeThing", resolveParams2, null);
				}
			}
			if (!flag && hasToSpawnLightSource)
			{
				BaseGen.symbolStack.Push("indoorLighting", rp, null);
			}
		}

		// Token: 0x04003DD0 RID: 15824
		private const float SpawnHeaterIfTemperatureBelow = 3f;

		// Token: 0x04003DD1 RID: 15825
		private const float SpawnSecondHeaterIfTemperatureBelow = -45f;

		// Token: 0x04003DD2 RID: 15826
		private const float NonIndustrial_SpawnCampfireIfTemperatureBelow = -20f;

		// Token: 0x04003DD3 RID: 15827
		private const float SpawnPassiveCoolerIfTemperatureAbove = 22f;
	}
}
