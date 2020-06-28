using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000554 RID: 1364
	public class MentalState_WanderOwnRoom : MentalState
	{
		// Token: 0x060026EE RID: 9966 RVA: 0x000E4834 File Offset: 0x000E2A34
		public override void PostStart(string reason)
		{
			base.PostStart(reason);
			if (this.pawn.ownership.OwnedBed != null)
			{
				this.target = this.pawn.ownership.OwnedBed.Position;
				return;
			}
			this.target = this.pawn.Position;
		}

		// Token: 0x060026EF RID: 9967 RVA: 0x000E4888 File Offset: 0x000E2A88
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<IntVec3>(ref this.target, "target", default(IntVec3), false);
		}

		// Token: 0x060026F0 RID: 9968 RVA: 0x00010306 File Offset: 0x0000E506
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}

		// Token: 0x0400174E RID: 5966
		public IntVec3 target;
	}
}
