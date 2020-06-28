using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000C5D RID: 3165
	[StaticConstructorOnStartup]
	public class Building_TurretGun : Building_Turret
	{
		// Token: 0x17000D4D RID: 3405
		// (get) Token: 0x06004BA4 RID: 19364 RVA: 0x00197A1C File Offset: 0x00195C1C
		public bool Active
		{
			get
			{
				return (this.powerComp == null || this.powerComp.PowerOn) && (this.dormantComp == null || this.dormantComp.Awake) && (this.initiatableComp == null || this.initiatableComp.Initiated);
			}
		}

		// Token: 0x17000D4E RID: 3406
		// (get) Token: 0x06004BA5 RID: 19365 RVA: 0x00197A6A File Offset: 0x00195C6A
		public CompEquippable GunCompEq
		{
			get
			{
				return this.gun.TryGetComp<CompEquippable>();
			}
		}

		// Token: 0x17000D4F RID: 3407
		// (get) Token: 0x06004BA6 RID: 19366 RVA: 0x00197A77 File Offset: 0x00195C77
		public override LocalTargetInfo CurrentTarget
		{
			get
			{
				return this.currentTargetInt;
			}
		}

		// Token: 0x17000D50 RID: 3408
		// (get) Token: 0x06004BA7 RID: 19367 RVA: 0x00197A7F File Offset: 0x00195C7F
		private bool WarmingUp
		{
			get
			{
				return this.burstWarmupTicksLeft > 0;
			}
		}

		// Token: 0x17000D51 RID: 3409
		// (get) Token: 0x06004BA8 RID: 19368 RVA: 0x00197A8A File Offset: 0x00195C8A
		public override Verb AttackVerb
		{
			get
			{
				return this.GunCompEq.PrimaryVerb;
			}
		}

		// Token: 0x17000D52 RID: 3410
		// (get) Token: 0x06004BA9 RID: 19369 RVA: 0x00197A97 File Offset: 0x00195C97
		public bool IsMannable
		{
			get
			{
				return this.mannableComp != null;
			}
		}

		// Token: 0x17000D53 RID: 3411
		// (get) Token: 0x06004BAA RID: 19370 RVA: 0x00197AA2 File Offset: 0x00195CA2
		private bool PlayerControlled
		{
			get
			{
				return (base.Faction == Faction.OfPlayer || this.MannedByColonist) && !this.MannedByNonColonist;
			}
		}

		// Token: 0x17000D54 RID: 3412
		// (get) Token: 0x06004BAB RID: 19371 RVA: 0x00197AC4 File Offset: 0x00195CC4
		private bool CanSetForcedTarget
		{
			get
			{
				return this.mannableComp != null && this.PlayerControlled;
			}
		}

		// Token: 0x17000D55 RID: 3413
		// (get) Token: 0x06004BAC RID: 19372 RVA: 0x00197AD6 File Offset: 0x00195CD6
		private bool CanToggleHoldFire
		{
			get
			{
				return this.PlayerControlled;
			}
		}

		// Token: 0x17000D56 RID: 3414
		// (get) Token: 0x06004BAD RID: 19373 RVA: 0x00197ADE File Offset: 0x00195CDE
		private bool IsMortar
		{
			get
			{
				return this.def.building.IsMortar;
			}
		}

		// Token: 0x17000D57 RID: 3415
		// (get) Token: 0x06004BAE RID: 19374 RVA: 0x00197AF0 File Offset: 0x00195CF0
		private bool IsMortarOrProjectileFliesOverhead
		{
			get
			{
				return this.AttackVerb.ProjectileFliesOverhead() || this.IsMortar;
			}
		}

		// Token: 0x17000D58 RID: 3416
		// (get) Token: 0x06004BAF RID: 19375 RVA: 0x00197B08 File Offset: 0x00195D08
		private bool CanExtractShell
		{
			get
			{
				if (!this.PlayerControlled)
				{
					return false;
				}
				CompChangeableProjectile compChangeableProjectile = this.gun.TryGetComp<CompChangeableProjectile>();
				return compChangeableProjectile != null && compChangeableProjectile.Loaded;
			}
		}

		// Token: 0x17000D59 RID: 3417
		// (get) Token: 0x06004BB0 RID: 19376 RVA: 0x00197B36 File Offset: 0x00195D36
		private bool MannedByColonist
		{
			get
			{
				return this.mannableComp != null && this.mannableComp.ManningPawn != null && this.mannableComp.ManningPawn.Faction == Faction.OfPlayer;
			}
		}

		// Token: 0x17000D5A RID: 3418
		// (get) Token: 0x06004BB1 RID: 19377 RVA: 0x00197B66 File Offset: 0x00195D66
		private bool MannedByNonColonist
		{
			get
			{
				return this.mannableComp != null && this.mannableComp.ManningPawn != null && this.mannableComp.ManningPawn.Faction != Faction.OfPlayer;
			}
		}

		// Token: 0x06004BB2 RID: 19378 RVA: 0x00197B99 File Offset: 0x00195D99
		public Building_TurretGun()
		{
			this.top = new TurretTop(this);
		}

		// Token: 0x06004BB3 RID: 19379 RVA: 0x00197BB8 File Offset: 0x00195DB8
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			this.dormantComp = base.GetComp<CompCanBeDormant>();
			this.initiatableComp = base.GetComp<CompInitiatable>();
			this.powerComp = base.GetComp<CompPowerTrader>();
			this.mannableComp = base.GetComp<CompMannable>();
			if (!respawningAfterLoad)
			{
				this.top.SetRotationFromOrientation();
				this.burstCooldownTicksLeft = this.def.building.turretInitialCooldownTime.SecondsToTicks();
			}
		}

		// Token: 0x06004BB4 RID: 19380 RVA: 0x00197C26 File Offset: 0x00195E26
		public override void PostMake()
		{
			base.PostMake();
			this.MakeGun();
		}

		// Token: 0x06004BB5 RID: 19381 RVA: 0x00197C34 File Offset: 0x00195E34
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			base.DeSpawn(mode);
			this.ResetCurrentTarget();
			Effecter effecter = this.progressBarEffecter;
			if (effecter == null)
			{
				return;
			}
			effecter.Cleanup();
		}

		// Token: 0x06004BB6 RID: 19382 RVA: 0x00197C54 File Offset: 0x00195E54
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.burstCooldownTicksLeft, "burstCooldownTicksLeft", 0, false);
			Scribe_Values.Look<int>(ref this.burstWarmupTicksLeft, "burstWarmupTicksLeft", 0, false);
			Scribe_TargetInfo.Look(ref this.currentTargetInt, "currentTarget");
			Scribe_Values.Look<bool>(ref this.holdFire, "holdFire", false, false);
			Scribe_Deep.Look<Thing>(ref this.gun, "gun", Array.Empty<object>());
			BackCompatibility.PostExposeData(this);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.UpdateGunVerbs();
			}
		}

		// Token: 0x06004BB7 RID: 19383 RVA: 0x00197CD8 File Offset: 0x00195ED8
		public override bool ClaimableBy(Faction by)
		{
			return base.ClaimableBy(by) && (this.mannableComp == null || this.mannableComp.ManningPawn == null) && (!this.Active || this.mannableComp != null) && (((this.dormantComp == null || this.dormantComp.Awake) && (this.initiatableComp == null || this.initiatableComp.Initiated)) || (this.powerComp != null && !this.powerComp.PowerOn));
		}

		// Token: 0x06004BB8 RID: 19384 RVA: 0x00197D5C File Offset: 0x00195F5C
		public override void OrderAttack(LocalTargetInfo targ)
		{
			if (!targ.IsValid)
			{
				if (this.forcedTarget.IsValid)
				{
					this.ResetForcedTarget();
				}
				return;
			}
			if ((targ.Cell - base.Position).LengthHorizontal < this.AttackVerb.verbProps.EffectiveMinRange(targ, this))
			{
				Messages.Message("MessageTargetBelowMinimumRange".Translate(), this, MessageTypeDefOf.RejectInput, false);
				return;
			}
			if ((targ.Cell - base.Position).LengthHorizontal > this.AttackVerb.verbProps.range)
			{
				Messages.Message("MessageTargetBeyondMaximumRange".Translate(), this, MessageTypeDefOf.RejectInput, false);
				return;
			}
			if (this.forcedTarget != targ)
			{
				this.forcedTarget = targ;
				if (this.burstCooldownTicksLeft <= 0)
				{
					this.TryStartShootSomething(false);
				}
			}
			if (this.holdFire)
			{
				Messages.Message("MessageTurretWontFireBecauseHoldFire".Translate(this.def.label), this, MessageTypeDefOf.RejectInput, false);
			}
		}

		// Token: 0x06004BB9 RID: 19385 RVA: 0x00197E80 File Offset: 0x00196080
		public override void Tick()
		{
			base.Tick();
			if (this.CanExtractShell && this.MannedByColonist)
			{
				CompChangeableProjectile compChangeableProjectile = this.gun.TryGetComp<CompChangeableProjectile>();
				if (!compChangeableProjectile.allowedShellsSettings.AllowedToAccept(compChangeableProjectile.LoadedShell))
				{
					this.ExtractShell();
				}
			}
			if (this.forcedTarget.IsValid && !this.CanSetForcedTarget)
			{
				this.ResetForcedTarget();
			}
			if (!this.CanToggleHoldFire)
			{
				this.holdFire = false;
			}
			if (this.forcedTarget.ThingDestroyed)
			{
				this.ResetForcedTarget();
			}
			if (this.Active && (this.mannableComp == null || this.mannableComp.MannedNow) && base.Spawned)
			{
				this.GunCompEq.verbTracker.VerbsTick();
				if (!this.stunner.Stunned && this.AttackVerb.state != VerbState.Bursting)
				{
					if (this.WarmingUp)
					{
						this.burstWarmupTicksLeft--;
						if (this.burstWarmupTicksLeft == 0)
						{
							this.BeginBurst();
						}
					}
					else
					{
						if (this.burstCooldownTicksLeft > 0)
						{
							this.burstCooldownTicksLeft--;
							if (this.IsMortar)
							{
								if (this.progressBarEffecter == null)
								{
									this.progressBarEffecter = EffecterDefOf.ProgressBar.Spawn();
								}
								this.progressBarEffecter.EffectTick(this, TargetInfo.Invalid);
								MoteProgressBar mote = ((SubEffecter_ProgressBar)this.progressBarEffecter.children[0]).mote;
								mote.progress = 1f - (float)Math.Max(this.burstCooldownTicksLeft, 0) / (float)this.BurstCooldownTime().SecondsToTicks();
								mote.offsetZ = -0.8f;
							}
						}
						if (this.burstCooldownTicksLeft <= 0 && this.IsHashIntervalTick(10))
						{
							this.TryStartShootSomething(true);
						}
					}
					this.top.TurretTopTick();
					return;
				}
			}
			else
			{
				this.ResetCurrentTarget();
			}
		}

		// Token: 0x06004BBA RID: 19386 RVA: 0x0019805C File Offset: 0x0019625C
		protected void TryStartShootSomething(bool canBeginBurstImmediately)
		{
			if (this.progressBarEffecter != null)
			{
				this.progressBarEffecter.Cleanup();
				this.progressBarEffecter = null;
			}
			if (!base.Spawned || (this.holdFire && this.CanToggleHoldFire) || (this.AttackVerb.ProjectileFliesOverhead() && base.Map.roofGrid.Roofed(base.Position)) || !this.AttackVerb.Available())
			{
				this.ResetCurrentTarget();
				return;
			}
			bool isValid = this.currentTargetInt.IsValid;
			if (this.forcedTarget.IsValid)
			{
				this.currentTargetInt = this.forcedTarget;
			}
			else
			{
				this.currentTargetInt = this.TryFindNewTarget();
			}
			if (!isValid && this.currentTargetInt.IsValid)
			{
				SoundDefOf.TurretAcquireTarget.PlayOneShot(new TargetInfo(base.Position, base.Map, false));
			}
			if (!this.currentTargetInt.IsValid)
			{
				this.ResetCurrentTarget();
				return;
			}
			if (this.def.building.turretBurstWarmupTime > 0f)
			{
				this.burstWarmupTicksLeft = this.def.building.turretBurstWarmupTime.SecondsToTicks();
				return;
			}
			if (canBeginBurstImmediately)
			{
				this.BeginBurst();
				return;
			}
			this.burstWarmupTicksLeft = 1;
		}

		// Token: 0x06004BBB RID: 19387 RVA: 0x00198190 File Offset: 0x00196390
		protected LocalTargetInfo TryFindNewTarget()
		{
			IAttackTargetSearcher attackTargetSearcher = this.TargSearcher();
			Faction faction = attackTargetSearcher.Thing.Faction;
			float range = this.AttackVerb.verbProps.range;
			Building t;
			if (Rand.Value < 0.5f && this.AttackVerb.ProjectileFliesOverhead() && faction.HostileTo(Faction.OfPlayer) && base.Map.listerBuildings.allBuildingsColonist.Where(delegate(Building x)
			{
				float num = this.AttackVerb.verbProps.EffectiveMinRange(x, this);
				float num2 = (float)x.Position.DistanceToSquared(this.Position);
				return num2 > num * num && num2 < range * range;
			}).TryRandomElement(out t))
			{
				return t;
			}
			TargetScanFlags targetScanFlags = TargetScanFlags.NeedThreat | TargetScanFlags.NeedAutoTargetable;
			if (!this.AttackVerb.ProjectileFliesOverhead())
			{
				targetScanFlags |= TargetScanFlags.NeedLOSToAll;
				targetScanFlags |= TargetScanFlags.LOSBlockableByGas;
			}
			if (this.AttackVerb.IsIncendiary())
			{
				targetScanFlags |= TargetScanFlags.NeedNonBurning;
			}
			return (Thing)AttackTargetFinder.BestShootTargetFromCurrentPosition(attackTargetSearcher, targetScanFlags, new Predicate<Thing>(this.IsValidTarget), 0f, 9999f);
		}

		// Token: 0x06004BBC RID: 19388 RVA: 0x00198283 File Offset: 0x00196483
		private IAttackTargetSearcher TargSearcher()
		{
			if (this.mannableComp != null && this.mannableComp.MannedNow)
			{
				return this.mannableComp.ManningPawn;
			}
			return this;
		}

		// Token: 0x06004BBD RID: 19389 RVA: 0x001982A8 File Offset: 0x001964A8
		private bool IsValidTarget(Thing t)
		{
			Pawn pawn = t as Pawn;
			if (pawn != null)
			{
				if (this.AttackVerb.ProjectileFliesOverhead())
				{
					RoofDef roofDef = base.Map.roofGrid.RoofAt(t.Position);
					if (roofDef != null && roofDef.isThickRoof)
					{
						return false;
					}
				}
				if (this.mannableComp == null)
				{
					return !GenAI.MachinesLike(base.Faction, pawn);
				}
				if (pawn.RaceProps.Animal && pawn.Faction == Faction.OfPlayer)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06004BBE RID: 19390 RVA: 0x00198325 File Offset: 0x00196525
		protected void BeginBurst()
		{
			this.AttackVerb.TryStartCastOn(this.CurrentTarget, false, true);
			base.OnAttackedTarget(this.CurrentTarget);
		}

		// Token: 0x06004BBF RID: 19391 RVA: 0x00198347 File Offset: 0x00196547
		protected void BurstComplete()
		{
			this.burstCooldownTicksLeft = this.BurstCooldownTime().SecondsToTicks();
		}

		// Token: 0x06004BC0 RID: 19392 RVA: 0x0019835A File Offset: 0x0019655A
		protected float BurstCooldownTime()
		{
			if (this.def.building.turretBurstCooldownTime >= 0f)
			{
				return this.def.building.turretBurstCooldownTime;
			}
			return this.AttackVerb.verbProps.defaultCooldownTime;
		}

		// Token: 0x06004BC1 RID: 19393 RVA: 0x00198394 File Offset: 0x00196594
		public override string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			string inspectString = base.GetInspectString();
			if (!inspectString.NullOrEmpty())
			{
				stringBuilder.AppendLine(inspectString);
			}
			if (this.AttackVerb.verbProps.minRange > 0f)
			{
				stringBuilder.AppendLine("MinimumRange".Translate() + ": " + this.AttackVerb.verbProps.minRange.ToString("F0"));
			}
			if (base.Spawned && this.IsMortarOrProjectileFliesOverhead && base.Position.Roofed(base.Map))
			{
				stringBuilder.AppendLine("CannotFire".Translate() + ": " + "Roofed".Translate().CapitalizeFirst());
			}
			else if (base.Spawned && this.burstCooldownTicksLeft > 0 && this.BurstCooldownTime() > 5f)
			{
				stringBuilder.AppendLine("CanFireIn".Translate() + ": " + this.burstCooldownTicksLeft.ToStringSecondsFromTicks());
			}
			CompChangeableProjectile compChangeableProjectile = this.gun.TryGetComp<CompChangeableProjectile>();
			if (compChangeableProjectile != null)
			{
				if (compChangeableProjectile.Loaded)
				{
					stringBuilder.AppendLine("ShellLoaded".Translate(compChangeableProjectile.LoadedShell.LabelCap, compChangeableProjectile.LoadedShell));
				}
				else
				{
					stringBuilder.AppendLine("ShellNotLoaded".Translate());
				}
			}
			return stringBuilder.ToString().TrimEndNewlines();
		}

		// Token: 0x06004BC2 RID: 19394 RVA: 0x00198529 File Offset: 0x00196729
		public override void Draw()
		{
			this.top.DrawTurret();
			base.Draw();
		}

		// Token: 0x06004BC3 RID: 19395 RVA: 0x0019853C File Offset: 0x0019673C
		public override void DrawExtraSelectionOverlays()
		{
			float range = this.AttackVerb.verbProps.range;
			if (range < 90f)
			{
				GenDraw.DrawRadiusRing(base.Position, range);
			}
			float num = this.AttackVerb.verbProps.EffectiveMinRange(true);
			if (num < 90f && num > 0.1f)
			{
				GenDraw.DrawRadiusRing(base.Position, num);
			}
			if (this.WarmingUp)
			{
				int degreesWide = (int)((float)this.burstWarmupTicksLeft * 0.5f);
				GenDraw.DrawAimPie(this, this.CurrentTarget, degreesWide, (float)this.def.size.x * 0.5f);
			}
			if (this.forcedTarget.IsValid && (!this.forcedTarget.HasThing || this.forcedTarget.Thing.Spawned))
			{
				Vector3 vector;
				if (this.forcedTarget.HasThing)
				{
					vector = this.forcedTarget.Thing.TrueCenter();
				}
				else
				{
					vector = this.forcedTarget.Cell.ToVector3Shifted();
				}
				Vector3 a = this.TrueCenter();
				vector.y = AltitudeLayer.MetaOverlays.AltitudeFor();
				a.y = vector.y;
				GenDraw.DrawLineBetween(a, vector, Building_TurretGun.ForcedTargetLineMat);
			}
		}

		// Token: 0x06004BC4 RID: 19396 RVA: 0x0019866B File Offset: 0x0019686B
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in this.<>n__0())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			if (this.CanExtractShell)
			{
				CompChangeableProjectile compChangeableProjectile = this.gun.TryGetComp<CompChangeableProjectile>();
				yield return new Command_Action
				{
					defaultLabel = "CommandExtractShell".Translate(),
					defaultDesc = "CommandExtractShellDesc".Translate(),
					icon = compChangeableProjectile.LoadedShell.uiIcon,
					iconAngle = compChangeableProjectile.LoadedShell.uiIconAngle,
					iconOffset = compChangeableProjectile.LoadedShell.uiIconOffset,
					iconDrawScale = GenUI.IconDrawScale(compChangeableProjectile.LoadedShell),
					action = delegate
					{
						this.ExtractShell();
					}
				};
			}
			CompChangeableProjectile compChangeableProjectile2 = this.gun.TryGetComp<CompChangeableProjectile>();
			if (compChangeableProjectile2 != null)
			{
				StorageSettings storeSettings = compChangeableProjectile2.GetStoreSettings();
				foreach (Gizmo gizmo2 in StorageSettingsClipboard.CopyPasteGizmosFor(storeSettings))
				{
					yield return gizmo2;
				}
				enumerator = null;
			}
			if (this.CanSetForcedTarget)
			{
				Command_VerbTarget command_VerbTarget = new Command_VerbTarget();
				command_VerbTarget.defaultLabel = "CommandSetForceAttackTarget".Translate();
				command_VerbTarget.defaultDesc = "CommandSetForceAttackTargetDesc".Translate();
				command_VerbTarget.icon = ContentFinder<Texture2D>.Get("UI/Commands/Attack", true);
				command_VerbTarget.verb = this.AttackVerb;
				command_VerbTarget.hotKey = KeyBindingDefOf.Misc4;
				command_VerbTarget.drawRadius = false;
				if (base.Spawned && this.IsMortarOrProjectileFliesOverhead && base.Position.Roofed(base.Map))
				{
					command_VerbTarget.Disable("CannotFire".Translate() + ": " + "Roofed".Translate().CapitalizeFirst());
				}
				yield return command_VerbTarget;
			}
			if (this.forcedTarget.IsValid)
			{
				Command_Action command_Action = new Command_Action();
				command_Action.defaultLabel = "CommandStopForceAttack".Translate();
				command_Action.defaultDesc = "CommandStopForceAttackDesc".Translate();
				command_Action.icon = ContentFinder<Texture2D>.Get("UI/Commands/Halt", true);
				command_Action.action = delegate
				{
					this.ResetForcedTarget();
					SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
				};
				if (!this.forcedTarget.IsValid)
				{
					command_Action.Disable("CommandStopAttackFailNotForceAttacking".Translate());
				}
				command_Action.hotKey = KeyBindingDefOf.Misc5;
				yield return command_Action;
			}
			if (this.CanToggleHoldFire)
			{
				yield return new Command_Toggle
				{
					defaultLabel = "CommandHoldFire".Translate(),
					defaultDesc = "CommandHoldFireDesc".Translate(),
					icon = ContentFinder<Texture2D>.Get("UI/Commands/HoldFire", true),
					hotKey = KeyBindingDefOf.Misc6,
					toggleAction = delegate
					{
						this.holdFire = !this.holdFire;
						if (this.holdFire)
						{
							this.ResetForcedTarget();
						}
					},
					isActive = (() => this.holdFire)
				};
			}
			yield break;
			yield break;
		}

		// Token: 0x06004BC5 RID: 19397 RVA: 0x0019867C File Offset: 0x0019687C
		private void ExtractShell()
		{
			GenPlace.TryPlaceThing(this.gun.TryGetComp<CompChangeableProjectile>().RemoveShell(), base.Position, base.Map, ThingPlaceMode.Near, null, null, default(Rot4));
		}

		// Token: 0x06004BC6 RID: 19398 RVA: 0x001986B7 File Offset: 0x001968B7
		private void ResetForcedTarget()
		{
			this.forcedTarget = LocalTargetInfo.Invalid;
			this.burstWarmupTicksLeft = 0;
			if (this.burstCooldownTicksLeft <= 0)
			{
				this.TryStartShootSomething(false);
			}
		}

		// Token: 0x06004BC7 RID: 19399 RVA: 0x001986DB File Offset: 0x001968DB
		private void ResetCurrentTarget()
		{
			this.currentTargetInt = LocalTargetInfo.Invalid;
			this.burstWarmupTicksLeft = 0;
		}

		// Token: 0x06004BC8 RID: 19400 RVA: 0x001986EF File Offset: 0x001968EF
		public void MakeGun()
		{
			this.gun = ThingMaker.MakeThing(this.def.building.turretGunDef, null);
			this.UpdateGunVerbs();
		}

		// Token: 0x06004BC9 RID: 19401 RVA: 0x00198714 File Offset: 0x00196914
		private void UpdateGunVerbs()
		{
			List<Verb> allVerbs = this.gun.TryGetComp<CompEquippable>().AllVerbs;
			for (int i = 0; i < allVerbs.Count; i++)
			{
				Verb verb = allVerbs[i];
				verb.caster = this;
				verb.castCompleteCallback = new Action(this.BurstComplete);
			}
		}

		// Token: 0x04002ABD RID: 10941
		protected int burstCooldownTicksLeft;

		// Token: 0x04002ABE RID: 10942
		protected int burstWarmupTicksLeft;

		// Token: 0x04002ABF RID: 10943
		protected LocalTargetInfo currentTargetInt = LocalTargetInfo.Invalid;

		// Token: 0x04002AC0 RID: 10944
		private bool holdFire;

		// Token: 0x04002AC1 RID: 10945
		public Thing gun;

		// Token: 0x04002AC2 RID: 10946
		protected TurretTop top;

		// Token: 0x04002AC3 RID: 10947
		protected CompPowerTrader powerComp;

		// Token: 0x04002AC4 RID: 10948
		protected CompCanBeDormant dormantComp;

		// Token: 0x04002AC5 RID: 10949
		protected CompInitiatable initiatableComp;

		// Token: 0x04002AC6 RID: 10950
		protected CompMannable mannableComp;

		// Token: 0x04002AC7 RID: 10951
		protected Effecter progressBarEffecter;

		// Token: 0x04002AC8 RID: 10952
		private const int TryStartShootSomethingIntervalTicks = 10;

		// Token: 0x04002AC9 RID: 10953
		public static Material ForcedTargetLineMat = MaterialPool.MatFrom(GenDraw.LineTexPath, ShaderDatabase.Transparent, new Color(1f, 0.5f, 0.5f));
	}
}
