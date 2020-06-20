using System;
using Verse.Sound;

namespace Verse
{
	// Token: 0x0200042F RID: 1071
	public class SubEffecter_SoundIntermittent : SubEffecter
	{
		// Token: 0x06001FEE RID: 8174 RVA: 0x000C3347 File Offset: 0x000C1547
		public SubEffecter_SoundIntermittent(SubEffecterDef def, Effecter parent) : base(def, parent)
		{
			this.ticksUntilSound = def.intermittentSoundInterval.RandomInRange;
		}

		// Token: 0x06001FEF RID: 8175 RVA: 0x000C3364 File Offset: 0x000C1564
		public override void SubEffectTick(TargetInfo A, TargetInfo B)
		{
			this.ticksUntilSound--;
			if (this.ticksUntilSound <= 0)
			{
				this.def.soundDef.PlayOneShot(A);
				this.ticksUntilSound = this.def.intermittentSoundInterval.RandomInRange;
			}
		}

		// Token: 0x040013B0 RID: 5040
		protected int ticksUntilSound;
	}
}
