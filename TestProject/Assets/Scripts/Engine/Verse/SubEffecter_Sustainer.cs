using System;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000435 RID: 1077
	public class SubEffecter_Sustainer : SubEffecter
	{
		// Token: 0x06001FFA RID: 8186 RVA: 0x000132A9 File Offset: 0x000114A9
		public SubEffecter_Sustainer(SubEffecterDef def, Effecter parent) : base(def, parent)
		{
		}

		// Token: 0x06001FFB RID: 8187 RVA: 0x000C3814 File Offset: 0x000C1A14
		public override void SubEffectTick(TargetInfo A, TargetInfo B)
		{
			this.age++;
			if (this.age > this.def.ticksBeforeSustainerStart)
			{
				if (this.sustainer == null)
				{
					SoundInfo info = SoundInfo.InMap(A, MaintenanceType.PerTick);
					this.sustainer = this.def.soundDef.TrySpawnSustainer(info);
					return;
				}
				this.sustainer.Maintain();
			}
		}

		// Token: 0x040013B2 RID: 5042
		private int age;

		// Token: 0x040013B3 RID: 5043
		private Sustainer sustainer;
	}
}
