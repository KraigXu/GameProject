using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000471 RID: 1137
	public class PrefsData
	{
		// Token: 0x060021A5 RID: 8613 RVA: 0x000CCF4C File Offset: 0x000CB14C
		public void Apply()
		{
			if (!UnityData.IsInMainThread)
			{
				return;
			}
			if (this.customCursorEnabled)
			{
				CustomCursor.Activate();
			}
			else
			{
				CustomCursor.Deactivate();
			}
			AudioListener.volume = this.volumeGame;
			Application.runInBackground = this.runInBackground;
			if (this.screenWidth == 0 || this.screenHeight == 0)
			{
				ResolutionUtility.SetNativeResolutionRaw();
				return;
			}
			ResolutionUtility.SetResolutionRaw(this.screenWidth, this.screenHeight, this.fullscreen);
		}

		// Token: 0x04001478 RID: 5240
		public float volumeGame = 0.8f;

		// Token: 0x04001479 RID: 5241
		public float volumeMusic = 0.4f;

		// Token: 0x0400147A RID: 5242
		public float volumeAmbient = 1f;

		// Token: 0x0400147B RID: 5243
		public int screenWidth;

		// Token: 0x0400147C RID: 5244
		public int screenHeight;

		// Token: 0x0400147D RID: 5245
		public bool fullscreen;

		// Token: 0x0400147E RID: 5246
		public float uiScale = 1f;

		// Token: 0x0400147F RID: 5247
		public bool customCursorEnabled = true;

		// Token: 0x04001480 RID: 5248
		public bool hatsOnlyOnMap;

		// Token: 0x04001481 RID: 5249
		public bool plantWindSway = true;

		// Token: 0x04001482 RID: 5250
		public bool showRealtimeClock;

		// Token: 0x04001483 RID: 5251
		public AnimalNameDisplayMode animalNameMode;

		// Token: 0x04001484 RID: 5252
		public bool extremeDifficultyUnlocked;

		// Token: 0x04001485 RID: 5253
		public bool adaptiveTrainingEnabled = true;

		// Token: 0x04001486 RID: 5254
		public List<string> preferredNames = new List<string>();

		// Token: 0x04001487 RID: 5255
		public bool resourceReadoutCategorized;

		// Token: 0x04001488 RID: 5256
		public bool runInBackground;

		// Token: 0x04001489 RID: 5257
		public bool edgeScreenScroll = true;

		// Token: 0x0400148A RID: 5258
		public TemperatureDisplayMode temperatureMode;

		// Token: 0x0400148B RID: 5259
		public float autosaveIntervalDays = 1f;

		// Token: 0x0400148C RID: 5260
		public bool testMapSizes;

		// Token: 0x0400148D RID: 5261
		[LoadAlias("maxNumberOfPlayerHomes")]
		public int maxNumberOfPlayerSettlements = 1;

		// Token: 0x0400148E RID: 5262
		public bool pauseOnLoad;

		// Token: 0x0400148F RID: 5263
		public AutomaticPauseMode automaticPauseMode = AutomaticPauseMode.MajorThreat;

		// Token: 0x04001490 RID: 5264
		public float mapDragSensitivity = 1.3f;

		// Token: 0x04001491 RID: 5265
		[Unsaved(true)]
		public bool? pauseOnUrgentLetter;

		// Token: 0x04001492 RID: 5266
		public bool devMode;

		// Token: 0x04001493 RID: 5267
		public string langFolderName = "unknown";

		// Token: 0x04001494 RID: 5268
		public bool logVerbose;

		// Token: 0x04001495 RID: 5269
		public bool pauseOnError;

		// Token: 0x04001496 RID: 5270
		public bool resetModsConfigOnCrash = true;

		// Token: 0x04001497 RID: 5271
		public bool simulateNotOwningRoyalty;
	}
}
