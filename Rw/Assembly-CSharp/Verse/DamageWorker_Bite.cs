using System;

namespace Verse
{
	// Token: 0x02000224 RID: 548
	public class DamageWorker_Bite : DamageWorker_AddInjury
	{
		// Token: 0x06000F5F RID: 3935 RVA: 0x00058B66 File Offset: 0x00056D66
		protected override BodyPartRecord ChooseHitPart(DamageInfo dinfo, Pawn pawn)
		{
			return pawn.health.hediffSet.GetRandomNotMissingPart(dinfo.Def, dinfo.Height, BodyPartDepth.Outside, null);
		}
	}
}
