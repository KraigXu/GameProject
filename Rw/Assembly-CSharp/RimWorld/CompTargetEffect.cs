using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D8F RID: 3471
	public abstract class CompTargetEffect : ThingComp
	{
		// Token: 0x0600548F RID: 21647
		public abstract void DoEffectOn(Pawn user, Thing target);
	}
}
