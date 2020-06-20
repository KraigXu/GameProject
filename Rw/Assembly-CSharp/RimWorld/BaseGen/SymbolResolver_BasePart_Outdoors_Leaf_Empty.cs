using System;

namespace RimWorld.BaseGen
{
	// Token: 0x020010A3 RID: 4259
	public class SymbolResolver_BasePart_Outdoors_Leaf_Empty : SymbolResolver
	{
		// Token: 0x060064E3 RID: 25827 RVA: 0x00232808 File Offset: 0x00230A08
		public override bool CanResolve(ResolveParams rp)
		{
			return base.CanResolve(rp) && BaseGen.globalSettings.basePart_buildingsResolved >= BaseGen.globalSettings.minBuildings;
		}

		// Token: 0x060064E4 RID: 25828 RVA: 0x0023282E File Offset: 0x00230A2E
		public override void Resolve(ResolveParams rp)
		{
			BaseGen.globalSettings.basePart_emptyNodesResolved++;
		}
	}
}
