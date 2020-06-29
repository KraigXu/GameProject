using System;
using RimWorld;

namespace Verse.AI
{
	
	public class MentalState_SocialFighting : MentalState
	{
		
		// (get) Token: 0x060026FA RID: 9978 RVA: 0x000E48B5 File Offset: 0x000E2AB5
		private bool ShouldStop
		{
			get
			{
				return !this.otherPawn.Spawned || this.otherPawn.Dead || this.otherPawn.Downed || !this.IsOtherPawnSocialFightingWithMe;
			}
		}

		
		// (get) Token: 0x060026FB RID: 9979 RVA: 0x000E48EC File Offset: 0x000E2AEC
		private bool IsOtherPawnSocialFightingWithMe
		{
			get
			{
				if (!this.otherPawn.InMentalState)
				{
					return false;
				}
				MentalState_SocialFighting mentalState_SocialFighting = this.otherPawn.MentalState as MentalState_SocialFighting;
				return mentalState_SocialFighting != null && mentalState_SocialFighting.otherPawn == this.pawn;
			}
		}

		
		public override void MentalStateTick()
		{
			if (this.ShouldStop)
			{
				base.RecoverFromState();
				return;
			}
			base.MentalStateTick();
		}

		
		public override void PostEnd()
		{
			base.PostEnd();
			this.pawn.jobs.StopAll(false, true);
			this.pawn.mindState.meleeThreat = null;
			if (this.IsOtherPawnSocialFightingWithMe)
			{
				this.otherPawn.MentalState.RecoverFromState();
			}
			if ((PawnUtility.ShouldSendNotificationAbout(this.pawn) || PawnUtility.ShouldSendNotificationAbout(this.otherPawn)) && this.pawn.thingIDNumber < this.otherPawn.thingIDNumber)
			{
				Messages.Message("MessageNoLongerSocialFighting".Translate(this.pawn.LabelShort, this.otherPawn.LabelShort, this.pawn.Named("PAWN1"), this.otherPawn.Named("PAWN2")), this.pawn, MessageTypeDefOf.SituationResolved, true);
			}
			if (!this.pawn.Dead && this.pawn.needs.mood != null && !this.otherPawn.Dead)
			{
				ThoughtDef def;
				if (Rand.Value < 0.5f)
				{
					def = ThoughtDefOf.HadAngeringFight;
				}
				else
				{
					def = ThoughtDefOf.HadCatharticFight;
				}
				this.pawn.needs.mood.thoughts.memories.TryGainMemory(def, this.otherPawn);
			}
		}

		
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.otherPawn, "otherPawn", false);
		}

		
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}

		
		public Pawn otherPawn;
	}
}
