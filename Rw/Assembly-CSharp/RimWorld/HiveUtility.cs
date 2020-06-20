using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000CA2 RID: 3234
	public static class HiveUtility
	{
		// Token: 0x06004E3B RID: 20027 RVA: 0x001A4BD9 File Offset: 0x001A2DD9
		public static int TotalSpawnedHivesCount(Map map)
		{
			return map.listerThings.ThingsOfDef(ThingDefOf.Hive).Count;
		}

		// Token: 0x06004E3C RID: 20028 RVA: 0x001A4BF0 File Offset: 0x001A2DF0
		public static bool AnyHivePreventsClaiming(Thing thing)
		{
			if (!thing.Spawned)
			{
				return false;
			}
			int num = GenRadial.NumCellsInRadius(2f);
			for (int i = 0; i < num; i++)
			{
				IntVec3 c = thing.Position + GenRadial.RadialPattern[i];
				if (c.InBounds(thing.Map) && c.GetFirstThing(thing.Map) != null)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06004E3D RID: 20029 RVA: 0x001A4C54 File Offset: 0x001A2E54
		public static void Notify_HiveDespawned(Hive hive, Map map)
		{
			int num = GenRadial.NumCellsInRadius(2f);
			for (int i = 0; i < num; i++)
			{
				IntVec3 c = hive.Position + GenRadial.RadialPattern[i];
				if (c.InBounds(map))
				{
					List<Thing> thingList = c.GetThingList(map);
					for (int j = 0; j < thingList.Count; j++)
					{
						if (thingList[j].Faction == Faction.OfInsects && !HiveUtility.AnyHivePreventsClaiming(thingList[j]) && !(thingList[j] is Pawn))
						{
							thingList[j].SetFaction(null, null);
						}
					}
				}
			}
		}

		// Token: 0x04002BEC RID: 11244
		private const float HivePreventsClaimingInRadius = 2f;
	}
}
