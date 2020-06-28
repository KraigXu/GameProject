using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000CAD RID: 3245
	[StaticConstructorOnStartup]
	public class Plant : ThingWithComps
	{
		// Token: 0x17000DDD RID: 3549
		// (get) Token: 0x06004E92 RID: 20114 RVA: 0x001A6B51 File Offset: 0x001A4D51
		// (set) Token: 0x06004E93 RID: 20115 RVA: 0x001A6B59 File Offset: 0x001A4D59
		public virtual float Growth
		{
			get
			{
				return this.growthInt;
			}
			set
			{
				this.growthInt = Mathf.Clamp01(value);
				this.cachedLabelMouseover = null;
			}
		}

		// Token: 0x17000DDE RID: 3550
		// (get) Token: 0x06004E94 RID: 20116 RVA: 0x001A6B6E File Offset: 0x001A4D6E
		// (set) Token: 0x06004E95 RID: 20117 RVA: 0x001A6B76 File Offset: 0x001A4D76
		public virtual int Age
		{
			get
			{
				return this.ageInt;
			}
			set
			{
				this.ageInt = value;
				this.cachedLabelMouseover = null;
			}
		}

		// Token: 0x17000DDF RID: 3551
		// (get) Token: 0x06004E96 RID: 20118 RVA: 0x001A6B86 File Offset: 0x001A4D86
		public virtual bool HarvestableNow
		{
			get
			{
				return this.def.plant.Harvestable && this.growthInt > this.def.plant.harvestMinGrowth;
			}
		}

		// Token: 0x17000DE0 RID: 3552
		// (get) Token: 0x06004E97 RID: 20119 RVA: 0x001A6BB4 File Offset: 0x001A4DB4
		public bool HarvestableSoon
		{
			get
			{
				if (this.HarvestableNow)
				{
					return true;
				}
				if (!this.def.plant.Harvestable)
				{
					return false;
				}
				float num = Mathf.Max(1f - this.Growth, 0f) * this.def.plant.growDays;
				float num2 = Mathf.Max(1f - this.def.plant.harvestMinGrowth, 0f) * this.def.plant.growDays;
				return (num <= 10f || num2 <= 1f) && this.GrowthRateFactor_Fertility > 0f && this.GrowthRateFactor_Temperature > 0f;
			}
		}

		// Token: 0x17000DE1 RID: 3553
		// (get) Token: 0x06004E98 RID: 20120 RVA: 0x001A6C64 File Offset: 0x001A4E64
		public virtual bool BlightableNow
		{
			get
			{
				return !this.Blighted && this.def.plant.Blightable && this.sown && this.LifeStage != PlantLifeStage.Sowing && !base.Map.Biome.AllWildPlants.Contains(this.def);
			}
		}

		// Token: 0x17000DE2 RID: 3554
		// (get) Token: 0x06004E99 RID: 20121 RVA: 0x001A6CBB File Offset: 0x001A4EBB
		public Blight Blight
		{
			get
			{
				if (!base.Spawned || !this.def.plant.Blightable)
				{
					return null;
				}
				return base.Position.GetFirstBlight(base.Map);
			}
		}

		// Token: 0x17000DE3 RID: 3555
		// (get) Token: 0x06004E9A RID: 20122 RVA: 0x001A6CEA File Offset: 0x001A4EEA
		public bool Blighted
		{
			get
			{
				return this.Blight != null;
			}
		}

		// Token: 0x17000DE4 RID: 3556
		// (get) Token: 0x06004E9B RID: 20123 RVA: 0x001A6CF8 File Offset: 0x001A4EF8
		public override bool IngestibleNow
		{
			get
			{
				return base.IngestibleNow && (this.def.plant.IsTree || (this.growthInt >= this.def.plant.harvestMinGrowth && !this.LeaflessNow && (!base.Spawned || base.Position.GetSnowDepth(base.Map) <= this.def.hideAtSnowDepth)));
			}
		}

		// Token: 0x17000DE5 RID: 3557
		// (get) Token: 0x06004E9C RID: 20124 RVA: 0x001A6D70 File Offset: 0x001A4F70
		public virtual float CurrentDyingDamagePerTick
		{
			get
			{
				if (!base.Spawned)
				{
					return 0f;
				}
				float num = 0f;
				if (this.def.plant.LimitedLifespan && this.ageInt > this.def.plant.LifespanTicks)
				{
					num = Mathf.Max(num, 0.005f);
				}
				if (!this.def.plant.cavePlant && this.def.plant.dieIfNoSunlight && this.unlitTicks > 450000)
				{
					num = Mathf.Max(num, 0.005f);
				}
				if (this.DyingBecauseExposedToLight)
				{
					float lerpPct = base.Map.glowGrid.GameGlowAt(base.Position, true);
					num = Mathf.Max(num, Plant.DyingDamagePerTickBecauseExposedToLight.LerpThroughRange(lerpPct));
				}
				return num;
			}
		}

		// Token: 0x17000DE6 RID: 3558
		// (get) Token: 0x06004E9D RID: 20125 RVA: 0x001A6E3A File Offset: 0x001A503A
		public virtual bool DyingBecauseExposedToLight
		{
			get
			{
				return this.def.plant.cavePlant && base.Spawned && base.Map.glowGrid.GameGlowAt(base.Position, true) > 0f;
			}
		}

		// Token: 0x17000DE7 RID: 3559
		// (get) Token: 0x06004E9E RID: 20126 RVA: 0x001A6E76 File Offset: 0x001A5076
		public bool Dying
		{
			get
			{
				return this.CurrentDyingDamagePerTick > 0f;
			}
		}

		// Token: 0x17000DE8 RID: 3560
		// (get) Token: 0x06004E9F RID: 20127 RVA: 0x001A6E85 File Offset: 0x001A5085
		protected virtual bool Resting
		{
			get
			{
				return GenLocalDate.DayPercent(this) < 0.25f || GenLocalDate.DayPercent(this) > 0.8f;
			}
		}

		// Token: 0x17000DE9 RID: 3561
		// (get) Token: 0x06004EA0 RID: 20128 RVA: 0x001A6EA4 File Offset: 0x001A50A4
		public virtual float GrowthRate
		{
			get
			{
				if (this.Blighted)
				{
					return 0f;
				}
				if (base.Spawned && !PlantUtility.GrowthSeasonNow(base.Position, base.Map, false))
				{
					return 0f;
				}
				return this.GrowthRateFactor_Fertility * this.GrowthRateFactor_Temperature * this.GrowthRateFactor_Light;
			}
		}

		// Token: 0x17000DEA RID: 3562
		// (get) Token: 0x06004EA1 RID: 20129 RVA: 0x001A6EF5 File Offset: 0x001A50F5
		protected float GrowthPerTick
		{
			get
			{
				if (this.LifeStage != PlantLifeStage.Growing || this.Resting)
				{
					return 0f;
				}
				return 1f / (60000f * this.def.plant.growDays) * this.GrowthRate;
			}
		}

		// Token: 0x17000DEB RID: 3563
		// (get) Token: 0x06004EA2 RID: 20130 RVA: 0x001A6F31 File Offset: 0x001A5131
		public float GrowthRateFactor_Fertility
		{
			get
			{
				return base.Map.fertilityGrid.FertilityAt(base.Position) * this.def.plant.fertilitySensitivity + (1f - this.def.plant.fertilitySensitivity);
			}
		}

		// Token: 0x17000DEC RID: 3564
		// (get) Token: 0x06004EA3 RID: 20131 RVA: 0x001A6F74 File Offset: 0x001A5174
		public float GrowthRateFactor_Light
		{
			get
			{
				float num = base.Map.glowGrid.GameGlowAt(base.Position, false);
				if (this.def.plant.growMinGlow == this.def.plant.growOptimalGlow && num == this.def.plant.growOptimalGlow)
				{
					return 1f;
				}
				return GenMath.InverseLerp(this.def.plant.growMinGlow, this.def.plant.growOptimalGlow, num);
			}
		}

		// Token: 0x17000DED RID: 3565
		// (get) Token: 0x06004EA4 RID: 20132 RVA: 0x001A6FFC File Offset: 0x001A51FC
		public float GrowthRateFactor_Temperature
		{
			get
			{
				float num;
				if (!GenTemperature.TryGetTemperatureForCell(base.Position, base.Map, out num))
				{
					return 1f;
				}
				if (num < 10f)
				{
					return Mathf.InverseLerp(0f, 10f, num);
				}
				if (num > 42f)
				{
					return Mathf.InverseLerp(58f, 42f, num);
				}
				return 1f;
			}
		}

		// Token: 0x17000DEE RID: 3566
		// (get) Token: 0x06004EA5 RID: 20133 RVA: 0x001A705C File Offset: 0x001A525C
		protected int TicksUntilFullyGrown
		{
			get
			{
				if (this.growthInt > 0.9999f)
				{
					return 0;
				}
				float growthPerTick = this.GrowthPerTick;
				if (growthPerTick == 0f)
				{
					return int.MaxValue;
				}
				return (int)((1f - this.growthInt) / growthPerTick);
			}
		}

		// Token: 0x17000DEF RID: 3567
		// (get) Token: 0x06004EA6 RID: 20134 RVA: 0x001A709C File Offset: 0x001A529C
		protected string GrowthPercentString
		{
			get
			{
				return (this.growthInt + 0.0001f).ToStringPercent();
			}
		}

		// Token: 0x17000DF0 RID: 3568
		// (get) Token: 0x06004EA7 RID: 20135 RVA: 0x001A70B0 File Offset: 0x001A52B0
		public override string LabelMouseover
		{
			get
			{
				if (this.cachedLabelMouseover == null)
				{
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.Append(this.def.LabelCap);
					stringBuilder.Append(" (" + "PercentGrowth".Translate(this.GrowthPercentString));
					if (this.Dying)
					{
						stringBuilder.Append(", " + "DyingLower".Translate());
					}
					stringBuilder.Append(")");
					this.cachedLabelMouseover = stringBuilder.ToString();
				}
				return this.cachedLabelMouseover;
			}
		}

		// Token: 0x17000DF1 RID: 3569
		// (get) Token: 0x06004EA8 RID: 20136 RVA: 0x001A7156 File Offset: 0x001A5356
		protected virtual bool HasEnoughLightToGrow
		{
			get
			{
				return this.GrowthRateFactor_Light > 0.001f;
			}
		}

		// Token: 0x17000DF2 RID: 3570
		// (get) Token: 0x06004EA9 RID: 20137 RVA: 0x001A7165 File Offset: 0x001A5365
		public virtual PlantLifeStage LifeStage
		{
			get
			{
				if (this.growthInt < 0.001f)
				{
					return PlantLifeStage.Sowing;
				}
				if (this.growthInt > 0.999f)
				{
					return PlantLifeStage.Mature;
				}
				return PlantLifeStage.Growing;
			}
		}

		// Token: 0x17000DF3 RID: 3571
		// (get) Token: 0x06004EAA RID: 20138 RVA: 0x001A7188 File Offset: 0x001A5388
		public override Graphic Graphic
		{
			get
			{
				if (this.LifeStage == PlantLifeStage.Sowing)
				{
					return Plant.GraphicSowing;
				}
				if (this.def.plant.leaflessGraphic != null && this.LeaflessNow && (!this.sown || !this.HarvestableNow))
				{
					return this.def.plant.leaflessGraphic;
				}
				if (this.def.plant.immatureGraphic != null && !this.HarvestableNow)
				{
					return this.def.plant.immatureGraphic;
				}
				return base.Graphic;
			}
		}

		// Token: 0x17000DF4 RID: 3572
		// (get) Token: 0x06004EAB RID: 20139 RVA: 0x001A720F File Offset: 0x001A540F
		public bool LeaflessNow
		{
			get
			{
				return Find.TickManager.TicksGame - this.madeLeaflessTick < 60000;
			}
		}

		// Token: 0x17000DF5 RID: 3573
		// (get) Token: 0x06004EAC RID: 20140 RVA: 0x001A722C File Offset: 0x001A542C
		protected virtual float LeaflessTemperatureThresh
		{
			get
			{
				float num = 8f;
				return (float)this.HashOffset() * 0.01f % num - num + -2f;
			}
		}

		// Token: 0x17000DF6 RID: 3574
		// (get) Token: 0x06004EAD RID: 20141 RVA: 0x001A7258 File Offset: 0x001A5458
		public bool IsCrop
		{
			get
			{
				if (!this.def.plant.Sowable)
				{
					return false;
				}
				if (!base.Spawned)
				{
					Log.Warning("Can't determine if crop when unspawned.", false);
					return false;
				}
				return this.def == WorkGiver_Grower.CalculateWantedPlantDef(base.Position, base.Map);
			}
		}

		// Token: 0x06004EAE RID: 20142 RVA: 0x001A72A7 File Offset: 0x001A54A7
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			if (Current.ProgramState == ProgramState.Playing && !respawningAfterLoad)
			{
				this.CheckTemperatureMakeLeafless();
			}
		}

		// Token: 0x06004EAF RID: 20143 RVA: 0x001A72C4 File Offset: 0x001A54C4
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			Blight firstBlight = base.Position.GetFirstBlight(base.Map);
			base.DeSpawn(mode);
			if (firstBlight != null)
			{
				firstBlight.Notify_PlantDeSpawned();
			}
		}

		// Token: 0x06004EB0 RID: 20144 RVA: 0x001A72F4 File Offset: 0x001A54F4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.growthInt, "growth", 0f, false);
			Scribe_Values.Look<int>(ref this.ageInt, "age", 0, false);
			Scribe_Values.Look<int>(ref this.unlitTicks, "unlitTicks", 0, false);
			Scribe_Values.Look<int>(ref this.madeLeaflessTick, "madeLeaflessTick", -99999, false);
			Scribe_Values.Look<bool>(ref this.sown, "sown", false, false);
		}

		// Token: 0x06004EB1 RID: 20145 RVA: 0x001A7369 File Offset: 0x001A5569
		public override void PostMapInit()
		{
			this.CheckTemperatureMakeLeafless();
		}

		// Token: 0x06004EB2 RID: 20146 RVA: 0x001A7374 File Offset: 0x001A5574
		protected override void IngestedCalculateAmounts(Pawn ingester, float nutritionWanted, out int numTaken, out float nutritionIngested)
		{
			float statValue = this.GetStatValue(StatDefOf.Nutrition, true);
			if (this.def.plant.HarvestDestroys)
			{
				numTaken = 1;
			}
			else
			{
				this.growthInt -= 0.3f;
				if (this.growthInt < 0.08f)
				{
					this.growthInt = 0.08f;
				}
				if (base.Spawned)
				{
					base.Map.mapDrawer.MapMeshDirty(base.Position, MapMeshFlag.Things);
				}
				numTaken = 0;
			}
			nutritionIngested = statValue;
		}

		// Token: 0x06004EB3 RID: 20147 RVA: 0x001A73F8 File Offset: 0x001A55F8
		public virtual void PlantCollected()
		{
			if (this.def.plant.HarvestDestroys)
			{
				this.Destroy(DestroyMode.Vanish);
				return;
			}
			this.growthInt = this.def.plant.harvestAfterGrowth;
			base.Map.mapDrawer.MapMeshDirty(base.Position, MapMeshFlag.Things);
		}

		// Token: 0x06004EB4 RID: 20148 RVA: 0x001A744C File Offset: 0x001A564C
		protected virtual void CheckTemperatureMakeLeafless()
		{
			if (base.AmbientTemperature < this.LeaflessTemperatureThresh)
			{
				this.MakeLeafless(Plant.LeaflessCause.Cold);
			}
		}

		// Token: 0x06004EB5 RID: 20149 RVA: 0x001A7464 File Offset: 0x001A5664
		public virtual void MakeLeafless(Plant.LeaflessCause cause)
		{
			bool flag = !this.LeaflessNow;
			Map map = base.Map;
			if (cause == Plant.LeaflessCause.Poison && this.def.plant.leaflessGraphic == null)
			{
				if (this.IsCrop && MessagesRepeatAvoider.MessageShowAllowed("MessagePlantDiedOfPoison-" + this.def.defName, 240f))
				{
					Messages.Message("MessagePlantDiedOfPoison".Translate(this.GetCustomLabelNoCount(false)), new TargetInfo(base.Position, map, false), MessageTypeDefOf.NegativeEvent, true);
				}
				base.TakeDamage(new DamageInfo(DamageDefOf.Rotting, 99999f, 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null));
			}
			else if (this.def.plant.dieIfLeafless)
			{
				if (this.IsCrop)
				{
					if (cause == Plant.LeaflessCause.Cold)
					{
						if (MessagesRepeatAvoider.MessageShowAllowed("MessagePlantDiedOfCold-" + this.def.defName, 240f))
						{
							Messages.Message("MessagePlantDiedOfCold".Translate(this.GetCustomLabelNoCount(false)), new TargetInfo(base.Position, map, false), MessageTypeDefOf.NegativeEvent, true);
						}
					}
					else if (cause == Plant.LeaflessCause.Poison && MessagesRepeatAvoider.MessageShowAllowed("MessagePlantDiedOfPoison-" + this.def.defName, 240f))
					{
						Messages.Message("MessagePlantDiedOfPoison".Translate(this.GetCustomLabelNoCount(false)), new TargetInfo(base.Position, map, false), MessageTypeDefOf.NegativeEvent, true);
					}
				}
				base.TakeDamage(new DamageInfo(DamageDefOf.Rotting, 99999f, 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null));
			}
			else
			{
				this.madeLeaflessTick = Find.TickManager.TicksGame;
			}
			if (flag)
			{
				map.mapDrawer.MapMeshDirty(base.Position, MapMeshFlag.Things);
			}
		}

		// Token: 0x06004EB6 RID: 20150 RVA: 0x001A7654 File Offset: 0x001A5854
		public override void TickLong()
		{
			this.CheckTemperatureMakeLeafless();
			if (base.Destroyed)
			{
				return;
			}
			if (PlantUtility.GrowthSeasonNow(base.Position, base.Map, false))
			{
				float num = this.growthInt;
				bool flag = this.LifeStage == PlantLifeStage.Mature;
				this.growthInt += this.GrowthPerTick * 2000f;
				if (this.growthInt > 1f)
				{
					this.growthInt = 1f;
				}
				if (((!flag && this.LifeStage == PlantLifeStage.Mature) || (int)(num * 10f) != (int)(this.growthInt * 10f)) && this.CurrentlyCultivated())
				{
					base.Map.mapDrawer.MapMeshDirty(base.Position, MapMeshFlag.Things);
				}
			}
			if (!this.HasEnoughLightToGrow)
			{
				this.unlitTicks += 2000;
			}
			else
			{
				this.unlitTicks = 0;
			}
			this.ageInt += 2000;
			if (this.Dying)
			{
				Map map = base.Map;
				bool isCrop = this.IsCrop;
				bool harvestableNow = this.HarvestableNow;
				bool dyingBecauseExposedToLight = this.DyingBecauseExposedToLight;
				int num2 = Mathf.CeilToInt(this.CurrentDyingDamagePerTick * 2000f);
				base.TakeDamage(new DamageInfo(DamageDefOf.Rotting, (float)num2, 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null));
				if (base.Destroyed)
				{
					if (isCrop && this.def.plant.Harvestable && MessagesRepeatAvoider.MessageShowAllowed("MessagePlantDiedOfRot-" + this.def.defName, 240f))
					{
						string key;
						if (harvestableNow)
						{
							key = "MessagePlantDiedOfRot_LeftUnharvested";
						}
						else if (dyingBecauseExposedToLight)
						{
							key = "MessagePlantDiedOfRot_ExposedToLight";
						}
						else
						{
							key = "MessagePlantDiedOfRot";
						}
						Messages.Message(key.Translate(this.GetCustomLabelNoCount(false)), new TargetInfo(base.Position, map, false), MessageTypeDefOf.NegativeEvent, true);
					}
					return;
				}
			}
			this.cachedLabelMouseover = null;
			if (this.def.plant.dropLeaves)
			{
				MoteLeaf moteLeaf = MoteMaker.MakeStaticMote(Vector3.zero, base.Map, ThingDefOf.Mote_Leaf, 1f) as MoteLeaf;
				if (moteLeaf != null)
				{
					float num3 = this.def.plant.visualSizeRange.LerpThroughRange(this.growthInt);
					float treeHeight = this.def.graphicData.drawSize.x * num3;
					Vector3 vector = Rand.InsideUnitCircleVec3 * Plant.LeafSpawnRadius;
					moteLeaf.Initialize(base.Position.ToVector3Shifted() + Vector3.up * Rand.Range(Plant.LeafSpawnYMin, Plant.LeafSpawnYMax) + vector + Vector3.forward * this.def.graphicData.shadowData.offset.z, Rand.Value * 2000.TicksToSeconds(), vector.z > 0f, treeHeight);
				}
			}
		}

		// Token: 0x06004EB7 RID: 20151 RVA: 0x001A7948 File Offset: 0x001A5B48
		protected virtual bool CurrentlyCultivated()
		{
			if (!this.def.plant.Sowable)
			{
				return false;
			}
			if (!base.Spawned)
			{
				return false;
			}
			Zone zone = base.Map.zoneManager.ZoneAt(base.Position);
			if (zone != null && zone is Zone_Growing)
			{
				return true;
			}
			Building edifice = base.Position.GetEdifice(base.Map);
			return edifice != null && edifice.def.building.SupportsPlants;
		}

		// Token: 0x06004EB8 RID: 20152 RVA: 0x001A79C1 File Offset: 0x001A5BC1
		public virtual bool CanYieldNow()
		{
			return this.HarvestableNow && this.def.plant.harvestYield > 0f && !this.Blighted;
		}

		// Token: 0x06004EB9 RID: 20153 RVA: 0x001A79F4 File Offset: 0x001A5BF4
		public virtual int YieldNow()
		{
			if (!this.CanYieldNow())
			{
				return 0;
			}
			float harvestYield = this.def.plant.harvestYield;
			float num = Mathf.InverseLerp(this.def.plant.harvestMinGrowth, 1f, this.growthInt);
			num = 0.5f + num * 0.5f;
			return GenMath.RoundRandom(harvestYield * num * Mathf.Lerp(0.5f, 1f, (float)this.HitPoints / (float)base.MaxHitPoints) * Find.Storyteller.difficulty.cropYieldFactor);
		}

		// Token: 0x06004EBA RID: 20154 RVA: 0x001A7A80 File Offset: 0x001A5C80
		public override void Print(SectionLayer layer)
		{
			Vector3 a = this.TrueCenter();
			Rand.PushState();
			Rand.Seed = base.Position.GetHashCode();
			int num = Mathf.CeilToInt(this.growthInt * (float)this.def.plant.maxMeshCount);
			if (num < 1)
			{
				num = 1;
			}
			float num2 = this.def.plant.visualSizeRange.LerpThroughRange(this.growthInt);
			float num3 = this.def.graphicData.drawSize.x * num2;
			Vector3 vector = Vector3.zero;
			int num4 = 0;
			int[] positionIndices = PlantPosIndices.GetPositionIndices(this);
			bool flag = false;
			foreach (int num5 in positionIndices)
			{
				if (this.def.plant.maxMeshCount != 1)
				{
					int num6 = 1;
					int maxMeshCount = this.def.plant.maxMeshCount;
					if (maxMeshCount <= 4)
					{
						if (maxMeshCount != 1)
						{
							if (maxMeshCount != 4)
							{
								goto IL_157;
							}
							num6 = 2;
						}
						else
						{
							num6 = 1;
						}
					}
					else if (maxMeshCount != 9)
					{
						if (maxMeshCount != 16)
						{
							if (maxMeshCount != 25)
							{
								goto IL_157;
							}
							num6 = 5;
						}
						else
						{
							num6 = 4;
						}
					}
					else
					{
						num6 = 3;
					}
					IL_16D:
					float num7 = 1f / (float)num6;
					vector = base.Position.ToVector3();
					vector.y = this.def.Altitude;
					vector.x += 0.5f * num7;
					vector.z += 0.5f * num7;
					int num8 = num5 / num6;
					int num9 = num5 % num6;
					vector.x += (float)num8 * num7;
					vector.z += (float)num9 * num7;
					float max = num7 * 0.3f;
					vector += Gen.RandomHorizontalVector(max);
					goto IL_20B;
					IL_157:
					Log.Error(this.def + " must have plant.MaxMeshCount that is a perfect square.", false);
					goto IL_16D;
				}
				vector = a + Gen.RandomHorizontalVector(0.05f);
				float num10 = (float)base.Position.z;
				if (vector.z - num2 / 2f < num10)
				{
					vector.z = num10 + num2 / 2f;
					flag = true;
				}
				IL_20B:
				bool @bool = Rand.Bool;
				Material matSingle = this.Graphic.MatSingle;
				PlantUtility.SetWindExposureColors(Plant.workingColors, this);
				Vector2 size = new Vector2(num3, num3);
				Printer_Plane.PrintPlane(layer, vector, size, matSingle, 0f, @bool, null, Plant.workingColors, 0.1f, (float)(this.HashOffset() % 1024));
				num4++;
				if (num4 >= num)
				{
					break;
				}
			}
			if (this.def.graphicData.shadowData != null)
			{
				Vector3 center = a + this.def.graphicData.shadowData.offset * num2;
				if (flag)
				{
					center.z = base.Position.ToVector3Shifted().z + this.def.graphicData.shadowData.offset.z;
				}
				center.y -= 0.0454545468f;
				Vector3 volume = this.def.graphicData.shadowData.volume * num2;
				Printer_Shadow.PrintShadow(layer, center, volume, Rot4.North);
			}
			Rand.PopState();
		}

		// Token: 0x06004EBB RID: 20155 RVA: 0x001A7DBC File Offset: 0x001A5FBC
		public override string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (this.LifeStage == PlantLifeStage.Growing)
			{
				stringBuilder.AppendLine("PercentGrowth".Translate(this.GrowthPercentString));
				stringBuilder.AppendLine("GrowthRate".Translate() + ": " + this.GrowthRate.ToStringPercent());
				if (!this.Blighted)
				{
					if (this.Resting)
					{
						stringBuilder.AppendLine("PlantResting".Translate());
					}
					if (!this.HasEnoughLightToGrow)
					{
						stringBuilder.AppendLine("PlantNeedsLightLevel".Translate() + ": " + this.def.plant.growMinGlow.ToStringPercent());
					}
					float growthRateFactor_Temperature = this.GrowthRateFactor_Temperature;
					if (growthRateFactor_Temperature < 0.99f)
					{
						if (growthRateFactor_Temperature < 0.01f)
						{
							stringBuilder.AppendLine("OutOfIdealTemperatureRangeNotGrowing".Translate());
						}
						else
						{
							stringBuilder.AppendLine("OutOfIdealTemperatureRange".Translate(Mathf.RoundToInt(growthRateFactor_Temperature * 100f).ToString()));
						}
					}
				}
			}
			else if (this.LifeStage == PlantLifeStage.Mature)
			{
				if (this.HarvestableNow)
				{
					stringBuilder.AppendLine("ReadyToHarvest".Translate());
				}
				else
				{
					stringBuilder.AppendLine("Mature".Translate());
				}
			}
			if (this.DyingBecauseExposedToLight)
			{
				stringBuilder.AppendLine("DyingBecauseExposedToLight".Translate());
			}
			if (this.Blighted)
			{
				stringBuilder.AppendLine("Blighted".Translate() + " (" + this.Blight.Severity.ToStringPercent() + ")");
			}
			string text = base.InspectStringPartsFromComps();
			if (!text.NullOrEmpty())
			{
				stringBuilder.Append(text);
			}
			return stringBuilder.ToString().TrimEndNewlines();
		}

		// Token: 0x06004EBC RID: 20156 RVA: 0x001A7FC0 File Offset: 0x001A61C0
		public virtual void CropBlighted()
		{
			if (!this.Blighted)
			{
				GenSpawn.Spawn(ThingDefOf.Blight, base.Position, base.Map, WipeMode.Vanish);
			}
		}

		// Token: 0x06004EBD RID: 20157 RVA: 0x001A7FE2 File Offset: 0x001A61E2
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in this.<>n__0())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			if (Prefs.DevMode && this.Blighted)
			{
				yield return new Command_Action
				{
					defaultLabel = "Dev: Spread blight",
					action = delegate
					{
						this.Blight.TryReproduceNow();
					}
				};
			}
			yield break;
			yield break;
		}

		// Token: 0x04002C16 RID: 11286
		protected float growthInt = 0.05f;

		// Token: 0x04002C17 RID: 11287
		protected int ageInt;

		// Token: 0x04002C18 RID: 11288
		protected int unlitTicks;

		// Token: 0x04002C19 RID: 11289
		protected int madeLeaflessTick = -99999;

		// Token: 0x04002C1A RID: 11290
		public bool sown;

		// Token: 0x04002C1B RID: 11291
		private string cachedLabelMouseover;

		// Token: 0x04002C1C RID: 11292
		private static Color32[] workingColors = new Color32[4];

		// Token: 0x04002C1D RID: 11293
		public const float BaseGrowthPercent = 0.05f;

		// Token: 0x04002C1E RID: 11294
		private const float BaseDyingDamagePerTick = 0.005f;

		// Token: 0x04002C1F RID: 11295
		private static readonly FloatRange DyingDamagePerTickBecauseExposedToLight = new FloatRange(0.0001f, 0.001f);

		// Token: 0x04002C20 RID: 11296
		private const float GridPosRandomnessFactor = 0.3f;

		// Token: 0x04002C21 RID: 11297
		private const int TicksWithoutLightBeforeStartDying = 450000;

		// Token: 0x04002C22 RID: 11298
		private const int LeaflessMinRecoveryTicks = 60000;

		// Token: 0x04002C23 RID: 11299
		public const float MinGrowthTemperature = 0f;

		// Token: 0x04002C24 RID: 11300
		public const float MinOptimalGrowthTemperature = 10f;

		// Token: 0x04002C25 RID: 11301
		public const float MaxOptimalGrowthTemperature = 42f;

		// Token: 0x04002C26 RID: 11302
		public const float MaxGrowthTemperature = 58f;

		// Token: 0x04002C27 RID: 11303
		public const float MaxLeaflessTemperature = -2f;

		// Token: 0x04002C28 RID: 11304
		private const float MinLeaflessTemperature = -10f;

		// Token: 0x04002C29 RID: 11305
		private const float MinAnimalEatPlantsTemperature = 0f;

		// Token: 0x04002C2A RID: 11306
		public const float TopVerticesAltitudeBias = 0.1f;

		// Token: 0x04002C2B RID: 11307
		private static Graphic GraphicSowing = GraphicDatabase.Get<Graphic_Single>("Things/Plant/Plant_Sowing", ShaderDatabase.Cutout, Vector2.one, Color.white);

		// Token: 0x04002C2C RID: 11308
		[TweakValue("Graphics", -1f, 1f)]
		private static float LeafSpawnRadius = 0.4f;

		// Token: 0x04002C2D RID: 11309
		[TweakValue("Graphics", 0f, 2f)]
		private static float LeafSpawnYMin = 0.3f;

		// Token: 0x04002C2E RID: 11310
		[TweakValue("Graphics", 0f, 2f)]
		private static float LeafSpawnYMax = 1f;

		// Token: 0x02001C0E RID: 7182
		public enum LeaflessCause
		{
			// Token: 0x04006A5D RID: 27229
			Cold,
			// Token: 0x04006A5E RID: 27230
			Poison
		}
	}
}
