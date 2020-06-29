using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse.AI;
using Verse.Sound;

namespace Verse
{
	
	public abstract class Verb : ITargetingSource, IExposable, ILoadReferenceable
	{
		
		// (get) Token: 0x06002218 RID: 8728 RVA: 0x000D01BE File Offset: 0x000CE3BE
		public IVerbOwner DirectOwner
		{
			get
			{
				return this.verbTracker.directOwner;
			}
		}

		
		// (get) Token: 0x06002219 RID: 8729 RVA: 0x000D01CB File Offset: 0x000CE3CB
		public ImplementOwnerTypeDef ImplementOwnerType
		{
			get
			{
				return this.verbTracker.directOwner.ImplementOwnerTypeDef;
			}
		}

		
		// (get) Token: 0x0600221A RID: 8730 RVA: 0x000D01DD File Offset: 0x000CE3DD
		public CompEquippable EquipmentCompSource
		{
			get
			{
				return this.DirectOwner as CompEquippable;
			}
		}

		
		// (get) Token: 0x0600221B RID: 8731 RVA: 0x000D01EA File Offset: 0x000CE3EA
		public ThingWithComps EquipmentSource
		{
			get
			{
				if (this.EquipmentCompSource == null)
				{
					return null;
				}
				return this.EquipmentCompSource.parent;
			}
		}

		
		// (get) Token: 0x0600221C RID: 8732 RVA: 0x000D0201 File Offset: 0x000CE401
		public HediffComp_VerbGiver HediffCompSource
		{
			get
			{
				return this.DirectOwner as HediffComp_VerbGiver;
			}
		}

		
		// (get) Token: 0x0600221D RID: 8733 RVA: 0x000D020E File Offset: 0x000CE40E
		public Hediff HediffSource
		{
			get
			{
				if (this.HediffCompSource == null)
				{
					return null;
				}
				return this.HediffCompSource.parent;
			}
		}

		
		// (get) Token: 0x0600221E RID: 8734 RVA: 0x000D0225 File Offset: 0x000CE425
		public Pawn_MeleeVerbs_TerrainSource TerrainSource
		{
			get
			{
				return this.DirectOwner as Pawn_MeleeVerbs_TerrainSource;
			}
		}

		
		// (get) Token: 0x0600221F RID: 8735 RVA: 0x000D0232 File Offset: 0x000CE432
		public TerrainDef TerrainDefSource
		{
			get
			{
				if (this.TerrainSource == null)
				{
					return null;
				}
				return this.TerrainSource.def;
			}
		}

		
		// (get) Token: 0x06002220 RID: 8736 RVA: 0x000D0249 File Offset: 0x000CE449
		public virtual Thing Caster
		{
			get
			{
				return this.caster;
			}
		}

		
		// (get) Token: 0x06002221 RID: 8737 RVA: 0x000D0251 File Offset: 0x000CE451
		public virtual Pawn CasterPawn
		{
			get
			{
				return this.caster as Pawn;
			}
		}

		
		// (get) Token: 0x06002222 RID: 8738 RVA: 0x0006461A File Offset: 0x0006281A
		public virtual Verb GetVerb
		{
			get
			{
				return this;
			}
		}

		
		// (get) Token: 0x06002223 RID: 8739 RVA: 0x000D025E File Offset: 0x000CE45E
		public virtual bool CasterIsPawn
		{
			get
			{
				return this.caster is Pawn;
			}
		}

		
		// (get) Token: 0x06002224 RID: 8740 RVA: 0x000D026E File Offset: 0x000CE46E
		public virtual bool Targetable
		{
			get
			{
				return this.verbProps.targetable;
			}
		}

		
		// (get) Token: 0x06002225 RID: 8741 RVA: 0x00010306 File Offset: 0x0000E506
		public virtual bool MultiSelect
		{
			get
			{
				return false;
			}
		}

		
		// (get) Token: 0x06002226 RID: 8742 RVA: 0x000D027B File Offset: 0x000CE47B
		public LocalTargetInfo CurrentTarget
		{
			get
			{
				return this.currentTarget;
			}
		}

		
		// (get) Token: 0x06002227 RID: 8743 RVA: 0x000D0283 File Offset: 0x000CE483
		public virtual TargetingParameters targetParams
		{
			get
			{
				return this.verbProps.targetParams;
			}
		}

		
		// (get) Token: 0x06002228 RID: 8744 RVA: 0x00019EA1 File Offset: 0x000180A1
		public virtual ITargetingSource DestinationSelector
		{
			get
			{
				return null;
			}
		}

		
		// (get) Token: 0x06002229 RID: 8745 RVA: 0x0001028D File Offset: 0x0000E48D
		protected virtual int ShotsPerBurst
		{
			get
			{
				return 1;
			}
		}

		
		// (get) Token: 0x0600222A RID: 8746 RVA: 0x000D0290 File Offset: 0x000CE490
		public virtual Texture2D UIIcon
		{
			get
			{
				if (this.EquipmentSource != null)
				{
					return this.EquipmentSource.def.uiIcon;
				}
				return BaseContent.BadTex;
			}
		}

		
		// (get) Token: 0x0600222B RID: 8747 RVA: 0x000D02B0 File Offset: 0x000CE4B0
		public bool Bursting
		{
			get
			{
				return this.burstShotsLeft > 0;
			}
		}

		
		// (get) Token: 0x0600222C RID: 8748 RVA: 0x000D02BB File Offset: 0x000CE4BB
		public virtual bool IsMeleeAttack
		{
			get
			{
				return this.verbProps.IsMeleeAttack;
			}
		}

		
		// (get) Token: 0x0600222D RID: 8749 RVA: 0x000D02C8 File Offset: 0x000CE4C8
		public bool BuggedAfterLoading
		{
			get
			{
				return this.verbProps == null;
			}
		}

		
		// (get) Token: 0x0600222E RID: 8750 RVA: 0x000D02D3 File Offset: 0x000CE4D3
		public bool WarmingUp
		{
			get
			{
				return this.WarmupStance != null;
			}
		}

		
		// (get) Token: 0x0600222F RID: 8751 RVA: 0x000D02E0 File Offset: 0x000CE4E0
		public Stance_Warmup WarmupStance
		{
			get
			{
				if (this.CasterPawn == null || !this.CasterPawn.Spawned)
				{
					return null;
				}
				Stance_Warmup stance_Warmup;
				if ((stance_Warmup = (this.CasterPawn.stances.curStance as Stance_Warmup)) == null || stance_Warmup.verb != this)
				{
					return null;
				}
				return stance_Warmup;
			}
		}

		
		// (get) Token: 0x06002230 RID: 8752 RVA: 0x000D0329 File Offset: 0x000CE529
		public virtual string ReportLabel
		{
			get
			{
				return this.verbProps.label;
			}
		}

		
		public bool IsStillUsableBy(Pawn pawn)
		{
			return this.Available() && this.DirectOwner.VerbsStillUsableBy(pawn) && this.verbProps.GetDamageFactorFor(this, pawn) != 0f;
		}

		
		public virtual bool IsUsableOn(Thing target)
		{
			return true;
		}

		
		public virtual void ExposeData()
		{
			Scribe_Values.Look<string>(ref this.loadID, "loadID", null, false);
			Scribe_Values.Look<VerbState>(ref this.state, "state", VerbState.Idle, false);
			Scribe_TargetInfo.Look(ref this.currentTarget, "currentTarget");
			Scribe_TargetInfo.Look(ref this.currentDestination, "currentDestination");
			Scribe_Values.Look<int>(ref this.burstShotsLeft, "burstShotsLeft", 0, false);
			Scribe_Values.Look<int>(ref this.ticksToNextBurstShot, "ticksToNextBurstShot", 0, false);
			Scribe_Values.Look<bool>(ref this.surpriseAttack, "surpriseAttack", false, false);
			Scribe_Values.Look<bool>(ref this.canHitNonTargetPawnsNow, "canHitNonTargetPawnsNow", false, false);
		}

		
		public string GetUniqueLoadID()
		{
			return "Verb_" + this.loadID;
		}

		
		public static string CalculateUniqueLoadID(IVerbOwner owner, Tool tool, ManeuverDef maneuver)
		{
			return string.Format("{0}_{1}_{2}", owner.UniqueVerbOwnerID(), (tool != null) ? tool.id : "NT", (maneuver != null) ? maneuver.defName : "NM");
		}

		
		public static string CalculateUniqueLoadID(IVerbOwner owner, int index)
		{
			return string.Format("{0}_{1}", owner.UniqueVerbOwnerID(), index);
		}

		
		public bool TryStartCastOn(LocalTargetInfo castTarg, bool surpriseAttack = false, bool canHitNonTargetPawns = true)
		{
			return this.TryStartCastOn(castTarg, LocalTargetInfo.Invalid, surpriseAttack, canHitNonTargetPawns);
		}

		
		public bool TryStartCastOn(LocalTargetInfo castTarg, LocalTargetInfo destTarg, bool surpriseAttack = false, bool canHitNonTargetPawns = true)
		{
			if (this.caster == null)
			{
				Log.Error("Verb " + this.GetUniqueLoadID() + " needs caster to work (possibly lost during saving/loading).", false);
				return false;
			}
			if (!this.caster.Spawned)
			{
				return false;
			}
			if (this.state == VerbState.Bursting || !this.CanHitTarget(castTarg))
			{
				return false;
			}
			if (this.CausesTimeSlowdown(castTarg))
			{
				Find.TickManager.slower.SignalForceNormalSpeed();
			}
			this.surpriseAttack = surpriseAttack;
			this.canHitNonTargetPawnsNow = canHitNonTargetPawns;
			this.currentTarget = castTarg;
			this.currentDestination = destTarg;
			if (this.CasterIsPawn && this.verbProps.warmupTime > 0f)
			{
				ShootLine newShootLine;
				if (!this.TryFindShootLineFromTo(this.caster.Position, castTarg, out newShootLine))
				{
					return false;
				}
				this.CasterPawn.Drawer.Notify_WarmingCastAlongLine(newShootLine, this.caster.Position);
				float statValue = this.CasterPawn.GetStatValue(StatDefOf.AimingDelayFactor, true);
				int ticks = (this.verbProps.warmupTime * statValue).SecondsToTicks();
				this.CasterPawn.stances.SetStance(new Stance_Warmup(ticks, castTarg, this));
			}
			else
			{
				this.WarmupComplete();
			}
			return true;
		}

		
		public virtual void WarmupComplete()
		{
			this.burstShotsLeft = this.ShotsPerBurst;
			this.state = VerbState.Bursting;
			this.TryCastNextBurstShot();
			if (this.CasterIsPawn && this.currentTarget.HasThing)
			{
				Pawn pawn = this.currentTarget.Thing as Pawn;
				if (pawn != null && pawn.IsColonistPlayerControlled)
				{
					this.CasterPawn.records.AccumulateStoryEvent(StoryEventDefOf.AttackedPlayer);
				}
			}
		}

		
		public void VerbTick()
		{
			if (this.state == VerbState.Bursting)
			{
				if (!this.caster.Spawned)
				{
					this.Reset();
					return;
				}
				this.ticksToNextBurstShot--;
				if (this.ticksToNextBurstShot <= 0)
				{
					this.TryCastNextBurstShot();
				}
			}
		}

		
		public virtual bool Available()
		{
			if (this.verbProps.consumeFuelPerShot > 0f)
			{
				CompRefuelable compRefuelable = this.caster.TryGetComp<CompRefuelable>();
				if (compRefuelable != null && compRefuelable.Fuel < this.verbProps.consumeFuelPerShot)
				{
					return false;
				}
			}
			return true;
		}

		
		protected void TryCastNextBurstShot()
		{
			LocalTargetInfo localTargetInfo = this.currentTarget;
			if (this.Available() && this.TryCastShot())
			{
				if (this.verbProps.muzzleFlashScale > 0.01f)
				{
					MoteMaker.MakeStaticMote(this.caster.Position, this.caster.Map, ThingDefOf.Mote_ShotFlash, this.verbProps.muzzleFlashScale);
				}
				if (this.verbProps.soundCast != null)
				{
					this.verbProps.soundCast.PlayOneShot(new TargetInfo(this.caster.Position, this.caster.Map, false));
				}
				if (this.verbProps.soundCastTail != null)
				{
					this.verbProps.soundCastTail.PlayOneShotOnCamera(this.caster.Map);
				}
				if (this.CasterIsPawn)
				{
					if (this.CasterPawn.thinker != null)
					{
						this.CasterPawn.mindState.Notify_EngagedTarget();
					}
					if (this.CasterPawn.mindState != null)
					{
						this.CasterPawn.mindState.Notify_AttackedTarget(localTargetInfo);
					}
					if (this.CasterPawn.MentalState != null)
					{
						this.CasterPawn.MentalState.Notify_AttackedTarget(localTargetInfo);
					}
					if (this.TerrainDefSource != null)
					{
						this.CasterPawn.meleeVerbs.Notify_UsedTerrainBasedVerb();
					}
					if (this.CasterPawn.health != null)
					{
						this.CasterPawn.health.Notify_UsedVerb(this, localTargetInfo);
					}
					if (this.EquipmentSource != null)
					{
						this.EquipmentSource.Notify_UsedWeapon(this.CasterPawn);
					}
					if (!this.CasterPawn.Spawned)
					{
						return;
					}
				}
				if (this.verbProps.consumeFuelPerShot > 0f)
				{
					CompRefuelable compRefuelable = this.caster.TryGetComp<CompRefuelable>();
					if (compRefuelable != null)
					{
						compRefuelable.ConsumeFuel(this.verbProps.consumeFuelPerShot);
					}
				}
				this.burstShotsLeft--;
			}
			else
			{
				this.burstShotsLeft = 0;
			}
			if (this.burstShotsLeft > 0)
			{
				this.ticksToNextBurstShot = this.verbProps.ticksBetweenBurstShots;
				if (this.CasterIsPawn)
				{
					this.CasterPawn.stances.SetStance(new Stance_Cooldown(this.verbProps.ticksBetweenBurstShots + 1, this.currentTarget, this));
					return;
				}
			}
			else
			{
				this.state = VerbState.Idle;
				if (this.CasterIsPawn)
				{
					this.CasterPawn.stances.SetStance(new Stance_Cooldown(this.verbProps.AdjustedCooldownTicks(this, this.CasterPawn), this.currentTarget, this));
				}
				if (this.castCompleteCallback != null)
				{
					this.castCompleteCallback();
				}
			}
		}

		
		public virtual void OrderForceTarget(LocalTargetInfo target)
		{
			if (this.verbProps.IsMeleeAttack)
			{
				Job job = JobMaker.MakeJob(JobDefOf.AttackMelee, target);
				job.playerForced = true;
				Pawn pawn = target.Thing as Pawn;
				if (pawn != null)
				{
					job.killIncappedTarget = pawn.Downed;
				}
				this.CasterPawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
				return;
			}
			float num = this.verbProps.EffectiveMinRange(target, this.CasterPawn);
			if ((float)this.CasterPawn.Position.DistanceToSquared(target.Cell) < num * num && this.CasterPawn.Position.AdjacentTo8WayOrInside(target.Cell))
			{
				Messages.Message("MessageCantShootInMelee".Translate(), this.CasterPawn, MessageTypeDefOf.RejectInput, false);
				return;
			}
			Job job2 = JobMaker.MakeJob(this.verbProps.ai_IsWeapon ? JobDefOf.AttackStatic : JobDefOf.UseVerbOnThing);
			job2.verbToUse = this;
			job2.targetA = target;
			job2.endIfCantShootInMelee = true;
			this.CasterPawn.jobs.TryTakeOrderedJob(job2, JobTag.Misc);
		}

		
		protected abstract bool TryCastShot();

		
		public void Notify_PickedUp()
		{
			this.Reset();
		}

		
		public virtual void Reset()
		{
			this.state = VerbState.Idle;
			this.currentTarget = null;
			this.currentDestination = null;
			this.burstShotsLeft = 0;
			this.ticksToNextBurstShot = 0;
			this.castCompleteCallback = null;
			this.surpriseAttack = false;
		}

		
		public virtual void Notify_EquipmentLost()
		{
			if (this.CasterIsPawn)
			{
				Pawn casterPawn = this.CasterPawn;
				if (casterPawn.Spawned)
				{
					Stance_Warmup stance_Warmup = casterPawn.stances.curStance as Stance_Warmup;
					if (stance_Warmup != null && stance_Warmup.verb == this)
					{
						casterPawn.stances.CancelBusyStanceSoft();
					}
					if (casterPawn.CurJob != null && casterPawn.CurJob.def == JobDefOf.AttackStatic)
					{
						casterPawn.jobs.EndCurrentJob(JobCondition.Incompletable, true, true);
					}
				}
			}
		}

		
		public virtual float HighlightFieldRadiusAroundTarget(out bool needLOSToCenter)
		{
			needLOSToCenter = false;
			return 0f;
		}

		
		private bool CausesTimeSlowdown(LocalTargetInfo castTarg)
		{
			if (!this.verbProps.CausesTimeSlowdown)
			{
				return false;
			}
			if (!castTarg.HasThing)
			{
				return false;
			}
			Thing thing = castTarg.Thing;
			if (thing.def.category != ThingCategory.Pawn && (thing.def.building == null || !thing.def.building.IsTurret))
			{
				return false;
			}
			Pawn pawn = thing as Pawn;
			bool flag = pawn != null && pawn.Downed;
			return (thing.Faction == Faction.OfPlayer && this.caster.HostileTo(Faction.OfPlayer)) || (this.caster.Faction == Faction.OfPlayer && thing.HostileTo(Faction.OfPlayer) && !flag);
		}

		
		public virtual bool CanHitTarget(LocalTargetInfo targ)
		{
			return this.caster != null && this.caster.Spawned && (targ == this.caster || this.CanHitTargetFrom(this.caster.Position, targ));
		}

		
		public virtual bool ValidateTarget(LocalTargetInfo target)
		{
			return true;
		}

		
		public virtual void DrawHighlight(LocalTargetInfo target)
		{
			this.verbProps.DrawRadiusRing(this.caster.Position);
			if (target.IsValid)
			{
				GenDraw.DrawTargetHighlight(target);
				bool flag;
				float num = this.HighlightFieldRadiusAroundTarget(out flag);
				ShootLine shootLine;
				if (num > 0.2f && this.TryFindShootLineFromTo(this.caster.Position, target, out shootLine))
				{
					if (flag)
					{
						GenExplosion.RenderPredictedAreaOfEffect(shootLine.Dest, num);
						return;
					}
					GenDraw.DrawFieldEdges((from x in GenRadial.RadialCellsAround(shootLine.Dest, num, true)
					where x.InBounds(Find.CurrentMap)
					select x).ToList<IntVec3>());
				}
			}
		}

		
		public virtual void OnGUI(LocalTargetInfo target)
		{
			Texture2D icon;
			if (target.IsValid)
			{
				if (this.UIIcon != BaseContent.BadTex)
				{
					icon = this.UIIcon;
				}
				else
				{
					icon = TexCommand.Attack;
				}
			}
			else
			{
				icon = TexCommand.CannotShoot;
			}
			GenUI.DrawMouseAttachment(icon);
		}

		
		public virtual bool CanHitTargetFrom(IntVec3 root, LocalTargetInfo targ)
		{
			if (targ.Thing != null && targ.Thing == this.caster)
			{
				return this.targetParams.canTargetSelf;
			}
			ShootLine shootLine;
			return !this.ApparelPreventsShooting(root, targ) && this.TryFindShootLineFromTo(root, targ, out shootLine);
		}

		
		public bool ApparelPreventsShooting(IntVec3 root, LocalTargetInfo targ)
		{
			if (this.CasterIsPawn && this.CasterPawn.apparel != null)
			{
				List<Apparel> wornApparel = this.CasterPawn.apparel.WornApparel;
				for (int i = 0; i < wornApparel.Count; i++)
				{
					if (!wornApparel[i].AllowVerbCast(root, this.caster.Map, targ, this))
					{
						return true;
					}
				}
			}
			return false;
		}

		
		public bool TryFindShootLineFromTo(IntVec3 root, LocalTargetInfo targ, out ShootLine resultingLine)
		{
			if (targ.HasThing && targ.Thing.Map != this.caster.Map)
			{
				resultingLine = default(ShootLine);
				return false;
			}
			if (this.verbProps.IsMeleeAttack || this.verbProps.range <= 1.42f)
			{
				resultingLine = new ShootLine(root, targ.Cell);
				return ReachabilityImmediate.CanReachImmediate(root, targ, this.caster.Map, PathEndMode.Touch, null);
			}
			CellRect cellRect = targ.HasThing ? targ.Thing.OccupiedRect() : CellRect.SingleCell(targ.Cell);
			float num = this.verbProps.EffectiveMinRange(targ, this.caster);
			float num2 = cellRect.ClosestDistSquaredTo(root);
			if (num2 > this.verbProps.range * this.verbProps.range || num2 < num * num)
			{
				resultingLine = new ShootLine(root, targ.Cell);
				return false;
			}
			if (!this.verbProps.requireLineOfSight)
			{
				resultingLine = new ShootLine(root, targ.Cell);
				return true;
			}
			if (this.CasterIsPawn)
			{
				IntVec3 dest;
				if (this.CanHitFromCellIgnoringRange(root, targ, out dest))
				{
					resultingLine = new ShootLine(root, dest);
					return true;
				}
				ShootLeanUtility.LeanShootingSourcesFromTo(root, cellRect.ClosestCellTo(root), this.caster.Map, Verb.tempLeanShootSources);
				for (int i = 0; i < Verb.tempLeanShootSources.Count; i++)
				{
					IntVec3 intVec = Verb.tempLeanShootSources[i];
					if (this.CanHitFromCellIgnoringRange(intVec, targ, out dest))
					{
						resultingLine = new ShootLine(intVec, dest);
						return true;
					}
				}
			}
			else
			{
				foreach (IntVec3 intVec2 in this.caster.OccupiedRect())
				{
					IntVec3 dest;
					if (this.CanHitFromCellIgnoringRange(intVec2, targ, out dest))
					{
						resultingLine = new ShootLine(intVec2, dest);
						return true;
					}
				}
			}
			resultingLine = new ShootLine(root, targ.Cell);
			return false;
		}

		
		private bool CanHitFromCellIgnoringRange(IntVec3 sourceCell, LocalTargetInfo targ, out IntVec3 goodDest)
		{
			if (targ.Thing != null)
			{
				if (targ.Thing.Map != this.caster.Map)
				{
					goodDest = IntVec3.Invalid;
					return false;
				}
				ShootLeanUtility.CalcShootableCellsOf(Verb.tempDestList, targ.Thing);
				for (int i = 0; i < Verb.tempDestList.Count; i++)
				{
					if (this.CanHitCellFromCellIgnoringRange(sourceCell, Verb.tempDestList[i], targ.Thing.def.Fillage == FillCategory.Full))
					{
						goodDest = Verb.tempDestList[i];
						return true;
					}
				}
			}
			else if (this.CanHitCellFromCellIgnoringRange(sourceCell, targ.Cell, false))
			{
				goodDest = targ.Cell;
				return true;
			}
			goodDest = IntVec3.Invalid;
			return false;
		}

		
		private bool CanHitCellFromCellIgnoringRange(IntVec3 sourceSq, IntVec3 targetLoc, bool includeCorners = false)
		{
			if (this.verbProps.mustCastOnOpenGround && (!targetLoc.Standable(this.caster.Map) || this.caster.Map.thingGrid.CellContains(targetLoc, ThingCategory.Pawn)))
			{
				return false;
			}
			if (this.verbProps.requireLineOfSight)
			{
				if (!includeCorners)
				{
					if (!GenSight.LineOfSight(sourceSq, targetLoc, this.caster.Map, true, null, 0, 0))
					{
						return false;
					}
				}
				else if (!GenSight.LineOfSightToEdges(sourceSq, targetLoc, this.caster.Map, true, null))
				{
					return false;
				}
			}
			return true;
		}

		
		public override string ToString()
		{
			string text;
			if (this.verbProps == null)
			{
				text = "null";
			}
			else if (!this.verbProps.label.NullOrEmpty())
			{
				text = this.verbProps.label;
			}
			else if (this.HediffCompSource != null)
			{
				text = this.HediffCompSource.Def.label;
			}
			else if (this.EquipmentSource != null)
			{
				text = this.EquipmentSource.def.label;
			}
			else if (this.verbProps.AdjustedLinkedBodyPartsGroup(this.tool) != null)
			{
				text = this.verbProps.AdjustedLinkedBodyPartsGroup(this.tool).defName;
			}
			else
			{
				text = "unknown";
			}
			if (this.tool != null)
			{
				text = text + "/" + this.loadID;
			}
			return base.GetType().ToString() + "(" + text + ")";
		}

		
		public VerbProperties verbProps;

		
		public VerbTracker verbTracker;

		
		public ManeuverDef maneuver;

		
		public Tool tool;

		
		public Thing caster;

		
		public string loadID;

		
		public VerbState state;

		
		protected LocalTargetInfo currentTarget;

		
		protected LocalTargetInfo currentDestination;

		
		protected int burstShotsLeft;

		
		protected int ticksToNextBurstShot;

		
		protected bool surpriseAttack;

		
		protected bool canHitNonTargetPawnsNow = true;

		
		public Action castCompleteCallback;

		
		private static List<IntVec3> tempLeanShootSources = new List<IntVec3>();

		
		private static List<IntVec3> tempDestList = new List<IntVec3>();
	}
}
