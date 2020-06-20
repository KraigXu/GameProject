using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020010A6 RID: 4262
	public class SymbolResolver_BasePart_Outdoors_LeafPossiblyDecorated : SymbolResolver
	{
		// Token: 0x060064EE RID: 25838 RVA: 0x00232BE4 File Offset: 0x00230DE4
		public override void Resolve(ResolveParams rp)
		{
			if (rp.rect.Width >= 10 && rp.rect.Height >= 10 && Rand.Chance(0.25f))
			{
				BaseGen.symbolStack.Push("basePart_outdoors_leafDecorated", rp, null);
				return;
			}
			BaseGen.symbolStack.Push("basePart_outdoors_leaf", rp, null);
		}
	}
}
