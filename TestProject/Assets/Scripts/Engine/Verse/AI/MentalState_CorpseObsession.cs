using System;

namespace Verse.AI
{
	
	public class MentalState_CorpseObsession : MentalState
	{
		
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Corpse>(ref this.corpse, "corpse", false);
			Scribe_Values.Look<bool>(ref this.alreadyHauledCorpse, "alreadyHauledCorpse", false, false);
		}

		
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

		
		public override void PostStart(string reason)
		{
			base.PostStart(reason);
			this.corpse = CorpseObsessionMentalStateUtility.GetClosestCorpseToDigUp(this.pawn);
		}

		
		public void Notify_CorpseHauled()
		{
			this.alreadyHauledCorpse = true;
		}

		
		public Corpse corpse;

		
		public bool alreadyHauledCorpse;

		
		private const int AnyCorpseStillValidCheckInterval = 500;
	}
}
