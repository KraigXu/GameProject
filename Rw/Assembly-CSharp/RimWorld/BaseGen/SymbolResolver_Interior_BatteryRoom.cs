using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020010DB RID: 4315
	public class SymbolResolver_Interior_BatteryRoom : SymbolResolver
	{
		// Token: 0x0600659A RID: 26010 RVA: 0x00239084 File Offset: 0x00237284
		public override void Resolve(ResolveParams rp)
		{
			BaseGen.symbolStack.Push("indoorLighting", rp, null);
			BaseGen.symbolStack.Push("chargeBatteries", rp, null);
			ResolveParams resolveParams = rp;
			resolveParams.singleThingDef = ThingDefOf.Battery;
			resolveParams.thingRot = new Rot4?(Rand.Bool ? Rot4.North : Rot4.East);
			resolveParams.fillWithThingsPadding = new int?(rp.fillWithThingsPadding ?? 1);
			BaseGen.symbolStack.Push("fillWithThings", resolveParams, null);
		}
	}
}
