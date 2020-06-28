using System;

namespace Verse
{
	// Token: 0x020001A8 RID: 424
	public class ScattererValidator_Buildable : ScattererValidator
	{
		// Token: 0x06000BE4 RID: 3044 RVA: 0x000438E4 File Offset: 0x00041AE4
		public override bool Allows(IntVec3 c, Map map)
		{
			CellRect cellRect = CellRect.CenteredOn(c, this.radius);
			for (int i = cellRect.minZ; i <= cellRect.maxZ; i++)
			{
				for (int j = cellRect.minX; j <= cellRect.maxX; j++)
				{
					IntVec3 c2 = new IntVec3(j, 0, i);
					if (!c2.InBounds(map))
					{
						return false;
					}
					if (c2.InNoBuildEdgeArea(map))
					{
						return false;
					}
					if (this.affordance != null && !c2.GetTerrain(map).affordances.Contains(this.affordance))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x04000980 RID: 2432
		public int radius = 1;

		// Token: 0x04000981 RID: 2433
		public TerrainAffordanceDef affordance;
	}
}
