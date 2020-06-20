using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C9C RID: 3228
	public static class FilthMaker
	{
		// Token: 0x06004DEE RID: 19950 RVA: 0x001A3658 File Offset: 0x001A1858
		public static bool CanMakeFilth(IntVec3 c, Map map, ThingDef filthDef, FilthSourceFlags additionalFlags = FilthSourceFlags.None)
		{
			return FilthMaker.TerrainAcceptsFilth(c.GetTerrain(map), filthDef, additionalFlags);
		}

		// Token: 0x06004DEF RID: 19951 RVA: 0x001A3668 File Offset: 0x001A1868
		public static bool TerrainAcceptsFilth(TerrainDef terrainDef, ThingDef filthDef, FilthSourceFlags additionalFlags = FilthSourceFlags.None)
		{
			if (terrainDef.filthAcceptanceMask == FilthSourceFlags.None)
			{
				return false;
			}
			FilthSourceFlags filthSourceFlags = filthDef.filth.placementMask | additionalFlags;
			return (terrainDef.filthAcceptanceMask & filthSourceFlags) == filthSourceFlags;
		}

		// Token: 0x06004DF0 RID: 19952 RVA: 0x001A3698 File Offset: 0x001A1898
		public static bool TryMakeFilth(IntVec3 c, Map map, ThingDef filthDef, int count = 1, FilthSourceFlags additionalFlags = FilthSourceFlags.None)
		{
			bool flag = false;
			for (int i = 0; i < count; i++)
			{
				flag |= FilthMaker.TryMakeFilth(c, map, filthDef, null, true, additionalFlags);
			}
			return flag;
		}

		// Token: 0x06004DF1 RID: 19953 RVA: 0x001A36C4 File Offset: 0x001A18C4
		public static bool TryMakeFilth(IntVec3 c, Map map, ThingDef filthDef, string source, int count = 1, FilthSourceFlags additionalFlags = FilthSourceFlags.None)
		{
			bool flag = false;
			for (int i = 0; i < count; i++)
			{
				flag |= FilthMaker.TryMakeFilth(c, map, filthDef, Gen.YieldSingle<string>(source), true, additionalFlags);
			}
			return flag;
		}

		// Token: 0x06004DF2 RID: 19954 RVA: 0x001A36F5 File Offset: 0x001A18F5
		public static bool TryMakeFilth(IntVec3 c, Map map, ThingDef filthDef, IEnumerable<string> sources, FilthSourceFlags additionalFlags = FilthSourceFlags.None)
		{
			return FilthMaker.TryMakeFilth(c, map, filthDef, sources, true, additionalFlags);
		}

		// Token: 0x06004DF3 RID: 19955 RVA: 0x001A3704 File Offset: 0x001A1904
		private static bool TryMakeFilth(IntVec3 c, Map map, ThingDef filthDef, IEnumerable<string> sources, bool shouldPropagate, FilthSourceFlags additionalFlags = FilthSourceFlags.None)
		{
			Filth filth = (Filth)(from t in c.GetThingList(map)
			where t.def == filthDef
			select t).FirstOrDefault<Thing>();
			if (!c.Walkable(map) || (filth != null && !filth.CanBeThickened))
			{
				if (shouldPropagate)
				{
					List<IntVec3> list = GenAdj.AdjacentCells8WayRandomized();
					for (int i = 0; i < 8; i++)
					{
						IntVec3 c2 = c + list[i];
						if (c2.InBounds(map) && FilthMaker.TryMakeFilth(c2, map, filthDef, sources, false, FilthSourceFlags.None))
						{
							return true;
						}
					}
				}
				if (filth != null)
				{
					filth.AddSources(sources);
				}
				return false;
			}
			if (filth != null)
			{
				filth.ThickenFilth();
				filth.AddSources(sources);
			}
			else
			{
				if (!FilthMaker.CanMakeFilth(c, map, filthDef, additionalFlags))
				{
					return false;
				}
				Filth filth2 = (Filth)ThingMaker.MakeThing(filthDef, null);
				filth2.AddSources(sources);
				GenSpawn.Spawn(filth2, c, map, WipeMode.Vanish);
			}
			FilthMonitor.Notify_FilthSpawned();
			return true;
		}

		// Token: 0x06004DF4 RID: 19956 RVA: 0x001A37F4 File Offset: 0x001A19F4
		public static void RemoveAllFilth(IntVec3 c, Map map)
		{
			FilthMaker.toBeRemoved.Clear();
			List<Thing> thingList = c.GetThingList(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				Filth filth = thingList[i] as Filth;
				if (filth != null)
				{
					FilthMaker.toBeRemoved.Add(filth);
				}
			}
			for (int j = 0; j < FilthMaker.toBeRemoved.Count; j++)
			{
				FilthMaker.toBeRemoved[j].Destroy(DestroyMode.Vanish);
			}
			FilthMaker.toBeRemoved.Clear();
		}

		// Token: 0x04002BC1 RID: 11201
		private static List<Filth> toBeRemoved = new List<Filth>();
	}
}
