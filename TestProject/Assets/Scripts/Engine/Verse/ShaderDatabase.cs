using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	
	[StaticConstructorOnStartup]
	public static class ShaderDatabase
	{
		
		// (get) Token: 0x0600036F RID: 879 RVA: 0x000122BF File Offset: 0x000104BF
		public static Shader DefaultShader
		{
			get
			{
				return ShaderDatabase.Cutout;
			}
		}

		
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

		
		public static readonly Shader Cutout = ShaderDatabase.LoadShader("Map/Cutout");

		
		public static readonly Shader CutoutPlant = ShaderDatabase.LoadShader("Map/CutoutPlant");

		
		public static readonly Shader CutoutComplex = ShaderDatabase.LoadShader("Map/CutoutComplex");

		
		public static readonly Shader CutoutSkin = ShaderDatabase.LoadShader("Map/CutoutSkin");

		
		public static readonly Shader CutoutFlying = ShaderDatabase.LoadShader("Map/CutoutFlying");

		
		public static readonly Shader Transparent = ShaderDatabase.LoadShader("Map/Transparent");

		
		public static readonly Shader TransparentPostLight = ShaderDatabase.LoadShader("Map/TransparentPostLight");

		
		public static readonly Shader TransparentPlant = ShaderDatabase.LoadShader("Map/TransparentPlant");

		
		public static readonly Shader Mote = ShaderDatabase.LoadShader("Map/Mote");

		
		public static readonly Shader MoteGlow = ShaderDatabase.LoadShader("Map/MoteGlow");

		
		public static readonly Shader MoteGlowPulse = ShaderDatabase.LoadShader("Map/MoteGlowPulse");

		
		public static readonly Shader MoteWater = ShaderDatabase.LoadShader("Map/MoteWater");

		
		public static readonly Shader MoteGlowDistorted = ShaderDatabase.LoadShader("Map/MoteGlowDistorted");

		
		public static readonly Shader MoteGlowDistortBG = ShaderDatabase.LoadShader("Map/MoteGlowDistortBackground");

		
		public static readonly Shader MoteProximityScannerRadius = ShaderDatabase.LoadShader("Map/MoteProximityScannerRadius");

		
		public static readonly Shader TerrainHard = ShaderDatabase.LoadShader("Map/TerrainHard");

		
		public static readonly Shader TerrainFade = ShaderDatabase.LoadShader("Map/TerrainFade");

		
		public static readonly Shader TerrainFadeRough = ShaderDatabase.LoadShader("Map/TerrainFadeRough");

		
		public static readonly Shader TerrainWater = ShaderDatabase.LoadShader("Map/TerrainWater");

		
		public static readonly Shader WorldTerrain = ShaderDatabase.LoadShader("World/WorldTerrain");

		
		public static readonly Shader WorldOcean = ShaderDatabase.LoadShader("World/WorldOcean");

		
		public static readonly Shader WorldOverlayCutout = ShaderDatabase.LoadShader("World/WorldOverlayCutout");

		
		public static readonly Shader WorldOverlayTransparent = ShaderDatabase.LoadShader("World/WorldOverlayTransparent");

		
		public static readonly Shader WorldOverlayTransparentLit = ShaderDatabase.LoadShader("World/WorldOverlayTransparentLit");

		
		public static readonly Shader WorldOverlayAdditive = ShaderDatabase.LoadShader("World/WorldOverlayAdditive");

		
		public static readonly Shader MetaOverlay = ShaderDatabase.LoadShader("Map/MetaOverlay");

		
		public static readonly Shader MetaOverlayDesaturated = ShaderDatabase.LoadShader("Map/MetaOverlayDesaturated");

		
		public static readonly Shader SolidColor = ShaderDatabase.LoadShader("Map/SolidColor");

		
		public static readonly Shader VertexColor = ShaderDatabase.LoadShader("Map/VertexColor");

		
		public static readonly Shader Invisible = ShaderDatabase.LoadShader("Misc/Invisible");

		
		private static Dictionary<string, Shader> lookup;
	}
}
