using System;

namespace RimWorld.BaseGen
{
	// Token: 0x0200109B RID: 4251
	public class SymbolResolver_BasePart_Indoors_Leaf_DiningRoom : SymbolResolver
	{
		// Token: 0x060064C4 RID: 25796 RVA: 0x00231955 File Offset: 0x0022FB55
		public override bool CanResolve(ResolveParams rp)
		{
			return base.CanResolve(rp) && BaseGen.globalSettings.basePart_barracksResolved >= BaseGen.globalSettings.minBarracks;
		}

		// Token: 0x060064C5 RID: 25797 RVA: 0x0023197B File Offset: 0x0022FB7B
		public override void Resolve(ResolveParams rp)
		{
			BaseGen.symbolStack.Push("diningRoom", rp, null);
		}
	}
}
