using System;

namespace RimWorld.BaseGen
{
	
	public class SymbolResolver_BasePart_Indoors_Leaf_Storage : SymbolResolver
	{
		
		public override bool CanResolve(ResolveParams rp)
		{
			return base.CanResolve(rp) && BaseGenCore.globalSettings.basePart_barracksResolved >= BaseGenCore.globalSettings.minBarracks;
		}

		
		public override void Resolve(ResolveParams rp)
		{
			BaseGenCore.symbolStack.Push("storage", rp, null);
		}
	}
}
