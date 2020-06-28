using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000CAF RID: 3247
	public static class PlantFallColors
	{
		// Token: 0x06004EC4 RID: 20164 RVA: 0x001A8128 File Offset: 0x001A6328
		public static float GetFallColorFactor(float latitude, int dayOfYear)
		{
			float a = GenCelestial.AverageGlow(latitude, dayOfYear);
			float b = GenCelestial.AverageGlow(latitude, dayOfYear + 1);
			float x = Mathf.LerpUnclamped(a, b, PlantFallColors.FallSlopeComponent);
			return GenMath.LerpDoubleClamped(PlantFallColors.FallColorBegin, PlantFallColors.FallColorEnd, 0f, 1f, x);
		}

		// Token: 0x06004EC5 RID: 20165 RVA: 0x001A816C File Offset: 0x001A636C
		public static void SetFallShaderGlobals(Map map)
		{
			if (PlantFallColors.FallIntensityOverride)
			{
				Shader.SetGlobalFloat(ShaderPropertyIDs.FallIntensity, PlantFallColors.FallIntensity);
			}
			else
			{
				Vector2 vector = Find.WorldGrid.LongLatOf(map.Tile);
				Shader.SetGlobalFloat(ShaderPropertyIDs.FallIntensity, PlantFallColors.GetFallColorFactor(vector.y, GenLocalDate.DayOfYear(map)));
			}
			Shader.SetGlobalInt("_FallGlobalControls", PlantFallColors.FallGlobalControls ? 1 : 0);
			if (PlantFallColors.FallGlobalControls)
			{
				Shader.SetGlobalVector("_FallSrc", new Vector3(PlantFallColors.FallSrcR, PlantFallColors.FallSrcG, PlantFallColors.FallSrcB));
				Shader.SetGlobalVector("_FallDst", new Vector3(PlantFallColors.FallDstR, PlantFallColors.FallDstG, PlantFallColors.FallDstB));
				Shader.SetGlobalVector("_FallRange", new Vector3(PlantFallColors.FallRangeBegin, PlantFallColors.FallRangeEnd));
			}
		}

		// Token: 0x04002C31 RID: 11313
		[TweakValue("Graphics", 0f, 1f)]
		private static float FallColorBegin = 0.55f;

		// Token: 0x04002C32 RID: 11314
		[TweakValue("Graphics", 0f, 1f)]
		private static float FallColorEnd = 0.45f;

		// Token: 0x04002C33 RID: 11315
		[TweakValue("Graphics", 0f, 30f)]
		private static float FallSlopeComponent = 15f;

		// Token: 0x04002C34 RID: 11316
		[TweakValue("Graphics", 0f, 100f)]
		private static bool FallIntensityOverride = false;

		// Token: 0x04002C35 RID: 11317
		[TweakValue("Graphics", 0f, 1f)]
		private static float FallIntensity = 0f;

		// Token: 0x04002C36 RID: 11318
		[TweakValue("Graphics", 0f, 100f)]
		private static bool FallGlobalControls = false;

		// Token: 0x04002C37 RID: 11319
		[TweakValue("Graphics", 0f, 1f)]
		private static float FallSrcR = 0.3803f;

		// Token: 0x04002C38 RID: 11320
		[TweakValue("Graphics", 0f, 1f)]
		private static float FallSrcG = 0.4352f;

		// Token: 0x04002C39 RID: 11321
		[TweakValue("Graphics", 0f, 1f)]
		private static float FallSrcB = 0.1451f;

		// Token: 0x04002C3A RID: 11322
		[TweakValue("Graphics", 0f, 1f)]
		private static float FallDstR = 0.4392f;

		// Token: 0x04002C3B RID: 11323
		[TweakValue("Graphics", 0f, 1f)]
		private static float FallDstG = 0.3254f;

		// Token: 0x04002C3C RID: 11324
		[TweakValue("Graphics", 0f, 1f)]
		private static float FallDstB = 0.1765f;

		// Token: 0x04002C3D RID: 11325
		[TweakValue("Graphics", 0f, 1f)]
		private static float FallRangeBegin = 0.02f;

		// Token: 0x04002C3E RID: 11326
		[TweakValue("Graphics", 0f, 1f)]
		private static float FallRangeEnd = 0.1f;
	}
}
