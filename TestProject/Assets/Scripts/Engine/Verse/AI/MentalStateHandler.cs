using System;
using RimWorld;
using RimWorld.Planet;

namespace Verse.AI
{
	// Token: 0x02000545 RID: 1349
	public class MentalStateHandler : IExposable
	{
		// Token: 0x1700078D RID: 1933
		// (get) Token: 0x0600268D RID: 9869 RVA: 0x000E31B9 File Offset: 0x000E13B9
		public bool InMentalState
		{
			get
			{
				return this.curStateInt != null;
			}
		}

		// Token: 0x1700078E RID: 1934
		// (get) Token: 0x0600268E RID: 9870 RVA: 0x000E31C4 File Offset: 0x000E13C4
		public MentalStateDef CurStateDef
		{
			get
			{
				if (this.curStateInt == null)
				{
					return null;
				}
				return this.curStateInt.def;
			}
		}

		// Token: 0x1700078F RID: 1935
		// (get) Token: 0x0600268F RID: 9871 RVA: 0x000E31DB File Offset: 0x000E13DB
		public MentalState CurState
		{
			get
			{
				return this.curStateInt;
			}
		}

		// Token: 0x06002690 RID: 9872 RVA: 0x0000F2A9 File Offset: 0x0000D4A9
		public MentalStateHandler()
		{
		}

		// Token: 0x06002691 RID: 9873 RVA: 0x000E31E3 File Offset: 0x000E13E3
		public MentalStateHandler(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x06002692 RID: 9874 RVA: 0x000E31F4 File Offset: 0x000E13F4
		public void ExposeData()
		{
			Scribe_Deep.Look<MentalState>(ref this.curStateInt, "curState", Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.neverFleeIndividual, "neverFleeIndividual", false, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.curStateInt != null)
				{
					this.curStateInt.pawn = this.pawn;
				}
				if (Current.ProgramState != ProgramState.Entry && this.pawn.Spawned)
				{
					this.pawn.Map.attackTargetsCache.UpdateTarget(this.pawn);
				}
			}
		}

		// Token: 0x06002693 RID: 9875 RVA: 0x000E3278 File Offset: 0x000E1478
		public void Reset()
		{
			this.ClearMentalStateDirect();
		}

		// Token: 0x06002694 RID: 9876 RVA: 0x000E3280 File Offset: 0x000E1480
		public void MentalStateHandlerTick()
		{
			if (this.curStateInt != null)
			{
				if (this.pawn.Downed && this.curStateInt.def.recoverFromDowned)
				{
					Log.Error("In mental state while downed, but not allowed: " + this.pawn, false);
					this.CurState.RecoverFromState();
					return;
				}
				this.curStateInt.MentalStateTick();
			}
		}

		// Token: 0x06002695 RID: 9877 RVA: 0x000E32E4 File Offset: 0x000E14E4
		public bool TryStartMentalState(MentalStateDef stateDef, string reason = null, bool forceWake = false, bool causedByMood = false, Pawn otherPawn = null, bool transitionSilently = false)
		{
			if ((!this.pawn.Spawned && !this.pawn.IsCaravanMember()) || this.CurStateDef == stateDef || this.pawn.Downed || (!forceWake && !this.pawn.Awake()))
			{
				return false;
			}
			if (TutorSystem.TutorialMode && this.pawn.Faction == Faction.OfPlayer)
			{
				return false;
			}
			if (!stateDef.Worker.StateCanOccur(this.pawn))
			{
				return false;
			}
			if (this.curStateInt != null && !transitionSilently)
			{
				this.curStateInt.RecoverFromState();
			}
			MentalState mentalState = (MentalState)Activator.CreateInstance(stateDef.stateClass);
			mentalState.pawn = this.pawn;
			mentalState.def = stateDef;
			mentalState.causedByMood = causedByMood;
			if (otherPawn != null)
			{
				((MentalState_SocialFighting)mentalState).otherPawn = otherPawn;
			}
			mentalState.PreStart();
			if (!transitionSilently)
			{
				if ((this.pawn.IsColonist || this.pawn.HostFaction == Faction.OfPlayer) && stateDef.tale != null)
				{
					TaleRecorder.RecordTale(stateDef.tale, new object[]
					{
						this.pawn
					});
				}
				if (stateDef.IsExtreme && this.pawn.IsPlayerControlledCaravanMember())
				{
					Messages.Message("MessageCaravanMemberHasExtremeMentalBreak".Translate(), this.pawn.GetCaravan(), MessageTypeDefOf.ThreatSmall, true);
				}
				this.pawn.records.Increment(RecordDefOf.TimesInMentalState);
			}
			if (this.pawn.Drafted)
			{
				this.pawn.drafter.Drafted = false;
			}
			this.curStateInt = mentalState;
			if (this.pawn.needs.mood != null)
			{
				this.pawn.needs.mood.thoughts.situational.Notify_SituationalThoughtsDirty();
			}
			if (stateDef != null && stateDef.IsAggro && this.pawn.caller != null)
			{
				this.pawn.caller.Notify_InAggroMentalState();
			}
			if (this.curStateInt != null)
			{
				this.curStateInt.PostStart(reason);
			}
			if (this.pawn.CurJob != null)
			{
				this.pawn.jobs.StopAll(false, true);
			}
			if (this.pawn.Spawned)
			{
				this.pawn.Map.attackTargetsCache.UpdateTarget(this.pawn);
			}
			if (this.pawn.Spawned && forceWake && !this.pawn.Awake())
			{
				this.pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
			}
			if (!transitionSilently && PawnUtility.ShouldSendNotificationAbout(this.pawn))
			{
				string text = mentalState.GetBeginLetterText();
				if (!text.NullOrEmpty())
				{
					string str = (stateDef.beginLetterLabel ?? stateDef.LabelCap).CapitalizeFirst() + ": " + this.pawn.LabelShortCap;
					if (!reason.NullOrEmpty())
					{
						text = text + "\n\n" + reason;
					}
					Find.LetterStack.ReceiveLetter(str, text, stateDef.beginLetterDef, this.pawn, null, null, null, null);
				}
			}
			return true;
		}

		// Token: 0x06002696 RID: 9878 RVA: 0x000E35F8 File Offset: 0x000E17F8
		public void Notify_DamageTaken(DamageInfo dinfo)
		{
			if (!this.neverFleeIndividual && this.pawn.Spawned && this.pawn.MentalStateDef == null && !this.pawn.Downed && dinfo.Def.ExternalViolenceFor(this.pawn) && this.pawn.RaceProps.Humanlike && this.pawn.mindState.canFleeIndividual)
			{
				float lerpPct = (float)(this.pawn.HashOffset() % 100) / 100f;
				float num = this.pawn.kindDef.fleeHealthThresholdRange.LerpThroughRange(lerpPct);
				if (this.pawn.health.summaryHealth.SummaryHealthPercent < num && this.pawn.Faction != Faction.OfPlayer && this.pawn.HostFaction == null)
				{
					this.TryStartMentalState(MentalStateDefOf.PanicFlee, null, false, false, null, false);
				}
			}
		}

		// Token: 0x06002697 RID: 9879 RVA: 0x000E36F4 File Offset: 0x000E18F4
		internal void ClearMentalStateDirect()
		{
			if (this.curStateInt == null)
			{
				return;
			}
			this.curStateInt = null;
			QuestUtility.SendQuestTargetSignals(this.pawn.questTags, "ExitMentalState", this.pawn.Named("SUBJECT"));
			if (this.pawn.Spawned)
			{
				this.pawn.Map.attackTargetsCache.UpdateTarget(this.pawn);
			}
		}

		// Token: 0x0400172F RID: 5935
		private Pawn pawn;

		// Token: 0x04001730 RID: 5936
		private MentalState curStateInt;

		// Token: 0x04001731 RID: 5937
		public bool neverFleeIndividual;
	}
}
