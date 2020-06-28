using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020011F8 RID: 4600
	[StaticConstructorOnStartup]
	public static class WorldMaterials
	{
		// Token: 0x170011D5 RID: 4565
		// (get) Token: 0x06006A60 RID: 27232 RVA: 0x0025147E File Offset: 0x0024F67E
		public static Material CurTargetingMat
		{
			get
			{
				WorldMaterials.TargetSquareMatSingle.color = GenDraw.CurTargetingColor;
				return WorldMaterials.TargetSquareMatSingle;
			}
		}

		// Token: 0x06006A61 RID: 27233 RVA: 0x00251494 File Offset: 0x0024F694
		static WorldMaterials()
		{
			WorldMaterials.GenerateMats(ref WorldMaterials.matsFertility, WorldMaterials.FertilitySpectrum, WorldMaterials.NumMatsPerMode);
			WorldMaterials.GenerateMats(ref WorldMaterials.matsTemperature, WorldMaterials.TemperatureSpectrum, WorldMaterials.NumMatsPerMode);
			WorldMaterials.GenerateMats(ref WorldMaterials.matsElevation, WorldMaterials.ElevationSpectrum, WorldMaterials.NumMatsPerMode);
			WorldMaterials.GenerateMats(ref WorldMaterials.matsRainfall, WorldMaterials.RainfallSpectrum, WorldMaterials.NumMatsPerMode);
		}

		// Token: 0x06006A62 RID: 27234 RVA: 0x002519E0 File Offset: 0x0024FBE0
		private static void GenerateMats(ref Material[] mats, Color[] colorSpectrum, int numMats)
		{
			mats = new Material[numMats];
			for (int i = 0; i < numMats; i++)
			{
				mats[i] = MatsFromSpectrum.Get(colorSpectrum, (float)i / (float)numMats);
			}
		}

		// Token: 0x06006A63 RID: 27235 RVA: 0x00251A10 File Offset: 0x0024FC10
		public static Material MatForFertilityOverlay(float fert)
		{
			int value = Mathf.FloorToInt(fert * (float)WorldMaterials.NumMatsPerMode);
			return WorldMaterials.matsFertility[Mathf.Clamp(value, 0, WorldMaterials.NumMatsPerMode - 1)];
		}

		// Token: 0x06006A64 RID: 27236 RVA: 0x00251A40 File Offset: 0x0024FC40
		public static Material MatForTemperature(float temp)
		{
			int value = Mathf.FloorToInt(Mathf.InverseLerp(-50f, 50f, temp) * (float)WorldMaterials.NumMatsPerMode);
			return WorldMaterials.matsTemperature[Mathf.Clamp(value, 0, WorldMaterials.NumMatsPerMode - 1)];
		}

		// Token: 0x06006A65 RID: 27237 RVA: 0x00251A80 File Offset: 0x0024FC80
		public static Material MatForElevation(float elev)
		{
			int value = Mathf.FloorToInt(Mathf.InverseLerp(0f, 5000f, elev) * (float)WorldMaterials.NumMatsPerMode);
			return WorldMaterials.matsElevation[Mathf.Clamp(value, 0, WorldMaterials.NumMatsPerMode - 1)];
		}

		// Token: 0x06006A66 RID: 27238 RVA: 0x00251AC0 File Offset: 0x0024FCC0
		public static Material MatForRainfallOverlay(float rain)
		{
			int value = Mathf.FloorToInt(Mathf.InverseLerp(0f, 5000f, rain) * (float)WorldMaterials.NumMatsPerMode);
			return WorldMaterials.matsRainfall[Mathf.Clamp(value, 0, WorldMaterials.NumMatsPerMode - 1)];
		}

		// Token: 0x04004244 RID: 16964
		public static readonly Material WorldTerrain = MatLoader.LoadMat("World/WorldTerrain", 3500);

		// Token: 0x04004245 RID: 16965
		public static readonly Material WorldIce = MatLoader.LoadMat("World/WorldIce", 3500);

		// Token: 0x04004246 RID: 16966
		public static readonly Material WorldOcean = MatLoader.LoadMat("World/WorldOcean", 3500);

		// Token: 0x04004247 RID: 16967
		public static readonly Material UngeneratedPlanetParts = MatLoader.LoadMat("World/UngeneratedPlanetParts", 3500);

		// Token: 0x04004248 RID: 16968
		public static readonly Material Rivers = MatLoader.LoadMat("World/Rivers", 3530);

		// Token: 0x04004249 RID: 16969
		public static readonly Material RiversBorder = MatLoader.LoadMat("World/RiversBorder", 3520);

		// Token: 0x0400424A RID: 16970
		public static readonly Material Roads = MatLoader.LoadMat("World/Roads", 3540);

		// Token: 0x0400424B RID: 16971
		public static int DebugTileRenderQueue = 3510;

		// Token: 0x0400424C RID: 16972
		public static int WorldObjectRenderQueue = 3550;

		// Token: 0x0400424D RID: 16973
		public static int WorldLineRenderQueue = 3590;

		// Token: 0x0400424E RID: 16974
		public static int DynamicObjectRenderQueue = 3600;

		// Token: 0x0400424F RID: 16975
		public static int FeatureNameRenderQueue = 3610;

		// Token: 0x04004250 RID: 16976
		public static readonly Material MouseTile = MaterialPool.MatFrom("World/MouseTile", ShaderDatabase.WorldOverlayAdditive, 3560);

		// Token: 0x04004251 RID: 16977
		public static readonly Material SelectedTile = MaterialPool.MatFrom("World/SelectedTile", ShaderDatabase.WorldOverlayAdditive, 3560);

		// Token: 0x04004252 RID: 16978
		public static readonly Material CurrentMapTile = MaterialPool.MatFrom("World/CurrentMapTile", ShaderDatabase.WorldOverlayTransparent, 3560);

		// Token: 0x04004253 RID: 16979
		public static readonly Material Stars = MatLoader.LoadMat("World/Stars", -1);

		// Token: 0x04004254 RID: 16980
		public static readonly Material Sun = MatLoader.LoadMat("World/Sun", -1);

		// Token: 0x04004255 RID: 16981
		public static readonly Material PlanetGlow = MatLoader.LoadMat("World/PlanetGlow", -1);

		// Token: 0x04004256 RID: 16982
		public static readonly Material SmallHills = MaterialPool.MatFrom("World/Hills/SmallHills", ShaderDatabase.WorldOverlayTransparentLit, 3510);

		// Token: 0x04004257 RID: 16983
		public static readonly Material LargeHills = MaterialPool.MatFrom("World/Hills/LargeHills", ShaderDatabase.WorldOverlayTransparentLit, 3510);

		// Token: 0x04004258 RID: 16984
		public static readonly Material Mountains = MaterialPool.MatFrom("World/Hills/Mountains", ShaderDatabase.WorldOverlayTransparentLit, 3510);

		// Token: 0x04004259 RID: 16985
		public static readonly Material ImpassableMountains = MaterialPool.MatFrom("World/Hills/Impassable", ShaderDatabase.WorldOverlayTransparentLit, 3510);

		// Token: 0x0400425A RID: 16986
		public static readonly Material VertexColor = MatLoader.LoadMat("World/WorldVertexColor", -1);

		// Token: 0x0400425B RID: 16987
		private static readonly Material TargetSquareMatSingle = MaterialPool.MatFrom("UI/Overlays/TargetHighlight_Square", ShaderDatabase.Transparent, 3560);

		// Token: 0x0400425C RID: 16988
		private static int NumMatsPerMode = 50;

		// Token: 0x0400425D RID: 16989
		public static Material OverlayModeMatOcean = SolidColorMaterials.NewSolidColorMaterial(new Color(0.09f, 0.18f, 0.2f), ShaderDatabase.Transparent);

		// Token: 0x0400425E RID: 16990
		private static Material[] matsFertility;

		// Token: 0x0400425F RID: 16991
		private static readonly Color[] FertilitySpectrum = new Color[]
		{
			new Color(0f, 1f, 0f, 0f),
			new Color(0f, 1f, 0f, 0.5f)
		};

		// Token: 0x04004260 RID: 16992
		private const float TempRange = 50f;

		// Token: 0x04004261 RID: 16993
		private static Material[] matsTemperature;

		// Token: 0x04004262 RID: 16994
		private static readonly Color[] TemperatureSpectrum = new Color[]
		{
			new Color(1f, 1f, 1f),
			new Color(0f, 0f, 1f),
			new Color(0.25f, 0.25f, 1f),
			new Color(0.6f, 0.6f, 1f),
			new Color(0.5f, 0.5f, 0.5f),
			new Color(0.5f, 0.3f, 0f),
			new Color(1f, 0.6f, 0.18f),
			new Color(1f, 0f, 0f)
		};

		// Token: 0x04004263 RID: 16995
		private const float ElevationMax = 5000f;

		// Token: 0x04004264 RID: 16996
		private static Material[] matsElevation;

		// Token: 0x04004265 RID: 16997
		private static readonly Color[] ElevationSpectrum = new Color[]
		{
			new Color(0.224f, 0.18f, 0.15f),
			new Color(0.447f, 0.369f, 0.298f),
			new Color(0.6f, 0.6f, 0.6f),
			new Color(1f, 1f, 1f)
		};

		// Token: 0x04004266 RID: 16998
		private const float RainfallMax = 5000f;

		// Token: 0x04004267 RID: 16999
		private static Material[] matsRainfall;

		// Token: 0x04004268 RID: 17000
		private static readonly Color[] RainfallSpectrum = new Color[]
		{
			new Color(0.9f, 0.9f, 0.9f),
			GenColor.FromBytes(190, 190, 190, 255),
			new Color(0.58f, 0.58f, 0.58f),
			GenColor.FromBytes(196, 112, 110, 255),
			GenColor.FromBytes(200, 179, 150, 255),
			GenColor.FromBytes(255, 199, 117, 255),
			GenColor.FromBytes(255, 255, 84, 255),
			GenColor.FromBytes(145, 255, 253, 255),
			GenColor.FromBytes(0, 255, 0, 255),
			GenColor.FromBytes(63, 198, 55, 255),
			GenColor.FromBytes(13, 150, 5, 255),
			GenColor.FromBytes(5, 112, 94, 255)
		};
	}
}
