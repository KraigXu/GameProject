using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020010A9 RID: 4265
	public class SymbolResolver_Bed : SymbolResolver
	{
		// Token: 0x060064F4 RID: 25844 RVA: 0x00232E10 File Offset: 0x00231010
		public override void Resolve(ResolveParams rp)
		{
			ThingDef singleThingDef = rp.singleThingDef ?? Rand.Element<ThingDef>(ThingDefOf.Bed, ThingDefOf.Bedroll, ThingDefOf.SleepingSpot);
			ResolveParams resolveParams = rp;
			resolveParams.singleThingDef = singleThingDef;
			resolveParams.skipSingleThingIfHasToWipeBuildingOrDoesntFit = new bool?(rp.skipSingleThingIfHasToWipeBuildingOrDoesntFit ?? true);
			BaseGen.symbolStack.Push("thing", resolveParams, null);
		}
	}
}
