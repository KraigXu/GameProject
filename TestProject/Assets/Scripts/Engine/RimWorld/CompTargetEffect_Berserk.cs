using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D90 RID: 3472
	public class CompTargetEffect_Berserk : CompTargetEffect
	{
		// Token: 0x06005491 RID: 21649 RVA: 0x001C353C File Offset: 0x001C173C
		public override void DoEffectOn(Pawn user, Thing target)
		{
			Pawn pawn = (Pawn)target;
			if (pawn.Dead)
			{
				return;
			}
			pawn.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.Berserk, null, true, false, null, false);
		}
	}
}
