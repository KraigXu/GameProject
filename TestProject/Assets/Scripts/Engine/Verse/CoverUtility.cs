using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000476 RID: 1142
	public static class CoverUtility
	{
		// Token: 0x060021B8 RID: 8632 RVA: 0x000CD404 File Offset: 0x000CB604
		public static List<CoverInfo> CalculateCoverGiverSet(LocalTargetInfo target, IntVec3 shooterLoc, Map map)
		{
			IntVec3 cell = target.Cell;
			List<CoverInfo> list = new List<CoverInfo>();
			for (int i = 0; i < 8; i++)
			{
				IntVec3 intVec = cell + GenAdj.AdjacentCells[i];
				CoverInfo item;
				if (intVec.InBounds(map) && CoverUtility.TryFindAdjustedCoverInCell(shooterLoc, target, intVec, map, out item) && item.BlockChance > 0f)
				{
					list.Add(item);
				}
			}
			return list;
		}

		// Token: 0x060021B9 RID: 8633 RVA: 0x000CD46C File Offset: 0x000CB66C
		public static float CalculateOverallBlockChance(LocalTargetInfo target, IntVec3 shooterLoc, Map map)
		{
			IntVec3 cell = target.Cell;
			float num = 0f;
			for (int i = 0; i < 8; i++)
			{
				IntVec3 intVec = cell + GenAdj.AdjacentCells[i];
				CoverInfo coverInfo;
				if (intVec.InBounds(map) && CoverUtility.TryFindAdjustedCoverInCell(shooterLoc, target, intVec, map, out coverInfo))
				{
					num += (1f - num) * coverInfo.BlockChance;
				}
			}
			return num;
		}

		// Token: 0x060021BA RID: 8634 RVA: 0x000CD4D0 File Offset: 0x000CB6D0
		private static bool TryFindAdjustedCoverInCell(IntVec3 shooterLoc, LocalTargetInfo target, IntVec3 adjCell, Map map, out CoverInfo result)
		{
			IntVec3 cell = target.Cell;
			Thing cover = adjCell.GetCover(map);
			if (cover == null || cover == target.Thing || shooterLoc == cell)
			{
				result = CoverInfo.Invalid;
				return false;
			}
			float angleFlat = (shooterLoc - cell).AngleFlat;
			float num = GenGeo.AngleDifferenceBetween((adjCell - cell).AngleFlat, angleFlat);
			if (!cell.AdjacentToCardinal(adjCell))
			{
				num *= 1.75f;
			}
			float num2 = cover.BaseBlockChance();
			if (num < 15f)
			{
				num2 *= 1f;
			}
			else if (num < 27f)
			{
				num2 *= 0.8f;
			}
			else if (num < 40f)
			{
				num2 *= 0.6f;
			}
			else if (num < 52f)
			{
				num2 *= 0.4f;
			}
			else
			{
				if (num >= 65f)
				{
					result = CoverInfo.Invalid;
					return false;
				}
				num2 *= 0.2f;
			}
			float lengthHorizontal = (shooterLoc - adjCell).LengthHorizontal;
			if (lengthHorizontal < 1.9f)
			{
				num2 *= 0.3333f;
			}
			else if (lengthHorizontal < 2.9f)
			{
				num2 *= 0.66666f;
			}
			result = new CoverInfo(cover, num2);
			return true;
		}

		// Token: 0x060021BB RID: 8635 RVA: 0x000CD60F File Offset: 0x000CB80F
		public static float BaseBlockChance(this ThingDef def)
		{
			if (def.Fillage == FillCategory.Full)
			{
				return 0.75f;
			}
			return def.fillPercent;
		}

		// Token: 0x060021BC RID: 8636 RVA: 0x000CD628 File Offset: 0x000CB828
		public static float BaseBlockChance(this Thing thing)
		{
			Building_Door building_Door = thing as Building_Door;
			if (building_Door != null && building_Door.Open)
			{
				return 0f;
			}
			return thing.def.BaseBlockChance();
		}

		// Token: 0x060021BD RID: 8637 RVA: 0x000CD658 File Offset: 0x000CB858
		public static float TotalSurroundingCoverScore(IntVec3 c, Map map)
		{
			float num = 0f;
			for (int i = 0; i < 8; i++)
			{
				IntVec3 c2 = c + GenAdj.AdjacentCells[i];
				if (c2.InBounds(map))
				{
					Thing cover = c2.GetCover(map);
					if (cover != null)
					{
						num += cover.BaseBlockChance();
					}
				}
			}
			return num;
		}

		// Token: 0x040014A6 RID: 5286
		public const float CoverPercent_Corner = 0.75f;
	}
}
