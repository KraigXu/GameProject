using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200124A RID: 4682
	public static class TilesPerDayCalculator
	{
		// Token: 0x06006D0E RID: 27918 RVA: 0x00262F2C File Offset: 0x0026112C
		public static float ApproxTilesPerDay(int caravanTicksPerMove, int tile, int nextTile, StringBuilder explanation = null, string caravanTicksPerMoveExplanation = null)
		{
			if (nextTile == -1)
			{
				nextTile = Find.WorldGrid.FindMostReasonableAdjacentTileForDisplayedPathCost(tile);
			}
			int num = Mathf.CeilToInt((float)Caravan_PathFollower.CostToMove(caravanTicksPerMove, tile, nextTile, null, false, explanation, caravanTicksPerMoveExplanation) / 1f);
			if (num == 0)
			{
				return 0f;
			}
			return 60000f / (float)num;
		}

		// Token: 0x06006D0F RID: 27919 RVA: 0x00262F7D File Offset: 0x0026117D
		public static float ApproxTilesPerDay(Caravan caravan, StringBuilder explanation = null)
		{
			return TilesPerDayCalculator.ApproxTilesPerDay(caravan.TicksPerMove, caravan.Tile, caravan.pather.Moving ? caravan.pather.nextTile : -1, explanation, (explanation != null) ? caravan.TicksPerMoveExplanation : null);
		}

		// Token: 0x06006D10 RID: 27920 RVA: 0x00262FB8 File Offset: 0x002611B8
		public static float ApproxTilesPerDay(List<TransferableOneWay> transferables, float massUsage, float massCapacity, int tile, int nextTile, StringBuilder explanation = null)
		{
			TilesPerDayCalculator.tmpPawns.Clear();
			for (int i = 0; i < transferables.Count; i++)
			{
				TransferableOneWay transferableOneWay = transferables[i];
				if (transferableOneWay.HasAnyThing && transferableOneWay.AnyThing is Pawn)
				{
					for (int j = 0; j < transferableOneWay.CountToTransfer; j++)
					{
						TilesPerDayCalculator.tmpPawns.Add((Pawn)transferableOneWay.things[j]);
					}
				}
			}
			if (!TilesPerDayCalculator.tmpPawns.Any<Pawn>())
			{
				return 0f;
			}
			StringBuilder stringBuilder = (explanation != null) ? new StringBuilder() : null;
			float result = TilesPerDayCalculator.ApproxTilesPerDay(CaravanTicksPerMoveUtility.GetTicksPerMove(TilesPerDayCalculator.tmpPawns, massUsage, massCapacity, stringBuilder), tile, nextTile, explanation, (stringBuilder != null) ? stringBuilder.ToString() : null);
			TilesPerDayCalculator.tmpPawns.Clear();
			return result;
		}

		// Token: 0x06006D11 RID: 27921 RVA: 0x00263078 File Offset: 0x00261278
		public static float ApproxTilesPerDayLeftAfterTransfer(List<TransferableOneWay> transferables, float massUsageLeftAfterTransfer, float massCapacityLeftAfterTransfer, int tile, int nextTile, StringBuilder explanation = null)
		{
			TilesPerDayCalculator.tmpPawns.Clear();
			for (int i = 0; i < transferables.Count; i++)
			{
				TransferableOneWay transferableOneWay = transferables[i];
				if (transferableOneWay.HasAnyThing && transferableOneWay.AnyThing is Pawn)
				{
					for (int j = transferableOneWay.things.Count - 1; j >= transferableOneWay.CountToTransfer; j--)
					{
						TilesPerDayCalculator.tmpPawns.Add((Pawn)transferableOneWay.things[j]);
					}
				}
			}
			if (!TilesPerDayCalculator.tmpPawns.Any<Pawn>())
			{
				return 0f;
			}
			StringBuilder stringBuilder = (explanation != null) ? new StringBuilder() : null;
			float result = TilesPerDayCalculator.ApproxTilesPerDay(CaravanTicksPerMoveUtility.GetTicksPerMove(TilesPerDayCalculator.tmpPawns, massUsageLeftAfterTransfer, massCapacityLeftAfterTransfer, stringBuilder), tile, nextTile, explanation, (stringBuilder != null) ? stringBuilder.ToString() : null);
			TilesPerDayCalculator.tmpPawns.Clear();
			return result;
		}

		// Token: 0x06006D12 RID: 27922 RVA: 0x00263141 File Offset: 0x00261341
		public static float ApproxTilesPerDayLeftAfterTradeableTransfer(List<Thing> allCurrentThings, List<Tradeable> tradeables, float massUsageLeftAfterTradeableTransfer, float massCapacityLeftAfterTradeableTransfer, int tile, int nextTile, StringBuilder explanation = null)
		{
			TilesPerDayCalculator.tmpThingCounts.Clear();
			TransferableUtility.SimulateTradeableTransfer(allCurrentThings, tradeables, TilesPerDayCalculator.tmpThingCounts);
			float result = TilesPerDayCalculator.ApproxTilesPerDay(TilesPerDayCalculator.tmpThingCounts, massUsageLeftAfterTradeableTransfer, massCapacityLeftAfterTradeableTransfer, tile, nextTile, explanation);
			TilesPerDayCalculator.tmpThingCounts.Clear();
			return result;
		}

		// Token: 0x06006D13 RID: 27923 RVA: 0x00263178 File Offset: 0x00261378
		public static float ApproxTilesPerDay(List<ThingCount> thingCounts, float massUsage, float massCapacity, int tile, int nextTile, StringBuilder explanation = null)
		{
			TilesPerDayCalculator.tmpPawns.Clear();
			for (int i = 0; i < thingCounts.Count; i++)
			{
				if (thingCounts[i].Count > 0)
				{
					Pawn pawn = thingCounts[i].Thing as Pawn;
					if (pawn != null)
					{
						TilesPerDayCalculator.tmpPawns.Add(pawn);
					}
				}
			}
			if (!TilesPerDayCalculator.tmpPawns.Any<Pawn>())
			{
				return 0f;
			}
			StringBuilder stringBuilder = (explanation != null) ? new StringBuilder() : null;
			float result = TilesPerDayCalculator.ApproxTilesPerDay(CaravanTicksPerMoveUtility.GetTicksPerMove(TilesPerDayCalculator.tmpPawns, massUsage, massCapacity, stringBuilder), tile, nextTile, explanation, (stringBuilder != null) ? stringBuilder.ToString() : null);
			TilesPerDayCalculator.tmpPawns.Clear();
			return result;
		}

		// Token: 0x040043CB RID: 17355
		private static List<Pawn> tmpPawns = new List<Pawn>();

		// Token: 0x040043CC RID: 17356
		private static List<ThingCount> tmpThingCounts = new List<ThingCount>();
	}
}
