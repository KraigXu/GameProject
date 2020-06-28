using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000662 RID: 1634
	public static class HuntJobUtility
	{
		// Token: 0x06002C96 RID: 11414 RVA: 0x000FDEAC File Offset: 0x000FC0AC
		public static bool WasKilledByHunter(Pawn pawn, DamageInfo? dinfo)
		{
			if (dinfo == null)
			{
				return false;
			}
			Pawn pawn2 = dinfo.Value.Instigator as Pawn;
			if (pawn2 == null || pawn2.CurJob == null)
			{
				return false;
			}
			JobDriver_Hunt jobDriver_Hunt = pawn2.jobs.curDriver as JobDriver_Hunt;
			return jobDriver_Hunt != null && jobDriver_Hunt.Victim == pawn;
		}
	}
}
