using System;
using Verse;

namespace RimWorld.BaseGen
{
	
	public static class InteriorSymbolResolverUtility
	{
		
		public static void PushBedroomHeatersCoolersAndLightSourcesSymbols(ResolveParams rp, bool hasToSpawnLightSource = true)
		{
			Map map = BaseGenCore.globalSettings.map;
			if (map.mapTemperature.OutdoorTemp > 22f)
			{
				ResolveParams resolveParams = rp;
				resolveParams.singleThingDef = ThingDefOf.PassiveCooler;
				BaseGenCore.symbolStack.Push("edgeThing", resolveParams, null);
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
					BaseGenCore.symbolStack.Push("edgeThing", resolveParams2, null);
				}
			}
			if (!flag && hasToSpawnLightSource)
			{
				BaseGenCore.symbolStack.Push("indoorLighting", rp, null);
			}
		}

		
		private const float SpawnHeaterIfTemperatureBelow = 3f;

		
		private const float SpawnSecondHeaterIfTemperatureBelow = -45f;

		
		private const float NonIndustrial_SpawnCampfireIfTemperatureBelow = -20f;

		
		private const float SpawnPassiveCoolerIfTemperatureAbove = 22f;
	}
}
