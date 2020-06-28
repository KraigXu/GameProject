using System;

namespace RimWorld.BaseGen
{
	// Token: 0x02001098 RID: 4248
	public class SymbolResolver_BasePart_Indoors_Leaf_Barracks : SymbolResolver
	{
		// Token: 0x060064BC RID: 25788 RVA: 0x00231788 File Offset: 0x0022F988
		public override void Resolve(ResolveParams rp)
		{
			BaseGen.symbolStack.Push("barracks", rp, null);
			BaseGen.globalSettings.basePart_barracksResolved++;
		}
	}
}
