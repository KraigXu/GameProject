using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000C9D RID: 3229
	public class Fire : AttachableThing, ISizeReporter
	{
		// Token: 0x17000DC8 RID: 3528
		// (get) Token: 0x06004DF6 RID: 19958 RVA: 0x001A387B File Offset: 0x001A1A7B
		public int TicksSinceSpawn
		{
			get
			{
				return this.ticksSinceSpawn;
			}
		}

		// Token: 0x17000DC9 RID: 3529
		// (get) Token: 0x06004DF7 RID: 19959 RVA: 0x001A3883 File Offset: 0x001A1A83
		public override string Label
		{
			get
			{
				if (this.parent != null)
				{
					return "FireOn".Translate(this.parent.LabelCap, this.parent);
				}
				return this.def.label;
			}
		}

		// Token: 0x17000DCA RID: 3530
		// (get) Token: 0x06004DF8 RID: 19960 RVA: 0x001A38C4 File Offset: 0x001A1AC4
		public override string InspectStringAddon
		{
			get
			{
				return "Burning".Translate() + " (" + "FireSizeLower".Translate((this.fireSize * 100f).ToString("F0")) + ")";
			}
		}

		// Token: 0x17000DCB RID: 3531
		// (get) Token: 0x06004DF9 RID: 19961 RVA: 0x001A3924 File Offset: 0x001A1B24
		private float SpreadInterval
		{
			get
			{
				float num = 150f - (this.fireSize - 1f) * 40f;
				if (num < 75f)
				{
					num = 75f;
				}
				return num;
			}
		}

		// Token: 0x06004DFA RID: 19962 RVA: 0x001A3959 File Offset: 0x001A1B59
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.ticksSinceSpawn, "ticksSinceSpawn", 0, false);
			Scribe_Values.Look<float>(ref this.fireSize, "fireSize", 0f, false);
		}

		// Token: 0x06004DFB RID: 19963 RVA: 0x001A3989 File Offset: 0x001A1B89
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			this.RecalcPathsOnAndAroundMe(map);
			LessonAutoActivator.TeachOpportunity(ConceptDefOf.HomeArea, this, OpportunityType.Important);
			this.ticksSinceSpread = (int)(this.SpreadInterval * Rand.Value);
		}

		// Token: 0x06004DFC RID: 19964 RVA: 0x001A39B9 File Offset: 0x001A1BB9
		public float CurrentSize()
		{
			return this.fireSize;
		}

		// Token: 0x06004DFD RID: 19965 RVA: 0x001A39C4 File Offset: 0x001A1BC4
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			if (this.sustainer != null)
			{
				if (this.sustainer.externalParams.sizeAggregator == null)
				{
					this.sustainer.externalParams.sizeAggregator = new SoundSizeAggregator();
				}
				this.sustainer.externalParams.sizeAggregator.RemoveReporter(this);
			}
			Map map = base.Map;
			base.DeSpawn(mode);
			this.RecalcPathsOnAndAroundMe(map);
		}

		// Token: 0x06004DFE RID: 19966 RVA: 0x001A3A2C File Offset: 0x001A1C2C
		private void RecalcPathsOnAndAroundMe(Map map)
		{
			IntVec3[] adjacentCellsAndInside = GenAdj.AdjacentCellsAndInside;
			for (int i = 0; i < adjacentCellsAndInside.Length; i++)
			{
				IntVec3 c = base.Position + adjacentCellsAndInside[i];
				if (c.InBounds(map))
				{
					map.pathGrid.RecalculatePerceivedPathCostAt(c);
				}
			}
		}

		// Token: 0x06004DFF RID: 19967 RVA: 0x001A3A78 File Offset: 0x001A1C78
		public override void AttachTo(Thing parent)
		{
			base.AttachTo(parent);
			Pawn pawn = parent as Pawn;
			if (pawn != null)
			{
				TaleRecorder.RecordTale(TaleDefOf.WasOnFire, new object[]
				{
					pawn
				});
			}
		}

		// Token: 0x06004E00 RID: 19968 RVA: 0x001A3AAC File Offset: 0x001A1CAC
		public override void Tick()
		{
			this.ticksSinceSpawn++;
			if (Fire.lastFireCountUpdateTick != Find.TickManager.TicksGame)
			{
				Fire.fireCount = base.Map.listerThings.ThingsOfDef(this.def).Count;
				Fire.lastFireCountUpdateTick = Find.TickManager.TicksGame;
			}
			if (this.sustainer != null)
			{
				this.sustainer.Maintain();
			}
			else if (!base.Position.Fogged(base.Map))
			{
				SoundInfo info = SoundInfo.InMap(new TargetInfo(base.Position, base.Map, false), MaintenanceType.PerTick);
				this.sustainer = SustainerAggregatorUtility.AggregateOrSpawnSustainerFor(this, SoundDefOf.FireBurning, info);
			}
			this.ticksUntilSmoke--;
			if (this.ticksUntilSmoke <= 0)
			{
				this.SpawnSmokeParticles();
			}
			if (Fire.fireCount < 15 && this.fireSize > 0.7f && Rand.Value < this.fireSize * 0.01f)
			{
				MoteMaker.ThrowMicroSparks(this.DrawPos, base.Map);
			}
			if (this.fireSize > 1f)
			{
				this.ticksSinceSpread++;
				if ((float)this.ticksSinceSpread >= this.SpreadInterval)
				{
					this.TrySpread();
					this.ticksSinceSpread = 0;
				}
			}
			if (this.IsHashIntervalTick(150))
			{
				this.DoComplexCalcs();
			}
			if (this.ticksSinceSpawn >= 7500)
			{
				this.TryBurnFloor();
			}
		}

		// Token: 0x06004E01 RID: 19969 RVA: 0x001A3C10 File Offset: 0x001A1E10
		private void SpawnSmokeParticles()
		{
			if (Fire.fireCount < 15)
			{
				MoteMaker.ThrowSmoke(this.DrawPos, base.Map, this.fireSize);
			}
			if (this.fireSize > 0.5f && this.parent == null)
			{
				MoteMaker.ThrowFireGlow(base.Position, base.Map, this.fireSize);
			}
			float num = this.fireSize / 2f;
			if (num > 1f)
			{
				num = 1f;
			}
			num = 1f - num;
			this.ticksUntilSmoke = Fire.SmokeIntervalRange.Lerped(num) + (int)(10f * Rand.Value);
		}

		// Token: 0x06004E02 RID: 19970 RVA: 0x001A3CB0 File Offset: 0x001A1EB0
		private void DoComplexCalcs()
		{
			bool flag = false;
			Fire.flammableList.Clear();
			this.flammabilityMax = 0f;
			if (!base.Position.GetTerrain(base.Map).extinguishesFire)
			{
				if (this.parent == null)
				{
					if (base.Position.TerrainFlammableNow(base.Map))
					{
						this.flammabilityMax = base.Position.GetTerrain(base.Map).GetStatValueAbstract(StatDefOf.Flammability, null);
					}
					List<Thing> list = base.Map.thingGrid.ThingsListAt(base.Position);
					for (int i = 0; i < list.Count; i++)
					{
						Thing thing = list[i];
						if (thing is Building_Door)
						{
							flag = true;
						}
						float statValue = thing.GetStatValue(StatDefOf.Flammability, true);
						if (statValue >= 0.01f)
						{
							Fire.flammableList.Add(list[i]);
							if (statValue > this.flammabilityMax)
							{
								this.flammabilityMax = statValue;
							}
							if (this.parent == null && this.fireSize > 0.4f && list[i].def.category == ThingCategory.Pawn)
							{
								list[i].TryAttachFire(this.fireSize * 0.2f);
							}
						}
					}
				}
				else
				{
					Fire.flammableList.Add(this.parent);
					this.flammabilityMax = this.parent.GetStatValue(StatDefOf.Flammability, true);
				}
			}
			if (this.flammabilityMax < 0.01f)
			{
				this.Destroy(DestroyMode.Vanish);
				return;
			}
			Thing thing2;
			if (this.parent != null)
			{
				thing2 = this.parent;
			}
			else if (Fire.flammableList.Count > 0)
			{
				thing2 = Fire.flammableList.RandomElement<Thing>();
			}
			else
			{
				thing2 = null;
			}
			if (thing2 != null && (this.fireSize >= 0.4f || thing2 == this.parent || thing2.def.category != ThingCategory.Pawn))
			{
				this.DoFireDamage(thing2);
			}
			if (base.Spawned)
			{
				float num = this.fireSize * 160f;
				if (flag)
				{
					num *= 0.15f;
				}
				GenTemperature.PushHeat(base.Position, base.Map, num);
				if (Rand.Value < 0.4f)
				{
					float radius = this.fireSize * 3f;
					SnowUtility.AddSnowRadial(base.Position, base.Map, radius, -(this.fireSize * 0.1f));
				}
				this.fireSize += 0.00055f * this.flammabilityMax * 150f;
				if (this.fireSize > 1.75f)
				{
					this.fireSize = 1.75f;
				}
				if (base.Map.weatherManager.RainRate > 0.01f && this.VulnerableToRain() && Rand.Value < 6f)
				{
					base.TakeDamage(new DamageInfo(DamageDefOf.Extinguish, 10f, 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null));
				}
			}
		}

		// Token: 0x06004E03 RID: 19971 RVA: 0x001A3F7F File Offset: 0x001A217F
		private void TryBurnFloor()
		{
			if (this.parent != null || !base.Spawned)
			{
				return;
			}
			if (base.Position.TerrainFlammableNow(base.Map))
			{
				base.Map.terrainGrid.Notify_TerrainBurned(base.Position);
			}
		}

		// Token: 0x06004E04 RID: 19972 RVA: 0x001A3FBC File Offset: 0x001A21BC
		private bool VulnerableToRain()
		{
			if (!base.Spawned)
			{
				return false;
			}
			RoofDef roofDef = base.Map.roofGrid.RoofAt(base.Position);
			if (roofDef == null)
			{
				return true;
			}
			if (roofDef.isThickRoof)
			{
				return false;
			}
			Thing edifice = base.Position.GetEdifice(base.Map);
			return edifice != null && edifice.def.holdsRoof;
		}

		// Token: 0x06004E05 RID: 19973 RVA: 0x001A401C File Offset: 0x001A221C
		private void DoFireDamage(Thing targ)
		{
			int num = GenMath.RoundRandom(Mathf.Clamp(0.0125f + 0.0036f * this.fireSize, 0.0125f, 0.05f) * 150f);
			if (num < 1)
			{
				num = 1;
			}
			Pawn pawn = targ as Pawn;
			if (pawn != null)
			{
				BattleLogEntry_DamageTaken battleLogEntry_DamageTaken = new BattleLogEntry_DamageTaken(pawn, RulePackDefOf.DamageEvent_Fire, null);
				Find.BattleLog.Add(battleLogEntry_DamageTaken);
				DamageInfo dinfo = new DamageInfo(DamageDefOf.Flame, (float)num, 0f, -1f, this, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null);
				dinfo.SetBodyRegion(BodyPartHeight.Undefined, BodyPartDepth.Outside);
				targ.TakeDamage(dinfo).AssociateWithLog(battleLogEntry_DamageTaken);
				Apparel apparel;
				if (pawn.apparel != null && pawn.apparel.WornApparel.TryRandomElement(out apparel))
				{
					apparel.TakeDamage(new DamageInfo(DamageDefOf.Flame, (float)num, 0f, -1f, this, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null));
					return;
				}
			}
			else
			{
				targ.TakeDamage(new DamageInfo(DamageDefOf.Flame, (float)num, 0f, -1f, this, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null));
			}
		}

		// Token: 0x06004E06 RID: 19974 RVA: 0x001A4118 File Offset: 0x001A2318
		protected void TrySpread()
		{
			IntVec3 intVec = base.Position;
			bool flag;
			if (Rand.Chance(0.8f))
			{
				intVec = base.Position + GenRadial.ManualRadialPattern[Rand.RangeInclusive(1, 8)];
				flag = true;
			}
			else
			{
				intVec = base.Position + GenRadial.ManualRadialPattern[Rand.RangeInclusive(10, 20)];
				flag = false;
			}
			if (!intVec.InBounds(base.Map))
			{
				return;
			}
			if (Rand.Chance(FireUtility.ChanceToStartFireIn(intVec, base.Map)))
			{
				if (!flag)
				{
					CellRect startRect = CellRect.SingleCell(base.Position);
					CellRect endRect = CellRect.SingleCell(intVec);
					if (!GenSight.LineOfSight(base.Position, intVec, base.Map, startRect, endRect, null))
					{
						return;
					}
					((Spark)GenSpawn.Spawn(ThingDefOf.Spark, base.Position, base.Map, WipeMode.Vanish)).Launch(this, intVec, intVec, ProjectileHitFlags.All, null);
					return;
				}
				else
				{
					FireUtility.TryStartFireIn(intVec, base.Map, 0.1f);
				}
			}
		}

		// Token: 0x04002BC2 RID: 11202
		private int ticksSinceSpawn;

		// Token: 0x04002BC3 RID: 11203
		public float fireSize = 0.1f;

		// Token: 0x04002BC4 RID: 11204
		private int ticksSinceSpread;

		// Token: 0x04002BC5 RID: 11205
		private float flammabilityMax = 0.5f;

		// Token: 0x04002BC6 RID: 11206
		private int ticksUntilSmoke;

		// Token: 0x04002BC7 RID: 11207
		private Sustainer sustainer;

		// Token: 0x04002BC8 RID: 11208
		private static List<Thing> flammableList = new List<Thing>();

		// Token: 0x04002BC9 RID: 11209
		private static int fireCount;

		// Token: 0x04002BCA RID: 11210
		private static int lastFireCountUpdateTick;

		// Token: 0x04002BCB RID: 11211
		public const float MinFireSize = 0.1f;

		// Token: 0x04002BCC RID: 11212
		private const float MinSizeForSpark = 1f;

		// Token: 0x04002BCD RID: 11213
		private const float TicksBetweenSparksBase = 150f;

		// Token: 0x04002BCE RID: 11214
		private const float TicksBetweenSparksReductionPerFireSize = 40f;

		// Token: 0x04002BCF RID: 11215
		private const float MinTicksBetweenSparks = 75f;

		// Token: 0x04002BD0 RID: 11216
		private const float MinFireSizeToEmitSpark = 1f;

		// Token: 0x04002BD1 RID: 11217
		public const float MaxFireSize = 1.75f;

		// Token: 0x04002BD2 RID: 11218
		private const int TicksToBurnFloor = 7500;

		// Token: 0x04002BD3 RID: 11219
		private const int ComplexCalcsInterval = 150;

		// Token: 0x04002BD4 RID: 11220
		private const float CellIgniteChancePerTickPerSize = 0.01f;

		// Token: 0x04002BD5 RID: 11221
		private const float MinSizeForIgniteMovables = 0.4f;

		// Token: 0x04002BD6 RID: 11222
		private const float FireBaseGrowthPerTick = 0.00055f;

		// Token: 0x04002BD7 RID: 11223
		private static readonly IntRange SmokeIntervalRange = new IntRange(130, 200);

		// Token: 0x04002BD8 RID: 11224
		private const int SmokeIntervalRandomAddon = 10;

		// Token: 0x04002BD9 RID: 11225
		private const float BaseSkyExtinguishChance = 0.04f;

		// Token: 0x04002BDA RID: 11226
		private const int BaseSkyExtinguishDamage = 10;

		// Token: 0x04002BDB RID: 11227
		private const float HeatPerFireSizePerInterval = 160f;

		// Token: 0x04002BDC RID: 11228
		private const float HeatFactorWhenDoorPresent = 0.15f;

		// Token: 0x04002BDD RID: 11229
		private const float SnowClearRadiusPerFireSize = 3f;

		// Token: 0x04002BDE RID: 11230
		private const float SnowClearDepthFactor = 0.1f;

		// Token: 0x04002BDF RID: 11231
		private const int FireCountParticlesOff = 15;
	}
}
