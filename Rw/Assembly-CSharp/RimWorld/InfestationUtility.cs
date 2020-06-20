﻿using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009E5 RID: 2533
	public static class InfestationUtility
	{
		// Token: 0x06003C5D RID: 15453 RVA: 0x0013ED54 File Offset: 0x0013CF54
		public static Thing SpawnTunnels(int hiveCount, Map map, bool spawnAnywhereIfNoGoodCell = false, bool ignoreRoofedRequirement = false, string questTag = null)
		{
			IntVec3 loc;
			if (!InfestationCellFinder.TryFindCell(out loc, map))
			{
				if (!spawnAnywhereIfNoGoodCell)
				{
					return null;
				}
				if (!RCellFinder.TryFindRandomCellNearTheCenterOfTheMapWith(delegate(IntVec3 x)
				{
					if (!x.Standable(map) || x.Fogged(map))
					{
						return false;
					}
					bool result = false;
					int num = GenRadial.NumCellsInRadius(3f);
					for (int j = 0; j < num; j++)
					{
						IntVec3 c = x + GenRadial.RadialPattern[j];
						if (c.InBounds(map))
						{
							RoofDef roof = c.GetRoof(map);
							if (roof != null && roof.isThickRoof)
							{
								result = true;
								break;
							}
						}
					}
					return result;
				}, map, out loc))
				{
					return null;
				}
			}
			Thing thing = GenSpawn.Spawn(ThingMaker.MakeThing(ThingDefOf.TunnelHiveSpawner, null), loc, map, WipeMode.FullRefund);
			QuestUtility.AddQuestTag(thing, questTag);
			for (int i = 0; i < hiveCount - 1; i++)
			{
				loc = CompSpawnerHives.FindChildHiveLocation(thing.Position, map, ThingDefOf.Hive, ThingDefOf.Hive.GetCompProperties<CompProperties_SpawnerHives>(), ignoreRoofedRequirement, true);
				if (loc.IsValid)
				{
					thing = GenSpawn.Spawn(ThingMaker.MakeThing(ThingDefOf.TunnelHiveSpawner, null), loc, map, WipeMode.FullRefund);
					QuestUtility.AddQuestTag(thing, questTag);
				}
			}
			return thing;
		}
	}
}
