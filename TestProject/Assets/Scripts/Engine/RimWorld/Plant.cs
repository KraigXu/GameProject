﻿using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	[StaticConstructorOnStartup]
	public class Plant : ThingWithComps
	{
		
		
		
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

		
		
		public virtual bool HarvestableNow
		{
			get
			{
				return this.def.plant.Harvestable && this.growthInt > this.def.plant.harvestMinGrowth;
			}
		}

		
		
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

		
		
		public virtual bool BlightableNow
		{
			get
			{
				return !this.Blighted && this.def.plant.Blightable && this.sown && this.LifeStage != PlantLifeStage.Sowing && !base.Map.Biome.AllWildPlants.Contains(this.def);
			}
		}

		
		
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

		
		
		public bool Blighted
		{
			get
			{
				return this.Blight != null;
			}
		}

		
		
		public override bool IngestibleNow
		{
			get
			{
				return base.IngestibleNow && (this.def.plant.IsTree || (this.growthInt >= this.def.plant.harvestMinGrowth && !this.LeaflessNow && (!base.Spawned || base.Position.GetSnowDepth(base.Map) <= this.def.hideAtSnowDepth)));
			}
		}

		
		
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

		
		
		public virtual bool DyingBecauseExposedToLight
		{
			get
			{
				return this.def.plant.cavePlant && base.Spawned && base.Map.glowGrid.GameGlowAt(base.Position, true) > 0f;
			}
		}

		
		
		public bool Dying
		{
			get
			{
				return this.CurrentDyingDamagePerTick > 0f;
			}
		}

		
		
		protected virtual bool Resting
		{
			get
			{
				return GenLocalDate.DayPercent(this) < 0.25f || GenLocalDate.DayPercent(this) > 0.8f;
			}
		}

		
		
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

		
		
		public float GrowthRateFactor_Fertility
		{
			get
			{
				return base.Map.fertilityGrid.FertilityAt(base.Position) * this.def.plant.fertilitySensitivity + (1f - this.def.plant.fertilitySensitivity);
			}
		}

		
		
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

		
		
		protected string GrowthPercentString
		{
			get
			{
				return (this.growthInt + 0.0001f).ToStringPercent();
			}
		}

		
		
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

		
		
		protected virtual bool HasEnoughLightToGrow
		{
			get
			{
				return this.GrowthRateFactor_Light > 0.001f;
			}
		}

		
		
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

		
		
		public bool LeaflessNow
		{
			get
			{
				return Find.TickManager.TicksGame - this.madeLeaflessTick < 60000;
			}
		}

		
		
		protected virtual float LeaflessTemperatureThresh
		{
			get
			{
				float num = 8f;
				return (float)this.HashOffset() * 0.01f % num - num + -2f;
			}
		}

		
		
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

		
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			if (Current.ProgramState == ProgramState.Playing && !respawningAfterLoad)
			{
				this.CheckTemperatureMakeLeafless();
			}
		}

		
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			Blight firstBlight = base.Position.GetFirstBlight(base.Map);
			base.DeSpawn(mode);
			if (firstBlight != null)
			{
				firstBlight.Notify_PlantDeSpawned();
			}
		}

		
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.growthInt, "growth", 0f, false);
			Scribe_Values.Look<int>(ref this.ageInt, "age", 0, false);
			Scribe_Values.Look<int>(ref this.unlitTicks, "unlitTicks", 0, false);
			Scribe_Values.Look<int>(ref this.madeLeaflessTick, "madeLeaflessTick", -99999, false);
			Scribe_Values.Look<bool>(ref this.sown, "sown", false, false);
		}

		
		public override void PostMapInit()
		{
			this.CheckTemperatureMakeLeafless();
		}

		
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

		
		protected virtual void CheckTemperatureMakeLeafless()
		{
			if (base.AmbientTemperature < this.LeaflessTemperatureThresh)
			{
				this.MakeLeafless(Plant.LeaflessCause.Cold);
			}
		}

		
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

		
		public virtual bool CanYieldNow()
		{
			return this.HarvestableNow && this.def.plant.harvestYield > 0f && !this.Blighted;
		}

		
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

		
		public virtual void CropBlighted()
		{
			if (!this.Blighted)
			{
				GenSpawn.Spawn(ThingDefOf.Blight, base.Position, base.Map, WipeMode.Vanish);
			}
		}

		
		public override IEnumerable<Gizmo> GetGizmos()
		{

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

		
		protected float growthInt = 0.05f;

		
		protected int ageInt;

		
		protected int unlitTicks;

		
		protected int madeLeaflessTick = -99999;

		
		public bool sown;

		
		private string cachedLabelMouseover;

		
		private static Color32[] workingColors = new Color32[4];

		
		public const float BaseGrowthPercent = 0.05f;

		
		private const float BaseDyingDamagePerTick = 0.005f;

		
		private static readonly FloatRange DyingDamagePerTickBecauseExposedToLight = new FloatRange(0.0001f, 0.001f);

		
		private const float GridPosRandomnessFactor = 0.3f;

		
		private const int TicksWithoutLightBeforeStartDying = 450000;

		
		private const int LeaflessMinRecoveryTicks = 60000;

		
		public const float MinGrowthTemperature = 0f;

		
		public const float MinOptimalGrowthTemperature = 10f;

		
		public const float MaxOptimalGrowthTemperature = 42f;

		
		public const float MaxGrowthTemperature = 58f;

		
		public const float MaxLeaflessTemperature = -2f;

		
		private const float MinLeaflessTemperature = -10f;

		
		private const float MinAnimalEatPlantsTemperature = 0f;

		
		public const float TopVerticesAltitudeBias = 0.1f;

		
		private static Graphic GraphicSowing = GraphicDatabase.Get<Graphic_Single>("Things/Plant/Plant_Sowing", ShaderDatabase.Cutout, Vector2.one, Color.white);

		
		[TweakValue("Graphics", -1f, 1f)]
		private static float LeafSpawnRadius = 0.4f;

		
		[TweakValue("Graphics", 0f, 2f)]
		private static float LeafSpawnYMin = 0.3f;

		
		[TweakValue("Graphics", 0f, 2f)]
		private static float LeafSpawnYMax = 1f;

		
		public enum LeaflessCause
		{
			
			Cold,
			
			Poison
		}
	}
}
