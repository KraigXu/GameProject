using System;

namespace Verse
{
	// Token: 0x02000228 RID: 552
	public class DamageWorker_Frostbite : DamageWorker_AddInjury
	{
		// Token: 0x06000F6C RID: 3948 RVA: 0x00059466 File Offset: 0x00057666
		protected override void ApplySpecialEffectsToPart(Pawn pawn, float totalDamage, DamageInfo dinfo, DamageWorker.DamageResult result)
		{
			base.FinalizeAndAddInjury(pawn, totalDamage, dinfo, result);
		}
	}
}
