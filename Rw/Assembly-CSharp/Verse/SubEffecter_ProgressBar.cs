using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000050 RID: 80
	public class SubEffecter_ProgressBar : SubEffecter
	{
		// Token: 0x060003B3 RID: 947 RVA: 0x000132A9 File Offset: 0x000114A9
		public SubEffecter_ProgressBar(SubEffecterDef def, Effecter parent) : base(def, parent)
		{
		}

		// Token: 0x060003B4 RID: 948 RVA: 0x00013478 File Offset: 0x00011678
		public override void SubEffectTick(TargetInfo A, TargetInfo B)
		{
			if (this.mote == null)
			{
				this.mote = (MoteProgressBar)MoteMaker.MakeInteractionOverlay(this.def.moteDef, A, B);
				this.mote.exactScale.x = 0.68f;
				this.mote.exactScale.z = 0.12f;
			}
		}

		// Token: 0x060003B5 RID: 949 RVA: 0x000134D4 File Offset: 0x000116D4
		public override void SubCleanup()
		{
			if (this.mote != null && !this.mote.Destroyed)
			{
				this.mote.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x04000108 RID: 264
		public MoteProgressBar mote;

		// Token: 0x04000109 RID: 265
		private const float Width = 0.68f;

		// Token: 0x0400010A RID: 266
		private const float Height = 0.12f;
	}
}
