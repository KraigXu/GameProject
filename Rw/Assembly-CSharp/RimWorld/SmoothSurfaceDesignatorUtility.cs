using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E35 RID: 3637
	public static class SmoothSurfaceDesignatorUtility
	{
		// Token: 0x060057E5 RID: 22501 RVA: 0x001D2939 File Offset: 0x001D0B39
		public static bool CanSmoothFloorUnder(Building b)
		{
			return b.def.Fillage != FillCategory.Full || b.def.passability != Traversability.Impassable;
		}

		// Token: 0x060057E6 RID: 22502 RVA: 0x001D295C File Offset: 0x001D0B5C
		public static void Notify_BuildingSpawned(Building b)
		{
			if (!SmoothSurfaceDesignatorUtility.CanSmoothFloorUnder(b))
			{
				foreach (IntVec3 c in b.OccupiedRect())
				{
					Designation designation = b.Map.designationManager.DesignationAt(c, DesignationDefOf.SmoothFloor);
					if (designation != null)
					{
						b.Map.designationManager.RemoveDesignation(designation);
					}
				}
			}
		}

		// Token: 0x060057E7 RID: 22503 RVA: 0x001D29E0 File Offset: 0x001D0BE0
		public static void Notify_BuildingDespawned(Building b, Map map)
		{
			foreach (IntVec3 c in b.OccupiedRect())
			{
				Designation designation = map.designationManager.DesignationAt(c, DesignationDefOf.SmoothWall);
				if (designation != null)
				{
					map.designationManager.RemoveDesignation(designation);
				}
			}
		}
	}
}
