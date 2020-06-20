using System;

namespace RimWorld.BaseGen
{
	// Token: 0x020010A4 RID: 4260
	public class SymbolResolver_BasePart_Outdoors_Leaf_Farm : SymbolResolver
	{
		// Token: 0x060064E6 RID: 25830 RVA: 0x00232844 File Offset: 0x00230A44
		public override bool CanResolve(ResolveParams rp)
		{
			return base.CanResolve(rp) && BaseGen.globalSettings.basePart_buildingsResolved >= BaseGen.globalSettings.minBuildings && BaseGen.globalSettings.basePart_emptyNodesResolved >= BaseGen.globalSettings.minEmptyNodes && BaseGen.globalSettings.basePart_farmsCoverage + (float)rp.rect.Area / (float)BaseGen.globalSettings.mainRect.Area < 0.55f && (rp.rect.Width <= 15 && rp.rect.Height <= 15) && (rp.cultivatedPlantDef != null || SymbolResolver_CultivatedPlants.DeterminePlantDef(rp.rect) != null);
		}

		// Token: 0x060064E7 RID: 25831 RVA: 0x002328F8 File Offset: 0x00230AF8
		public override void Resolve(ResolveParams rp)
		{
			BaseGen.symbolStack.Push("farm", rp, null);
			BaseGen.globalSettings.basePart_farmsCoverage += (float)rp.rect.Area / (float)BaseGen.globalSettings.mainRect.Area;
		}

		// Token: 0x04003DA3 RID: 15779
		private const float MaxCoverage = 0.55f;
	}
}
