using System;

namespace Verse
{
	// Token: 0x0200004D RID: 77
	public class SubEffecter_DrifterEmoteContinuous : SubEffecter_DrifterEmote
	{
		// Token: 0x060003AD RID: 941 RVA: 0x00013409 File Offset: 0x00011609
		public SubEffecter_DrifterEmoteContinuous(SubEffecterDef def, Effecter parent) : base(def, parent)
		{
		}

		// Token: 0x060003AE RID: 942 RVA: 0x00013413 File Offset: 0x00011613
		public override void SubEffectTick(TargetInfo A, TargetInfo B)
		{
			this.ticksUntilMote--;
			if (this.ticksUntilMote <= 0)
			{
				base.MakeMote(A);
				this.ticksUntilMote = this.def.ticksBetweenMotes;
			}
		}

		// Token: 0x04000107 RID: 263
		private int ticksUntilMote;
	}
}
