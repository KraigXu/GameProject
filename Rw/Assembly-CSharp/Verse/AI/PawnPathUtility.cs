using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000579 RID: 1401
	public static class PawnPathUtility
	{
		// Token: 0x060027AF RID: 10159 RVA: 0x000E888C File Offset: 0x000E6A8C
		public static Thing FirstBlockingBuilding(this PawnPath path, out IntVec3 cellBefore, Pawn pawn = null)
		{
			if (!path.Found)
			{
				cellBefore = IntVec3.Invalid;
				return null;
			}
			List<IntVec3> nodesReversed = path.NodesReversed;
			if (nodesReversed.Count == 1)
			{
				cellBefore = nodesReversed[0];
				return null;
			}
			Building building = null;
			IntVec3 intVec = IntVec3.Invalid;
			for (int i = nodesReversed.Count - 2; i >= 0; i--)
			{
				Building edifice = nodesReversed[i].GetEdifice(pawn.Map);
				if (edifice != null)
				{
					Building_Door building_Door = edifice as Building_Door;
					if ((building_Door != null && !building_Door.FreePassage && (pawn == null || !building_Door.PawnCanOpen(pawn))) || edifice.def.passability == Traversability.Impassable)
					{
						if (building != null)
						{
							cellBefore = intVec;
							return building;
						}
						cellBefore = nodesReversed[i + 1];
						return edifice;
					}
				}
				if (edifice != null && edifice.def.passability == Traversability.PassThroughOnly && edifice.def.Fillage == FillCategory.Full)
				{
					if (building == null)
					{
						building = edifice;
						intVec = nodesReversed[i + 1];
					}
				}
				else if (edifice == null || edifice.def.passability != Traversability.PassThroughOnly)
				{
					building = null;
				}
			}
			cellBefore = nodesReversed[0];
			return null;
		}

		// Token: 0x060027B0 RID: 10160 RVA: 0x000E89B8 File Offset: 0x000E6BB8
		public static IntVec3 FinalWalkableNonDoorCell(this PawnPath path, Map map)
		{
			if (path.NodesReversed.Count == 1)
			{
				return path.NodesReversed[0];
			}
			List<IntVec3> nodesReversed = path.NodesReversed;
			for (int i = 0; i < nodesReversed.Count; i++)
			{
				Building edifice = nodesReversed[i].GetEdifice(map);
				if (edifice == null || edifice.def.passability != Traversability.Impassable)
				{
					Building_Door building_Door = edifice as Building_Door;
					if (building_Door == null || building_Door.FreePassage)
					{
						return nodesReversed[i];
					}
				}
			}
			return nodesReversed[0];
		}

		// Token: 0x060027B1 RID: 10161 RVA: 0x000E8A38 File Offset: 0x000E6C38
		public static IntVec3 LastCellBeforeBlockerOrFinalCell(this PawnPath path, Map map)
		{
			if (path.NodesReversed.Count == 1)
			{
				return path.NodesReversed[0];
			}
			List<IntVec3> nodesReversed = path.NodesReversed;
			for (int i = nodesReversed.Count - 2; i >= 1; i--)
			{
				Building edifice = nodesReversed[i].GetEdifice(map);
				if (edifice != null)
				{
					if (edifice.def.passability == Traversability.Impassable)
					{
						return nodesReversed[i + 1];
					}
					Building_Door building_Door = edifice as Building_Door;
					if (building_Door != null && !building_Door.FreePassage)
					{
						return nodesReversed[i + 1];
					}
				}
			}
			return nodesReversed[0];
		}

		// Token: 0x060027B2 RID: 10162 RVA: 0x000E8AC8 File Offset: 0x000E6CC8
		public static bool TryFindLastCellBeforeBlockingDoor(this PawnPath path, Pawn pawn, out IntVec3 result)
		{
			if (path.NodesReversed.Count == 1)
			{
				result = path.NodesReversed[0];
				return false;
			}
			List<IntVec3> nodesReversed = path.NodesReversed;
			for (int i = nodesReversed.Count - 2; i >= 1; i--)
			{
				Building_Door building_Door = nodesReversed[i].GetEdifice(pawn.Map) as Building_Door;
				if (building_Door != null && !building_Door.CanPhysicallyPass(pawn))
				{
					result = nodesReversed[i + 1];
					return true;
				}
			}
			result = nodesReversed[0];
			return false;
		}

		// Token: 0x060027B3 RID: 10163 RVA: 0x000E8B54 File Offset: 0x000E6D54
		public static bool TryFindCellAtIndex(PawnPath path, int index, out IntVec3 result)
		{
			if (path.NodesReversed.Count <= index || index < 0)
			{
				result = IntVec3.Invalid;
				return false;
			}
			result = path.NodesReversed[path.NodesReversed.Count - 1 - index];
			return true;
		}
	}
}
