using System;

namespace Verse.AI
{
	// Token: 0x0200054A RID: 1354
	public class MentalState_CorpseObsession : MentalState
	{
		// Token: 0x060026C0 RID: 9920 RVA: 0x000E402D File Offset: 0x000E222D
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Corpse>(ref this.corpse, "corpse", false);
			Scribe_Values.Look<bool>(ref this.alreadyHauledCorpse, "alreadyHauledCorpse", false, false);
		}

		// Token: 0x060026C1 RID: 9921 RVA: 0x000E4058 File Offset: 0x000E2258
		public override void MentalStateTick()
		{
			if (this.alreadyHauledCorpse)
			{
				base.MentalStateTick();
				return;
			}
			bool flag = false;
			if (this.pawn.IsHashIntervalTick(500) && !CorpseObsessionMentalStateUtility.IsCorpseValid(this.corpse, this.pawn, false))
			{
				this.corpse = CorpseObsessionMentalStateUtility.GetClosestCorpseToDigUp(this.pawn);
				if (this.corpse == null)
				{
					base.RecoverFromState();
					flag = true;
				}
			}
			if (!flag)
			{
				base.MentalStateTick();
			}
		}

		// Token: 0x060026C2 RID: 9922 RVA: 0x000E40C6 File Offset: 0x000E22C6
		public override void PostStart(string reason)
		{
			base.PostStart(reason);
			this.corpse = CorpseObsessionMentalStateUtility.GetClosestCorpseToDigUp(this.pawn);
		}

		// Token: 0x060026C3 RID: 9923 RVA: 0x000E40E0 File Offset: 0x000E22E0
		public void Notify_CorpseHauled()
		{
			this.alreadyHauledCorpse = true;
		}

		// Token: 0x04001740 RID: 5952
		public Corpse corpse;

		// Token: 0x04001741 RID: 5953
		public bool alreadyHauledCorpse;

		// Token: 0x04001742 RID: 5954
		private const int AnyCorpseStillValidCheckInterval = 500;
	}
}
