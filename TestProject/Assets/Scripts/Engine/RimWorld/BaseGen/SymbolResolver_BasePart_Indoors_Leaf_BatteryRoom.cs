using System;

namespace RimWorld.BaseGen
{
	
	public class SymbolResolver_BasePart_Indoors_Leaf_BatteryRoom : SymbolResolver
	{
		
		public override bool CanResolve(ResolveParams rp)
		{
			return base.CanResolve(rp) && BaseGenCore.globalSettings.basePart_barracksResolved >= BaseGenCore.globalSettings.minBarracks && BaseGenCore.globalSettings.basePart_batteriesCoverage + (float)rp.rect.Area / (float)BaseGenCore.globalSettings.mainRect.Area < 0.06f && (rp.faction == null || rp.faction.def.techLevel >= TechLevel.Industrial);
		}

		
		public override void Resolve(ResolveParams rp)
		{
			BaseGenCore.symbolStack.Push("batteryRoom", rp, null);
			BaseGenCore.globalSettings.basePart_batteriesCoverage += (float)rp.rect.Area / (float)BaseGenCore.globalSettings.mainRect.Area;
		}

		
		private const float MaxCoverage = 0.06f;
	}
}
