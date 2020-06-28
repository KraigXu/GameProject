using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x020002E1 RID: 737
	public class Explosion : Thing
	{
		// Token: 0x060014CC RID: 5324 RVA: 0x0007A9E0 File Offset: 0x00078BE0
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			if (!respawningAfterLoad)
			{
				this.cellsToAffect = SimplePool<List<IntVec3>>.Get();
				this.cellsToAffect.Clear();
				this.damagedThings = SimplePool<List<Thing>>.Get();
				this.damagedThings.Clear();
				this.addedCellsAffectedOnlyByDamage = SimplePool<HashSet<IntVec3>>.Get();
				this.addedCellsAffectedOnlyByDamage.Clear();
			}
		}

		// Token: 0x060014CD RID: 5325 RVA: 0x0007AA3C File Offset: 0x00078C3C
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			base.DeSpawn(mode);
			this.cellsToAffect.Clear();
			SimplePool<List<IntVec3>>.Return(this.cellsToAffect);
			this.cellsToAffect = null;
			this.damagedThings.Clear();
			SimplePool<List<Thing>>.Return(this.damagedThings);
			this.damagedThings = null;
			this.addedCellsAffectedOnlyByDamage.Clear();
			SimplePool<HashSet<IntVec3>>.Return(this.addedCellsAffectedOnlyByDamage);
			this.addedCellsAffectedOnlyByDamage = null;
		}

		// Token: 0x060014CE RID: 5326 RVA: 0x0007AAA8 File Offset: 0x00078CA8
		public virtual void StartExplosion(SoundDef explosionSound, List<Thing> ignoredThings)
		{
			if (!base.Spawned)
			{
				Log.Error("Called StartExplosion() on unspawned thing.", false);
				return;
			}
			this.startTick = Find.TickManager.TicksGame;
			this.ignoredThings = ignoredThings;
			this.cellsToAffect.Clear();
			this.damagedThings.Clear();
			this.addedCellsAffectedOnlyByDamage.Clear();
			this.cellsToAffect.AddRange(this.damType.Worker.ExplosionCellsToHit(this));
			if (this.applyDamageToExplosionCellsNeighbors)
			{
				this.AddCellsNeighbors(this.cellsToAffect);
			}
			this.damType.Worker.ExplosionStart(this, this.cellsToAffect);
			this.PlayExplosionSound(explosionSound);
			MoteMaker.MakeWaterSplash(base.Position.ToVector3Shifted(), base.Map, this.radius * 6f, 20f);
			this.cellsToAffect.Sort((IntVec3 a, IntVec3 b) => this.GetCellAffectTick(b).CompareTo(this.GetCellAffectTick(a)));
			RegionTraverser.BreadthFirstTraverse(base.Position, base.Map, (Region from, Region to) => true, delegate(Region x)
			{
				List<Thing> allThings = x.ListerThings.AllThings;
				for (int i = allThings.Count - 1; i >= 0; i--)
				{
					if (allThings[i].Spawned)
					{
						allThings[i].Notify_Explosion(this);
					}
				}
				return false;
			}, 25, RegionType.Set_Passable);
		}

		// Token: 0x060014CF RID: 5327 RVA: 0x0007ABD0 File Offset: 0x00078DD0
		public override void Tick()
		{
			int ticksGame = Find.TickManager.TicksGame;
			int num = this.cellsToAffect.Count - 1;
			while (num >= 0 && ticksGame >= this.GetCellAffectTick(this.cellsToAffect[num]))
			{
				try
				{
					this.AffectCell(this.cellsToAffect[num]);
				}
				catch (Exception ex)
				{
					Log.Error(string.Concat(new object[]
					{
						"Explosion could not affect cell ",
						this.cellsToAffect[num],
						": ",
						ex
					}), false);
				}
				this.cellsToAffect.RemoveAt(num);
				num--;
			}
			if (!this.cellsToAffect.Any<IntVec3>())
			{
				this.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x060014D0 RID: 5328 RVA: 0x0007AC98 File Offset: 0x00078E98
		public int GetDamageAmountAt(IntVec3 c)
		{
			if (!this.damageFalloff)
			{
				return this.damAmount;
			}
			float t = c.DistanceTo(base.Position) / this.radius;
			return Mathf.Max(GenMath.RoundRandom(Mathf.Lerp((float)this.damAmount, (float)this.damAmount * 0.2f, t)), 1);
		}

		// Token: 0x060014D1 RID: 5329 RVA: 0x0007ACF0 File Offset: 0x00078EF0
		public float GetArmorPenetrationAt(IntVec3 c)
		{
			if (!this.damageFalloff)
			{
				return this.armorPenetration;
			}
			float t = c.DistanceTo(base.Position) / this.radius;
			return Mathf.Lerp(this.armorPenetration, this.armorPenetration * 0.2f, t);
		}

		// Token: 0x060014D2 RID: 5330 RVA: 0x0007AD38 File Offset: 0x00078F38
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.radius, "radius", 0f, false);
			Scribe_Defs.Look<DamageDef>(ref this.damType, "damType");
			Scribe_Values.Look<int>(ref this.damAmount, "damAmount", 0, false);
			Scribe_Values.Look<float>(ref this.armorPenetration, "armorPenetration", 0f, false);
			Scribe_References.Look<Thing>(ref this.instigator, "instigator", false);
			Scribe_Defs.Look<ThingDef>(ref this.weapon, "weapon");
			Scribe_Defs.Look<ThingDef>(ref this.projectile, "projectile");
			Scribe_References.Look<Thing>(ref this.intendedTarget, "intendedTarget", false);
			Scribe_Values.Look<bool>(ref this.applyDamageToExplosionCellsNeighbors, "applyDamageToExplosionCellsNeighbors", false, false);
			Scribe_Defs.Look<ThingDef>(ref this.preExplosionSpawnThingDef, "preExplosionSpawnThingDef");
			Scribe_Values.Look<float>(ref this.preExplosionSpawnChance, "preExplosionSpawnChance", 0f, false);
			Scribe_Values.Look<int>(ref this.preExplosionSpawnThingCount, "preExplosionSpawnThingCount", 1, false);
			Scribe_Defs.Look<ThingDef>(ref this.postExplosionSpawnThingDef, "postExplosionSpawnThingDef");
			Scribe_Values.Look<float>(ref this.postExplosionSpawnChance, "postExplosionSpawnChance", 0f, false);
			Scribe_Values.Look<int>(ref this.postExplosionSpawnThingCount, "postExplosionSpawnThingCount", 1, false);
			Scribe_Values.Look<float>(ref this.chanceToStartFire, "chanceToStartFire", 0f, false);
			Scribe_Values.Look<bool>(ref this.damageFalloff, "dealMoreDamageAtCenter", false, false);
			Scribe_Values.Look<IntVec3?>(ref this.needLOSToCell1, "needLOSToCell1", null, false);
			Scribe_Values.Look<IntVec3?>(ref this.needLOSToCell2, "needLOSToCell2", null, false);
			Scribe_Values.Look<int>(ref this.startTick, "startTick", 0, false);
			Scribe_Collections.Look<IntVec3>(ref this.cellsToAffect, "cellsToAffect", LookMode.Value, Array.Empty<object>());
			Scribe_Collections.Look<Thing>(ref this.damagedThings, "damagedThings", LookMode.Reference, Array.Empty<object>());
			Scribe_Collections.Look<Thing>(ref this.ignoredThings, "ignoredThings", LookMode.Reference, Array.Empty<object>());
			Scribe_Collections.Look<IntVec3>(ref this.addedCellsAffectedOnlyByDamage, "addedCellsAffectedOnlyByDamage", LookMode.Value);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.damagedThings != null)
				{
					this.damagedThings.RemoveAll((Thing x) => x == null);
				}
				if (this.ignoredThings != null)
				{
					this.ignoredThings.RemoveAll((Thing x) => x == null);
				}
			}
		}

		// Token: 0x060014D3 RID: 5331 RVA: 0x0007AF8C File Offset: 0x0007918C
		private int GetCellAffectTick(IntVec3 cell)
		{
			return this.startTick + (int)((cell - base.Position).LengthHorizontal * 1.5f);
		}

		// Token: 0x060014D4 RID: 5332 RVA: 0x0007AFBC File Offset: 0x000791BC
		private void AffectCell(IntVec3 c)
		{
			if (!c.InBounds(base.Map))
			{
				return;
			}
			bool flag = this.ShouldCellBeAffectedOnlyByDamage(c);
			if (!flag && Rand.Chance(this.preExplosionSpawnChance) && c.Walkable(base.Map))
			{
				this.TrySpawnExplosionThing(this.preExplosionSpawnThingDef, c, this.preExplosionSpawnThingCount);
			}
			this.damType.Worker.ExplosionAffectCell(this, c, this.damagedThings, this.ignoredThings, !flag);
			if (!flag && Rand.Chance(this.postExplosionSpawnChance) && c.Walkable(base.Map))
			{
				this.TrySpawnExplosionThing(this.postExplosionSpawnThingDef, c, this.postExplosionSpawnThingCount);
			}
			float num = this.chanceToStartFire;
			if (this.damageFalloff)
			{
				num *= Mathf.Lerp(1f, 0.2f, c.DistanceTo(base.Position) / this.radius);
			}
			if (Rand.Chance(num))
			{
				FireUtility.TryStartFireIn(c, base.Map, Rand.Range(0.1f, 0.925f));
			}
		}

		// Token: 0x060014D5 RID: 5333 RVA: 0x0007B0BC File Offset: 0x000792BC
		private void TrySpawnExplosionThing(ThingDef thingDef, IntVec3 c, int count)
		{
			if (thingDef == null)
			{
				return;
			}
			if (thingDef.IsFilth)
			{
				FilthMaker.TryMakeFilth(c, base.Map, thingDef, count, FilthSourceFlags.None);
				return;
			}
			Thing thing = ThingMaker.MakeThing(thingDef, null);
			thing.stackCount = count;
			GenSpawn.Spawn(thing, c, base.Map, WipeMode.Vanish);
		}

		// Token: 0x060014D6 RID: 5334 RVA: 0x0007B0F8 File Offset: 0x000792F8
		private void PlayExplosionSound(SoundDef explosionSound)
		{
			bool flag;
			if (Prefs.DevMode)
			{
				flag = (explosionSound != null);
			}
			else
			{
				flag = !explosionSound.NullOrUndefined();
			}
			if (flag)
			{
				explosionSound.PlayOneShot(new TargetInfo(base.Position, base.Map, false));
				return;
			}
			this.damType.soundExplosion.PlayOneShot(new TargetInfo(base.Position, base.Map, false));
		}

		// Token: 0x060014D7 RID: 5335 RVA: 0x0007B168 File Offset: 0x00079368
		private void AddCellsNeighbors(List<IntVec3> cells)
		{
			Explosion.tmpCells.Clear();
			this.addedCellsAffectedOnlyByDamage.Clear();
			for (int i = 0; i < cells.Count; i++)
			{
				Explosion.tmpCells.Add(cells[i]);
			}
			for (int j = 0; j < cells.Count; j++)
			{
				if (cells[j].Walkable(base.Map))
				{
					for (int k = 0; k < GenAdj.AdjacentCells.Length; k++)
					{
						IntVec3 intVec = cells[j] + GenAdj.AdjacentCells[k];
						if (intVec.InBounds(base.Map) && Explosion.tmpCells.Add(intVec))
						{
							this.addedCellsAffectedOnlyByDamage.Add(intVec);
						}
					}
				}
			}
			cells.Clear();
			foreach (IntVec3 item in Explosion.tmpCells)
			{
				cells.Add(item);
			}
			Explosion.tmpCells.Clear();
		}

		// Token: 0x060014D8 RID: 5336 RVA: 0x0007B27C File Offset: 0x0007947C
		private bool ShouldCellBeAffectedOnlyByDamage(IntVec3 c)
		{
			return this.applyDamageToExplosionCellsNeighbors && this.addedCellsAffectedOnlyByDamage.Contains(c);
		}

		// Token: 0x04000DC5 RID: 3525
		public float radius;

		// Token: 0x04000DC6 RID: 3526
		public DamageDef damType;

		// Token: 0x04000DC7 RID: 3527
		public int damAmount;

		// Token: 0x04000DC8 RID: 3528
		public float armorPenetration;

		// Token: 0x04000DC9 RID: 3529
		public Thing instigator;

		// Token: 0x04000DCA RID: 3530
		public ThingDef weapon;

		// Token: 0x04000DCB RID: 3531
		public ThingDef projectile;

		// Token: 0x04000DCC RID: 3532
		public Thing intendedTarget;

		// Token: 0x04000DCD RID: 3533
		public bool applyDamageToExplosionCellsNeighbors;

		// Token: 0x04000DCE RID: 3534
		public ThingDef preExplosionSpawnThingDef;

		// Token: 0x04000DCF RID: 3535
		public float preExplosionSpawnChance;

		// Token: 0x04000DD0 RID: 3536
		public int preExplosionSpawnThingCount = 1;

		// Token: 0x04000DD1 RID: 3537
		public ThingDef postExplosionSpawnThingDef;

		// Token: 0x04000DD2 RID: 3538
		public float postExplosionSpawnChance;

		// Token: 0x04000DD3 RID: 3539
		public int postExplosionSpawnThingCount = 1;

		// Token: 0x04000DD4 RID: 3540
		public float chanceToStartFire;

		// Token: 0x04000DD5 RID: 3541
		public bool damageFalloff;

		// Token: 0x04000DD6 RID: 3542
		public IntVec3? needLOSToCell1;

		// Token: 0x04000DD7 RID: 3543
		public IntVec3? needLOSToCell2;

		// Token: 0x04000DD8 RID: 3544
		private int startTick;

		// Token: 0x04000DD9 RID: 3545
		private List<IntVec3> cellsToAffect;

		// Token: 0x04000DDA RID: 3546
		private List<Thing> damagedThings;

		// Token: 0x04000DDB RID: 3547
		private List<Thing> ignoredThings;

		// Token: 0x04000DDC RID: 3548
		private HashSet<IntVec3> addedCellsAffectedOnlyByDamage;

		// Token: 0x04000DDD RID: 3549
		private const float DamageFactorAtEdge = 0.2f;

		// Token: 0x04000DDE RID: 3550
		private static HashSet<IntVec3> tmpCells = new HashSet<IntVec3>();
	}
}
