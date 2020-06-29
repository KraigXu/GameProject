using System;

namespace RimWorld.BaseGen
{
	
	public class SymbolResolver_BasePart_Outdoors_Leaf_Empty : SymbolResolver
	{
		
		public override bool CanResolve(ResolveParams rp)
		{
			return base.CanResolve(rp) && BaseGenCore.globalSettings.basePart_buildingsResolved >= BaseGenCore.globalSettings.minBuildings;
		}

		
		public override void Resolve(ResolveParams rp)
		{
			BaseGenCore.globalSettings.basePart_emptyNodesResolved++;
		}
	}
}
