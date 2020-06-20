using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020011BD RID: 4541
	public static class WorldReachabilityUtility
	{
		// Token: 0x06006902 RID: 26882 RVA: 0x0024AD91 File Offset: 0x00248F91
		public static bool CanReach(this Caravan c, int tile)
		{
			return Find.WorldReachability.CanReach(c, tile);
		}
	}
}
