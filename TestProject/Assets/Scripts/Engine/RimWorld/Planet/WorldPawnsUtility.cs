using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02001289 RID: 4745
	public static class WorldPawnsUtility
	{
		// Token: 0x06006F8C RID: 28556 RVA: 0x0026D451 File Offset: 0x0026B651
		public static bool IsWorldPawn(this Pawn p)
		{
			return Find.WorldPawns.Contains(p);
		}
	}
}
