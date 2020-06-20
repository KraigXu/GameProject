using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A41 RID: 2625
	public static class OverlayDrawHandler
	{
		// Token: 0x06003E19 RID: 15897 RVA: 0x00147180 File Offset: 0x00145380
		public static void DrawPowerGridOverlayThisFrame()
		{
			OverlayDrawHandler.lastPowerGridDrawFrame = Time.frameCount;
		}

		// Token: 0x17000B07 RID: 2823
		// (get) Token: 0x06003E1A RID: 15898 RVA: 0x0014718C File Offset: 0x0014538C
		public static bool ShouldDrawPowerGrid
		{
			get
			{
				return OverlayDrawHandler.lastPowerGridDrawFrame + 1 >= Time.frameCount;
			}
		}

		// Token: 0x06003E1B RID: 15899 RVA: 0x0014719F File Offset: 0x0014539F
		public static void DrawZonesThisFrame()
		{
			OverlayDrawHandler.lastZoneDrawFrame = Time.frameCount;
		}

		// Token: 0x17000B08 RID: 2824
		// (get) Token: 0x06003E1C RID: 15900 RVA: 0x001471AB File Offset: 0x001453AB
		public static bool ShouldDrawZones
		{
			get
			{
				return Find.PlaySettings.showZones || Time.frameCount <= OverlayDrawHandler.lastZoneDrawFrame + 1;
			}
		}

		// Token: 0x0400242D RID: 9261
		private static int lastPowerGridDrawFrame;

		// Token: 0x0400242E RID: 9262
		private static int lastZoneDrawFrame;
	}
}
