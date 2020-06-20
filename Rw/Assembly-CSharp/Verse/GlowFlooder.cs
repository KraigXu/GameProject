using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000163 RID: 355
	public class GlowFlooder
	{
		// Token: 0x060009ED RID: 2541 RVA: 0x000360B8 File Offset: 0x000342B8
		public GlowFlooder(Map map)
		{
			this.map = map;
			this.mapSizeX = map.Size.x;
			this.mapSizeZ = map.Size.z;
			this.calcGrid = new GlowFlooder.GlowFloodCell[this.mapSizeX * this.mapSizeZ];
			this.openSet = new FastPriorityQueue<int>(new GlowFlooder.CompareGlowFlooderLightSquares(this.calcGrid));
		}

		// Token: 0x060009EE RID: 2542 RVA: 0x0003613C File Offset: 0x0003433C
		public void AddFloodGlowFor(CompGlower theGlower, Color32[] glowGrid)
		{
			this.cellIndices = this.map.cellIndices;
			this.glowGrid = glowGrid;
			this.glower = theGlower;
			this.attenLinearSlope = -1f / theGlower.Props.glowRadius;
			Building[] innerArray = this.map.edificeGrid.InnerArray;
			IntVec3 position = theGlower.parent.Position;
			int num = Mathf.RoundToInt(this.glower.Props.glowRadius * 100f);
			int num2 = this.cellIndices.CellToIndex(position);
			this.InitStatusesAndPushStartNode(ref num2, position);
			while (this.openSet.Count != 0)
			{
				num2 = this.openSet.Pop();
				IntVec3 intVec = this.cellIndices.IndexToCell(num2);
				this.calcGrid[num2].status = this.statusFinalizedValue;
				this.SetGlowGridFromDist(num2);
				for (int i = 0; i < 8; i++)
				{
					uint num3 = (uint)(intVec.x + (int)GlowFlooder.Directions[i, 0]);
					uint num4 = (uint)(intVec.z + (int)GlowFlooder.Directions[i, 1]);
					if ((ulong)num3 < (ulong)((long)this.mapSizeX) && (ulong)num4 < (ulong)((long)this.mapSizeZ))
					{
						int x = (int)num3;
						int z = (int)num4;
						int num5 = this.cellIndices.CellToIndex(x, z);
						if (this.calcGrid[num5].status != this.statusFinalizedValue)
						{
							this.blockers[i] = innerArray[num5];
							if (this.blockers[i] != null)
							{
								if (this.blockers[i].def.blockLight)
								{
									goto IL_2DE;
								}
								this.blockers[i] = null;
							}
							int num6;
							if (i < 4)
							{
								num6 = 100;
							}
							else
							{
								num6 = 141;
							}
							int num7 = this.calcGrid[num2].intDist + num6;
							if (num7 <= num)
							{
								if (i >= 4)
								{
									switch (i)
									{
									case 4:
										if (this.blockers[0] != null && this.blockers[1] != null)
										{
											goto IL_2DE;
										}
										break;
									case 5:
										if (this.blockers[1] != null && this.blockers[2] != null)
										{
											goto IL_2DE;
										}
										break;
									case 6:
										if (this.blockers[2] != null && this.blockers[3] != null)
										{
											goto IL_2DE;
										}
										break;
									case 7:
										if (this.blockers[0] != null && this.blockers[3] != null)
										{
											goto IL_2DE;
										}
										break;
									}
								}
								if (this.calcGrid[num5].status <= this.statusUnseenValue)
								{
									this.calcGrid[num5].intDist = 999999;
									this.calcGrid[num5].status = this.statusOpenValue;
								}
								if (num7 < this.calcGrid[num5].intDist)
								{
									this.calcGrid[num5].intDist = num7;
									this.calcGrid[num5].status = this.statusOpenValue;
									this.openSet.Push(num5);
								}
							}
						}
					}
					IL_2DE:;
				}
			}
		}

		// Token: 0x060009EF RID: 2543 RVA: 0x0003643C File Offset: 0x0003463C
		private void InitStatusesAndPushStartNode(ref int curIndex, IntVec3 start)
		{
			this.statusUnseenValue += 3u;
			this.statusOpenValue += 3u;
			this.statusFinalizedValue += 3u;
			curIndex = this.cellIndices.CellToIndex(start);
			this.openSet.Clear();
			this.calcGrid[curIndex].intDist = 100;
			this.openSet.Clear();
			this.openSet.Push(curIndex);
		}

		// Token: 0x060009F0 RID: 2544 RVA: 0x000364B8 File Offset: 0x000346B8
		private void SetGlowGridFromDist(int index)
		{
			float num = (float)this.calcGrid[index].intDist / 100f;
			ColorInt colorInt = default(ColorInt);
			if (num <= this.glower.Props.glowRadius)
			{
				float b = 1f / (num * num);
				float b2 = Mathf.Lerp(1f + this.attenLinearSlope * num, b, 0.4f);
				colorInt = this.glower.Props.glowColor * b2;
			}
			if (colorInt.r > 0 || colorInt.g > 0 || colorInt.b > 0)
			{
				colorInt.ClampToNonNegative();
				ColorInt colA = this.glowGrid[index].AsColorInt();
				colA += colorInt;
				if (num < this.glower.Props.overlightRadius)
				{
					colA.a = 1;
				}
				Color32 toColor = colA.ToColor32;
				this.glowGrid[index] = toColor;
			}
		}

		// Token: 0x0400080A RID: 2058
		private Map map;

		// Token: 0x0400080B RID: 2059
		private GlowFlooder.GlowFloodCell[] calcGrid;

		// Token: 0x0400080C RID: 2060
		private FastPriorityQueue<int> openSet;

		// Token: 0x0400080D RID: 2061
		private uint statusUnseenValue;

		// Token: 0x0400080E RID: 2062
		private uint statusOpenValue = 1u;

		// Token: 0x0400080F RID: 2063
		private uint statusFinalizedValue = 2u;

		// Token: 0x04000810 RID: 2064
		private int mapSizeX;

		// Token: 0x04000811 RID: 2065
		private int mapSizeZ;

		// Token: 0x04000812 RID: 2066
		private CompGlower glower;

		// Token: 0x04000813 RID: 2067
		private CellIndices cellIndices;

		// Token: 0x04000814 RID: 2068
		private Color32[] glowGrid;

		// Token: 0x04000815 RID: 2069
		private float attenLinearSlope;

		// Token: 0x04000816 RID: 2070
		private Thing[] blockers = new Thing[8];

		// Token: 0x04000817 RID: 2071
		private static readonly sbyte[,] Directions = new sbyte[,]
		{
			{
				0,
				-1
			},
			{
				1,
				0
			},
			{
				0,
				1
			},
			{
				-1,
				0
			},
			{
				1,
				-1
			},
			{
				1,
				1
			},
			{
				-1,
				1
			},
			{
				-1,
				-1
			}
		};

		// Token: 0x020013B7 RID: 5047
		private struct GlowFloodCell
		{
			// Token: 0x04004AF2 RID: 19186
			public int intDist;

			// Token: 0x04004AF3 RID: 19187
			public uint status;
		}

		// Token: 0x020013B8 RID: 5048
		private class CompareGlowFlooderLightSquares : IComparer<int>
		{
			// Token: 0x06007741 RID: 30529 RVA: 0x00290995 File Offset: 0x0028EB95
			public CompareGlowFlooderLightSquares(GlowFlooder.GlowFloodCell[] grid)
			{
				this.grid = grid;
			}

			// Token: 0x06007742 RID: 30530 RVA: 0x002909A4 File Offset: 0x0028EBA4
			public int Compare(int a, int b)
			{
				return this.grid[a].intDist.CompareTo(this.grid[b].intDist);
			}

			// Token: 0x04004AF4 RID: 19188
			private GlowFlooder.GlowFloodCell[] grid;
		}
	}
}
