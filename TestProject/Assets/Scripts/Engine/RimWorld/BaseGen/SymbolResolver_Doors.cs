using System;
using Verse;

namespace RimWorld.BaseGen
{
	
	public class SymbolResolver_Doors : SymbolResolver
	{
		
		public override void Resolve(ResolveParams rp)
		{
			if (Rand.Chance(0.25f) || (rp.rect.Width >= 10 && rp.rect.Height >= 10 && Rand.Chance(0.8f)))
			{
				BaseGen.symbolStack.Push("extraDoor", rp, null);
			}
			BaseGen.symbolStack.Push("ensureCanReachMapEdge", rp, null);
		}

		
		private const float ExtraDoorChance = 0.25f;
	}
}
