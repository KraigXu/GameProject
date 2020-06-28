using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020010BA RID: 4282
	public class SymbolResolver_Roof : SymbolResolver
	{
		// Token: 0x06006533 RID: 25907 RVA: 0x00234EB4 File Offset: 0x002330B4
		public override void Resolve(ResolveParams rp)
		{
			if (rp.noRoof != null && rp.noRoof.Value)
			{
				return;
			}
			RoofGrid roofGrid = BaseGen.globalSettings.map.roofGrid;
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
