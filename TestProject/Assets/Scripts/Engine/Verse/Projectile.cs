﻿using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	
	public abstract class Projectile : ThingWithComps
	{
		
		// (get) Token: 0x060015E7 RID: 5607 RVA: 0x0007F40B File Offset: 0x0007D60B
		// (set) Token: 0x060015E8 RID: 5608 RVA: 0x0007F43B File Offset: 0x0007D63B
		public ProjectileHitFlags HitFlags
		{
			get
			{
				if (this.def.projectile.alwaysFreeIntercept)
				{
					return ProjectileHitFlags.All;
				}
				if (this.def.projectile.flyOverhead)
				{
					return ProjectileHitFlags.None;
				}
				return this.desiredHitFlags;
			}
			set
			{
				this.desiredHitFlags = value;
			}
		}

		
		// (get) Token: 0x060015E9 RID: 5609 RVA: 0x0007F444 File Offset: 0x0007D644
		protected float StartingTicksToImpact
		{
			get
			{
				float num = (this.origin - this.destination).magnitude / this.def.projectile.SpeedTilesPerTick;
				if (num <= 0f)
				{
					num = 0.001f;
				}
				return num;
			}
		}

		
		// (get) Token: 0x060015EA RID: 5610 RVA: 0x0007F48B File Offset: 0x0007D68B
		protected IntVec3 DestinationCell
		{
			get
			{
				return new IntVec3(this.destination);
			}
		}

		
		// (get) Token: 0x060015EB RID: 5611 RVA: 0x0007F498 File Offset: 0x0007D698
		public virtual Vector3 ExactPosition
		{
			get
			{
				Vector3 b = (this.destination - this.origin) * Mathf.Clamp01(1f - (float)this.ticksToImpact / this.StartingTicksToImpact);
				return this.origin + b + Vector3.up * this.def.Altitude;
			}
		}

		
		// (get) Token: 0x060015EC RID: 5612 RVA: 0x0007F4FB File Offset: 0x0007D6FB
		public virtual Quaternion ExactRotation
		{
			get
			{
				return Quaternion.LookRotation(this.destination - this.origin);
			}
		}

		
		// (get) Token: 0x060015ED RID: 5613 RVA: 0x0007F513 File Offset: 0x0007D713
		public override Vector3 DrawPos
		{
			get
			{
				return this.ExactPosition;
			}
		}

		
		// (get) Token: 0x060015EE RID: 5614 RVA: 0x0007F51B File Offset: 0x0007D71B
		public int DamageAmount
		{
			get
			{
				return this.def.projectile.GetDamageAmount(this.weaponDamageMultiplier, null);
			}
		}

		
		// (get) Token: 0x060015EF RID: 5615 RVA: 0x0007F534 File Offset: 0x0007D734
		public float ArmorPenetration
		{
			get
			{
				return this.def.projectile.GetArmorPenetration(this.weaponDamageMultiplier, null);
			}
		}

		
		// (get) Token: 0x060015F0 RID: 5616 RVA: 0x0007F54D File Offset: 0x0007D74D
		public ThingDef EquipmentDef
		{
			get
			{
				return this.equipmentDef;
			}
		}

		
		// (get) Token: 0x060015F1 RID: 5617 RVA: 0x0007F555 File Offset: 0x0007D755
		public Thing Launcher
		{
			get
			{
				return this.launcher;
			}
		}

		
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<Vector3>(ref this.origin, "origin", default(Vector3), false);
			Scribe_Values.Look<Vector3>(ref this.destination, "destination", default(Vector3), false);
			Scribe_Values.Look<int>(ref this.ticksToImpact, "ticksToImpact", 0, false);
			Scribe_TargetInfo.Look(ref this.usedTarget, "usedTarget");
			Scribe_TargetInfo.Look(ref this.intendedTarget, "intendedTarget");
			Scribe_References.Look<Thing>(ref this.launcher, "launcher", false);
			Scribe_Defs.Look<ThingDef>(ref this.equipmentDef, "equipmentDef");
			Scribe_Defs.Look<ThingDef>(ref this.targetCoverDef, "targetCoverDef");
			Scribe_Values.Look<ProjectileHitFlags>(ref this.desiredHitFlags, "desiredHitFlags", ProjectileHitFlags.All, false);
			Scribe_Values.Look<float>(ref this.weaponDamageMultiplier, "weaponDamageMultiplier", 1f, false);
			Scribe_Values.Look<bool>(ref this.landed, "landed", false, false);
		}

		
		public void Launch(Thing launcher, LocalTargetInfo usedTarget, LocalTargetInfo intendedTarget, ProjectileHitFlags hitFlags, Thing equipment = null)
		{
			this.Launch(launcher, base.Position.ToVector3Shifted(), usedTarget, intendedTarget, hitFlags, equipment, null);
		}

		
		public void Launch(Thing launcher, Vector3 origin, LocalTargetInfo usedTarget, LocalTargetInfo intendedTarget, ProjectileHitFlags hitFlags, Thing equipment = null, ThingDef targetCoverDef = null)
		{
			this.launcher = launcher;
			this.origin = origin;
			this.usedTarget = usedTarget;
			this.intendedTarget = intendedTarget;
			this.targetCoverDef = targetCoverDef;
			this.HitFlags = hitFlags;
			if (equipment != null)
			{
				this.equipmentDef = equipment.def;
				this.weaponDamageMultiplier = equipment.GetStatValue(StatDefOf.RangedWeapon_DamageMultiplier, true);
			}
			else
			{
				this.equipmentDef = null;
				this.weaponDamageMultiplier = 1f;
			}
			this.destination = usedTarget.Cell.ToVector3Shifted() + Gen.RandomHorizontalVector(0.3f);
			this.ticksToImpact = Mathf.CeilToInt(this.StartingTicksToImpact);
			if (this.ticksToImpact < 1)
			{
				this.ticksToImpact = 1;
			}
			if (!this.def.projectile.soundAmbient.NullOrUndefined())
			{
				SoundInfo info = SoundInfo.InMap(this, MaintenanceType.PerTick);
				this.ambientSustainer = this.def.projectile.soundAmbient.TrySpawnSustainer(info);
			}
		}

		
		public override void Tick()
		{
			base.Tick();
			if (this.landed)
			{
				return;
			}
			Vector3 exactPosition = this.ExactPosition;
			this.ticksToImpact--;
			if (!this.ExactPosition.InBounds(base.Map))
			{
				this.ticksToImpact++;
				base.Position = this.ExactPosition.ToIntVec3();
				this.Destroy(DestroyMode.Vanish);
				return;
			}
			Vector3 exactPosition2 = this.ExactPosition;
			if (this.CheckForFreeInterceptBetween(exactPosition, exactPosition2))
			{
				return;
			}
			base.Position = this.ExactPosition.ToIntVec3();
			if (this.ticksToImpact == 60 && Find.TickManager.CurTimeSpeed == TimeSpeed.Normal && this.def.projectile.soundImpactAnticipate != null)
			{
				this.def.projectile.soundImpactAnticipate.PlayOneShot(this);
			}
			if (this.ticksToImpact <= 0)
			{
				if (this.DestinationCell.InBounds(base.Map))
				{
					base.Position = this.DestinationCell;
				}
				this.ImpactSomething();
				return;
			}
			if (this.ambientSustainer != null)
			{
				this.ambientSustainer.Maintain();
			}
		}

		
		private bool CheckForFreeInterceptBetween(Vector3 lastExactPos, Vector3 newExactPos)
		{
			if (lastExactPos == newExactPos)
			{
				return false;
			}
			List<Thing> list = base.Map.listerThings.ThingsInGroup(ThingRequestGroup.ProjectileInterceptor);
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].TryGetComp<CompProjectileInterceptor>().CheckIntercept(this, lastExactPos, newExactPos))
				{
					this.Destroy(DestroyMode.Vanish);
					return true;
				}
			}
			IntVec3 intVec = lastExactPos.ToIntVec3();
			IntVec3 intVec2 = newExactPos.ToIntVec3();
			if (intVec2 == intVec)
			{
				return false;
			}
			if (!intVec.InBounds(base.Map) || !intVec2.InBounds(base.Map))
			{
				return false;
			}
			if (intVec2.AdjacentToCardinal(intVec))
			{
				return this.CheckForFreeIntercept(intVec2);
			}
			if (VerbUtility.InterceptChanceFactorFromDistance(this.origin, intVec2) <= 0f)
			{
				return false;
			}
			Vector3 vector = lastExactPos;
			Vector3 v = newExactPos - lastExactPos;
			Vector3 b = v.normalized * 0.2f;
			int num = (int)(v.MagnitudeHorizontal() / 0.2f);
			Projectile.checkedCells.Clear();
			int num2 = 0;
			for (;;)
			{
				vector += b;
				IntVec3 intVec3 = vector.ToIntVec3();
				if (!Projectile.checkedCells.Contains(intVec3))
				{
					if (this.CheckForFreeIntercept(intVec3))
					{
						break;
					}
					Projectile.checkedCells.Add(intVec3);
				}
				num2++;
				if (num2 > num)
				{
					return false;
				}
				if (intVec3 == intVec2)
				{
					return false;
				}
			}
			return true;
		}

		
		private bool CheckForFreeIntercept(IntVec3 c)
		{
			if (this.destination.ToIntVec3() == c)
			{
				return false;
			}
			float num = VerbUtility.InterceptChanceFactorFromDistance(this.origin, c);
			if (num <= 0f)
			{
				return false;
			}
			bool flag = false;
			List<Thing> thingList = c.GetThingList(base.Map);
			for (int i = 0; i < thingList.Count; i++)
			{
				Thing thing = thingList[i];
				if (this.CanHit(thing))
				{
					bool flag2 = false;
					if (thing.def.Fillage == FillCategory.Full)
					{
						Building_Door building_Door = thing as Building_Door;
						if (building_Door == null || !building_Door.Open)
						{
							this.ThrowDebugText("int-wall", c);
							this.Impact(thing);
							return true;
						}
						flag2 = true;
					}
					float num2 = 0f;
					Pawn pawn = thing as Pawn;
					if (pawn != null)
					{
						num2 = 0.4f * Mathf.Clamp(pawn.BodySize, 0.1f, 2f);
						if (pawn.GetPosture() != PawnPosture.Standing)
						{
							num2 *= 0.1f;
						}
						if (this.launcher != null && pawn.Faction != null && this.launcher.Faction != null && !pawn.Faction.HostileTo(this.launcher.Faction))
						{
							num2 *= 0.4f;
						}
					}
					else if (thing.def.fillPercent > 0.2f)
					{
						if (flag2)
						{
							num2 = 0.05f;
						}
						else if (this.DestinationCell.AdjacentTo8Way(c))
						{
							num2 = thing.def.fillPercent * 1f;
						}
						else
						{
							num2 = thing.def.fillPercent * 0.15f;
						}
					}
					num2 *= num;
					if (num2 > 1E-05f)
					{
						if (Rand.Chance(num2))
						{
							this.ThrowDebugText("int-" + num2.ToStringPercent(), c);
							this.Impact(thing);
							return true;
						}
						flag = true;
						this.ThrowDebugText(num2.ToStringPercent(), c);
					}
				}
			}
			if (!flag)
			{
				this.ThrowDebugText("o", c);
			}
			return false;
		}

		
		private void ThrowDebugText(string text, IntVec3 c)
		{
			if (DebugViewSettings.drawShooting)
			{
				MoteMaker.ThrowText(c.ToVector3Shifted(), base.Map, text, -1f);
			}
		}

		
		public override void Draw()
		{
			Graphics.DrawMesh(MeshPool.GridPlane(this.def.graphicData.drawSize), this.DrawPos, this.ExactRotation, this.def.DrawMatSingle, 0);
			base.Comps_PostDraw();
		}

		
		protected bool CanHit(Thing thing)
		{
			if (!thing.Spawned)
			{
				return false;
			}
			if (thing == this.launcher)
			{
				return false;
			}
			bool flag = false;
			foreach (IntVec3 c in thing.OccupiedRect())
			{
				List<Thing> thingList = c.GetThingList(base.Map);
				bool flag2 = false;
				for (int i = 0; i < thingList.Count; i++)
				{
					if (thingList[i] != thing && thingList[i].def.Fillage == FillCategory.Full && thingList[i].def.Altitude >= thing.def.Altitude)
					{
						flag2 = true;
						break;
					}
				}
				if (!flag2)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				return false;
			}
			ProjectileHitFlags hitFlags = this.HitFlags;
			if (thing == this.intendedTarget && (hitFlags & ProjectileHitFlags.IntendedTarget) != ProjectileHitFlags.None)
			{
				return true;
			}
			if (thing != this.intendedTarget)
			{
				if (thing is Pawn)
				{
					if ((hitFlags & ProjectileHitFlags.NonTargetPawns) != ProjectileHitFlags.None)
					{
						return true;
					}
				}
				else if ((hitFlags & ProjectileHitFlags.NonTargetWorld) != ProjectileHitFlags.None)
				{
					return true;
				}
			}
			return thing == this.intendedTarget && thing.def.Fillage == FillCategory.Full;
		}

		
		private void ImpactSomething()
		{
			if (this.def.projectile.flyOverhead)
			{
				RoofDef roofDef = base.Map.roofGrid.RoofAt(base.Position);
				if (roofDef != null)
				{
					if (roofDef.isThickRoof)
					{
						this.ThrowDebugText("hit-thick-roof", base.Position);
						this.def.projectile.soundHitThickRoof.PlayOneShot(new TargetInfo(base.Position, base.Map, false));
						this.Destroy(DestroyMode.Vanish);
						return;
					}
					if (base.Position.GetEdifice(base.Map) == null || base.Position.GetEdifice(base.Map).def.Fillage != FillCategory.Full)
					{
						RoofCollapserImmediate.DropRoofInCells(base.Position, base.Map, null);
					}
				}
			}
			if (!this.usedTarget.HasThing || !this.CanHit(this.usedTarget.Thing))
			{
				Projectile.cellThingsFiltered.Clear();
				List<Thing> thingList = base.Position.GetThingList(base.Map);
				for (int i = 0; i < thingList.Count; i++)
				{
					Thing thing = thingList[i];
					if ((thing.def.category == ThingCategory.Building || thing.def.category == ThingCategory.Pawn || thing.def.category == ThingCategory.Item || thing.def.category == ThingCategory.Plant) && this.CanHit(thing))
					{
						Projectile.cellThingsFiltered.Add(thing);
					}
				}
				Projectile.cellThingsFiltered.Shuffle<Thing>();
				for (int j = 0; j < Projectile.cellThingsFiltered.Count; j++)
				{
					Thing thing2 = Projectile.cellThingsFiltered[j];
					Pawn pawn = thing2 as Pawn;
					float num;
					if (pawn != null)
					{
						num = 0.5f * Mathf.Clamp(pawn.BodySize, 0.1f, 2f);
						if (pawn.GetPosture() != PawnPosture.Standing && (this.origin - this.destination).MagnitudeHorizontalSquared() >= 20.25f)
						{
							num *= 0.2f;
						}
						if (this.launcher != null && pawn.Faction != null && this.launcher.Faction != null && !pawn.Faction.HostileTo(this.launcher.Faction))
						{
							num *= VerbUtility.InterceptChanceFactorFromDistance(this.origin, base.Position);
						}
					}
					else
					{
						num = 1.5f * thing2.def.fillPercent;
					}
					if (Rand.Chance(num))
					{
						this.ThrowDebugText("hit-" + num.ToStringPercent(), base.Position);
						this.Impact(Projectile.cellThingsFiltered.RandomElement<Thing>());
						return;
					}
					this.ThrowDebugText("miss-" + num.ToStringPercent(), base.Position);
				}
				this.Impact(null);
				return;
			}
			Pawn pawn2 = this.usedTarget.Thing as Pawn;
			if (pawn2 != null && pawn2.GetPosture() != PawnPosture.Standing && (this.origin - this.destination).MagnitudeHorizontalSquared() >= 20.25f && !Rand.Chance(0.2f))
			{
				this.ThrowDebugText("miss-laying", base.Position);
				this.Impact(null);
				return;
			}
			this.Impact(this.usedTarget.Thing);
		}

		
		protected virtual void Impact(Thing hitThing)
		{
			GenClamor.DoClamor(this, 2.1f, ClamorDefOf.Impact);
			this.Destroy(DestroyMode.Vanish);
		}

		
		protected Vector3 origin;

		
		protected Vector3 destination;

		
		public LocalTargetInfo usedTarget;

		
		public LocalTargetInfo intendedTarget;

		
		protected ThingDef equipmentDef;

		
		protected Thing launcher;

		
		protected ThingDef targetCoverDef;

		
		private ProjectileHitFlags desiredHitFlags = ProjectileHitFlags.All;

		
		protected float weaponDamageMultiplier = 1f;

		
		protected bool landed;

		
		protected int ticksToImpact;

		
		private Sustainer ambientSustainer;

		
		private static List<IntVec3> checkedCells = new List<IntVec3>();

		
		private static readonly List<Thing> cellThingsFiltered = new List<Thing>();
	}
}
