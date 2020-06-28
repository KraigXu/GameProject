using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020010E0 RID: 4320
	public class SymbolResolver_Interior_Storage : SymbolResolver
	{
		// Token: 0x060065A5 RID: 26021 RVA: 0x00239388 File Offset: 0x00237588
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

		// Token: 0x04003DDA RID: 15834
		private const float SpawnPassiveCoolerIfTemperatureAbove = 15f;
	}
}
