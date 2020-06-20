using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020010CB RID: 4299
	public class SymbolResolver_GenericRoom : SymbolResolver
	{
		// Token: 0x06006565 RID: 25957 RVA: 0x00237388 File Offset: 0x00235588
		public override void Resolve(ResolveParams rp)
		{
			BaseGen.symbolStack.Push("doors", rp, null);
			if (!this.interior.NullOrEmpty())
			{
				ResolveParams resolveParams = rp;
				resolveParams.rect = rp.rect.ContractedBy(1);
				BaseGen.symbolStack.Push(this.interior, resolveParams, null);
			}
			BaseGen.symbolStack.Push("emptyRoom", rp, null);
		}

		// Token: 0x04003DC2 RID: 15810
		public string interior;
	}
}
