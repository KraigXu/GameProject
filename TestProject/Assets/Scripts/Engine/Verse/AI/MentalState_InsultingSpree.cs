using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000546 RID: 1350
	public abstract class MentalState_InsultingSpree : MentalState
	{
		// Token: 0x06002698 RID: 9880 RVA: 0x000E375E File Offset: 0x000E195E
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.target, "target", false);
			Scribe_Values.Look<bool>(ref this.insultedTargetAtLeastOnce, "insultedTargetAtLeastOnce", false, false);
			Scribe_Values.Look<int>(ref this.lastInsultTicks, "lastInsultTicks", 0, false);
		}

		// Token: 0x06002699 RID: 9881 RVA: 0x00010306 File Offset: 0x0000E506
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}

		// Token: 0x04001732 RID: 5938
		public Pawn target;

		// Token: 0x04001733 RID: 5939
		public bool insultedTargetAtLeastOnce;

		// Token: 0x04001734 RID: 5940
		public int lastInsultTicks = -999999;
	}
}
