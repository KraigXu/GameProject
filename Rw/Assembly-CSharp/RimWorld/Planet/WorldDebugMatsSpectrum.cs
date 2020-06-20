using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020011E3 RID: 4579
	[StaticConstructorOnStartup]
	public static class WorldDebugMatsSpectrum
	{
		// Token: 0x060069FC RID: 27132 RVA: 0x0024FF0C File Offset: 0x0024E10C
		static WorldDebugMatsSpectrum()
		{
			for (int i = 0; i < 100; i++)
			{
				WorldDebugMatsSpectrum.spectrumMats[i] = MatsFromSpectrum.Get(WorldDebugMatsSpectrum.DebugSpectrum, (float)i / 100f, ShaderDatabase.WorldOverlayTransparent);
				WorldDebugMatsSpectrum.spectrumMats[i].renderQueue = WorldMaterials.DebugTileRenderQueue;
			}
		}

		// Token: 0x060069FD RID: 27133 RVA: 0x0024FF6B File Offset: 0x0024E16B
		public static Material Mat(int ind)
		{
			ind = Mathf.Clamp(ind, 0, 99);
			return WorldDebugMatsSpectrum.spectrumMats[ind];
		}

		// Token: 0x0400420E RID: 16910
		private static readonly Material[] spectrumMats = new Material[100];

		// Token: 0x0400420F RID: 16911
		public const int MaterialCount = 100;

		// Token: 0x04004210 RID: 16912
		private const float Opacity = 0.25f;

		// Token: 0x04004211 RID: 16913
		private static readonly Color[] DebugSpectrum = DebugMatsSpectrum.DebugSpectrum;
	}
}
