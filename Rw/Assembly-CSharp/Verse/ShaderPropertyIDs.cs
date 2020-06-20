using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000463 RID: 1123
	[StaticConstructorOnStartup]
	public static class ShaderPropertyIDs
	{
		// Token: 0x04001443 RID: 5187
		private static readonly string PlanetSunLightDirectionName = "_PlanetSunLightDirection";

		// Token: 0x04001444 RID: 5188
		private static readonly string PlanetSunLightEnabledName = "_PlanetSunLightEnabled";

		// Token: 0x04001445 RID: 5189
		private static readonly string PlanetRadiusName = "_PlanetRadius";

		// Token: 0x04001446 RID: 5190
		private static readonly string MapSunLightDirectionName = "_CastVect";

		// Token: 0x04001447 RID: 5191
		private static readonly string GlowRadiusName = "_GlowRadius";

		// Token: 0x04001448 RID: 5192
		private static readonly string GameSecondsName = "_GameSeconds";

		// Token: 0x04001449 RID: 5193
		private static readonly string ColorName = "_Color";

		// Token: 0x0400144A RID: 5194
		private static readonly string ColorTwoName = "_ColorTwo";

		// Token: 0x0400144B RID: 5195
		private static readonly string MaskTexName = "_MaskTex";

		// Token: 0x0400144C RID: 5196
		private static readonly string SwayHeadName = "_SwayHead";

		// Token: 0x0400144D RID: 5197
		private static readonly string ShockwaveSpanName = "_ShockwaveSpan";

		// Token: 0x0400144E RID: 5198
		private static readonly string AgeSecsName = "_AgeSecs";

		// Token: 0x0400144F RID: 5199
		public static int PlanetSunLightDirection = Shader.PropertyToID(ShaderPropertyIDs.PlanetSunLightDirectionName);

		// Token: 0x04001450 RID: 5200
		public static int PlanetSunLightEnabled = Shader.PropertyToID(ShaderPropertyIDs.PlanetSunLightEnabledName);

		// Token: 0x04001451 RID: 5201
		public static int PlanetRadius = Shader.PropertyToID(ShaderPropertyIDs.PlanetRadiusName);

		// Token: 0x04001452 RID: 5202
		public static int MapSunLightDirection = Shader.PropertyToID(ShaderPropertyIDs.MapSunLightDirectionName);

		// Token: 0x04001453 RID: 5203
		public static int GlowRadius = Shader.PropertyToID(ShaderPropertyIDs.GlowRadiusName);

		// Token: 0x04001454 RID: 5204
		public static int GameSeconds = Shader.PropertyToID(ShaderPropertyIDs.GameSecondsName);

		// Token: 0x04001455 RID: 5205
		public static int AgeSecs = Shader.PropertyToID(ShaderPropertyIDs.AgeSecsName);

		// Token: 0x04001456 RID: 5206
		public static int Color = Shader.PropertyToID(ShaderPropertyIDs.ColorName);

		// Token: 0x04001457 RID: 5207
		public static int ColorTwo = Shader.PropertyToID(ShaderPropertyIDs.ColorTwoName);

		// Token: 0x04001458 RID: 5208
		public static int MaskTex = Shader.PropertyToID(ShaderPropertyIDs.MaskTexName);

		// Token: 0x04001459 RID: 5209
		public static int SwayHead = Shader.PropertyToID(ShaderPropertyIDs.SwayHeadName);

		// Token: 0x0400145A RID: 5210
		public static int ShockwaveColor = Shader.PropertyToID("_ShockwaveColor");

		// Token: 0x0400145B RID: 5211
		public static int ShockwaveSpan = Shader.PropertyToID(ShaderPropertyIDs.ShockwaveSpanName);

		// Token: 0x0400145C RID: 5212
		public static int WaterCastVectSun = Shader.PropertyToID("_WaterCastVectSun");

		// Token: 0x0400145D RID: 5213
		public static int WaterCastVectMoon = Shader.PropertyToID("_WaterCastVectMoon");

		// Token: 0x0400145E RID: 5214
		public static int WaterOutputTex = Shader.PropertyToID("_WaterOutputTex");

		// Token: 0x0400145F RID: 5215
		public static int WaterOffsetTex = Shader.PropertyToID("_WaterOffsetTex");

		// Token: 0x04001460 RID: 5216
		public static int ShadowCompositeTex = Shader.PropertyToID("_ShadowCompositeTex");

		// Token: 0x04001461 RID: 5217
		public static int FallIntensity = Shader.PropertyToID("_FallIntensity");
	}
}
