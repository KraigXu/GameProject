using System;

namespace RimWorld.BaseGen
{
	
	public class SymbolResolver_BasePart_Outdoors_Leaf_Farm : SymbolResolver
	{
		
		public override bool CanResolve(ResolveParams rp)
		{
			return base.CanResolve(rp) && BaseGenCore.globalSettings.basePart_buildingsResolved >= BaseGenCore.globalSettings.minBuildings && BaseGenCore.globalSettings.basePart_emptyNodesResolved >= BaseGenCore.globalSettings.minEmptyNodes && BaseGenCore.globalSettings.basePart_farmsCoverage + (float)rp.rect.Area / (float)BaseGenCore.globalSettings.mainRect.Area < 0.55f && (rp.rect.Width <= 15 && rp.rect.Height <= 15) && (rp.cultivatedPlantDef != null || SymbolResolver_CultivatedPlants.DeterminePlantDef(rp.rect) != null);
		}

		
		public override void Resolve(ResolveParams rp)
		{
			BaseGenCore.symbolStack.Push("farm", rp, null);
			BaseGenCore.globalSettings.basePart_farmsCoverage += (float)rp.rect.Area / (float)BaseGenCore.globalSettings.mainRect.Area;
		}

		
		private const float MaxCoverage = 0.55f;
	}
}
