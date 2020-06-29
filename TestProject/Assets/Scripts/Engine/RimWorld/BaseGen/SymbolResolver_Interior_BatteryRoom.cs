using System;
using Verse;

namespace RimWorld.BaseGen
{
	
	public class SymbolResolver_Interior_BatteryRoom : SymbolResolver
	{
		
		public override void Resolve(ResolveParams rp)
		{
			BaseGenCore.symbolStack.Push("indoorLighting", rp, null);
			BaseGenCore.symbolStack.Push("chargeBatteries", rp, null);
			ResolveParams resolveParams = rp;
			resolveParams.singleThingDef = ThingDefOf.Battery;
			resolveParams.thingRot = new Rot4?(Rand.Bool ? Rot4.North : Rot4.East);
			resolveParams.fillWithThingsPadding = new int?(rp.fillWithThingsPadding ?? 1);
			BaseGenCore.symbolStack.Push("fillWithThings", resolveParams, null);
		}
	}
}
