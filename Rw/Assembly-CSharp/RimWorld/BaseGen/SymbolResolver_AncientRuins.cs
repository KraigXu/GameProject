using System;

namespace RimWorld.BaseGen
{
	// Token: 0x020010C0 RID: 4288
	public class SymbolResolver_AncientRuins : SymbolResolver
	{
		// Token: 0x06006549 RID: 25929 RVA: 0x00235C94 File Offset: 0x00233E94
		public override void Resolve(ResolveParams rp)
		{
			ResolveParams resolveParams = rp;
			resolveParams.wallStuff = (rp.wallStuff ?? BaseGenUtility.RandomCheapWallStuff(rp.faction, true));
			resolveParams.chanceToSkipWallBlock = new float?(rp.chanceToSkipWallBlock ?? 0.1f);
			resolveParams.clearEdificeOnly = new bool?(rp.clearEdificeOnly ?? true);
			resolveParams.noRoof = new bool?(rp.noRoof ?? true);
			BaseGen.symbolStack.Push("emptyRoom", resolveParams, null);
		}
	}
}
