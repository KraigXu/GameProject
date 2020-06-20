using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D2B RID: 3371
	public class CompMelter : ThingComp
	{
		// Token: 0x060051F3 RID: 20979 RVA: 0x001B64C8 File Offset: 0x001B46C8
		public override void CompTickRare()
		{
			float ambientTemperature = this.parent.AmbientTemperature;
			if (ambientTemperature < 0f)
			{
				return;
			}
			int num = GenMath.RoundRandom(0.15f * (ambientTemperature / 10f));
			if (num > 0)
			{
				this.parent.TakeDamage(new DamageInfo(DamageDefOf.Rotting, (float)num, 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null));
			}
		}

		// Token: 0x04002D2B RID: 11563
		private const float MeltPerIntervalPer10Degrees = 0.15f;
	}
}
