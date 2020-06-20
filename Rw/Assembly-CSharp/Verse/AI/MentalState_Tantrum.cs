using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x0200055C RID: 1372
	public abstract class MentalState_Tantrum : MentalState
	{
		// Token: 0x06002709 RID: 9993 RVA: 0x000E4BC5 File Offset: 0x000E2DC5
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Thing>(ref this.target, "target", false);
			Scribe_Values.Look<bool>(ref this.hitTargetAtLeastOnce, "hitTargetAtLeastOnce", false, false);
		}

		// Token: 0x0600270A RID: 9994 RVA: 0x00010306 File Offset: 0x0000E506
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}

		// Token: 0x0600270B RID: 9995 RVA: 0x000E4BF0 File Offset: 0x000E2DF0
		public override void Notify_AttackedTarget(LocalTargetInfo hitTarget)
		{
			base.Notify_AttackedTarget(hitTarget);
			if (this.target != null && hitTarget.Thing == this.target)
			{
				this.hitTargetAtLeastOnce = true;
			}
		}

		// Token: 0x04001752 RID: 5970
		public Thing target;

		// Token: 0x04001753 RID: 5971
		protected bool hitTargetAtLeastOnce;
	}
}
