using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000477 RID: 1143
	public static class ShootLeanUtility
	{
		// Token: 0x060021BE RID: 8638 RVA: 0x000CD6A7 File Offset: 0x000CB8A7
		private static bool[] GetWorkingBlockedArray()
		{
			if (ShootLeanUtility.blockedArrays.Count > 0)
			{
				return ShootLeanUtility.blockedArrays.Dequeue();
			}
			return new bool[8];
		}

		// Token: 0x060021BF RID: 8639 RVA: 0x000CD6C7 File Offset: 0x000CB8C7
		private static void ReturnWorkingBlockedArray(bool[] ar)
		{
			ShootLeanUtility.blockedArrays.Enqueue(ar);
			if (ShootLeanUtility.blockedArrays.Count > 128)
			{
				Log.ErrorOnce("Too many blocked arrays to be feasible. >128", 388121, false);
			}
		}

		// Token: 0x060021C0 RID: 8640 RVA: 0x000CD6F8 File Offset: 0x000CB8F8
		public static void LeanShootingSourcesFromTo(IntVec3 shooterLoc, IntVec3 targetPos, Map map, List<IntVec3> listToFill)
		{
			listToFill.Clear();
			float angleFlat = (targetPos - shooterLoc).AngleFlat;
			bool flag = angleFlat > 270f || angleFlat < 90f;
			bool flag2 = angleFlat > 90f && angleFlat < 270f;
			bool flag3 = angleFlat > 180f;
			bool flag4 = angleFlat < 180f;
			bool[] workingBlockedArray = ShootLeanUtility.GetWorkingBlockedArray();
			for (int i = 0; i < 8; i++)
			{
				workingBlockedArray[i] = !(shooterLoc + GenAdj.AdjacentCells[i]).CanBeSeenOver(map);
			}
			if (!workingBlockedArray[1] && ((workingBlockedArray[0] && !workingBlockedArray[5] && flag) || (workingBlockedArray[2] && !workingBlockedArray[4] && flag2)))
			{
				listToFill.Add(shooterLoc + new IntVec3(1, 0, 0));
			}
			if (!workingBlockedArray[3] && ((workingBlockedArray[0] && !workingBlockedArray[6] && flag) || (workingBlockedArray[2] && !workingBlockedArray[7] && flag2)))
			{
				listToFill.Add(shooterLoc + new IntVec3(-1, 0, 0));
			}
			if (!workingBlockedArray[2] && ((workingBlockedArray[3] && !workingBlockedArray[7] && flag3) || (workingBlockedArray[1] && !workingBlockedArray[4] && flag4)))
			{
				listToFill.Add(shooterLoc + new IntVec3(0, 0, -1));
			}
			if (!workingBlockedArray[0] && ((workingBlockedArray[3] && !workingBlockedArray[6] && flag3) || (workingBlockedArray[1] && !workingBlockedArray[5] && flag4)))
			{
				listToFill.Add(shooterLoc + new IntVec3(0, 0, 1));
			}
			for (int j = 0; j < 4; j++)
			{
				if (!workingBlockedArray[j] && (j != 0 || flag) && (j != 1 || flag4) && (j != 2 || flag2) && (j != 3 || flag3) && (shooterLoc + GenAdj.AdjacentCells[j]).GetCover(map) != null)
				{
					listToFill.Add(shooterLoc + GenAdj.AdjacentCells[j]);
				}
			}
			ShootLeanUtility.ReturnWorkingBlockedArray(workingBlockedArray);
		}

		// Token: 0x060021C1 RID: 8641 RVA: 0x000CD908 File Offset: 0x000CBB08
		public static void CalcShootableCellsOf(List<IntVec3> outCells, Thing t)
		{
			outCells.Clear();
			if (t is Pawn)
			{
				outCells.Add(t.Position);
				for (int i = 0; i < 4; i++)
				{
					IntVec3 intVec = t.Position + GenAdj.CardinalDirections[i];
					if (intVec.CanBeSeenOver(t.Map))
					{
						outCells.Add(intVec);
					}
				}
				return;
			}
			outCells.Add(t.Position);
			if (t.def.size.x != 1 || t.def.size.z != 1)
			{
				foreach (IntVec3 intVec2 in t.OccupiedRect())
				{
					if (intVec2 != t.Position)
					{
						outCells.Add(intVec2);
					}
				}
			}
		}

		// Token: 0x040014A7 RID: 5287
		private static Queue<bool[]> blockedArrays = new Queue<bool[]>();
	}
}
