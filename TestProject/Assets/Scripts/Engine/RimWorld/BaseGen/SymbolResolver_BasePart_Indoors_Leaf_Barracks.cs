using System;

namespace RimWorld.BaseGen
{
	
	public class SymbolResolver_BasePart_Indoors_Leaf_Barracks : SymbolResolver
	{
		
		public override void Resolve(ResolveParams rp)
		{
			BaseGenCore.symbolStack.Push("barracks", rp, null);
			BaseGenCore.globalSettings.basePart_barracksResolved++;
		}
	}
}
