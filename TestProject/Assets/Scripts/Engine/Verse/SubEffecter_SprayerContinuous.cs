using System;

namespace Verse
{
	// Token: 0x02000432 RID: 1074
	public class SubEffecter_SprayerContinuous : SubEffecter_Sprayer
	{
		// Token: 0x06001FF4 RID: 8180 RVA: 0x000C375A File Offset: 0x000C195A
		public SubEffecter_SprayerContinuous(SubEffecterDef def, Effecter parent) : base(def, parent)
		{
		}

		// Token: 0x06001FF5 RID: 8181 RVA: 0x000C3764 File Offset: 0x000C1964
		public override void SubEffectTick(TargetInfo A, TargetInfo B)
		{
			this.ticksUntilMote--;
			if (this.ticksUntilMote <= 0)
			{
				base.MakeMote(A, B);
				this.ticksUntilMote = this.def.ticksBetweenMotes;
			}
		}

		// Token: 0x040013B1 RID: 5041
		private int ticksUntilMote;
	}
}
