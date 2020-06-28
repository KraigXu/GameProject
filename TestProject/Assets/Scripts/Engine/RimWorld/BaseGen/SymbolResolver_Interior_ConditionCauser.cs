using System;

namespace RimWorld.BaseGen
{
	// Token: 0x020010DD RID: 4317
	public class SymbolResolver_Interior_ConditionCauser : SymbolResolver
	{
		// Token: 0x0600659F RID: 26015 RVA: 0x0023923C File Offset: 0x0023743C
		public override void Resolve(ResolveParams rp)
		{
			ResolveParams resolveParams = rp;
			resolveParams.singleThingToSpawn = rp.conditionCauser;
			BaseGen.symbolStack.Push("thing", resolveParams, null);
		}
	}
}
