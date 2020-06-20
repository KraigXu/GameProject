using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000AC1 RID: 2753
	public class Ability : IVerbOwner, IExposable
	{
		// Token: 0x17000B92 RID: 2962
		// (get) Token: 0x06004149 RID: 16713 RVA: 0x0015D575 File Offset: 0x0015B775
		public Verb verb
		{
			get
			{
				return this.verbTracker.PrimaryVerb;
			}
		}

		// Token: 0x0600414A RID: 16714 RVA: 0x0015D582 File Offset: 0x0015B782
		public string UniqueVerbOwnerID()
		{
			return "Ability_" + this.def.label + this.pawn.ThingID;
		}

		// Token: 0x0600414B RID: 16715 RVA: 0x0001028D File Offset: 0x0000E48D
		public bool VerbsStillUsableBy(Pawn p)
		{
			return true;
		}

		// Token: 0x17000B93 RID: 2963
		// (get) Token: 0x0600414C RID: 16716 RVA: 0x0015D5A4 File Offset: 0x0015B7A4
		// (set) Token: 0x0600414D RID: 16717 RVA: 0x0015D5AC File Offset: 0x0015B7AC
		public List<Tool> Tools { get; private set; }

		// Token: 0x17000B94 RID: 2964
		// (get) Token: 0x0600414E RID: 16718 RVA: 0x0015D5B5 File Offset: 0x0015B7B5
		public Thing ConstantCaster
		{
			get
			{
				return this.pawn;
			}
		}

		// Token: 0x17000B95 RID: 2965
		// (get) Token: 0x0600414F RID: 16719 RVA: 0x0015D5BD File Offset: 0x0015B7BD
		public List<VerbProperties> VerbProperties
		{
			get
			{
				return new List<VerbProperties>
				{
					this.def.verbProperties
				};
			}
		}

		// Token: 0x17000B96 RID: 2966
		// (get) Token: 0x06004150 RID: 16720 RVA: 0x0015D5D5 File Offset: 0x0015B7D5
		public ImplementOwnerTypeDef ImplementOwnerTypeDef
		{
			get
			{
				return ImplementOwnerTypeDefOf.NativeVerb;
			}
		}

		// Token: 0x17000B97 RID: 2967
		// (get) Token: 0x06004151 RID: 16721 RVA: 0x0015D5DC File Offset: 0x0015B7DC
		public int CooldownTicksRemaining
		{
			get
			{
				return this.cooldownTicks;
			}
		}

		// Token: 0x17000B98 RID: 2968
		// (get) Token: 0x06004152 RID: 16722 RVA: 0x0015D5E4 File Offset: 0x0015B7E4
		public int CooldownTicksTotal
		{
			get
			{
				return this.cooldownTicksDuration;
			}
		}

		// Token: 0x17000B99 RID: 2969
		// (get) Token: 0x06004153 RID: 16723 RVA: 0x0015D5EC File Offset: 0x0015B7EC
		public VerbTracker VerbTracker
		{
			get
			{
				if (this.verbTracker == null)
				{
					this.verbTracker = new VerbTracker(this);
				}
				return this.verbTracker;
			}
		}

		// Token: 0x17000B9A RID: 2970
		// (get) Token: 0x06004154 RID: 16724 RVA: 0x0015D608 File Offset: 0x0015B808
		public bool HasCooldown
		{
			get
			{
				return this.def.cooldownTicksRange != default(IntRange);
			}
		}

		// Token: 0x17000B9B RID: 2971
		// (get) Token: 0x06004155 RID: 16725 RVA: 0x0015D62E File Offset: 0x0015B82E
		public virtual bool CanCast
		{
			get
			{
				return this.cooldownTicks <= 0;
			}
		}

		// Token: 0x17000B9C RID: 2972
		// (get) Token: 0x06004156 RID: 16726 RVA: 0x0015D63C File Offset: 0x0015B83C
		public virtual bool CanQueueCast
		{
			get
			{
				return !this.HasCooldown || ((this.pawn.jobs.curJob == null || this.pawn.jobs.curJob.verbToUse != this.verb) && !(from qj in this.pawn.jobs.jobQueue
				where qj.job.verbToUse == this.verb
				select qj).Any<QueuedJob>());
			}
		}

		// Token: 0x17000B9D RID: 2973
		// (get) Token: 0x06004157 RID: 16727 RVA: 0x0015D6AD File Offset: 0x0015B8AD
		public List<CompAbilityEffect> EffectComps
		{
			get
			{
				if (this.effectComps == null)
				{
					this.effectComps = this.CompsOfType<CompAbilityEffect>().ToList<CompAbilityEffect>();
				}
				return this.effectComps;
			}
		}

		// Token: 0x06004158 RID: 16728 RVA: 0x0015D6CE File Offset: 0x0015B8CE
		public Ability(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x06004159 RID: 16729 RVA: 0x0015D6DD File Offset: 0x0015B8DD
		public Ability(Pawn pawn, AbilityDef def)
		{
			this.pawn = pawn;
			this.def = def;
			this.Initialize();
		}

		// Token: 0x0600415A RID: 16730 RVA: 0x0015D6FC File Offset: 0x0015B8FC
		public virtual bool CanApplyOn(LocalTargetInfo target)
		{
			using (List<CompAbilityEffect>.Enumerator enumerator = this.effectComps.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!enumerator.Current.CanApplyOn(target, null))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x0600415B RID: 16731 RVA: 0x0015D75C File Offset: 0x0015B95C
		public virtual bool Activate(LocalTargetInfo target, LocalTargetInfo dest)
		{
			if (!this.EffectComps.Any<CompAbilityEffect>())
			{
				return false;
			}
			this.ApplyEffects(this.EffectComps, this.GetAffectedTargets(target), dest);
			Find.BattleLog.Add(new BattleLogEntry_AbilityUsed(this.pawn, target.Thing, this.def, RulePackDefOf.Event_AbilityUsed));
			return true;
		}

		// Token: 0x0600415C RID: 16732 RVA: 0x0015D7B4 File Offset: 0x0015B9B4
		public IEnumerable<LocalTargetInfo> GetAffectedTargets(LocalTargetInfo target)
		{
			if (this.def.HasAreaOfEffect && this.def.canUseAoeToGetTargets)
			{
				foreach (LocalTargetInfo localTargetInfo in from t in GenRadial.RadialDistinctThingsAround(target.Cell, this.pawn.Map, this.def.EffectRadius, true)
				where this.verb.targetParams.CanTarget(t)
				select new LocalTargetInfo(t))
				{
					yield return localTargetInfo;
				}
				IEnumerator<LocalTargetInfo> enumerator = null;
			}
			else
			{
				yield return target;
			}
			yield break;
			yield break;
		}

		// Token: 0x0600415D RID: 16733 RVA: 0x0015D7CC File Offset: 0x0015B9CC
		public virtual void QueueCastingJob(LocalTargetInfo target, LocalTargetInfo destination)
		{
			if (!this.CanQueueCast || !this.CanApplyOn(target))
			{
				return;
			}
			Job job = JobMaker.MakeJob(this.def.jobDef ?? JobDefOf.CastAbilityOnThing);
			job.verbToUse = this.verb;
			job.targetA = target;
			job.targetB = destination;
			this.pawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
		}

		// Token: 0x0600415E RID: 16734 RVA: 0x0015D834 File Offset: 0x0015BA34
		public virtual bool GizmoDisabled(out string reason)
		{
			if (!this.CanCast)
			{
				reason = "AbilityOnCooldown".Translate(this.cooldownTicks.ToStringTicksToPeriod(true, false, false, true));
				return true;
			}
			if (!this.CanQueueCast)
			{
				reason = "AbilityAlreadyQueued".Translate();
				return true;
			}
			if (!this.pawn.Drafted && this.def.disableGizmoWhileUndrafted)
			{
				reason = "AbilityDisabledUndrafted".Translate();
				return true;
			}
			if (this.pawn.Downed)
			{
				reason = "CommandDisabledUnconscious".TranslateWithBackup("CommandCallRoyalAidUnconscious").Formatted(this.pawn);
				return true;
			}
			for (int i = 0; i < this.comps.Count; i++)
			{
				if (this.comps[i].GizmoDisabled(out reason))
				{
					return true;
				}
			}
			reason = null;
			return false;
		}

		// Token: 0x0600415F RID: 16735 RVA: 0x0015D924 File Offset: 0x0015BB24
		public virtual void AbilityTick()
		{
			this.VerbTracker.VerbsTick();
			if (this.def.warmupMote != null && this.verb.WarmingUp)
			{
				if (this.warmupMote == null || this.warmupMote.Destroyed)
				{
					this.warmupMote = MoteMaker.MakeStaticMote(this.pawn.DrawPos + this.def.moteDrawOffset, this.pawn.Map, this.def.warmupMote, 1f);
				}
				else
				{
					this.warmupMote.Maintain();
				}
			}
			if (this.verb.WarmingUp && !this.CanApplyOn(this.verb.CurrentTarget))
			{
				Stance_Warmup warmupStance = this.verb.WarmupStance;
				if (warmupStance != null)
				{
					warmupStance.Interrupt();
				}
				this.verb.Reset();
			}
			if (this.cooldownTicks > 0)
			{
				this.cooldownTicks--;
				if (this.cooldownTicks == 0 && this.def.sendLetterOnCooldownComplete)
				{
					Find.LetterStack.ReceiveLetter("AbilityReadyLabel".Translate(this.def.LabelCap), "AbilityReadyText".Translate(this.pawn, this.def.label), LetterDefOf.NeutralEvent, new LookTargets(this.pawn), null, null, null, null);
				}
			}
		}

		// Token: 0x06004160 RID: 16736 RVA: 0x0015DA88 File Offset: 0x0015BC88
		public void DrawEffectPreviews(LocalTargetInfo target)
		{
			for (int i = 0; i < this.EffectComps.Count; i++)
			{
				this.EffectComps[i].DrawEffectPreview(target);
			}
		}

		// Token: 0x06004161 RID: 16737 RVA: 0x0015DABD File Offset: 0x0015BCBD
		public virtual IEnumerable<Command> GetGizmos()
		{
			if (this.gizmo == null)
			{
				this.gizmo = (Command)Activator.CreateInstance(this.def.gizmoClass, new object[]
				{
					this
				});
			}
			yield return this.gizmo;
			if (Prefs.DevMode && this.cooldownTicks > 0)
			{
				yield return new Command_Action
				{
					defaultLabel = "Reset cooldown",
					action = delegate
					{
						this.cooldownTicks = 0;
					}
				};
			}
			yield break;
		}

		// Token: 0x06004162 RID: 16738 RVA: 0x0015DAD0 File Offset: 0x0015BCD0
		private void ApplyEffects(IEnumerable<CompAbilityEffect> effects, IEnumerable<LocalTargetInfo> targets, LocalTargetInfo dest)
		{
			foreach (LocalTargetInfo target in targets)
			{
				this.ApplyEffects(effects, target, dest);
			}
			if (this.HasCooldown)
			{
				this.StartCooldown(this.def.cooldownTicksRange.RandomInRange);
			}
		}

		// Token: 0x06004163 RID: 16739 RVA: 0x0015DB38 File Offset: 0x0015BD38
		public void StartCooldown(int ticks)
		{
			this.cooldownTicksDuration = ticks;
			this.cooldownTicks = this.cooldownTicksDuration;
		}

		// Token: 0x06004164 RID: 16740 RVA: 0x0015DB50 File Offset: 0x0015BD50
		protected virtual void ApplyEffects(IEnumerable<CompAbilityEffect> effects, LocalTargetInfo target, LocalTargetInfo dest)
		{
			foreach (CompAbilityEffect compAbilityEffect in effects)
			{
				compAbilityEffect.Apply(target, dest);
			}
		}

		// Token: 0x06004165 RID: 16741 RVA: 0x0015DB98 File Offset: 0x0015BD98
		public IEnumerable<T> CompsOfType<T>() where T : AbilityComp
		{
			if (this.comps == null)
			{
				return null;
			}
			return (from c in this.comps
			where c is T
			select c).Cast<T>();
		}

		// Token: 0x06004166 RID: 16742 RVA: 0x0015DBD4 File Offset: 0x0015BDD4
		public T CompOfType<T>() where T : AbilityComp
		{
			if (this.comps == null)
			{
				return default(T);
			}
			return this.comps.FirstOrDefault((AbilityComp c) => c is T) as T;
		}

		// Token: 0x06004167 RID: 16743 RVA: 0x0015DC28 File Offset: 0x0015BE28
		public void Initialize()
		{
			if (this.def.comps.Any<AbilityCompProperties>())
			{
				this.comps = new List<AbilityComp>();
				for (int i = 0; i < this.def.comps.Count; i++)
				{
					AbilityComp abilityComp = null;
					try
					{
						abilityComp = (AbilityComp)Activator.CreateInstance(this.def.comps[i].compClass);
						abilityComp.parent = this;
						this.comps.Add(abilityComp);
						abilityComp.Initialize(this.def.comps[i]);
					}
					catch (Exception arg)
					{
						Log.Error("Could not instantiate or initialize an AbilityComp: " + arg, false);
						this.comps.Remove(abilityComp);
					}
				}
			}
			Verb_CastAbility verb_CastAbility = this.VerbTracker.PrimaryVerb as Verb_CastAbility;
			if (verb_CastAbility != null)
			{
				verb_CastAbility.ability = this;
			}
		}

		// Token: 0x06004168 RID: 16744 RVA: 0x0015DD10 File Offset: 0x0015BF10
		public virtual void ExposeData()
		{
			Scribe_Defs.Look<AbilityDef>(ref this.def, "def");
			if (this.def == null)
			{
				return;
			}
			Scribe_Deep.Look<VerbTracker>(ref this.verbTracker, "verbTracker", new object[]
			{
				this
			});
			Scribe_Values.Look<int>(ref this.cooldownTicks, "cooldownTicks", 0, false);
			Scribe_Values.Look<int>(ref this.cooldownTicksDuration, "cooldownTicksDuration", 0, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.Initialize();
			}
		}

		// Token: 0x040025DF RID: 9695
		public Pawn pawn;

		// Token: 0x040025E0 RID: 9696
		public AbilityDef def;

		// Token: 0x040025E1 RID: 9697
		public List<AbilityComp> comps;

		// Token: 0x040025E2 RID: 9698
		protected Command gizmo;

		// Token: 0x040025E3 RID: 9699
		private VerbTracker verbTracker;

		// Token: 0x040025E4 RID: 9700
		private int cooldownTicks;

		// Token: 0x040025E5 RID: 9701
		private int cooldownTicksDuration;

		// Token: 0x040025E6 RID: 9702
		private Mote warmupMote;

		// Token: 0x040025E7 RID: 9703
		private List<CompAbilityEffect> effectComps;
	}
}
