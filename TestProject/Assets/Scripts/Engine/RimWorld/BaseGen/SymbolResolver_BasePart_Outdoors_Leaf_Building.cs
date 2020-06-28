using System;

namespace RimWorld.BaseGen
{
	// Token: 0x020010A2 RID: 4258
	public class SymbolResolver_BasePart_Outdoors_Leaf_Building : SymbolResolver
	{
		// Token: 0x060064E0 RID: 25824 RVA: 0x0023275E File Offset: 0x0023095E
		public override bool CanResolve(ResolveParams rp)
		{
			return base.CanResolve(rp) && (BaseGen.globalSettings.basePart_emptyNodesResolved >= BaseGen.globalSettings.minEmptyNodes || BaseGen.globalSettings.basePart_buildingsResolved < BaseGen.globalSettings.minBuildings);
		}

		// Token: 0x060064E1 RID: 25825 RVA: 0x0023279C File Offset: 0x0023099C
		public override void Resolve(ResolveParams rp)
		{
			ResolveParams resolveParams = rp;
			resolveParams.wallStuff = (rp.wallStuff ?? BaseGenUtility.RandomCheapWallStuff(rp.faction, false));
			resolveParams.floorDef = (rp.floorDef ?? BaseGenUtility.RandomBasicFloorDef(rp.faction, true));
			BaseGen.symbolStack.Push("basePart_indoors", resolveParams, null);
			BaseGen.globalSettings.basePart_buildingsResolved++;
		}
	}
}
