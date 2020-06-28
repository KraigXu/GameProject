using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B88 RID: 2952
	public static class BeautyUtility
	{
		// Token: 0x06004541 RID: 17729 RVA: 0x00176650 File Offset: 0x00174850
		public static float AverageBeautyPerceptible(IntVec3 root, Map map)
		{
			if (!root.IsValid || !root.InBounds(map))
			{
				return 0f;
			}
			BeautyUtility.tempCountedThings.Clear();
			float num = 0f;
			int num2 = 0;
			BeautyUtility.FillBeautyRelevantCells(root, map);
			for (int i = 0; i < BeautyUtility.beautyRelevantCells.Count; i++)
			{
				num += BeautyUtility.CellBeauty(BeautyUtility.beautyRelevantCells[i], map, BeautyUtility.tempCountedThings);
				num2++;
			}
			BeautyUtility.tempCountedThings.Clear();
			if (num2 == 0)
			{
				return 0f;
			}
			return num / (float)num2;
		}

		// Token: 0x06004542 RID: 17730 RVA: 0x001766D8 File Offset: 0x001748D8
		public static void FillBeautyRelevantCells(IntVec3 root, Map map)
		{
			BeautyUtility.beautyRelevantCells.Clear();
			Room room = root.GetRoom(map, RegionType.Set_Passable);
			if (room == null)
			{
				return;
			}
			BeautyUtility.visibleRooms.Clear();
			BeautyUtility.visibleRooms.Add(room);
			if (room.Regions.Count == 1 && room.Regions[0].type == RegionType.Portal)
			{
				foreach (Region region in room.Regions[0].Neighbors)
				{
					if (!BeautyUtility.visibleRooms.Contains(region.Room))
					{
						BeautyUtility.visibleRooms.Add(region.Room);
					}
				}
			}
			for (int i = 0; i < BeautyUtility.SampleNumCells_Beauty; i++)
			{
				IntVec3 intVec = root + GenRadial.RadialPattern[i];
				if (intVec.InBounds(map) && !intVec.Fogged(map))
				{
					Room room2 = intVec.GetRoom(map, RegionType.Set_Passable);
					if (!BeautyUtility.visibleRooms.Contains(room2))
					{
						bool flag = false;
						for (int j = 0; j < 8; j++)
						{
							IntVec3 loc = intVec + GenAdj.AdjacentCells[j];
							if (BeautyUtility.visibleRooms.Contains(loc.GetRoom(map, RegionType.Set_Passable)))
							{
								flag = true;
								break;
							}
						}
						if (!flag)
						{
							goto IL_13D;
						}
					}
					BeautyUtility.beautyRelevantCells.Add(intVec);
				}
				IL_13D:;
			}
			BeautyUtility.visibleRooms.Clear();
		}

		// Token: 0x06004543 RID: 17731 RVA: 0x0017684C File Offset: 0x00174A4C
		public static float CellBeauty(IntVec3 c, Map map, List<Thing> countedThings = null)
		{
			float num = 0f;
			float num2 = 0f;
			bool flag = false;
			List<Thing> list = map.thingGrid.ThingsListAt(c);
			for (int i = 0; i < list.Count; i++)
			{
				Thing thing = list[i];
				if (BeautyUtility.BeautyRelevant(thing.def.category))
				{
					if (countedThings != null)
					{
						if (countedThings.Contains(thing))
						{
							goto IL_CB;
						}
						countedThings.Add(thing);
					}
					SlotGroup slotGroup = thing.GetSlotGroup();
					if (slotGroup == null || slotGroup.parent == thing || !slotGroup.parent.IgnoreStoredThingsBeauty)
					{
						float num3 = thing.GetStatValue(StatDefOf.Beauty, true);
						if (thing is Filth && !map.roofGrid.Roofed(c))
						{
							num3 *= 0.3f;
						}
						if (thing.def.Fillage == FillCategory.Full)
						{
							flag = true;
							num2 += num3;
						}
						else
						{
							num += num3;
						}
					}
				}
				IL_CB:;
			}
			if (flag)
			{
				return num2;
			}
			return num + map.terrainGrid.TerrainAt(c).GetStatValueAbstract(StatDefOf.Beauty, null);
		}

		// Token: 0x06004544 RID: 17732 RVA: 0x00176957 File Offset: 0x00174B57
		public static bool BeautyRelevant(ThingCategory cat)
		{
			return cat == ThingCategory.Building || cat == ThingCategory.Item || cat == ThingCategory.Plant || cat == ThingCategory.Filth;
		}

		// Token: 0x0400279E RID: 10142
		public static List<IntVec3> beautyRelevantCells = new List<IntVec3>();

		// Token: 0x0400279F RID: 10143
		private static List<Room> visibleRooms = new List<Room>();

		// Token: 0x040027A0 RID: 10144
		public static readonly int SampleNumCells_Beauty = GenRadial.NumCellsInRadius(8.9f);

		// Token: 0x040027A1 RID: 10145
		private static List<Thing> tempCountedThings = new List<Thing>();
	}
}
