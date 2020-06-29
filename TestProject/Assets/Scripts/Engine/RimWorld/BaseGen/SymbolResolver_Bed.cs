using System;
using Verse;

namespace RimWorld.BaseGen
{
	
	public class SymbolResolver_Bed : SymbolResolver
	{
		
		public override void Resolve(ResolveParams rp)
		{
			ThingDef singleThingDef = rp.singleThingDef ?? Rand.Element<ThingDef>(ThingDefOf.Bed, ThingDefOf.Bedroll, ThingDefOf.SleepingSpot);
			ResolveParams resolveParams = rp;
			resolveParams.singleThingDef = singleThingDef;
			resolveParams.skipSingleThingIfHasToWipeBuildingOrDoesntFit = new bool?(rp.skipSingleThingIfHasToWipeBuildingOrDoesntFit ?? true);
			BaseGenCore.symbolStack.Push("thing", resolveParams, null);
		}
	}
}
