using System;

namespace RimWorld.BaseGen
{
	// Token: 0x020010DA RID: 4314
	public class SymbolResolver_Interior_Barracks : SymbolResolver
	{
		// Token: 0x06006598 RID: 26008 RVA: 0x00239068 File Offset: 0x00237268
		public override void Resolve(ResolveParams rp)
		{
			InteriorSymbolResolverUtility.PushBedroomHeatersCoolersAndLightSourcesSymbols(rp, true);
			BaseGen.symbolStack.Push("fillWithBeds", rp, null);
		}
	}
}
