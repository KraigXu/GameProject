using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000AB3 RID: 2739
	[StaticConstructorOnStartup]
	public class WeatherOverlay_Fog : SkyOverlay
	{
		// Token: 0x060040ED RID: 16621 RVA: 0x0015BC54 File Offset: 0x00159E54
		public WeatherOverlay_Fog()
		{
			this.worldOverlayMat = WeatherOverlay_Fog.FogOverlayWorld;
			this.worldOverlayPanSpeed1 = 0.0005f;
			this.worldOverlayPanSpeed2 = 0.0004f;
			this.worldPanDir1 = new Vector2(1f, 1f);
			this.worldPanDir2 = new Vector2(1f, -1f);
		}

		// Token: 0x040025B2 RID: 9650
		private static readonly Material FogOverlayWorld = MatLoader.LoadMat("Weather/FogOverlayWorld", -1);
	}
}
