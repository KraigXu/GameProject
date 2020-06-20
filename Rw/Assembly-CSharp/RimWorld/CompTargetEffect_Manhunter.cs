using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D95 RID: 3477
	public class CompTargetEffect_Manhunter : CompTargetEffect
	{
		// Token: 0x0600549B RID: 21659 RVA: 0x001C3700 File Offset: 0x001C1900
		public override void DoEffectOn(Pawn user, Thing target)
		{
			Pawn pawn = (Pawn)target;
			if (pawn.Dead)
			{
				return;
			}
			pawn.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.Manhunter, null, false, false, null, false);
		}
	}
}
