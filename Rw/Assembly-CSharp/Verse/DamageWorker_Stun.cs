using System;

namespace Verse
{
	// Token: 0x0200022B RID: 555
	public class DamageWorker_Stun : DamageWorker
	{
		// Token: 0x06000F74 RID: 3956 RVA: 0x0005975D File Offset: 0x0005795D
		public override DamageWorker.DamageResult Apply(DamageInfo dinfo, Thing victim)
		{
			DamageWorker.DamageResult damageResult = base.Apply(dinfo, victim);
			damageResult.stunned = true;
			return damageResult;
		}
	}
}
