using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02001235 RID: 4661
	public static class CaravanIncidentUtility
	{
		// Token: 0x06006C8C RID: 27788 RVA: 0x0025DF26 File Offset: 0x0025C126
		public static int CalculateIncidentMapSize(List<Pawn> caravanPawns, List<Pawn> enemies)
		{
			return Mathf.Clamp(Mathf.RoundToInt(Mathf.Sqrt((float)Mathf.RoundToInt((float)((caravanPawns.Count + enemies.Count) * 900)))), 75, 110);
		}

		// Token: 0x06006C8D RID: 27789 RVA: 0x0025DF58 File Offset: 0x0025C158
		public static bool CanFireIncidentWhichWantsToGenerateMapAt(int tile)
		{
			if (Current.Game.FindMap(tile) != null)
			{
				return false;
			}
			if (!Find.WorldGrid[tile].biome.implemented)
			{
				return false;
			}
			List<WorldObject> allWorldObjects = Find.WorldObjects.AllWorldObjects;
			for (int i = 0; i < allWorldObjects.Count; i++)
			{
				if (allWorldObjects[i].Tile == tile && !allWorldObjects[i].def.allowCaravanIncidentsWhichGenerateMap)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06006C8E RID: 27790 RVA: 0x0025DFD0 File Offset: 0x0025C1D0
		public static Map SetupCaravanAttackMap(Caravan caravan, List<Pawn> enemies, bool sendLetterIfRelatedPawns)
		{
			int num = CaravanIncidentUtility.CalculateIncidentMapSize(caravan.PawnsListForReading, enemies);
			Map map = CaravanIncidentUtility.GetOrGenerateMapForIncident(caravan, new IntVec3(num, 1, num), WorldObjectDefOf.Ambush);
			IntVec3 playerStartingSpot;
			IntVec3 root;
			MultipleCaravansCellFinder.FindStartingCellsFor2Groups(map, out playerStartingSpot, out root);
			CaravanEnterMapUtility.Enter(caravan, map, (Pawn x) => CellFinder.RandomSpawnCellForPawnNear(playerStartingSpot, map, 4), CaravanDropInventoryMode.DoNotDrop, true);
			for (int i = 0; i < enemies.Count; i++)
			{
				IntVec3 loc = CellFinder.RandomSpawnCellForPawnNear(root, map, 4);
				GenSpawn.Spawn(enemies[i], loc, map, Rot4.Random, WipeMode.Vanish, false);
			}
			if (sendLetterIfRelatedPawns)
			{
				PawnRelationUtility.Notify_PawnsSeenByPlayer_Letter_Send(enemies, "LetterRelatedPawnsGroupGeneric".Translate(Faction.OfPlayer.def.pawnsPlural), LetterDefOf.NeutralEvent, true, true);
			}
			return map;
		}

		// Token: 0x06006C8F RID: 27791 RVA: 0x0025E0AC File Offset: 0x0025C2AC
		public static Map GetOrGenerateMapForIncident(Caravan caravan, IntVec3 size, WorldObjectDef suggestedMapParentDef)
		{
			int tile = caravan.Tile;
			bool flag = Current.Game.FindMap(tile) == null;
			Map orGenerateMap = GetOrGenerateMapUtility.GetOrGenerateMap(tile, size, suggestedMapParentDef);
			if (flag && orGenerateMap != null)
			{
				orGenerateMap.retainedCaravanData.Notify_GeneratedTempIncidentMapFor(caravan);
			}
			return orGenerateMap;
		}

		// Token: 0x0400438B RID: 17291
		private const int MapCellsPerPawn = 900;

		// Token: 0x0400438C RID: 17292
		private const int MinMapSize = 75;

		// Token: 0x0400438D RID: 17293
		private const int MaxMapSize = 110;
	}
}
