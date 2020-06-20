using System;

namespace Verse
{
	// Token: 0x02000424 RID: 1060
	public static class DebugViewSettings
	{
		// Token: 0x06001FBC RID: 8124 RVA: 0x000C1FB7 File Offset: 0x000C01B7
		public static void drawTerrainWaterToggled()
		{
			if (Find.CurrentMap != null)
			{
				Find.CurrentMap.mapDrawer.WholeMapChanged(MapMeshFlag.Terrain);
			}
		}

		// Token: 0x06001FBD RID: 8125 RVA: 0x000C1FD1 File Offset: 0x000C01D1
		public static void drawShadowsToggled()
		{
			if (Find.CurrentMap != null)
			{
				Find.CurrentMap.mapDrawer.WholeMapChanged((MapMeshFlag)(-1));
			}
		}

		// Token: 0x0400133F RID: 4927
		public static bool drawFog = true;

		// Token: 0x04001340 RID: 4928
		public static bool drawSnow = true;

		// Token: 0x04001341 RID: 4929
		public static bool drawTerrain = true;

		// Token: 0x04001342 RID: 4930
		public static bool drawTerrainWater = true;

		// Token: 0x04001343 RID: 4931
		public static bool drawThingsDynamic = true;

		// Token: 0x04001344 RID: 4932
		public static bool drawThingsPrinted = true;

		// Token: 0x04001345 RID: 4933
		public static bool drawShadows = true;

		// Token: 0x04001346 RID: 4934
		public static bool drawLightingOverlay = true;

		// Token: 0x04001347 RID: 4935
		public static bool drawWorldOverlays = true;

		// Token: 0x04001348 RID: 4936
		public static bool drawPaths = false;

		// Token: 0x04001349 RID: 4937
		public static bool drawCastPositionSearch = false;

		// Token: 0x0400134A RID: 4938
		public static bool drawDestSearch = false;

		// Token: 0x0400134B RID: 4939
		public static bool drawSectionEdges = false;

		// Token: 0x0400134C RID: 4940
		public static bool drawRiverDebug = false;

		// Token: 0x0400134D RID: 4941
		public static bool drawPawnDebug = false;

		// Token: 0x0400134E RID: 4942
		public static bool drawPawnRotatorTarget = false;

		// Token: 0x0400134F RID: 4943
		public static bool drawRegions = false;

		// Token: 0x04001350 RID: 4944
		public static bool drawRegionLinks = false;

		// Token: 0x04001351 RID: 4945
		public static bool drawRegionDirties = false;

		// Token: 0x04001352 RID: 4946
		public static bool drawRegionTraversal = false;

		// Token: 0x04001353 RID: 4947
		public static bool drawRegionThings = false;

		// Token: 0x04001354 RID: 4948
		public static bool drawRooms = false;

		// Token: 0x04001355 RID: 4949
		public static bool drawRoomGroups = false;

		// Token: 0x04001356 RID: 4950
		public static bool drawPower = false;

		// Token: 0x04001357 RID: 4951
		public static bool drawPowerNetGrid = false;

		// Token: 0x04001358 RID: 4952
		public static bool drawOpportunisticJobs = false;

		// Token: 0x04001359 RID: 4953
		public static bool drawTooltipEdges = false;

		// Token: 0x0400135A RID: 4954
		public static bool drawRecordedNoise = false;

		// Token: 0x0400135B RID: 4955
		public static bool drawFoodSearchFromMouse = false;

		// Token: 0x0400135C RID: 4956
		public static bool drawPreyInfo = false;

		// Token: 0x0400135D RID: 4957
		public static bool drawGlow = false;

		// Token: 0x0400135E RID: 4958
		public static bool drawAvoidGrid = false;

		// Token: 0x0400135F RID: 4959
		public static bool drawLords = false;

		// Token: 0x04001360 RID: 4960
		public static bool drawDuties = false;

		// Token: 0x04001361 RID: 4961
		public static bool drawShooting = false;

		// Token: 0x04001362 RID: 4962
		public static bool drawInfestationChance = false;

		// Token: 0x04001363 RID: 4963
		public static bool drawStealDebug = false;

		// Token: 0x04001364 RID: 4964
		public static bool drawDeepResources = false;

		// Token: 0x04001365 RID: 4965
		public static bool drawAttackTargetScores = false;

		// Token: 0x04001366 RID: 4966
		public static bool drawInteractionCells = false;

		// Token: 0x04001367 RID: 4967
		public static bool drawDoorsDebug = false;

		// Token: 0x04001368 RID: 4968
		public static bool drawDestReservations = false;

		// Token: 0x04001369 RID: 4969
		public static bool drawDamageRects = false;

		// Token: 0x0400136A RID: 4970
		public static bool writeGame = false;

		// Token: 0x0400136B RID: 4971
		public static bool writeSteamItems = false;

		// Token: 0x0400136C RID: 4972
		public static bool writeConcepts = false;

		// Token: 0x0400136D RID: 4973
		public static bool writeReservations = false;

		// Token: 0x0400136E RID: 4974
		public static bool writePathCosts = false;

		// Token: 0x0400136F RID: 4975
		public static bool writeFertility = false;

		// Token: 0x04001370 RID: 4976
		public static bool writeLinkFlags = false;

		// Token: 0x04001371 RID: 4977
		public static bool writeCover = false;

		// Token: 0x04001372 RID: 4978
		public static bool writeCellContents = false;

		// Token: 0x04001373 RID: 4979
		public static bool writeMusicManagerPlay = false;

		// Token: 0x04001374 RID: 4980
		public static bool writeStoryteller = false;

		// Token: 0x04001375 RID: 4981
		public static bool writePlayingSounds = false;

		// Token: 0x04001376 RID: 4982
		public static bool writeSoundEventsRecord = false;

		// Token: 0x04001377 RID: 4983
		public static bool writeMoteSaturation = false;

		// Token: 0x04001378 RID: 4984
		public static bool writeSnowDepth = false;

		// Token: 0x04001379 RID: 4985
		public static bool writeEcosystem = false;

		// Token: 0x0400137A RID: 4986
		public static bool writeRecentStrikes = false;

		// Token: 0x0400137B RID: 4987
		public static bool writeBeauty = false;

		// Token: 0x0400137C RID: 4988
		public static bool writeListRepairableBldgs = false;

		// Token: 0x0400137D RID: 4989
		public static bool writeListFilthInHomeArea = false;

		// Token: 0x0400137E RID: 4990
		public static bool writeListHaulables = false;

		// Token: 0x0400137F RID: 4991
		public static bool writeListMergeables = false;

		// Token: 0x04001380 RID: 4992
		public static bool writeTotalSnowDepth = false;

		// Token: 0x04001381 RID: 4993
		public static bool writeCanReachColony = false;

		// Token: 0x04001382 RID: 4994
		public static bool writeMentalStateCalcs = false;

		// Token: 0x04001383 RID: 4995
		public static bool writeWind = false;

		// Token: 0x04001384 RID: 4996
		public static bool writeTerrain = false;

		// Token: 0x04001385 RID: 4997
		public static bool writeApparelScore = false;

		// Token: 0x04001386 RID: 4998
		public static bool writeWorkSettings = false;

		// Token: 0x04001387 RID: 4999
		public static bool writeSkyManager = false;

		// Token: 0x04001388 RID: 5000
		public static bool writeMemoryUsage = false;

		// Token: 0x04001389 RID: 5001
		public static bool writeMapGameConditions = false;

		// Token: 0x0400138A RID: 5002
		public static bool writeAttackTargets = false;

		// Token: 0x0400138B RID: 5003
		public static bool logIncapChance = false;

		// Token: 0x0400138C RID: 5004
		public static bool logInput = false;

		// Token: 0x0400138D RID: 5005
		public static bool logApparelGeneration = false;

		// Token: 0x0400138E RID: 5006
		public static bool logLordToilTransitions = false;

		// Token: 0x0400138F RID: 5007
		public static bool logGrammarResolution = false;

		// Token: 0x04001390 RID: 5008
		public static bool logCombatLogMouseover = false;

		// Token: 0x04001391 RID: 5009
		public static bool logMapLoad = false;

		// Token: 0x04001392 RID: 5010
		public static bool logTutor = false;

		// Token: 0x04001393 RID: 5011
		public static bool logSignals = false;

		// Token: 0x04001394 RID: 5012
		public static bool logWorldPawnGC = false;

		// Token: 0x04001395 RID: 5013
		public static bool logTaleRecording = false;

		// Token: 0x04001396 RID: 5014
		public static bool logHourlyScreenshot = false;

		// Token: 0x04001397 RID: 5015
		public static bool logFilthSummary = false;

		// Token: 0x04001398 RID: 5016
		public static bool debugApparelOptimize = false;

		// Token: 0x04001399 RID: 5017
		public static bool showAllRoomStats = false;

		// Token: 0x0400139A RID: 5018
		public static bool showFloatMenuWorkGivers = false;
	}
}
