using System;
using RimWorld;

namespace Verse.AI
{
	
	public class MentalState_Jailbreaker : MentalState
	{
		
		public override void MentalStateTick()
		{
			base.MentalStateTick();
			if (this.pawn.IsHashIntervalTick(500) && this.pawn.CurJobDef != JobDefOf.InducePrisonerToEscape && JailbreakerMentalStateUtility.FindPrisoner(this.pawn) == null)
			{
				base.RecoverFromState();
			}
		}

		
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

		
		private const int NoPrisonerToFreeCheckInterval = 500;
	}
}
