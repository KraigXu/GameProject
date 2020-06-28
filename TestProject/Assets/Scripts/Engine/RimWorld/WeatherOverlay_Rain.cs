using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000AB4 RID: 2740
	[StaticConstructorOnStartup]
	public class WeatherOverlay_Rain : SkyOverlay
	{
		// Token: 0x060040EF RID: 16623 RVA: 0x0015BCC4 File Offset: 0x00159EC4
		public WeatherOverlay_Rain()
		{
			this.worldOverlayMat = WeatherOverlay_Rain.RainOverlayWorld;
			this.worldOverlayPanSpeed1 = 0.015f;
			this.worldPanDir1 = new Vector2(-0.25f, -1f);
			this.worldPanDir1.Normalize();
			this.worldOverlayPanSpeed2 = 0.022f;
			this.worldPanDir2 = new Vector2(-0.24f, -1f);
			this.worldPanDir2.Normalize();
		}

		// Token: 0x040025B3 RID: 9651
		private static readonly Material RainOverlayWorld = MatLoader.LoadMat("Weather/RainOverlayWorld", -1);
	}
}
