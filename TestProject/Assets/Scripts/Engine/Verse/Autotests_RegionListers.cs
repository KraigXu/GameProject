using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x0200004B RID: 75
	public static class Autotests_RegionListers
	{
		// Token: 0x060003A4 RID: 932 RVA: 0x00012E97 File Offset: 0x00011097
		public static void CheckBugs(Map map)
		{
			Autotests_RegionListers.CalculateExpectedListers(map);
			Autotests_RegionListers.CheckThingRegisteredTwice(map);
			Autotests_RegionListers.CheckThingNotRegisteredButShould();
			Autotests_RegionListers.CheckThingRegisteredButShouldnt(map);
		}

		// Token: 0x060003A5 RID: 933 RVA: 0x00012EB0 File Offset: 0x000110B0
		private static void CheckThingRegisteredTwice(Map map)
		{
			foreach (KeyValuePair<Region, List<Thing>> keyValuePair in Autotests_RegionListers.expectedListers)
			{
				Autotests_RegionListers.CheckDuplicates(keyValuePair.Value, keyValuePair.Key, true);
			}
			foreach (Region region in map.regionGrid.AllRegions)
			{
				Autotests_RegionListers.CheckDuplicates(region.ListerThings.AllThings, region, false);
			}
		}

		// Token: 0x060003A6 RID: 934 RVA: 0x00012F5C File Offset: 0x0001115C
		private static void CheckDuplicates(List<Thing> lister, Region region, bool expected)
		{
			for (int i = 1; i < lister.Count; i++)
			{
				for (int j = 0; j < i; j++)
				{
					if (lister[i] == lister[j])
					{
						if (expected)
						{
							Log.Error(string.Concat(new object[]
							{
								"Region error: thing ",
								lister[i],
								" is expected to be registered twice in ",
								region,
								"? This should never happen."
							}), false);
						}
						else
						{
							Log.Error(string.Concat(new object[]
							{
								"Region error: thing ",
								lister[i],
								" is registered twice in ",
								region
							}), false);
						}
					}
				}
			}
		}

		// Token: 0x060003A7 RID: 935 RVA: 0x0001300C File Offset: 0x0001120C
		private static void CheckThingNotRegisteredButShould()
		{
			foreach (KeyValuePair<Region, List<Thing>> keyValuePair in Autotests_RegionListers.expectedListers)
			{
				List<Thing> value = keyValuePair.Value;
				List<Thing> allThings = keyValuePair.Key.ListerThings.AllThings;
				for (int i = 0; i < value.Count; i++)
				{
					if (!allThings.Contains(value[i]))
					{
						Log.Error(string.Concat(new object[]
						{
							"Region error: thing ",
							value[i],
							" at ",
							value[i].Position,
							" should be registered in ",
							keyValuePair.Key,
							" but it's not."
						}), false);
					}
				}
			}
		}

		// Token: 0x060003A8 RID: 936 RVA: 0x000130F8 File Offset: 0x000112F8
		private static void CheckThingRegisteredButShouldnt(Map map)
		{
			foreach (Region region in map.regionGrid.AllRegions)
			{
				List<Thing> list;
				if (!Autotests_RegionListers.expectedListers.TryGetValue(region, out list))
				{
					list = null;
				}
				List<Thing> allThings = region.ListerThings.AllThings;
				for (int i = 0; i < allThings.Count; i++)
				{
					if (list == null || !list.Contains(allThings[i]))
					{
						Log.Error(string.Concat(new object[]
						{
							"Region error: thing ",
							allThings[i],
							" at ",
							allThings[i].Position,
							" is registered in ",
							region,
							" but it shouldn't be."
						}), false);
					}
				}
			}
		}

		// Token: 0x060003A9 RID: 937 RVA: 0x000131E4 File Offset: 0x000113E4
		private static void CalculateExpectedListers(Map map)
		{
			Autotests_RegionListers.expectedListers.Clear();
			List<Thing> allThings = map.listerThings.AllThings;
			for (int i = 0; i < allThings.Count; i++)
			{
				Thing thing = allThings[i];
				if (ListerThings.EverListable(thing.def, ListerThingsUse.Region))
				{
					RegionListersUpdater.GetTouchableRegions(thing, map, Autotests_RegionListers.tmpTouchableRegions, false);
					for (int j = 0; j < Autotests_RegionListers.tmpTouchableRegions.Count; j++)
					{
						Region key = Autotests_RegionListers.tmpTouchableRegions[j];
						List<Thing> list;
						if (!Autotests_RegionListers.expectedListers.TryGetValue(key, out list))
						{
							list = new List<Thing>();
							Autotests_RegionListers.expectedListers.Add(key, list);
						}
						list.Add(allThings[i]);
					}
				}
			}
		}

		// Token: 0x04000105 RID: 261
		private static Dictionary<Region, List<Thing>> expectedListers = new Dictionary<Region, List<Thing>>();

		// Token: 0x04000106 RID: 262
		private static List<Region> tmpTouchableRegions = new List<Region>();
	}
}
