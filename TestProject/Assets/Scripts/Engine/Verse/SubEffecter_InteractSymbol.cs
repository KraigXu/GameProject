using System;
using RimWorld;

namespace Verse
{
	// Token: 0x0200042E RID: 1070
	public class SubEffecter_InteractSymbol : SubEffecter
	{
		// Token: 0x06001FEB RID: 8171 RVA: 0x000132A9 File Offset: 0x000114A9
		public SubEffecter_InteractSymbol(SubEffecterDef def, Effecter parent) : base(def, parent)
		{
		}

		// Token: 0x06001FEC RID: 8172 RVA: 0x000C3302 File Offset: 0x000C1502
		public override void SubEffectTick(TargetInfo A, TargetInfo B)
		{
			if (this.interactMote == null)
			{
				this.interactMote = MoteMaker.MakeInteractionOverlay(this.def.moteDef, A, B);
			}
		}

		// Token: 0x06001FED RID: 8173 RVA: 0x000C3324 File Offset: 0x000C1524
		public override void SubCleanup()
		{
			if (this.interactMote != null && !this.interactMote.Destroyed)
			{
				this.interactMote.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x040013AF RID: 5039
		private Mote interactMote;
	}
}
