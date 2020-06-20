using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x0200054B RID: 1355
	public class MentalState_Jailbreaker : MentalState
	{
		// Token: 0x060026C5 RID: 9925 RVA: 0x000E40F1 File Offset: 0x000E22F1
		public override void MentalStateTick()
		{
			base.MentalStateTick();
			if (this.pawn.IsHashIntervalTick(500) && this.pawn.CurJobDef != JobDefOf.InducePrisonerToEscape && JailbreakerMentalStateUtility.FindPrisoner(this.pawn) == null)
			{
				base.RecoverFromState();
			}
		}

		// Token: 0x060026C6 RID: 9926 RVA: 0x000E4130 File Offset: 0x000E2330
		public void Notify_InducedPrisonerToEscape()
		{
			if (MentalStateDefOf.Wander_OwnRoom.Worker.StateCanOccur(this.pawn))
			{
				this.pawn.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.Wander_OwnRoom, null, false, this.causedByMood, null, true);
				return;
			}
			if (MentalStateDefOf.Wander_Sad.Worker.StateCanOccur(this.pawn))
			{
				this.pawn.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.Wander_Sad, null, false, this.causedByMood, null, true);
				return;
			}
			base.RecoverFromState();
		}

		// Token: 0x04001743 RID: 5955
		private const int NoPrisonerToFreeCheckInterval = 500;
	}
}
