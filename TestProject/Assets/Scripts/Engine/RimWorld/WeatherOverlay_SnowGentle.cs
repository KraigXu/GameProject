using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000AB6 RID: 2742
	[StaticConstructorOnStartup]
	public class WeatherOverlay_SnowGentle : SkyOverlay
	{
		// Token: 0x060040F3 RID: 16627 RVA: 0x0015BDD4 File Offset: 0x00159FD4
		public WeatherOverlay_SnowGentle()
		{
			this.worldOverlayMat = WeatherOverlay_SnowGentle.SnowGentleOverlayWorld;
			this.worldOverlayPanSpeed1 = 0.002f;
			this.worldPanDir1 = new Vector2(-0.25f, -1f);
			this.worldPanDir1.Normalize();
			this.worldOverlayPanSpeed2 = 0.003f;
			this.worldPanDir2 = new Vector2(-0.24f, -1f);
			this.worldPanDir2.Normalize();
		}

		// Token: 0x040025B5 RID: 9653
		private static readonly Material SnowGentleOverlayWorld = MatLoader.LoadMat("Weather/SnowOverlayWorld", -1);
	}
}
