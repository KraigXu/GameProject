using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	public static class CaravanArrivalTimeEstimator
	{
		private static int cacheTicks = -1;

		private static Caravan cachedForCaravan;

		private static int cachedForDest = -1;

		private static int cachedResult = -1;

		private const int CacheDuration = 100;

		private const int MaxIterations = 10000;

		private static List<Pair<int, int>> tmpTicksToArrive = new List<Pair<int, int>>();

		public static int EstimatedTicksToArrive(Caravan caravan, bool allowCaching)
		{
			if (allowCaching && caravan == cachedForCaravan && caravan.pather.Destination == cachedForDest && Find.TickManager.TicksGame - cacheTicks < 100)
			{
				return cachedResult;
			}
			int to;
			int result;
			if (!caravan.Spawned || !caravan.pather.Moving || caravan.pather.curPath == null)
			{
				to = -1;
				result = 0;
			}
			else
			{
				to = caravan.pather.Destination;
				result = EstimatedTicksToArrive(caravan.Tile, to, caravan.pather.curPath, caravan.pather.nextTileCostLeft, caravan.TicksPerMove, Find.TickManager.TicksAbs);
			}
			if (allowCaching)
			{
				cacheTicks = Find.TickManager.TicksGame;
				cachedForCaravan = caravan;
				cachedForDest = to;
				cachedResult = result;
			}
			return result;
		}

		public static int EstimatedTicksToArrive(int from, int to, Caravan caravan)
		{
			using (WorldPath worldPath = Find.WorldPathFinder.FindPath(from, to, caravan))
			{
				if (!worldPath.Found)
				{
					return 0;
				}
				return EstimatedTicksToArrive(from, to, worldPath, 0f, caravan?.TicksPerMove ?? 3300, Find.TickManager.TicksAbs);
			}
		}

		public static int EstimatedTicksToArrive(int from, int to, WorldPath path, float nextTileCostLeft, int caravanTicksPerMove, int curTicksAbs)
		{
			tmpTicksToArrive.Clear();
			EstimatedTicksToArriveToEvery(from, to, path, nextTileCostLeft, caravanTicksPerMove, curTicksAbs, tmpTicksToArrive);
			return EstimatedTicksToArrive(to, tmpTicksToArrive);
		}

		public static void EstimatedTicksToArriveToEvery(int from, int to, WorldPath path, float nextTileCostLeft, int caravanTicksPerMove, int curTicksAbs, List<Pair<int, int>> outTicksToArrive)
		{
			outTicksToArrive.Clear();
			outTicksToArrive.Add(new Pair<int, int>(from, 0));
			if (from == to)
			{
				outTicksToArrive.Add(new Pair<int, int>(to, 0));
				return;
			}
			int num = 0;
			int num2 = from;
			int num3 = 0;
			int num4 = Mathf.CeilToInt(20000f) - 1;
			int num5 = 60000 - num4;
			int num6 = 0;
			int num7 = 0;
			int num9;
			if (CaravanNightRestUtility.WouldBeRestingAt(from, curTicksAbs))
			{
				if (Caravan_PathFollower.IsValidFinalPushDestination(to) && (path.Peek(0) == to || (nextTileCostLeft <= 0f && path.NodesLeftCount >= 2 && path.Peek(1) == to)))
				{
					int num8 = Mathf.CeilToInt(GetCostToMove(nextTileCostLeft, path.Peek(0) == to, curTicksAbs, num, caravanTicksPerMove, from, to) / 1f);
					if (num8 <= 10000)
					{
						num += num8;
						outTicksToArrive.Add(new Pair<int, int>(to, num));
						return;
					}
				}
				num += CaravanNightRestUtility.LeftRestTicksAt(from, curTicksAbs);
				num9 = num5;
			}
			else
			{
				num9 = CaravanNightRestUtility.LeftNonRestTicksAt(from, curTicksAbs);
			}
			while (true)
			{
				num7++;
				if (num7 >= 10000)
				{
					Log.ErrorOnce("Could not calculate estimated ticks to arrive. Too many iterations.", 1837451324);
					outTicksToArrive.Add(new Pair<int, int>(to, num));
					return;
				}
				if (num6 <= 0)
				{
					if (num2 == to)
					{
						outTicksToArrive.Add(new Pair<int, int>(to, num));
						return;
					}
					bool firstInPath = num3 == 0;
					int num10 = num2;
					num2 = path.Peek(num3);
					num3++;
					outTicksToArrive.Add(new Pair<int, int>(num10, num));
					num6 = Mathf.CeilToInt(GetCostToMove(nextTileCostLeft, firstInPath, curTicksAbs, num, caravanTicksPerMove, num10, num2) / 1f);
				}
				if (num9 < num6)
				{
					num += num9;
					num6 -= num9;
					if (num2 == to && num6 <= 10000 && Caravan_PathFollower.IsValidFinalPushDestination(to))
					{
						break;
					}
					num += num4;
					num9 = num5;
				}
				else
				{
					num += num6;
					num9 -= num6;
					num6 = 0;
				}
			}
			num += num6;
			outTicksToArrive.Add(new Pair<int, int>(to, num));
		}

		private static float GetCostToMove(float initialNextTileCostLeft, bool firstInPath, int initialTicksAbs, int curResult, int caravanTicksPerMove, int curTile, int nextTile)
		{
			if (firstInPath)
			{
				return initialNextTileCostLeft;
			}
			int value = initialTicksAbs + curResult;
			return Caravan_PathFollower.CostToMove(caravanTicksPerMove, curTile, nextTile, value);
		}

		public static int EstimatedTicksToArrive(int destinationTile, List<Pair<int, int>> estimatedTicksToArriveToEvery)
		{
			if (destinationTile == -1)
			{
				return 0;
			}
			for (int i = 0; i < estimatedTicksToArriveToEvery.Count; i++)
			{
				if (destinationTile == estimatedTicksToArriveToEvery[i].First)
				{
					return estimatedTicksToArriveToEvery[i].Second;
				}
			}
			return 0;
		}

		public static int TileIllBeInAt(int ticksAbs, List<Pair<int, int>> estimatedTicksToArriveToEvery, int ticksAbsUsedToCalculateEstimatedTicksToArriveToEvery)
		{
			if (!estimatedTicksToArriveToEvery.Any())
			{
				return -1;
			}
			for (int num = estimatedTicksToArriveToEvery.Count - 1; num >= 0; num--)
			{
				int num2 = ticksAbsUsedToCalculateEstimatedTicksToArriveToEvery + estimatedTicksToArriveToEvery[num].Second;
				if (ticksAbs >= num2)
				{
					return estimatedTicksToArriveToEvery[num].First;
				}
			}
			return estimatedTicksToArriveToEvery[0].First;
		}
	}
}
