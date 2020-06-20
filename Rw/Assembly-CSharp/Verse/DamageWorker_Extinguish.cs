using System;
using RimWorld;

namespace Verse
{
	// Token: 0x0200022E RID: 558
	public class DamageWorker_Extinguish : DamageWorker
	{
		// Token: 0x06000F7A RID: 3962 RVA: 0x00059984 File Offset: 0x00057B84
		public override DamageWorker.DamageResult Apply(DamageInfo dinfo, Thing victim)
		{
			DamageWorker.DamageResult result = new DamageWorker.DamageResult();
			Fire fire = victim as Fire;
			if (fire == null || fire.Destroyed)
			{
				return result;
			}
			base.Apply(dinfo, victim);
			fire.fireSize -= dinfo.Amount * 0.01f;
			if (fire.fireSize <= 0.1f)
			{
				fire.Destroy(DestroyMode.Vanish);
			}
			return result;
		}

		// Token: 0x04000B7B RID: 2939
		private const float DamageAmountToFireSizeRatio = 0.01f;
	}
}
