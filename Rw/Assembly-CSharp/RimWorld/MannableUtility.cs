using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D1F RID: 3359
	public static class MannableUtility
	{
		// Token: 0x060051B9 RID: 20921 RVA: 0x001B5BF0 File Offset: 0x001B3DF0
		public static Thing MannedThing(this Pawn pawn)
		{
			if (pawn.Dead)
			{
				return null;
			}
			Thing lastMannedThing = pawn.mindState.lastMannedThing;
			if (lastMannedThing == null || lastMannedThing.TryGetComp<CompMannable>().ManningPawn != pawn)
			{
				return null;
			}
			return lastMannedThing;
		}
	}
}
