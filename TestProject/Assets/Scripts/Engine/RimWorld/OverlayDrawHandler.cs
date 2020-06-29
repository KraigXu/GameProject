using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public static class OverlayDrawHandler
	{
		
		public static void DrawPowerGridOverlayThisFrame()
		{
			OverlayDrawHandler.lastPowerGridDrawFrame = Time.frameCount;
		}

		
		// (get) Token: 0x06003E1A RID: 15898 RVA: 0x0014718C File Offset: 0x0014538C
		public static bool ShouldDrawPowerGrid
		{
			get
			{
				return OverlayDrawHandler.lastPowerGridDrawFrame + 1 >= Time.frameCount;
			}
		}

		
		public static void DrawZonesThisFrame()
		{
			OverlayDrawHandler.lastZoneDrawFrame = Time.frameCount;
		}

		
		// (get) Token: 0x06003E1C RID: 15900 RVA: 0x001471AB File Offset: 0x001453AB
		public static bool ShouldDrawZones
		{
			get
			{
				return Find.PlaySettings.showZones || Time.frameCount <= OverlayDrawHandler.lastZoneDrawFrame + 1;
			}
		}

		
		private static int lastPowerGridDrawFrame;

		
		private static int lastZoneDrawFrame;
	}
}
