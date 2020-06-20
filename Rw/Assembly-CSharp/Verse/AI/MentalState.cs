using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000549 RID: 1353
	public class MentalState : IExposable
	{
		// Token: 0x17000792 RID: 1938
		// (get) Token: 0x060026AF RID: 9903 RVA: 0x000E3C53 File Offset: 0x000E1E53
		public int Age
		{
			get
			{
				return this.age;
			}
		}

		// Token: 0x17000793 RID: 1939
		// (get) Token: 0x060026B0 RID: 9904 RVA: 0x000E3C5B File Offset: 0x000E1E5B
		public virtual string InspectLine
		{
			get
			{
				return this.def.baseInspectLine;
			}
		}

		// Token: 0x17000794 RID: 1940
		// (get) Token: 0x060026B1 RID: 9905 RVA: 0x0001028D File Offset: 0x0000E48D
		protected virtual bool CanEndBeforeMaxDurationNow
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060026B2 RID: 9906 RVA: 0x000E3C68 File Offset: 0x000E1E68
		public virtual void ExposeData()
		{
			Scribe_Defs.Look<MentalStateDef>(ref this.def, "def");
			Scribe_Values.Look<int>(ref this.age, "age", 0, false);
			Scribe_Values.Look<bool>(ref this.causedByMood, "causedByMood", false, false);
			Scribe_Values.Look<int>(ref this.forceRecoverAfterTicks, "forceRecoverAfterTicks", 0, false);
		}

		// Token: 0x060026B3 RID: 9907 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void PostStart(string reason)
		{
		}

		// Token: 0x060026B4 RID: 9908 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void PreStart()
		{
		}

		// Token: 0x060026B5 RID: 9909 RVA: 0x000E3CBC File Offset: 0x000E1EBC
		public virtual void PostEnd()
		{
			if (!this.def.recoveryMessage.NullOrEmpty() && PawnUtility.ShouldSendNotificationAbout(this.pawn))
			{
				TaggedString taggedString = this.def.recoveryMessage.Formatted(this.pawn.LabelShort, this.pawn.Named("PAWN"));
				if (!taggedString.NullOrEmpty())
				{
					Messages.Message(taggedString.AdjustedFor(this.pawn, "PAWN", true).CapitalizeFirst(), this.pawn, MessageTypeDefOf.SituationResolved, true);
				}
			}
		}

		// Token: 0x060026B6 RID: 9910 RVA: 0x000E3D58 File Offset: 0x000E1F58
		public virtual void MentalStateTick()
		{
			if (this.pawn.IsHashIntervalTick(150))
			{
				this.age += 150;
				if (this.age >= this.def.maxTicksBeforeRecovery || (this.age >= this.def.minTicksBeforeRecovery && this.CanEndBeforeMaxDurationNow && Rand.MTBEventOccurs(this.def.recoveryMtbDays, 60000f, 150f)) || (this.forceRecoverAfterTicks != -1 && this.age >= this.forceRecoverAfterTicks))
				{
					this.RecoverFromState();
					return;
				}
				if (this.def.recoverFromSleep && !this.pawn.Awake())
				{
					this.RecoverFromState();
					return;
				}
			}
		}

		// Token: 0x060026B7 RID: 9911 RVA: 0x000E3E18 File Offset: 0x000E2018
		public void RecoverFromState()
		{
			if (this.pawn.MentalState != this)
			{
				Log.Error(string.Concat(new object[]
				{
					"Recovered from ",
					this.def,
					" but pawn's mental state is not this, it is ",
					this.pawn.MentalState
				}), false);
			}
			if (!this.pawn.Dead)
			{
				this.pawn.mindState.mentalStateHandler.ClearMentalStateDirect();
				if (this.causedByMood && this.def.moodRecoveryThought != null && this.pawn.needs.mood != null)
				{
					this.pawn.needs.mood.thoughts.memories.TryGainMemory(this.def.moodRecoveryThought, null);
				}
				this.pawn.mindState.mentalBreaker.Notify_RecoveredFromMentalState();
				if (this.pawn.story != null && this.pawn.story.traits != null)
				{
					foreach (Trait trait in this.pawn.story.traits.allTraits)
					{
						trait.Notify_MentalStateEndedOn(this.pawn, this.causedByMood);
					}
				}
			}
			if (this.pawn.Spawned)
			{
				this.pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
			}
			this.PostEnd();
		}

		// Token: 0x060026B8 RID: 9912 RVA: 0x00010306 File Offset: 0x0000E506
		public virtual bool ForceHostileTo(Thing t)
		{
			return false;
		}

		// Token: 0x060026B9 RID: 9913 RVA: 0x00010306 File Offset: 0x0000E506
		public virtual bool ForceHostileTo(Faction f)
		{
			return false;
		}

		// Token: 0x060026BA RID: 9914 RVA: 0x000E3F9C File Offset: 0x000E219C
		public EffecterDef CurrentStateEffecter()
		{
			return this.def.stateEffecter;
		}

		// Token: 0x060026BB RID: 9915 RVA: 0x000E3FA9 File Offset: 0x000E21A9
		public virtual RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.SuperActive;
		}

		// Token: 0x060026BC RID: 9916 RVA: 0x000E3FAC File Offset: 0x000E21AC
		public virtual string GetBeginLetterText()
		{
			if (this.def.beginLetter.NullOrEmpty())
			{
				return null;
			}
			return this.def.beginLetter.Formatted(this.pawn.LabelShort, this.pawn.Named("PAWN")).AdjustedFor(this.pawn, "PAWN", true).CapitalizeFirst();
		}

		// Token: 0x060026BD RID: 9917 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void Notify_AttackedTarget(LocalTargetInfo hitTarget)
		{
		}

		// Token: 0x060026BE RID: 9918 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void Notify_SlaughteredAnimal()
		{
		}

		// Token: 0x0400173A RID: 5946
		public Pawn pawn;

		// Token: 0x0400173B RID: 5947
		public MentalStateDef def;

		// Token: 0x0400173C RID: 5948
		private int age;

		// Token: 0x0400173D RID: 5949
		public bool causedByMood;

		// Token: 0x0400173E RID: 5950
		public int forceRecoverAfterTicks = -1;

		// Token: 0x0400173F RID: 5951
		private const int TickInterval = 150;
	}
}
