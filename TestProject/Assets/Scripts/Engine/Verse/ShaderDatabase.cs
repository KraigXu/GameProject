using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200003D RID: 61
	[StaticConstructorOnStartup]
	public static class ShaderDatabase
	{
		// Token: 0x17000099 RID: 153
		// (get) Token: 0x0600036F RID: 879 RVA: 0x000122BF File Offset: 0x000104BF
		public static Shader DefaultShader
		{
			get
			{
				return ShaderDatabase.Cutout;
			}
		}

		// Token: 0x06000370 RID: 880 RVA: 0x000122C8 File Offset: 0x000104C8
		public static Shader LoadShader(string shaderPath)
		{
			if (ShaderDatabase.lookup == null)
			{
				ShaderDatabase.lookup = new Dictionary<string, Shader>();
			}
			if (!ShaderDatabase.lookup.ContainsKey(shaderPath))
			{
				ShaderDatabase.lookup[shaderPath] = (Shader)Resources.Load("Materials/" + shaderPath, typeof(Shader));
			}
			Shader shader = ShaderDatabase.lookup[shaderPath];
			if (shader == null)
			{
				Log.Warning("Could not load shader " + shaderPath, false);
				return ShaderDatabase.DefaultShader;
			}
			return shader;
		}

		// Token: 0x040000D1 RID: 209
		public static readonly Shader Cutout = ShaderDatabase.LoadShader("Map/Cutout");

		// Token: 0x040000D2 RID: 210
		public static readonly Shader CutoutPlant = ShaderDatabase.LoadShader("Map/CutoutPlant");

		// Token: 0x040000D3 RID: 211
		public static readonly Shader CutoutComplex = ShaderDatabase.LoadShader("Map/CutoutComplex");

		// Token: 0x040000D4 RID: 212
		public static readonly Shader CutoutSkin = ShaderDatabase.LoadShader("Map/CutoutSkin");

		// Token: 0x040000D5 RID: 213
		public static readonly Shader CutoutFlying = ShaderDatabase.LoadShader("Map/CutoutFlying");

		// Token: 0x040000D6 RID: 214
		public static readonly Shader Transparent = ShaderDatabase.LoadShader("Map/Transparent");

		// Token: 0x040000D7 RID: 215
		public static readonly Shader TransparentPostLight = ShaderDatabase.LoadShader("Map/TransparentPostLight");

		// Token: 0x040000D8 RID: 216
		public static readonly Shader TransparentPlant = ShaderDatabase.LoadShader("Map/TransparentPlant");

		// Token: 0x040000D9 RID: 217
		public static readonly Shader Mote = ShaderDatabase.LoadShader("Map/Mote");

		// Token: 0x040000DA RID: 218
		public static readonly Shader MoteGlow = ShaderDatabase.LoadShader("Map/MoteGlow");

		// Token: 0x040000DB RID: 219
		public static readonly Shader MoteGlowPulse = ShaderDatabase.LoadShader("Map/MoteGlowPulse");

		// Token: 0x040000DC RID: 220
		public static readonly Shader MoteWater = ShaderDatabase.LoadShader("Map/MoteWater");

		// Token: 0x040000DD RID: 221
		public static readonly Shader MoteGlowDistorted = ShaderDatabase.LoadShader("Map/MoteGlowDistorted");

		// Token: 0x040000DE RID: 222
		public static readonly Shader MoteGlowDistortBG = ShaderDatabase.LoadShader("Map/MoteGlowDistortBackground");

		// Token: 0x040000DF RID: 223
		public static readonly Shader MoteProximityScannerRadius = ShaderDatabase.LoadShader("Map/MoteProximityScannerRadius");

		// Token: 0x040000E0 RID: 224
		public static readonly Shader TerrainHard = ShaderDatabase.LoadShader("Map/TerrainHard");

		// Token: 0x040000E1 RID: 225
		public static readonly Shader TerrainFade = ShaderDatabase.LoadShader("Map/TerrainFade");

		// Token: 0x040000E2 RID: 226
		public static readonly Shader TerrainFadeRough = ShaderDatabase.LoadShader("Map/TerrainFadeRough");

		// Token: 0x040000E3 RID: 227
		public static readonly Shader TerrainWater = ShaderDatabase.LoadShader("Map/TerrainWater");

		// Token: 0x040000E4 RID: 228
		public static readonly Shader WorldTerrain = ShaderDatabase.LoadShader("World/WorldTerrain");

		// Token: 0x040000E5 RID: 229
		public static readonly Shader WorldOcean = ShaderDatabase.LoadShader("World/WorldOcean");

		// Token: 0x040000E6 RID: 230
		public static readonly Shader WorldOverlayCutout = ShaderDatabase.LoadShader("World/WorldOverlayCutout");

		// Token: 0x040000E7 RID: 231
		public static readonly Shader WorldOverlayTransparent = ShaderDatabase.LoadShader("World/WorldOverlayTransparent");

		// Token: 0x040000E8 RID: 232
		public static readonly Shader WorldOverlayTransparentLit = ShaderDatabase.LoadShader("World/WorldOverlayTransparentLit");

		// Token: 0x040000E9 RID: 233
		public static readonly Shader WorldOverlayAdditive = ShaderDatabase.LoadShader("World/WorldOverlayAdditive");

		// Token: 0x040000EA RID: 234
		public static readonly Shader MetaOverlay = ShaderDatabase.LoadShader("Map/MetaOverlay");

		// Token: 0x040000EB RID: 235
		public static readonly Shader MetaOverlayDesaturated = ShaderDatabase.LoadShader("Map/MetaOverlayDesaturated");

		// Token: 0x040000EC RID: 236
		public static readonly Shader SolidColor = ShaderDatabase.LoadShader("Map/SolidColor");

		// Token: 0x040000ED RID: 237
		public static readonly Shader VertexColor = ShaderDatabase.LoadShader("Map/VertexColor");

		// Token: 0x040000EE RID: 238
		public static readonly Shader Invisible = ShaderDatabase.LoadShader("Misc/Invisible");

		// Token: 0x040000EF RID: 239
		private static Dictionary<string, Shader> lookup;
	}
}
