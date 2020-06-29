using System;

namespace RimWorld.BaseGen
{
	
	public class SymbolResolver_BasePart_Outdoors_LeafDecorated_EdgeStreet : SymbolResolver
	{
		
		public override void Resolve(ResolveParams rp)
		{
			ResolveParams resolveParams = rp;
			resolveParams.floorDef = (rp.pathwayFloorDef ?? BaseGenUtility.RandomBasicFloorDef(rp.faction, false));
			BaseGenCore.symbolStack.Push("edgeStreet", resolveParams, null);
			ResolveParams resolveParams2 = rp;
			resolveParams2.rect = rp.rect.ContractedBy(1);
			BaseGenCore.symbolStack.Push("basePart_outdoors_leaf", resolveParams2, null);
		}
	}
}
