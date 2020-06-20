using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Noise;

namespace RimWorld.Planet
{
	// Token: 0x02001218 RID: 4632
	public class WorldGenStep_Terrain : WorldGenStep
	{
		// Token: 0x170011EF RID: 4591
		// (get) Token: 0x06006B25 RID: 27429 RVA: 0x002563AF File Offset: 0x002545AF
		public override int SeedPart
		{
			get
			{
				return 83469557;
			}
		}

		// Token: 0x170011F0 RID: 4592
		// (get) Token: 0x06006B26 RID: 27430 RVA: 0x0001BFCE File Offset: 0x0001A1CE
		private static float FreqMultiplier
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x06006B27 RID: 27431 RVA: 0x002563B6 File Offset: 0x002545B6
		public override void GenerateFresh(string seed)
		{
			this.GenerateGridIntoWorld();
		}

		// Token: 0x06006B28 RID: 27432 RVA: 0x002563BE File Offset: 0x002545BE
		public override void GenerateFromScribe(string seed)
		{
			Find.World.pathGrid = new WorldPathGrid();
			NoiseDebugUI.ClearPlanetNoises();
		}

		// Token: 0x06006B29 RID: 27433 RVA: 0x002563D4 File Offset: 0x002545D4
		private void GenerateGridIntoWorld()
		{
			Find.World.grid = new WorldGrid();
			Find.World.pathGrid = new WorldPathGrid();
			NoiseDebugUI.ClearPlanetNoises();
			this.SetupElevationNoise();
			this.SetupTemperatureOffsetNoise();
			this.SetupRainfallNoise();
			this.SetupHillinessNoise();
			this.SetupSwampinessNoise();
			List<Tile> tiles = Find.WorldGrid.tiles;
			tiles.Clear();
			int tilesCount = Find.WorldGrid.TilesCount;
			for (int i = 0; i < tilesCount; i++)
			{
				Tile item = this.GenerateTileFor(i);
				tiles.Add(item);
			}
		}

		// Token: 0x06006B2A RID: 27434 RVA: 0x0025645C File Offset: 0x0025465C
		private void SetupElevationNoise()
		{
			float freqMultiplier = WorldGenStep_Terrain.FreqMultiplier;
			ModuleBase lhs = new Perlin((double)(0.035f * freqMultiplier), 2.0, 0.40000000596046448, 6, Rand.Range(0, int.MaxValue), QualityMode.High);
			ModuleBase moduleBase = new RidgedMultifractal((double)(0.012f * freqMultiplier), 2.0, 6, Rand.Range(0, int.MaxValue), QualityMode.High);
			ModuleBase moduleBase2 = new Perlin((double)(0.12f * freqMultiplier), 2.0, 0.5, 5, Rand.Range(0, int.MaxValue), QualityMode.High);
			ModuleBase moduleBase3 = new Perlin((double)(0.01f * freqMultiplier), 2.0, 0.5, 5, Rand.Range(0, int.MaxValue), QualityMode.High);
			float num;
			if (Find.World.PlanetCoverage < 0.55f)
			{
				ModuleBase moduleBase4 = new DistanceFromPlanetViewCenter(Find.WorldGrid.viewCenter, Find.WorldGrid.viewAngle, true);
				moduleBase4 = new ScaleBias(2.0, -1.0, moduleBase4);
				moduleBase3 = new Blend(moduleBase3, moduleBase4, new Const(0.40000000596046448));
				num = Rand.Range(-0.4f, -0.35f);
			}
			else
			{
				num = Rand.Range(0.15f, 0.25f);
			}
			NoiseDebugUI.StorePlanetNoise(moduleBase3, "elevContinents");
			moduleBase2 = new ScaleBias(0.5, 0.5, moduleBase2);
			moduleBase = new Multiply(moduleBase, moduleBase2);
			float num2 = Rand.Range(0.4f, 0.6f);
			this.noiseElevation = new Blend(lhs, moduleBase, new Const((double)num2));
			this.noiseElevation = new Blend(this.noiseElevation, moduleBase3, new Const((double)num));
			if (Find.World.PlanetCoverage < 0.9999f)
			{
				this.noiseElevation = new ConvertToIsland(Find.WorldGrid.viewCenter, Find.WorldGrid.viewAngle, this.noiseElevation);
			}
			this.noiseElevation = new ScaleBias(0.5, 0.5, this.noiseElevation);
			this.noiseElevation = new Power(this.noiseElevation, new Const(3.0));
			NoiseDebugUI.StorePlanetNoise(this.noiseElevation, "noiseElevation");
			this.noiseElevation = new ScaleBias((double)WorldGenStep_Terrain.ElevationRange.Span, (double)WorldGenStep_Terrain.ElevationRange.min, this.noiseElevation);
		}

		// Token: 0x06006B2B RID: 27435 RVA: 0x002566C4 File Offset: 0x002548C4
		private void SetupTemperatureOffsetNoise()
		{
			float freqMultiplier = WorldGenStep_Terrain.FreqMultiplier;
			this.noiseTemperatureOffset = new Perlin((double)(0.018f * freqMultiplier), 2.0, 0.5, 6, Rand.Range(0, int.MaxValue), QualityMode.High);
			this.noiseTemperatureOffset = new Multiply(this.noiseTemperatureOffset, new Const(4.0));
		}

		// Token: 0x06006B2C RID: 27436 RVA: 0x00256728 File Offset: 0x00254928
		private void SetupRainfallNoise()
		{
			float freqMultiplier = WorldGenStep_Terrain.FreqMultiplier;
			ModuleBase moduleBase = new Perlin((double)(0.015f * freqMultiplier), 2.0, 0.5, 6, Rand.Range(0, int.MaxValue), QualityMode.High);
			moduleBase = new ScaleBias(0.5, 0.5, moduleBase);
			NoiseDebugUI.StorePlanetNoise(moduleBase, "basePerlin");
			ModuleBase moduleBase2 = new AbsLatitudeCurve(new SimpleCurve
			{
				{
					0f,
					1.12f,
					true
				},
				{
					25f,
					0.94f,
					true
				},
				{
					45f,
					0.7f,
					true
				},
				{
					70f,
					0.3f,
					true
				},
				{
					80f,
					0.05f,
					true
				},
				{
					90f,
					0.05f,
					true
				}
			}, 100f);
			NoiseDebugUI.StorePlanetNoise(moduleBase2, "latCurve");
			this.noiseRainfall = new Multiply(moduleBase, moduleBase2);
			float num = 0.000222222225f;
			float num2 = -500f * num;
			ModuleBase moduleBase3 = new ScaleBias((double)num, (double)num2, this.noiseElevation);
			moduleBase3 = new ScaleBias(-1.0, 1.0, moduleBase3);
			moduleBase3 = new Clamp(0.0, 1.0, moduleBase3);
			NoiseDebugUI.StorePlanetNoise(moduleBase3, "elevationRainfallEffect");
			this.noiseRainfall = new Multiply(this.noiseRainfall, moduleBase3);
			Func<double, double> processor = delegate(double val)
			{
				if (val < 0.0)
				{
					val = 0.0;
				}
				if (val < 0.12)
				{
					val = (val + 0.12) / 2.0;
					if (val < 0.03)
					{
						val = (val + 0.03) / 2.0;
					}
				}
				return val;
			};
			this.noiseRainfall = new Arbitrary(this.noiseRainfall, processor);
			this.noiseRainfall = new Power(this.noiseRainfall, new Const(1.5));
			this.noiseRainfall = new Clamp(0.0, 999.0, this.noiseRainfall);
			NoiseDebugUI.StorePlanetNoise(this.noiseRainfall, "noiseRainfall before mm");
			this.noiseRainfall = new ScaleBias(4000.0, 0.0, this.noiseRainfall);
			SimpleCurve rainfallCurve = Find.World.info.overallRainfall.GetRainfallCurve();
			if (rainfallCurve != null)
			{
				this.noiseRainfall = new CurveSimple(this.noiseRainfall, rainfallCurve);
			}
		}

		// Token: 0x06006B2D RID: 27437 RVA: 0x00256968 File Offset: 0x00254B68
		private void SetupHillinessNoise()
		{
			float freqMultiplier = WorldGenStep_Terrain.FreqMultiplier;
			this.noiseMountainLines = new Perlin((double)(0.025f * freqMultiplier), 2.0, 0.5, 6, Rand.Range(0, int.MaxValue), QualityMode.High);
			ModuleBase moduleBase = new Perlin((double)(0.06f * freqMultiplier), 2.0, 0.5, 6, Rand.Range(0, int.MaxValue), QualityMode.High);
			this.noiseMountainLines = new Abs(this.noiseMountainLines);
			this.noiseMountainLines = new OneMinus(this.noiseMountainLines);
			moduleBase = new Filter(moduleBase, -0.3f, 1f);
			this.noiseMountainLines = new Multiply(this.noiseMountainLines, moduleBase);
			this.noiseMountainLines = new OneMinus(this.noiseMountainLines);
			NoiseDebugUI.StorePlanetNoise(this.noiseMountainLines, "noiseMountainLines");
			this.noiseHillsPatchesMacro = new Perlin((double)(0.032f * freqMultiplier), 2.0, 0.5, 5, Rand.Range(0, int.MaxValue), QualityMode.Medium);
			this.noiseHillsPatchesMicro = new Perlin((double)(0.19f * freqMultiplier), 2.0, 0.5, 6, Rand.Range(0, int.MaxValue), QualityMode.High);
		}

		// Token: 0x06006B2E RID: 27438 RVA: 0x00256AA4 File Offset: 0x00254CA4
		private void SetupSwampinessNoise()
		{
			float freqMultiplier = WorldGenStep_Terrain.FreqMultiplier;
			ModuleBase moduleBase = new Perlin((double)(0.09f * freqMultiplier), 2.0, 0.40000000596046448, 6, Rand.Range(0, int.MaxValue), QualityMode.High);
			ModuleBase moduleBase2 = new RidgedMultifractal((double)(0.025f * freqMultiplier), 2.0, 6, Rand.Range(0, int.MaxValue), QualityMode.High);
			moduleBase = new ScaleBias(0.5, 0.5, moduleBase);
			moduleBase2 = new ScaleBias(0.5, 0.5, moduleBase2);
			this.noiseSwampiness = new Multiply(moduleBase, moduleBase2);
			InverseLerp rhs = new InverseLerp(this.noiseElevation, WorldGenStep_Terrain.SwampinessMaxElevation.max, WorldGenStep_Terrain.SwampinessMaxElevation.min);
			this.noiseSwampiness = new Multiply(this.noiseSwampiness, rhs);
			InverseLerp rhs2 = new InverseLerp(this.noiseRainfall, WorldGenStep_Terrain.SwampinessMinRainfall.min, WorldGenStep_Terrain.SwampinessMinRainfall.max);
			this.noiseSwampiness = new Multiply(this.noiseSwampiness, rhs2);
			NoiseDebugUI.StorePlanetNoise(this.noiseSwampiness, "noiseSwampiness");
		}

		// Token: 0x06006B2F RID: 27439 RVA: 0x00256BC0 File Offset: 0x00254DC0
		private Tile GenerateTileFor(int tileID)
		{
			Tile tile = new Tile();
			Vector3 tileCenter = Find.WorldGrid.GetTileCenter(tileID);
			tile.elevation = this.noiseElevation.GetValue(tileCenter);
			float value = this.noiseMountainLines.GetValue(tileCenter);
			if (value > 0.235f || tile.elevation <= 0f)
			{
				if (tile.elevation > 0f && this.noiseHillsPatchesMicro.GetValue(tileCenter) > 0.46f && this.noiseHillsPatchesMacro.GetValue(tileCenter) > -0.3f)
				{
					if (Rand.Bool)
					{
						tile.hilliness = Hilliness.SmallHills;
					}
					else
					{
						tile.hilliness = Hilliness.LargeHills;
					}
				}
				else
				{
					tile.hilliness = Hilliness.Flat;
				}
			}
			else if (value > 0.12f)
			{
				switch (Rand.Range(0, 4))
				{
				case 0:
					tile.hilliness = Hilliness.Flat;
					break;
				case 1:
					tile.hilliness = Hilliness.SmallHills;
					break;
				case 2:
					tile.hilliness = Hilliness.LargeHills;
					break;
				case 3:
					tile.hilliness = Hilliness.Mountainous;
					break;
				}
			}
			else if (value > 0.0363f)
			{
				tile.hilliness = Hilliness.Mountainous;
			}
			else
			{
				tile.hilliness = Hilliness.Impassable;
			}
			float num = WorldGenStep_Terrain.BaseTemperatureAtLatitude(Find.WorldGrid.LongLatOf(tileID).y);
			num -= WorldGenStep_Terrain.TemperatureReductionAtElevation(tile.elevation);
			num += this.noiseTemperatureOffset.GetValue(tileCenter);
			SimpleCurve temperatureCurve = Find.World.info.overallTemperature.GetTemperatureCurve();
			if (temperatureCurve != null)
			{
				num = temperatureCurve.Evaluate(num);
			}
			tile.temperature = num;
			tile.rainfall = this.noiseRainfall.GetValue(tileCenter);
			if (float.IsNaN(tile.rainfall))
			{
				Log.ErrorOnce(this.noiseRainfall.GetValue(tileCenter) + " rain bad at " + tileID, 694822, false);
			}
			if (tile.hilliness == Hilliness.Flat || tile.hilliness == Hilliness.SmallHills)
			{
				tile.swampiness = this.noiseSwampiness.GetValue(tileCenter);
			}
			tile.biome = this.BiomeFrom(tile, tileID);
			return tile;
		}

		// Token: 0x06006B30 RID: 27440 RVA: 0x00256DAC File Offset: 0x00254FAC
		private BiomeDef BiomeFrom(Tile ws, int tileID)
		{
			List<BiomeDef> allDefsListForReading = DefDatabase<BiomeDef>.AllDefsListForReading;
			BiomeDef biomeDef = null;
			float num = 0f;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				BiomeDef biomeDef2 = allDefsListForReading[i];
				if (biomeDef2.implemented)
				{
					float score = biomeDef2.Worker.GetScore(ws, tileID);
					if (score > num || biomeDef == null)
					{
						biomeDef = biomeDef2;
						num = score;
					}
				}
			}
			return biomeDef;
		}

		// Token: 0x06006B31 RID: 27441 RVA: 0x00256E0C File Offset: 0x0025500C
		private static float FertilityFactorFromTemperature(float temp)
		{
			if (temp < -15f)
			{
				return 0f;
			}
			if (temp < 30f)
			{
				return Mathf.InverseLerp(-15f, 30f, temp);
			}
			if (temp < 50f)
			{
				return Mathf.InverseLerp(50f, 30f, temp);
			}
			return 0f;
		}

		// Token: 0x06006B32 RID: 27442 RVA: 0x00256E60 File Offset: 0x00255060
		private static float BaseTemperatureAtLatitude(float lat)
		{
			float x = Mathf.Abs(lat) / 90f;
			return WorldGenStep_Terrain.AvgTempByLatitudeCurve.Evaluate(x);
		}

		// Token: 0x06006B33 RID: 27443 RVA: 0x00256E88 File Offset: 0x00255088
		private static float TemperatureReductionAtElevation(float elev)
		{
			if (elev < 250f)
			{
				return 0f;
			}
			float t = (elev - 250f) / 4750f;
			return Mathf.Lerp(0f, 40f, t);
		}

		// Token: 0x040042EA RID: 17130
		[Unsaved(false)]
		private ModuleBase noiseElevation;

		// Token: 0x040042EB RID: 17131
		[Unsaved(false)]
		private ModuleBase noiseTemperatureOffset;

		// Token: 0x040042EC RID: 17132
		[Unsaved(false)]
		private ModuleBase noiseRainfall;

		// Token: 0x040042ED RID: 17133
		[Unsaved(false)]
		private ModuleBase noiseSwampiness;

		// Token: 0x040042EE RID: 17134
		[Unsaved(false)]
		private ModuleBase noiseMountainLines;

		// Token: 0x040042EF RID: 17135
		[Unsaved(false)]
		private ModuleBase noiseHillsPatchesMicro;

		// Token: 0x040042F0 RID: 17136
		[Unsaved(false)]
		private ModuleBase noiseHillsPatchesMacro;

		// Token: 0x040042F1 RID: 17137
		private const float ElevationFrequencyMicro = 0.035f;

		// Token: 0x040042F2 RID: 17138
		private const float ElevationFrequencyMacro = 0.012f;

		// Token: 0x040042F3 RID: 17139
		private const float ElevationMacroFactorFrequency = 0.12f;

		// Token: 0x040042F4 RID: 17140
		private const float ElevationContinentsFrequency = 0.01f;

		// Token: 0x040042F5 RID: 17141
		private const float MountainLinesFrequency = 0.025f;

		// Token: 0x040042F6 RID: 17142
		private const float MountainLinesHolesFrequency = 0.06f;

		// Token: 0x040042F7 RID: 17143
		private const float HillsPatchesFrequencyMicro = 0.19f;

		// Token: 0x040042F8 RID: 17144
		private const float HillsPatchesFrequencyMacro = 0.032f;

		// Token: 0x040042F9 RID: 17145
		private const float SwampinessFrequencyMacro = 0.025f;

		// Token: 0x040042FA RID: 17146
		private const float SwampinessFrequencyMicro = 0.09f;

		// Token: 0x040042FB RID: 17147
		private static readonly FloatRange SwampinessMaxElevation = new FloatRange(650f, 750f);

		// Token: 0x040042FC RID: 17148
		private static readonly FloatRange SwampinessMinRainfall = new FloatRange(725f, 900f);

		// Token: 0x040042FD RID: 17149
		private static readonly FloatRange ElevationRange = new FloatRange(-500f, 5000f);

		// Token: 0x040042FE RID: 17150
		private const float TemperatureOffsetFrequency = 0.018f;

		// Token: 0x040042FF RID: 17151
		private const float TemperatureOffsetFactor = 4f;

		// Token: 0x04004300 RID: 17152
		private static readonly SimpleCurve AvgTempByLatitudeCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 30f),
				true
			},
			{
				new CurvePoint(0.1f, 29f),
				true
			},
			{
				new CurvePoint(0.5f, 7f),
				true
			},
			{
				new CurvePoint(1f, -37f),
				true
			}
		};

		// Token: 0x04004301 RID: 17153
		private const float ElevationTempReductionStartAlt = 250f;

		// Token: 0x04004302 RID: 17154
		private const float ElevationTempReductionEndAlt = 5000f;

		// Token: 0x04004303 RID: 17155
		private const float MaxElevationTempReduction = 40f;

		// Token: 0x04004304 RID: 17156
		private const float RainfallOffsetFrequency = 0.013f;

		// Token: 0x04004305 RID: 17157
		private const float RainfallPower = 1.5f;

		// Token: 0x04004306 RID: 17158
		private const float RainfallFactor = 4000f;

		// Token: 0x04004307 RID: 17159
		private const float RainfallStartFallAltitude = 500f;

		// Token: 0x04004308 RID: 17160
		private const float RainfallFinishFallAltitude = 5000f;

		// Token: 0x04004309 RID: 17161
		private const float FertilityTempMinimum = -15f;

		// Token: 0x0400430A RID: 17162
		private const float FertilityTempOptimal = 30f;

		// Token: 0x0400430B RID: 17163
		private const float FertilityTempMaximum = 50f;
	}
}
