using System;
using RimWorld;

namespace Verse.AI
{
	
	public class MentalState : IExposable
	{
		
		
		public int Age
		{
			get
			{
				return this.age;
			}
		}

		
		
		public virtual string InspectLine
		{
			get
			{
				return this.def.baseInspectLine;
			}
		}

		
		
		protected virtual bool CanEndBeforeMaxDurationNow
		{
			get
			{
				return true;
			}
		}

		
		public virtual void ExposeData()
		{
			Scribe_Defs.Look<MentalStateDef>(ref this.def, "def");
			Scribe_Values.Look<int>(ref this.age, "age", 0, false);
			Scribe_Values.Look<bool>(ref this.causedByMood, "causedByMood", false, false);
			Scribe_Values.Look<int>(ref this.forceRecoverAfterTicks, "forceRecoverAfterTicks", 0, false);
		}

		
		public virtual void PostStart(string reason)
		{
		}

		
		public virtual void PreStart()
		{
		}

		
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

		
		public virtual bool ForceHostileTo(Thing t)
		{
			return false;
		}

		
		public virtual bool ForceHostileTo(Faction f)
		{
			return false;
		}

		
		public EffecterDef CurrentStateEffecter()
		{
			return this.def.stateEffecter;
		}

		
		public virtual RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.SuperActive;
		}

		
		public virtual string GetBeginLetterText()
		{
			if (this.def.beginLetter.NullOrEmpty())
			{
				return null;
			}
			return this.def.beginLetter.Formatted(this.pawn.LabelShort, this.pawn.Named("PAWN")).AdjustedFor(this.pawn, "PAWN", true).CapitalizeFirst();
		}

		
		public virtual void Notify_AttackedTarget(LocalTargetInfo hitTarget)
		{
		}

		
		public virtual void Notify_SlaughteredAnimal()
		{
		}

		
		public Pawn pawn;

		
		public MentalStateDef def;

		
		private int age;

		
		public bool causedByMood;

		
		public int forceRecoverAfterTicks = -1;

		
		private const int TickInterval = 150;
	}
}
