using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000AB7 RID: 2743
	[StaticConstructorOnStartup]
	public class WeatherOverlay_Fallout : SkyOverlay
	{
		// Token: 0x060040F5 RID: 16629 RVA: 0x0015BE5C File Offset: 0x0015A05C
		public WeatherOverlay_Fallout()
		{
			this.worldOverlayMat = WeatherOverlay_Fallout.FalloutOverlayWorld;
			this.worldOverlayPanSpeed1 = 0.0008f;
			this.worldPanDir1 = new Vector2(-0.25f, -1f);
			this.worldPanDir1.Normalize();
			this.worldOverlayPanSpeed2 = 0.0012f;
			this.worldPanDir2 = new Vector2(-0.24f, -1f);
			this.worldPanDir2.Normalize();
		}

		// Token: 0x040025B6 RID: 9654
		private static readonly Material FalloutOverlayWorld = MatLoader.LoadMat("Weather/SnowOverlayWorld", -1);
	}
}
