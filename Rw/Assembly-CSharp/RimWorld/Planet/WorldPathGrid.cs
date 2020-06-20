using System;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020011BA RID: 4538
	public class WorldPathGrid
	{
		// Token: 0x17001175 RID: 4469
		// (get) Token: 0x060068E9 RID: 26857 RVA: 0x0024A72D File Offset: 0x0024892D
		private static int DayOfYearAt0Long
		{
			get
			{
				return GenDate.DayOfYear((long)GenTicks.TicksAbs, 0f);
			}
		}

		// Token: 0x060068EA RID: 26858 RVA: 0x0024A73F File Offset: 0x0024893F
		public WorldPathGrid()
		{
			this.ResetPathGrid();
		}

		// Token: 0x060068EB RID: 26859 RVA: 0x0024A754 File Offset: 0x00248954
		public void ResetPathGrid()
		{
			this.movementDifficulty = new float[Find.WorldGrid.TilesCount];
		}

		// Token: 0x060068EC RID: 26860 RVA: 0x0024A76B File Offset: 0x0024896B
		public void WorldPathGridTick()
		{
			if (this.allPathCostsRecalculatedDayOfYear != WorldPathGrid.DayOfYearAt0Long)
			{
				this.RecalculateAllPerceivedPathCosts();
			}
		}

		// Token: 0x060068ED RID: 26861 RVA: 0x0024A780 File Offset: 0x00248980
		public bool Passable(int tile)
		{
			return Find.WorldGrid.InBounds(tile) && this.movementDifficulty[tile] < 1000f;
		}

		// Token: 0x060068EE RID: 26862 RVA: 0x0024A7A0 File Offset: 0x002489A0
		public bool PassableFast(int tile)
		{
			return this.movementDifficulty[tile] < 1000f;
		}

		// Token: 0x060068EF RID: 26863 RVA: 0x0024A7B1 File Offset: 0x002489B1
		public float PerceivedMovementDifficultyAt(int tile)
		{
			return this.movementDifficulty[tile];
		}

		// Token: 0x060068F0 RID: 26864 RVA: 0x0024A7BB File Offset: 0x002489BB
		public void RecalculatePerceivedMovementDifficultyAt(int tile, int? ticksAbs = null)
		{
			if (!Find.WorldGrid.InBounds(tile))
			{
				return;
			}
			bool flag = this.PassableFast(tile);
			this.movementDifficulty[tile] = WorldPathGrid.CalculatedMovementDifficultyAt(tile, true, ticksAbs, null);
			if (flag != this.PassableFast(tile))
			{
				Find.WorldReachability.ClearCache();
			}
		}

		// Token: 0x060068F1 RID: 26865 RVA: 0x0024A7F8 File Offset: 0x002489F8
		public void RecalculateAllPerceivedPathCosts()
		{
			this.RecalculateAllPerceivedPathCosts(null);
			this.allPathCostsRecalculatedDayOfYear = WorldPathGrid.DayOfYearAt0Long;
		}

		// Token: 0x060068F2 RID: 26866 RVA: 0x0024A820 File Offset: 0x00248A20
		public void RecalculateAllPerceivedPathCosts(int? ticksAbs)
		{
			this.allPathCostsRecalculatedDayOfYear = -1;
			for (int i = 0; i < this.movementDifficulty.Length; i++)
			{
				this.RecalculatePerceivedMovementDifficultyAt(i, ticksAbs);
			}
		}

		// Token: 0x060068F3 RID: 26867 RVA: 0x0024A850 File Offset: 0x00248A50
		public static float CalculatedMovementDifficultyAt(int tile, bool perceivedStatic, int? ticksAbs = null, StringBuilder explanation = null)
		{
			Tile tile2 = Find.WorldGrid[tile];
			if (explanation != null && explanation.Length > 0)
			{
				explanation.AppendLine();
			}
			if (tile2.biome.impassable || tile2.hilliness == Hilliness.Impassable)
			{
				if (explanation != null)
				{
					explanation.Append("Impassable".Translate());
				}
				return 1000f;
			}
			float num = 0f + tile2.biome.movementDifficulty;
			if (explanation != null)
			{
				explanation.Append(tile2.biome.LabelCap + ": " + tile2.biome.movementDifficulty.ToStringWithSign("0.#"));
			}
			float num2 = WorldPathGrid.HillinessMovementDifficultyOffset(tile2.hilliness);
			float num3 = num + num2;
			if (explanation != null && num2 != 0f)
			{
				explanation.AppendLine();
				explanation.Append(tile2.hilliness.GetLabelCap() + ": " + num2.ToStringWithSign("0.#"));
			}
			return num3 + WorldPathGrid.GetCurrentWinterMovementDifficultyOffset(tile, new int?(ticksAbs ?? GenTicks.TicksAbs), explanation);
		}

		// Token: 0x060068F4 RID: 26868 RVA: 0x0024A970 File Offset: 0x00248B70
		public static float GetCurrentWinterMovementDifficultyOffset(int tile, int? ticksAbs = null, StringBuilder explanation = null)
		{
			if (ticksAbs == null)
			{
				ticksAbs = new int?(GenTicks.TicksAbs);
			}
			Vector2 vector = Find.WorldGrid.LongLatOf(tile);
			float num;
			float num2;
			float num3;
			float num4;
			float num5;
			float num6;
			SeasonUtility.GetSeason(GenDate.YearPercent((long)ticksAbs.Value, vector.x), vector.y, out num, out num2, out num3, out num4, out num5, out num6);
			float num7 = num4 + num6;
			num7 *= Mathf.InverseLerp(5f, 0f, GenTemperature.GetTemperatureFromSeasonAtTile(ticksAbs.Value, tile));
			if (num7 > 0.01f)
			{
				float num8 = 2f * num7;
				if (explanation != null)
				{
					explanation.AppendLine();
					explanation.Append("Winter".Translate());
					if (num7 < 0.999f)
					{
						explanation.Append(" (" + num7.ToStringPercent("F0") + ")");
					}
					explanation.Append(": ");
					explanation.Append(num8.ToStringWithSign("0.#"));
				}
				return num8;
			}
			return 0f;
		}

		// Token: 0x060068F5 RID: 26869 RVA: 0x0024AA78 File Offset: 0x00248C78
		public static bool WillWinterEverAffectMovementDifficulty(int tile)
		{
			int ticksAbs = GenTicks.TicksAbs;
			for (int i = 0; i < 3600000; i += 60000)
			{
				if (GenTemperature.GetTemperatureFromSeasonAtTile(ticksAbs + i, tile) < 5f)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060068F6 RID: 26870 RVA: 0x0024AAB4 File Offset: 0x00248CB4
		private static float HillinessMovementDifficultyOffset(Hilliness hilliness)
		{
			switch (hilliness)
			{
			case Hilliness.Flat:
				return 0f;
			case Hilliness.SmallHills:
				return 0.5f;
			case Hilliness.LargeHills:
				return 1.5f;
			case Hilliness.Mountainous:
				return 3f;
			case Hilliness.Impassable:
				return 1000f;
			default:
				return 0f;
			}
		}

		// Token: 0x0400414D RID: 16717
		public float[] movementDifficulty;

		// Token: 0x0400414E RID: 16718
		private int allPathCostsRecalculatedDayOfYear = -1;

		// Token: 0x0400414F RID: 16719
		private const float ImpassableMovemenetDificulty = 1000f;

		// Token: 0x04004150 RID: 16720
		public const float WinterMovementDifficultyOffset = 2f;

		// Token: 0x04004151 RID: 16721
		public const float MaxTempForWinterOffset = 5f;
	}
}
