using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse.AI;

namespace Verse
{
	
	public class Hediff : IExposable, ILoadReferenceable
	{
		
		// (get) Token: 0x06000F94 RID: 3988 RVA: 0x0005AA52 File Offset: 0x00058C52
		public virtual string LabelBase
		{
			get
			{
				return this.def.label;
			}
		}

		
		// (get) Token: 0x06000F95 RID: 3989 RVA: 0x0005AA5F File Offset: 0x00058C5F
		public string LabelBaseCap
		{
			get
			{
				return this.LabelBase.CapitalizeFirst(this.def);
			}
		}

		
		// (get) Token: 0x06000F96 RID: 3990 RVA: 0x0005AA74 File Offset: 0x00058C74
		public virtual string Label
		{
			get
			{
				string labelInBrackets = this.LabelInBrackets;
				return this.LabelBase + (labelInBrackets.NullOrEmpty() ? "" : (" (" + labelInBrackets + ")"));
			}
		}

		
		// (get) Token: 0x06000F97 RID: 3991 RVA: 0x0005AAB2 File Offset: 0x00058CB2
		public string LabelCap
		{
			get
			{
				return this.Label.CapitalizeFirst(this.def);
			}
		}

		
		// (get) Token: 0x06000F98 RID: 3992 RVA: 0x0005AAC5 File Offset: 0x00058CC5
		public virtual Color LabelColor
		{
			get
			{
				return this.def.defaultLabelColor;
			}
		}

		
		// (get) Token: 0x06000F99 RID: 3993 RVA: 0x0005AAD2 File Offset: 0x00058CD2
		public virtual string LabelInBrackets
		{
			get
			{
				if (this.CurStage != null && !this.CurStage.label.NullOrEmpty())
				{
					return this.CurStage.label;
				}
				return null;
			}
		}

		
		// (get) Token: 0x06000F9A RID: 3994 RVA: 0x0005AAFB File Offset: 0x00058CFB
		public virtual string SeverityLabel
		{
			get
			{
				if (this.def.lethalSeverity > 0f)
				{
					return (this.Severity / this.def.lethalSeverity).ToStringPercent();
				}
				return null;
			}
		}

		
		// (get) Token: 0x06000F9B RID: 3995 RVA: 0x0005AB28 File Offset: 0x00058D28
		public virtual int UIGroupKey
		{
			get
			{
				return this.Label.GetHashCode();
			}
		}

		
		// (get) Token: 0x06000F9C RID: 3996 RVA: 0x0005AB38 File Offset: 0x00058D38
		public virtual string TipStringExtra
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach (StatDrawEntry statDrawEntry in HediffStatsUtility.SpecialDisplayStats(this.CurStage, this))
				{
					if (statDrawEntry.ShouldDisplay)
					{
						stringBuilder.AppendLine(statDrawEntry.LabelCap + ": " + statDrawEntry.ValueString);
					}
				}
				return stringBuilder.ToString();
			}
		}

		
		// (get) Token: 0x06000F9D RID: 3997 RVA: 0x0005ABB8 File Offset: 0x00058DB8
		public virtual HediffStage CurStage
		{
			get
			{
				if (!this.def.stages.NullOrEmpty<HediffStage>())
				{
					return this.def.stages[this.CurStageIndex];
				}
				return null;
			}
		}

		
		// (get) Token: 0x06000F9E RID: 3998 RVA: 0x0005ABE4 File Offset: 0x00058DE4
		public virtual bool ShouldRemove
		{
			get
			{
				return this.Severity <= 0f;
			}
		}

		
		// (get) Token: 0x06000F9F RID: 3999 RVA: 0x0005ABF6 File Offset: 0x00058DF6
		public virtual bool Visible
		{
			get
			{
				return this.visible || this.CurStage == null || this.CurStage.becomeVisible;
			}
		}

		
		// (get) Token: 0x06000FA0 RID: 4000 RVA: 0x0005AC15 File Offset: 0x00058E15
		public virtual float BleedRate
		{
			get
			{
				return 0f;
			}
		}

		
		// (get) Token: 0x06000FA1 RID: 4001 RVA: 0x0005AC1C File Offset: 0x00058E1C
		public bool Bleeding
		{
			get
			{
				return this.BleedRate > 1E-05f;
			}
		}

		
		// (get) Token: 0x06000FA2 RID: 4002 RVA: 0x0005AC2B File Offset: 0x00058E2B
		public virtual float PainOffset
		{
			get
			{
				if (this.CurStage != null && !this.causesNoPain)
				{
					return this.CurStage.painOffset;
				}
				return 0f;
			}
		}

		
		// (get) Token: 0x06000FA3 RID: 4003 RVA: 0x0005AC4E File Offset: 0x00058E4E
		public virtual float PainFactor
		{
			get
			{
				if (this.CurStage != null)
				{
					return this.CurStage.painFactor;
				}
				return 1f;
			}
		}

		
		// (get) Token: 0x06000FA4 RID: 4004 RVA: 0x0005AC69 File Offset: 0x00058E69
		public List<PawnCapacityModifier> CapMods
		{
			get
			{
				if (this.CurStage != null)
				{
					return this.CurStage.capMods;
				}
				return null;
			}
		}

		
		// (get) Token: 0x06000FA5 RID: 4005 RVA: 0x0005AC15 File Offset: 0x00058E15
		public virtual float SummaryHealthPercentImpact
		{
			get
			{
				return 0f;
			}
		}

		
		// (get) Token: 0x06000FA6 RID: 4006 RVA: 0x0005AC80 File Offset: 0x00058E80
		public virtual float TendPriority
		{
			get
			{
				float num = 0f;
				HediffStage curStage = this.CurStage;
				if (curStage != null && curStage.lifeThreatening)
				{
					num = Mathf.Max(num, 1f);
				}
				num = Mathf.Max(num, this.BleedRate * 1.5f);
				HediffComp_TendDuration hediffComp_TendDuration = this.TryGetComp<HediffComp_TendDuration>();
				if (hediffComp_TendDuration != null && hediffComp_TendDuration.TProps.severityPerDayTended < 0f)
				{
					num = Mathf.Max(num, 0.025f);
				}
				return num;
			}
		}

		
		// (get) Token: 0x06000FA7 RID: 4007 RVA: 0x0005ACED File Offset: 0x00058EED
		public virtual TextureAndColor StateIcon
		{
			get
			{
				return TextureAndColor.None;
			}
		}

		
		// (get) Token: 0x06000FA8 RID: 4008 RVA: 0x0005ACF4 File Offset: 0x00058EF4
		public virtual int CurStageIndex
		{
			get
			{
				if (this.def.stages == null)
				{
					return 0;
				}
				List<HediffStage> stages = this.def.stages;
				float severity = this.Severity;
				for (int i = stages.Count - 1; i >= 0; i--)
				{
					if (severity >= stages[i].minSeverity)
					{
						return i;
					}
				}
				return 0;
			}
		}

		
		// (get) Token: 0x06000FA9 RID: 4009 RVA: 0x0005AD48 File Offset: 0x00058F48
		// (set) Token: 0x06000FAA RID: 4010 RVA: 0x0005AD50 File Offset: 0x00058F50
		public virtual float Severity
		{
			get
			{
				return this.severityInt;
			}
			set
			{
				bool flag = false;
				if (this.def.lethalSeverity > 0f && value >= this.def.lethalSeverity)
				{
					value = this.def.lethalSeverity;
					flag = true;
				}
				bool flag2 = this is Hediff_Injury && value > this.severityInt && Mathf.RoundToInt(value) != Mathf.RoundToInt(this.severityInt);
				int curStageIndex = this.CurStageIndex;
				this.severityInt = Mathf.Clamp(value, this.def.minSeverity, this.def.maxSeverity);
				if ((this.CurStageIndex != curStageIndex || flag || flag2) && this.pawn.health.hediffSet.hediffs.Contains(this))
				{
					this.pawn.health.Notify_HediffChanged(this);
					if (!this.pawn.Dead && this.pawn.needs.mood != null)
					{
						this.pawn.needs.mood.thoughts.situational.Notify_SituationalThoughtsDirty();
					}
				}
			}
		}

		
		// (get) Token: 0x06000FAB RID: 4011 RVA: 0x0005AE60 File Offset: 0x00059060
		// (set) Token: 0x06000FAC RID: 4012 RVA: 0x0005AE68 File Offset: 0x00059068
		public BodyPartRecord Part
		{
			get
			{
				return this.part;
			}
			set
			{
				if (this.pawn == null && this.part != null)
				{
					Log.Error("Hediff: Cannot set Part without setting pawn first.", false);
					return;
				}
				this.part = value;
			}
		}

		
		public virtual bool TendableNow(bool ignoreTimer = false)
		{
			if (!this.def.tendable || this.Severity <= 0f || this.FullyImmune() || !this.Visible || this.IsPermanent())
			{
				return false;
			}
			if (!ignoreTimer)
			{
				HediffComp_TendDuration hediffComp_TendDuration = this.TryGetComp<HediffComp_TendDuration>();
				if (hediffComp_TendDuration != null && !hediffComp_TendDuration.AllowTend)
				{
					return false;
				}
			}
			return true;
		}

		
		public virtual void ExposeData()
		{
			if (Scribe.mode == LoadSaveMode.Saving && this.combatLogEntry != null)
			{
				LogEntry target = this.combatLogEntry.Target;
				if (target == null || !Current.Game.battleLog.IsEntryActive(target))
				{
					this.combatLogEntry = null;
				}
			}
			Scribe_Values.Look<int>(ref this.loadID, "loadID", 0, false);
			Scribe_Defs.Look<HediffDef>(ref this.def, "def");
			Scribe_Values.Look<int>(ref this.ageTicks, "ageTicks", 0, false);
			Scribe_Defs.Look<ThingDef>(ref this.source, "source");
			Scribe_Defs.Look<BodyPartGroupDef>(ref this.sourceBodyPartGroup, "sourceBodyPartGroup");
			Scribe_Defs.Look<HediffDef>(ref this.sourceHediffDef, "sourceHediffDef");
			Scribe_BodyParts.Look(ref this.part, "part", null);
			Scribe_Values.Look<float>(ref this.severityInt, "severity", 0f, false);
			Scribe_Values.Look<bool>(ref this.recordedTale, "recordedTale", false, false);
			Scribe_Values.Look<bool>(ref this.causesNoPain, "causesNoPain", false, false);
			Scribe_Values.Look<bool>(ref this.visible, "visible", false, false);
			Scribe_References.Look<LogEntry>(ref this.combatLogEntry, "combatLogEntry", false);
			Scribe_Values.Look<string>(ref this.combatLogText, "combatLogText", null, false);
			BackCompatibility.PostExposeData(this);
		}

		
		public virtual void Tick()
		{
			this.ageTicks++;
			if (this.def.hediffGivers != null && this.pawn.IsHashIntervalTick(60))
			{
				for (int i = 0; i < this.def.hediffGivers.Count; i++)
				{
					this.def.hediffGivers[i].OnIntervalPassed(this.pawn, this);
				}
			}
			if (this.Visible && !this.visible)
			{
				this.visible = true;
				if (this.def.taleOnVisible != null)
				{
					TaleRecorder.RecordTale(this.def.taleOnVisible, new object[]
					{
						this.pawn,
						this.def
					});
				}
			}
			HediffStage curStage = this.CurStage;
			if (curStage != null)
			{
				if (curStage.hediffGivers != null && this.pawn.IsHashIntervalTick(60))
				{
					for (int j = 0; j < curStage.hediffGivers.Count; j++)
					{
						curStage.hediffGivers[j].OnIntervalPassed(this.pawn, this);
					}
				}
				if (curStage.mentalStateGivers != null && this.pawn.IsHashIntervalTick(60) && !this.pawn.InMentalState)
				{
					for (int k = 0; k < curStage.mentalStateGivers.Count; k++)
					{
						MentalStateGiver mentalStateGiver = curStage.mentalStateGivers[k];
						if (Rand.MTBEventOccurs(mentalStateGiver.mtbDays, 60000f, 60f))
						{
							this.pawn.mindState.mentalStateHandler.TryStartMentalState(mentalStateGiver.mentalState, "MentalStateReason_Hediff".Translate(this.Label), false, false, null, false);
						}
					}
				}
				if (curStage.mentalBreakMtbDays > 0f && this.pawn.IsHashIntervalTick(60) && !this.pawn.InMentalState && !this.pawn.Downed && Rand.MTBEventOccurs(curStage.mentalBreakMtbDays, 60000f, 60f))
				{
					this.TryDoRandomMentalBreak();
				}
				if (curStage.vomitMtbDays > 0f && this.pawn.IsHashIntervalTick(600) && Rand.MTBEventOccurs(curStage.vomitMtbDays, 60000f, 600f) && this.pawn.Spawned && this.pawn.Awake() && this.pawn.RaceProps.IsFlesh)
				{
					this.pawn.jobs.StartJob(JobMaker.MakeJob(JobDefOf.Vomit), JobCondition.InterruptForced, null, true, true, null, null, false, false);
				}
				Thought_Memory th;
				if (curStage.forgetMemoryThoughtMtbDays > 0f && this.pawn.needs != null && this.pawn.needs.mood != null && this.pawn.IsHashIntervalTick(400) && Rand.MTBEventOccurs(curStage.forgetMemoryThoughtMtbDays, 60000f, 400f) && this.pawn.needs.mood.thoughts.memories.Memories.TryRandomElement(out th))
				{
					this.pawn.needs.mood.thoughts.memories.RemoveMemory(th);
				}
				if (!this.recordedTale && curStage.tale != null)
				{
					TaleRecorder.RecordTale(curStage.tale, new object[]
					{
						this.pawn
					});
					this.recordedTale = true;
				}
				if (curStage.destroyPart && this.Part != null && this.Part != this.pawn.RaceProps.body.corePart)
				{
					this.pawn.health.AddHediff(HediffDefOf.MissingBodyPart, this.Part, null, null);
				}
				if (curStage.deathMtbDays > 0f && this.pawn.IsHashIntervalTick(200) && Rand.MTBEventOccurs(curStage.deathMtbDays, 60000f, 200f))
				{
					bool flag = PawnUtility.ShouldSendNotificationAbout(this.pawn);
					Caravan caravan = this.pawn.GetCaravan();
					this.pawn.Kill(null, null);
					if (flag)
					{
						this.pawn.health.NotifyPlayerOfKilled(null, this, caravan);
					}
					return;
				}
			}
		}

		
		private void TryDoRandomMentalBreak()
		{
			HediffStage curStage = this.CurStage;
			if (curStage == null)
			{
				return;
			}
			MentalBreakDef mentalBreakDef;
			if ((from x in DefDatabase<MentalBreakDef>.AllDefsListForReading
			where x.Worker.BreakCanOccur(this.pawn) && (curStage.allowedMentalBreakIntensities == null || curStage.allowedMentalBreakIntensities.Contains(x.intensity))
			select x).TryRandomElementByWeight((MentalBreakDef x) => x.Worker.CommonalityFor(this.pawn, false), out mentalBreakDef))
			{
				mentalBreakDef.Worker.TryStart(this.pawn, "MentalStateReason_Hediff".Translate(this.Label), false);
			}
		}

		
		public virtual void PostMake()
		{
			this.Severity = Mathf.Max(this.Severity, this.def.initialSeverity);
			this.causesNoPain = (Rand.Value < this.def.chanceToCauseNoPain);
		}

		
		public virtual void PostAdd(DamageInfo? dinfo)
		{
			if (this.def.disablesNeed != null)
			{
				this.pawn.needs.AddOrRemoveNeedsAsAppropriate();
			}
		}

		
		public virtual void PostRemoved()
		{
			if ((this.def.causesNeed != null || this.def.disablesNeed != null) && !this.pawn.Dead)
			{
				this.pawn.needs.AddOrRemoveNeedsAsAppropriate();
			}
		}

		
		public virtual void PostTick()
		{
		}

		
		public virtual void Tended(float quality, int batchPosition = 0)
		{
		}

		
		public virtual void Heal(float amount)
		{
			if (amount <= 0f)
			{
				return;
			}
			this.Severity -= amount;
			this.pawn.health.Notify_HediffChanged(this);
		}

		
		public virtual void ModifyChemicalEffect(ChemicalDef chem, ref float effect)
		{
		}

		
		public virtual bool TryMergeWith(Hediff other)
		{
			if (other == null || other.def != this.def || other.Part != this.Part)
			{
				return false;
			}
			this.Severity += other.Severity;
			this.ageTicks = 0;
			return true;
		}

		
		public virtual bool CauseDeathNow()
		{
			return this.def.lethalSeverity >= 0f && this.Severity >= this.def.lethalSeverity;
		}

		
		public virtual void Notify_PawnDied()
		{
		}

		
		public virtual void Notify_PawnKilled()
		{
		}

		
		public virtual void Notify_PawnPostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
		{
		}

		
		public virtual void Notify_PawnUsedVerb(Verb verb, LocalTargetInfo targets)
		{
		}

		
		public virtual void Notify_EntropyGained(float baseAmount, float finalAmount, Thing source = null)
		{
		}

		
		public virtual void Notify_ImplantUsed(string violationSourceName, float detectionChance, int violationSourceLevel = -1)
		{
		}

		
		public virtual string DebugString()
		{
			string text = "";
			if (!this.Visible)
			{
				text += "hidden\n";
			}
			text = text + "severity: " + this.Severity.ToString("F3") + ((this.Severity >= this.def.maxSeverity) ? " (reached max)" : "");
			if (this.TendableNow(false))
			{
				text = text + "\ntend priority: " + this.TendPriority;
			}
			return text.Indented("    ");
		}

		
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"(",
				this.def.defName,
				(this.part != null) ? (" " + this.part.Label) : "",
				" ticksSinceCreation=",
				this.ageTicks,
				")"
			});
		}

		
		public string GetUniqueLoadID()
		{
			return "Hediff_" + this.loadID;
		}

		
		public HediffDef def;

		
		public int ageTicks;

		
		private BodyPartRecord part;

		
		public ThingDef source;

		
		public BodyPartGroupDef sourceBodyPartGroup;

		
		public HediffDef sourceHediffDef;

		
		public int loadID = -1;

		
		protected float severityInt;

		
		private bool recordedTale;

		
		protected bool causesNoPain;

		
		private bool visible;

		
		public WeakReference<LogEntry> combatLogEntry;

		
		public string combatLogText;

		
		public int temp_partIndexToSetLater = -1;

		
		[Unsaved(false)]
		public Pawn pawn;
	}
}
