using System;

namespace RimWorld.BaseGen
{
	
	public class SymbolResolver_BasePart_Outdoors_Leaf_Building : SymbolResolver
	{
		
		public override bool CanResolve(ResolveParams rp)
		{
			return base.CanResolve(rp) && (BaseGenCore.globalSettings.basePart_emptyNodesResolved >= BaseGenCore.globalSettings.minEmptyNodes || BaseGenCore.globalSettings.basePart_buildingsResolved < BaseGenCore.globalSettings.minBuildings);
		}

		
		public override void Resolve(ResolveParams rp)
		{
			ResolveParams resolveParams = rp;
			resolveParams.wallStuff = (rp.wallStuff ?? BaseGenUtility.RandomCheapWallStuff(rp.faction, false));
			resolveParams.floorDef = (rp.floorDef ?? BaseGenUtility.RandomBasicFloorDef(rp.faction, true));
			BaseGenCore.symbolStack.Push("basePart_indoors", resolveParams, null);
			BaseGenCore.globalSettings.basePart_buildingsResolved++;
		}
	}
}
