using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	
	public class CompExplosive : ThingComp
	{
		
		// (get) Token: 0x06005109 RID: 20745 RVA: 0x001B3182 File Offset: 0x001B1382
		public CompProperties_Explosive Props
		{
			get
			{
				return (CompProperties_Explosive)this.props;
			}
		}

		
		// (get) Token: 0x0600510A RID: 20746 RVA: 0x001B318F File Offset: 0x001B138F
		protected int StartWickThreshold
		{
			get
			{
				return Mathf.RoundToInt(this.Props.startWickHitPointsPercent * (float)this.parent.MaxHitPoints);
			}
		}

		
		// (get) Token: 0x0600510B RID: 20747 RVA: 0x001B31B0 File Offset: 0x001B13B0
		private bool CanEverExplodeFromDamage
		{
			get
			{
				if (this.Props.chanceNeverExplodeFromDamage < 1E-05f)
				{
					return true;
				}
				Rand.PushState();
				Rand.Seed = this.parent.thingIDNumber.GetHashCode();
				bool result = Rand.Value < this.Props.chanceNeverExplodeFromDamage;
				Rand.PopState();
				return result;
			}
		}

		
		public void AddThingsIgnoredByExplosion(List<Thing> things)
		{
			if (this.thingsIgnoredByExplosion == null)
			{
				this.thingsIgnoredByExplosion = new List<Thing>();
			}
			this.thingsIgnoredByExplosion.AddRange(things);
		}

		
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_References.Look<Thing>(ref this.instigator, "instigator", false);
			Scribe_Collections.Look<Thing>(ref this.thingsIgnoredByExplosion, "thingsIgnoredByExplosion", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.wickStarted, "wickStarted", false, false);
			Scribe_Values.Look<int>(ref this.wickTicksLeft, "wickTicksLeft", 0, false);
			Scribe_Values.Look<bool>(ref this.destroyedThroughDetonation, "destroyedThroughDetonation", false, false);
			Scribe_Values.Look<int>(ref this.countdownTicksLeft, "countdownTicksLeft", 0, false);
		}

		
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			if (this.Props.countdownTicks != null)
			{
				this.countdownTicksLeft = this.Props.countdownTicks.Value.RandomInRange;
			}
		}

		
		public override void CompTick()
		{
			if (this.countdownTicksLeft > 0)
			{
				this.countdownTicksLeft--;
				if (this.countdownTicksLeft == 0)
				{
					this.StartWick(null);
					this.countdownTicksLeft = -1;
				}
			}
			if (this.wickStarted)
			{
				if (this.wickSoundSustainer == null)
				{
					this.StartWickSustainer();
				}
				else
				{
					this.wickSoundSustainer.Maintain();
				}
				this.wickTicksLeft--;
				if (this.wickTicksLeft <= 0)
				{
					this.Detonate(this.parent.MapHeld, false);
				}
			}
		}

		
		private void StartWickSustainer()
		{
			SoundDefOf.MetalHitImportant.PlayOneShot(new TargetInfo(this.parent.Position, this.parent.Map, false));
			SoundInfo info = SoundInfo.InMap(this.parent, MaintenanceType.PerTick);
			this.wickSoundSustainer = SoundDefOf.HissSmall.TrySpawnSustainer(info);
		}

		
		private void EndWickSustainer()
		{
			if (this.wickSoundSustainer != null)
			{
				this.wickSoundSustainer.End();
				this.wickSoundSustainer = null;
			}
		}

		
		public override void PostDraw()
		{
			if (this.wickStarted)
			{
				this.parent.Map.overlayDrawer.DrawOverlay(this.parent, OverlayTypes.BurningWick);
			}
		}

		
		public override void PostDestroy(DestroyMode mode, Map previousMap)
		{
			if (mode == DestroyMode.KillFinalize && this.Props.explodeOnKilled)
			{
				this.Detonate(previousMap, true);
			}
		}

		
		public override void PostPreApplyDamage(DamageInfo dinfo, out bool absorbed)
		{
			absorbed = false;
			if (this.CanEverExplodeFromDamage)
			{
				if (dinfo.Def.ExternalViolenceFor(this.parent) && dinfo.Amount >= (float)this.parent.HitPoints && this.CanExplodeFromDamageType(dinfo.Def))
				{
					if (this.parent.MapHeld != null)
					{
						this.Detonate(this.parent.MapHeld, false);
						if (this.parent.Destroyed)
						{
							absorbed = true;
							return;
						}
					}
				}
				else if (!this.wickStarted && this.Props.startWickOnDamageTaken != null && this.Props.startWickOnDamageTaken.Contains(dinfo.Def))
				{
					this.StartWick(dinfo.Instigator);
				}
			}
		}

		
		public override void PostPostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
		{
			if (!this.CanEverExplodeFromDamage)
			{
				return;
			}
			if (!this.CanExplodeFromDamageType(dinfo.Def))
			{
				return;
			}
			if (!this.parent.Destroyed)
			{
				if (this.wickStarted && dinfo.Def == DamageDefOf.Stun)
				{
					this.StopWick();
					return;
				}
				if (!this.wickStarted && this.parent.HitPoints <= this.StartWickThreshold && dinfo.Def.ExternalViolenceFor(this.parent))
				{
					this.StartWick(dinfo.Instigator);
				}
			}
		}

		
		public void StartWick(Thing instigator = null)
		{
			if (this.wickStarted)
			{
				return;
			}
			if (this.ExplosiveRadius() <= 0f)
			{
				return;
			}
			this.instigator = instigator;
			this.wickStarted = true;
			this.wickTicksLeft = this.Props.wickTicks.RandomInRange;
			this.StartWickSustainer();
			GenExplosion.NotifyNearbyPawnsOfDangerousExplosive(this.parent, this.Props.explosiveDamageType, null);
		}

		
		public void StopWick()
		{
			this.wickStarted = false;
			this.instigator = null;
		}

		
		public float ExplosiveRadius()
		{
			CompProperties_Explosive props = this.Props;
			float num = props.explosiveRadius;
			if (this.parent.stackCount > 1 && props.explosiveExpandPerStackcount > 0f)
			{
				num += Mathf.Sqrt((float)(this.parent.stackCount - 1) * props.explosiveExpandPerStackcount);
			}
			if (props.explosiveExpandPerFuel > 0f && this.parent.GetComp<CompRefuelable>() != null)
			{
				num += Mathf.Sqrt(this.parent.GetComp<CompRefuelable>().Fuel * props.explosiveExpandPerFuel);
			}
			return num;
		}

		
		protected void Detonate(Map map, bool ignoreUnspawned = false)
		{
			if (!ignoreUnspawned && !this.parent.SpawnedOrAnyParentSpawned)
			{
				return;
			}
			CompProperties_Explosive props = this.Props;
			float num = this.ExplosiveRadius();
			if (props.explosiveExpandPerFuel > 0f && this.parent.GetComp<CompRefuelable>() != null)
			{
				this.parent.GetComp<CompRefuelable>().ConsumeFuel(this.parent.GetComp<CompRefuelable>().Fuel);
			}
			if (props.destroyThingOnExplosionSize <= num && !this.parent.Destroyed)
			{
				this.destroyedThroughDetonation = true;
				this.parent.Kill(null, null);
			}
			this.EndWickSustainer();
			this.wickStarted = false;
			if (map == null)
			{
				Log.Warning("Tried to detonate CompExplosive in a null map.", false);
				return;
			}
			if (props.explosionEffect != null)
			{
				Effecter effecter = props.explosionEffect.Spawn();
				effecter.Trigger(new TargetInfo(this.parent.PositionHeld, map, false), new TargetInfo(this.parent.PositionHeld, map, false));
				effecter.Cleanup();
			}
			Thing parent;
			if (this.instigator != null && !this.instigator.HostileTo(this.parent.Faction))
			{
				parent = this.instigator;
			}
			else
			{
				parent = this.parent;
			}
			GenExplosion.DoExplosion(this.parent.PositionHeld, map, num, props.explosiveDamageType, parent, props.damageAmountBase, props.armorPenetrationBase, props.explosionSound, null, null, null, props.postExplosionSpawnThingDef, props.postExplosionSpawnChance, props.postExplosionSpawnThingCount, props.applyDamageToExplosionCellsNeighbors, props.preExplosionSpawnThingDef, props.preExplosionSpawnChance, props.preExplosionSpawnThingCount, props.chanceToStartFire, props.damageFalloff, null, this.thingsIgnoredByExplosion);
		}

		
		private bool CanExplodeFromDamageType(DamageDef damage)
		{
			return this.Props.requiredDamageTypeToExplode == null || this.Props.requiredDamageTypeToExplode == damage;
		}

		
		public override string CompInspectStringExtra()
		{
			string text = "";
			if (this.countdownTicksLeft != -1)
			{
				text += "DetonationCountdown".Translate(this.countdownTicksLeft.TicksToDays().ToString("0.0"));
			}
			if (this.Props.extraInspectStringKey != null)
			{
				text += ((text != "") ? "\n" : "") + this.Props.extraInspectStringKey.Translate();
			}
			return text;
		}

		
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			if (this.countdownTicksLeft > 0)
			{
				yield return new Command_Action
				{
					defaultLabel = "DEV: Trigger countdown",
					action = delegate
					{
						this.countdownTicksLeft = 1;
					}
				};
			}
			yield break;
		}

		
		public bool wickStarted;

		
		protected int wickTicksLeft;

		
		private Thing instigator;

		
		private int countdownTicksLeft = -1;

		
		public bool destroyedThroughDetonation;

		
		private List<Thing> thingsIgnoredByExplosion;

		
		protected Sustainer wickSoundSustainer;
	}
}
