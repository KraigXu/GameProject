using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B2A RID: 2858
	public static class PawnsArrivalModeWorkerUtility
	{
		// Token: 0x06004327 RID: 17191 RVA: 0x00169878 File Offset: 0x00167A78
		public static void DropInDropPodsNearSpawnCenter(IncidentParms parms, List<Pawn> pawns)
		{
			Map map = (Map)parms.target;
			bool flag = parms.faction != null && parms.faction.HostileTo(Faction.OfPlayer);
			DropPodUtility.DropThingsNear(parms.spawnCenter, map, pawns.Cast<Thing>(), parms.podOpenDelay, false, true, flag || parms.raidArrivalModeForQuickMilitaryAid, true);
		}

		// Token: 0x06004328 RID: 17192 RVA: 0x001698D4 File Offset: 0x00167AD4
		public static List<Pair<List<Pawn>, IntVec3>> SplitIntoRandomGroupsNearMapEdge(List<Pawn> pawns, Map map, bool arriveInPods)
		{
			List<Pair<List<Pawn>, IntVec3>> list = new List<Pair<List<Pawn>, IntVec3>>();
			if (!pawns.Any<Pawn>())
			{
				return list;
			}
			int maxGroupsCount = PawnsArrivalModeWorkerUtility.GetMaxGroupsCount(pawns.Count);
			int num = (maxGroupsCount == 1) ? 1 : Rand.RangeInclusive(2, maxGroupsCount);
			for (int i = 0; i < num; i++)
			{
				IntVec3 second = PawnsArrivalModeWorkerUtility.FindNewMapEdgeGroupCenter(map, list, arriveInPods);
				list.Add(new Pair<List<Pawn>, IntVec3>(new List<Pawn>(), second)
				{
					First = 
					{
						pawns[i]
					}
				});
			}
			for (int j = num; j < pawns.Count; j++)
			{
				list.RandomElement<Pair<List<Pawn>, IntVec3>>().First.Add(pawns[j]);
			}
			return list;
		}

		// Token: 0x06004329 RID: 17193 RVA: 0x00169980 File Offset: 0x00167B80
		private static IntVec3 FindNewMapEdgeGroupCenter(Map map, List<Pair<List<Pawn>, IntVec3>> groups, bool arriveInPods)
		{
			IntVec3 result = IntVec3.Invalid;
			float num = 0f;
			for (int i = 0; i < 4; i++)
			{
				IntVec3 intVec;
				if (arriveInPods)
				{
					intVec = DropCellFinder.FindRaidDropCenterDistant_NewTemp(map, false);
				}
				else if (!RCellFinder.TryFindRandomPawnEntryCell(out intVec, map, CellFinder.EdgeRoadChance_Hostile, false, null))
				{
					intVec = DropCellFinder.FindRaidDropCenterDistant_NewTemp(map, false);
				}
				if (!groups.Any<Pair<List<Pawn>, IntVec3>>())
				{
					result = intVec;
					break;
				}
				float num2 = float.MaxValue;
				for (int j = 0; j < groups.Count; j++)
				{
					float num3 = (float)intVec.DistanceToSquared(groups[j].Second);
					if (num3 < num2)
					{
						num2 = num3;
					}
				}
				if (!result.IsValid || num2 > num)
				{
					num = num2;
					result = intVec;
				}
			}
			return result;
		}

		// Token: 0x0600432A RID: 17194 RVA: 0x00169A31 File Offset: 0x00167C31
		private static int GetMaxGroupsCount(int pawnsCount)
		{
			if (pawnsCount <= 1)
			{
				return 1;
			}
			return Mathf.Clamp(pawnsCount / 2, 2, 3);
		}

		// Token: 0x0600432B RID: 17195 RVA: 0x00169A44 File Offset: 0x00167C44
		public static void SetPawnGroupsInfo(IncidentParms parms, List<Pair<List<Pawn>, IntVec3>> groups)
		{
			parms.pawnGroups = new Dictionary<Pawn, int>();
			for (int i = 0; i < groups.Count; i++)
			{
				for (int j = 0; j < groups[i].First.Count; j++)
				{
					parms.pawnGroups.Add(groups[i].First[j], i);
				}
			}
		}

		// Token: 0x04002691 RID: 9873
		private const int MaxGroupsCount = 3;
	}
}
