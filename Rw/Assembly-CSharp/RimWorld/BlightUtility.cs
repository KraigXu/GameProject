using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C8F RID: 3215
	public static class BlightUtility
	{
		// Token: 0x06004D77 RID: 19831 RVA: 0x001A01B8 File Offset: 0x0019E3B8
		public static Plant GetFirstBlightableNowPlant(IntVec3 c, Map map)
		{
			Plant plant = c.GetPlant(map);
			if (plant != null && plant.BlightableNow)
			{
				return plant;
			}
			return null;
		}

		// Token: 0x06004D78 RID: 19832 RVA: 0x001A01DC File Offset: 0x0019E3DC
		public static Plant GetFirstBlightableEverPlant(IntVec3 c, Map map)
		{
			Plant plant = c.GetPlant(map);
			if (plant != null && plant.def.plant.Blightable)
			{
				return plant;
			}
			return null;
		}
	}
}
