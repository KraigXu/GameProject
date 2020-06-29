using System;
using Verse;

namespace RimWorld.BaseGen
{
	
	public class SymbolResolver_Interior_Storage : SymbolResolver
	{
		
		public override void Resolve(ResolveParams rp)
		{
			Map map = BaseGen.globalSettings.map;
			BaseGen.symbolStack.Push("stockpile", rp, null);
			if (map.mapTemperature.OutdoorTemp > 15f)
			{
				ResolveParams resolveParams = rp;
				resolveParams.singleThingDef = ThingDefOf.PassiveCooler;
				BaseGen.symbolStack.Push("edgeThing", resolveParams, null);
			}
		}

		
		private const float SpawnPassiveCoolerIfTemperatureAbove = 15f;
	}
}
