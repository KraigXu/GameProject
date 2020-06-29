using System;
using Verse;

namespace RimWorld.BaseGen
{
	
	public class SymbolResolver_BasePart_Outdoors_LeafPossiblyDecorated : SymbolResolver
	{
		
		public override void Resolve(ResolveParams rp)
		{
			if (rp.rect.Width >= 10 && rp.rect.Height >= 10 && Rand.Chance(0.25f))
			{
				BaseGenCore.symbolStack.Push("basePart_outdoors_leafDecorated", rp, null);
				return;
			}
			BaseGenCore.symbolStack.Push("basePart_outdoors_leaf", rp, null);
		}
	}
}
