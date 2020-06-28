using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse.AI;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000484 RID: 1156
	public abstract class Verb : ITargetingSource, IExposable, ILoadReferenceable
	{
		// Token: 0x170006AC RID: 1708
		// (get) Token: 0x06002218 RID: 8728 RVA: 0x000D01BE File Offset: 0x000CE3BE
		public IVerbOwner DirectOwner
		{
			get
			{
				return this.verbTracker.directOwner;
			}
		}

		// Token: 0x170006AD RID: 1709
		// (get) Token: 0x06002219 RID: 8729 RVA: 0x000D01CB File Offset: 0x000CE3CB
		public ImplementOwnerTypeDef ImplementOwnerType
		{
			get
			{
				return this.verbTracker.directOwner.ImplementOwnerTypeDef;
			}
		}

		// Token: 0x170006AE RID: 1710
		// (get) Token: 0x0600221A RID: 8730 RVA: 0x000D01DD File Offset: 0x000CE3DD
		public CompEquippable EquipmentCompSource
		{
			get
			{
				return this.DirectOwner as CompEquippable;
			}
		}

		// Token: 0x170006AF RID: 1711
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

		// Token: 0x170006B0 RID: 1712
		// (get) Token: 0x0600221C RID: 8732 RVA: 0x000D0201 File Offset: 0x000CE401
		public HediffComp_VerbGiver HediffCompSource
		{
			get
			{
				return this.DirectOwner as HediffComp_VerbGiver;
			}
		}

		// Token: 0x170006B1 RID: 1713
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

		// Token: 0x170006B2 RID: 1714
		// (get) Token: 0x0600221E RID: 8734 RVA: 0x000D0225 File Offset: 0x000CE425
		public Pawn_MeleeVerbs_TerrainSource TerrainSource
		{
			get
			{
				return this.DirectOwner as Pawn_MeleeVerbs_TerrainSource;
			}
		}

		// Token: 0x170006B3 RID: 1715
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

		// Token: 0x170006B4 RID: 1716
		// (get) Token: 0x06002220 RID: 8736 RVA: 0x000D0249 File Offset: 0x000CE449
		public virtual Thing Caster
		{
			get
			{
				return this.caster;
			}
		}

		// Token: 0x170006B5 RID: 1717
		// (get) Token: 0x06002221 RID: 8737 RVA: 0x000D0251 File Offset: 0x000CE451
		public virtual Pawn CasterPawn
		{
			get
			{
				return this.caster as Pawn;
			}
		}

		// Token: 0x170006B6 RID: 1718
		// (get) Token: 0x06002222 RID: 8738 RVA: 0x0006461A File Offset: 0x0006281A
		public virtual Verb GetVerb
		{
			get
			{
				return this;
			}
		}

		// Token: 0x170006B7 RID: 1719
		// (get) Token: 0x06002223 RID: 8739 RVA: 0x000D025E File Offset: 0x000CE45E
		public virtual bool CasterIsPawn
		{
			get
			{
				return this.caster is Pawn;
			}
		}

		// Token: 0x170006B8 RID: 1720
		// (get) Token: 0x06002224 RID: 8740 RVA: 0x000D026E File Offset: 0x000CE46E
		public virtual bool Targetable
		{
			get
			{
				return this.verbProps.targetable;
			}
		}

		// Token: 0x170006B9 RID: 1721
		// (get) Token: 0x06002225 RID: 8741 RVA: 0x00010306 File Offset: 0x0000E506
		public virtual bool MultiSelect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170006BA RID: 1722
		// (get) Token: 0x06002226 RID: 8742 RVA: 0x000D027B File Offset: 0x000CE47B
		public LocalTargetInfo CurrentTarget
		{
			get
			{
				return this.currentTarget;
			}
		}

		// Token: 0x170006BB RID: 1723
		// (get) Token: 0x06002227 RID: 8743 RVA: 0x000D0283 File Offset: 0x000CE483
		public virtual TargetingParameters targetParams
		{
			get
			{
				return this.verbProps.targetParams;
			}
		}

		// Token: 0x170006BC RID: 1724
		// (get) Token: 0x06002228 RID: 8744 RVA: 0x00019EA1 File Offset: 0x000180A1
		public virtual ITargetingSource DestinationSelector
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170006BD RID: 1725
		// (get) Token: 0x06002229 RID: 8745 RVA: 0x0001028D File Offset: 0x0000E48D
		protected virtual int ShotsPerBurst
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x170006BE RID: 1726
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

		// Token: 0x170006BF RID: 1727
		// (get) Token: 0x0600222B RID: 8747 RVA: 0x000D02B0 File Offset: 0x000CE4B0
		public bool Bursting
		{
			get
			{
				return this.burstShotsLeft > 0;
			}
		}

		// Token: 0x170006C0 RID: 1728
		// (get) Token: 0x0600222C RID: 8748 RVA: 0x000D02BB File Offset: 0x000CE4BB
		public virtual bool IsMeleeAttack
		{
			get
			{
				return this.verbProps.IsMeleeAttack;
			}
		}

		// Token: 0x170006C1 RID: 1729
		// (get) Token: 0x0600222D RID: 8749 RVA: 0x000D02C8 File Offset: 0x000CE4C8
		public bool BuggedAfterLoading
		{
			get
			{
				return this.verbProps == null;
			}
		}

		// Token: 0x170006C2 RID: 1730
		// (get) Token: 0x0600222E RID: 8750 RVA: 0x000D02D3 File Offset: 0x000CE4D3
		public bool WarmingUp
		{
			get
			{
				return this.WarmupStance != null;
			}
		}

		// Token: 0x170006C3 RID: 1731
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

		// Token: 0x170006C4 RID: 1732
		// (get) Token: 0x06002230 RID: 8752 RVA: 0x000D0329 File Offset: 0x000CE529
		public virtual string ReportLabel
		{
			get
			{
				return this.verbProps.label;
			}
		}

		// Token: 0x06002231 RID: 8753 RVA: 0x000D0336 File Offset: 0x000CE536
		public bool IsStillUsableBy(Pawn pawn)
		{
			return this.Available() && this.DirectOwner.VerbsStillUsableBy(pawn) && this.verbProps.GetDamageFactorFor(this, pawn) != 0f;
		}

		// Token: 0x06002232 RID: 8754 RVA: 0x0001028D File Offset: 0x0000E48D
		public virtual bool IsUsableOn(Thing target)
		{
			return true;
		}

		// Token: 0x06002233 RID: 8755 RVA: 0x000D036C File Offset: 0x000CE56C
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

		// Token: 0x06002234 RID: 8756 RVA: 0x000D0405 File Offset: 0x000CE605
		public string GetUniqueLoadID()
		{
			return "Verb_" + this.loadID;
		}

		// Token: 0x06002235 RID: 8757 RVA: 0x000D0417 File Offset: 0x000CE617
		public static string CalculateUniqueLoadID(IVerbOwner owner, Tool tool, ManeuverDef maneuver)
		{
			return string.Format("{0}_{1}_{2}", owner.UniqueVerbOwnerID(), (tool != null) ? tool.id : "NT", (maneuver != null) ? maneuver.defName : "NM");
		}

		// Token: 0x06002236 RID: 8758 RVA: 0x000D0449 File Offset: 0x000CE649
		public static string CalculateUniqueLoadID(IVerbOwner owner, int index)
		{
			return string.Format("{0}_{1}", owner.UniqueVerbOwnerID(), index);
		}

		// Token: 0x06002237 RID: 8759 RVA: 0x000D0461 File Offset: 0x000CE661
		public bool TryStartCastOn(LocalTargetInfo castTarg, bool surpriseAttack = false, bool canHitNonTargetPawns = true)
		{
			return this.TryStartCastOn(castTarg, LocalTargetInfo.Invalid, surpriseAttack, canHitNonTargetPawns);
		}

		// Token: 0x06002238 RID: 8760 RVA: 0x000D0474 File Offset: 0x000CE674
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

		// Token: 0x06002239 RID: 8761 RVA: 0x000D0598 File Offset: 0x000CE798
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

		// Token: 0x0600223A RID: 8762 RVA: 0x000D0604 File Offset: 0x000CE804
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

		// Token: 0x0600223B RID: 8763 RVA: 0x000D0640 File Offset: 0x000CE840
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

		// Token: 0x0600223C RID: 8764 RVA: 0x000D0684 File Offset: 0x000CE884
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

		// Token: 0x0600223D RID: 8765 RVA: 0x000D08F8 File Offset: 0x000CEAF8
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

		// Token: 0x0600223E RID: 8766
		protected abstract bool TryCastShot();

		// Token: 0x0600223F RID: 8767 RVA: 0x000D0A0B File Offset: 0x000CEC0B
		public void Notify_PickedUp()
		{
			this.Reset();
		}

		// Token: 0x06002240 RID: 8768 RVA: 0x000D0A13 File Offset: 0x000CEC13
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

		// Token: 0x06002241 RID: 8769 RVA: 0x000D0A50 File Offset: 0x000CEC50
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

		// Token: 0x06002242 RID: 8770 RVA: 0x000D0AC4 File Offset: 0x000CECC4
		public virtual float HighlightFieldRadiusAroundTarget(out bool needLOSToCenter)
		{
			needLOSToCenter = false;
			return 0f;
		}

		// Token: 0x06002243 RID: 8771 RVA: 0x000D0AD0 File Offset: 0x000CECD0
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

		// Token: 0x06002244 RID: 8772 RVA: 0x000D0B88 File Offset: 0x000CED88
		public virtual bool CanHitTarget(LocalTargetInfo targ)
		{
			return this.caster != null && this.caster.Spawned && (targ == this.caster || this.CanHitTargetFrom(this.caster.Position, targ));
		}

		// Token: 0x06002245 RID: 8773 RVA: 0x0001028D File Offset: 0x0000E48D
		public virtual bool ValidateTarget(LocalTargetInfo target)
		{
			return true;
		}

		// Token: 0x06002246 RID: 8774 RVA: 0x000D0BC8 File Offset: 0x000CEDC8
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

		// Token: 0x06002247 RID: 8775 RVA: 0x000D0C70 File Offset: 0x000CEE70
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

		// Token: 0x06002248 RID: 8776 RVA: 0x000D0CB8 File Offset: 0x000CEEB8
		public virtual bool CanHitTargetFrom(IntVec3 root, LocalTargetInfo targ)
		{
			if (targ.Thing != null && targ.Thing == this.caster)
			{
				return this.targetParams.canTargetSelf;
			}
			ShootLine shootLine;
			return !this.ApparelPreventsShooting(root, targ) && this.TryFindShootLineFromTo(root, targ, out shootLine);
		}

		// Token: 0x06002249 RID: 8777 RVA: 0x000D0D00 File Offset: 0x000CEF00
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

		// Token: 0x0600224A RID: 8778 RVA: 0x000D0D64 File Offset: 0x000CEF64
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

		// Token: 0x0600224B RID: 8779 RVA: 0x000D0F84 File Offset: 0x000CF184
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

		// Token: 0x0600224C RID: 8780 RVA: 0x000D1054 File Offset: 0x000CF254
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

		// Token: 0x0600224D RID: 8781 RVA: 0x000D10E0 File Offset: 0x000CF2E0
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

		// Token: 0x04001500 RID: 5376
		public VerbProperties verbProps;

		// Token: 0x04001501 RID: 5377
		public VerbTracker verbTracker;

		// Token: 0x04001502 RID: 5378
		public ManeuverDef maneuver;

		// Token: 0x04001503 RID: 5379
		public Tool tool;

		// Token: 0x04001504 RID: 5380
		public Thing caster;

		// Token: 0x04001505 RID: 5381
		public string loadID;

		// Token: 0x04001506 RID: 5382
		public VerbState state;

		// Token: 0x04001507 RID: 5383
		protected LocalTargetInfo currentTarget;

		// Token: 0x04001508 RID: 5384
		protected LocalTargetInfo currentDestination;

		// Token: 0x04001509 RID: 5385
		protected int burstShotsLeft;

		// Token: 0x0400150A RID: 5386
		protected int ticksToNextBurstShot;

		// Token: 0x0400150B RID: 5387
		protected bool surpriseAttack;

		// Token: 0x0400150C RID: 5388
		protected bool canHitNonTargetPawnsNow = true;

		// Token: 0x0400150D RID: 5389
		public Action castCompleteCallback;

		// Token: 0x0400150E RID: 5390
		private static List<IntVec3> tempLeanShootSources = new List<IntVec3>();

		// Token: 0x0400150F RID: 5391
		private static List<IntVec3> tempDestList = new List<IntVec3>();
	}
}
