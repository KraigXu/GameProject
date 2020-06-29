using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	
	public class GlowFlooder
	{
		
		public GlowFlooder(Map map)
		{
			this.map = map;
			this.mapSizeX = map.Size.x;
			this.mapSizeZ = map.Size.z;
			this.calcGrid = new GlowFlooder.GlowFloodCell[this.mapSizeX * this.mapSizeZ];
			this.openSet = new FastPriorityQueue<int>(new GlowFlooder.CompareGlowFlooderLightSquares(this.calcGrid));
		}

		
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

		
		private Map map;

		
		private GlowFlooder.GlowFloodCell[] calcGrid;

		
		private FastPriorityQueue<int> openSet;

		
		private uint statusUnseenValue;

		
		private uint statusOpenValue = 1u;

		
		private uint statusFinalizedValue = 2u;

		
		private int mapSizeX;

		
		private int mapSizeZ;

		
		private CompGlower glower;

		
		private CellIndices cellIndices;

		
		private Color32[] glowGrid;

		
		private float attenLinearSlope;

		
		private Thing[] blockers = new Thing[8];

		
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

		
		private struct GlowFloodCell
		{
			
			public int intDist;

			
			public uint status;
		}

		
		private class CompareGlowFlooderLightSquares : IComparer<int>
		{
			
			public CompareGlowFlooderLightSquares(GlowFlooder.GlowFloodCell[] grid)
			{
				this.grid = grid;
			}

			
			public int Compare(int a, int b)
			{
				return this.grid[a].intDist.CompareTo(this.grid[b].intDist);
			}

			
			private GlowFlooder.GlowFloodCell[] grid;
		}
	}
}
