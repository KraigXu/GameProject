using System;
using Verse;

namespace RimWorld.BaseGen
{
	
	public class SymbolResolver_Roof : SymbolResolver
	{
		
		public override void Resolve(ResolveParams rp)
		{
			if (rp.noRoof != null && rp.noRoof.Value)
			{
				return;
			}
			RoofGrid roofGrid = BaseGenCore.globalSettings.map.roofGrid;
			RoofDef def = rp.roofDef ?? RoofDefOf.RoofConstructed;
			foreach (IntVec3 c in rp.rect)
			{
				if (!roofGrid.Roofed(c))
				{
					roofGrid.SetRoof(c, def);
				}
			}
		}
	}
}
