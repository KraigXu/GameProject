using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000CFF RID: 3327
	public static class DeepDrillUtility
	{
		// Token: 0x060050EC RID: 20716 RVA: 0x001B2858 File Offset: 0x001B0A58
		public static ThingDef GetNextResource(IntVec3 p, Map map)
		{
			ThingDef result;
			int num;
			IntVec3 intVec;
			DeepDrillUtility.GetNextResource(p, map, out result, out num, out intVec);
			return result;
		}

		// Token: 0x060050ED RID: 20717 RVA: 0x001B2874 File Offset: 0x001B0A74
		public static bool GetNextResource(IntVec3 p, Map map, out ThingDef resDef, out int countPresent, out IntVec3 cell)
		{
			for (int i = 0; i < 21; i++)
			{
				IntVec3 intVec = p + GenRadial.RadialPattern[i];
				if (intVec.InBounds(map))
				{
					ThingDef thingDef = map.deepResourceGrid.ThingDefAt(intVec);
					if (thingDef != null)
					{
						resDef = thingDef;
						countPresent = map.deepResourceGrid.CountAt(intVec);
						cell = intVec;
						return true;
					}
				}
			}
			resDef = DeepDrillUtility.GetBaseResource(map, p);
			countPresent = int.MaxValue;
			cell = p;
			return false;
		}

		// Token: 0x060050EE RID: 20718 RVA: 0x001B28F0 File Offset: 0x001B0AF0
		public static ThingDef GetBaseResource(Map map, IntVec3 cell)
		{
			if (!map.Biome.hasBedrock)
			{
				return null;
			}
			Rand.PushState();
			Rand.Seed = cell.GetHashCode();
			ThingDef result = (from rock in Find.World.NaturalRockTypesIn(map.Tile)
			select rock.building.mineableThing).RandomElement<ThingDef>();
			Rand.PopState();
			return result;
		}

		// Token: 0x04002CEB RID: 11499
		public const int NumCellsToScan = 21;
	}
}
