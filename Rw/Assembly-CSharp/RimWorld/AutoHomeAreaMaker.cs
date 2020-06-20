using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F32 RID: 3890
	public static class AutoHomeAreaMaker
	{
		// Token: 0x06005F4A RID: 24394 RVA: 0x0020E820 File Offset: 0x0020CA20
		private static bool ShouldAdd()
		{
			return Find.PlaySettings.autoHomeArea && Current.ProgramState == ProgramState.Playing;
		}

		// Token: 0x06005F4B RID: 24395 RVA: 0x0020E838 File Offset: 0x0020CA38
		public static void Notify_BuildingSpawned(Thing b)
		{
			if (!AutoHomeAreaMaker.ShouldAdd() || !b.def.building.expandHomeArea || b.Faction != Faction.OfPlayer)
			{
				return;
			}
			AutoHomeAreaMaker.MarkHomeAroundThing(b);
		}

		// Token: 0x06005F4C RID: 24396 RVA: 0x0020E838 File Offset: 0x0020CA38
		public static void Notify_BuildingClaimed(Thing b)
		{
			if (!AutoHomeAreaMaker.ShouldAdd() || !b.def.building.expandHomeArea || b.Faction != Faction.OfPlayer)
			{
				return;
			}
			AutoHomeAreaMaker.MarkHomeAroundThing(b);
		}

		// Token: 0x06005F4D RID: 24397 RVA: 0x0020E868 File Offset: 0x0020CA68
		public static void MarkHomeAroundThing(Thing t)
		{
			if (!AutoHomeAreaMaker.ShouldAdd())
			{
				return;
			}
			CellRect cellRect = new CellRect(t.Position.x - t.RotatedSize.x / 2 - 4, t.Position.z - t.RotatedSize.z / 2 - 4, t.RotatedSize.x + 8, t.RotatedSize.z + 8);
			cellRect.ClipInsideMap(t.Map);
			foreach (IntVec3 c in cellRect)
			{
				t.Map.areaManager.Home[c] = true;
			}
		}

		// Token: 0x06005F4E RID: 24398 RVA: 0x0020E934 File Offset: 0x0020CB34
		public static void Notify_ZoneCellAdded(IntVec3 c, Zone zone)
		{
			if (!AutoHomeAreaMaker.ShouldAdd())
			{
				return;
			}
			foreach (IntVec3 c2 in CellRect.CenteredOn(c, 4).ClipInsideMap(zone.Map))
			{
				zone.Map.areaManager.Home[c2] = true;
			}
		}

		// Token: 0x040033B5 RID: 13237
		private const int BorderWidth = 4;
	}
}
