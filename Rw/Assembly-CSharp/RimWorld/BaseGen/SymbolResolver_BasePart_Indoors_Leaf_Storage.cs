using System;

namespace RimWorld.BaseGen
{
	// Token: 0x0200109C RID: 4252
	public class SymbolResolver_BasePart_Indoors_Leaf_Storage : SymbolResolver
	{
		// Token: 0x060064C7 RID: 25799 RVA: 0x00231955 File Offset: 0x0022FB55
		public override bool CanResolve(ResolveParams rp)
		{
			return base.CanResolve(rp) && BaseGen.globalSettings.basePart_barracksResolved >= BaseGen.globalSettings.minBarracks;
		}

		// Token: 0x060064C8 RID: 25800 RVA: 0x0023198E File Offset: 0x0022FB8E
		public override void Resolve(ResolveParams rp)
		{
			BaseGen.symbolStack.Push("storage", rp, null);
		}
	}
}
