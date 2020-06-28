using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020011B8 RID: 4536
	public class WorldPath : IDisposable
	{
		// Token: 0x1700116E RID: 4462
		// (get) Token: 0x060068D0 RID: 26832 RVA: 0x00249A85 File Offset: 0x00247C85
		public bool Found
		{
			get
			{
				return this.totalCostInt >= 0f;
			}
		}

		// Token: 0x1700116F RID: 4463
		// (get) Token: 0x060068D1 RID: 26833 RVA: 0x00249A97 File Offset: 0x00247C97
		public float TotalCost
		{
			get
			{
				return this.totalCostInt;
			}
		}

		// Token: 0x17001170 RID: 4464
		// (get) Token: 0x060068D2 RID: 26834 RVA: 0x00249A9F File Offset: 0x00247C9F
		public int NodesLeftCount
		{
			get
			{
				return this.curNodeIndex + 1;
			}
		}

		// Token: 0x17001171 RID: 4465
		// (get) Token: 0x060068D3 RID: 26835 RVA: 0x00249AA9 File Offset: 0x00247CA9
		public List<int> NodesReversed
		{
			get
			{
				return this.nodes;
			}
		}

		// Token: 0x17001172 RID: 4466
		// (get) Token: 0x060068D4 RID: 26836 RVA: 0x00249AB1 File Offset: 0x00247CB1
		public int FirstNode
		{
			get
			{
				return this.nodes[this.nodes.Count - 1];
			}
		}

		// Token: 0x17001173 RID: 4467
		// (get) Token: 0x060068D5 RID: 26837 RVA: 0x00249ACB File Offset: 0x00247CCB
		public int LastNode
		{
			get
			{
				return this.nodes[0];
			}
		}

		// Token: 0x17001174 RID: 4468
		// (get) Token: 0x060068D6 RID: 26838 RVA: 0x00249AD9 File Offset: 0x00247CD9
		public static WorldPath NotFound
		{
			get
			{
				return WorldPathPool.NotFoundPath;
			}
		}

		// Token: 0x060068D7 RID: 26839 RVA: 0x00249AE0 File Offset: 0x00247CE0
		public void AddNodeAtStart(int tile)
		{
			this.nodes.Add(tile);
		}

		// Token: 0x060068D8 RID: 26840 RVA: 0x00249AEE File Offset: 0x00247CEE
		public void SetupFound(float totalCost)
		{
			if (this == WorldPath.NotFound)
			{
				Log.Warning("Calling SetupFound with totalCost=" + totalCost + " on WorldPath.NotFound", false);
				return;
			}
			this.totalCostInt = totalCost;
			this.curNodeIndex = this.nodes.Count - 1;
		}

		// Token: 0x060068D9 RID: 26841 RVA: 0x00249B2E File Offset: 0x00247D2E
		public void Dispose()
		{
			this.ReleaseToPool();
		}

		// Token: 0x060068DA RID: 26842 RVA: 0x00249B36 File Offset: 0x00247D36
		public void ReleaseToPool()
		{
			if (this != WorldPath.NotFound)
			{
				this.totalCostInt = 0f;
				this.nodes.Clear();
				this.inUse = false;
			}
		}

		// Token: 0x060068DB RID: 26843 RVA: 0x00249B5D File Offset: 0x00247D5D
		public static WorldPath NewNotFound()
		{
			return new WorldPath
			{
				totalCostInt = -1f
			};
		}

		// Token: 0x060068DC RID: 26844 RVA: 0x00249B6F File Offset: 0x00247D6F
		public int ConsumeNextNode()
		{
			int result = this.Peek(1);
			this.curNodeIndex--;
			return result;
		}

		// Token: 0x060068DD RID: 26845 RVA: 0x00249B86 File Offset: 0x00247D86
		public int Peek(int nodesAhead)
		{
			return this.nodes[this.curNodeIndex - nodesAhead];
		}

		// Token: 0x060068DE RID: 26846 RVA: 0x00249B9C File Offset: 0x00247D9C
		public override string ToString()
		{
			if (!this.Found)
			{
				return "WorldPath(not found)";
			}
			if (!this.inUse)
			{
				return "WorldPath(not in use)";
			}
			return string.Concat(new object[]
			{
				"WorldPath(nodeCount= ",
				this.nodes.Count,
				(this.nodes.Count > 0) ? string.Concat(new object[]
				{
					" first=",
					this.FirstNode,
					" last=",
					this.LastNode
				}) : "",
				" cost=",
				this.totalCostInt,
				" )"
			});
		}

		// Token: 0x060068DF RID: 26847 RVA: 0x00249C58 File Offset: 0x00247E58
		public void DrawPath(Caravan pathingCaravan)
		{
			if (!this.Found)
			{
				return;
			}
			if (this.NodesLeftCount > 0)
			{
				WorldGrid worldGrid = Find.WorldGrid;
				float d = 0.05f;
				for (int i = 0; i < this.NodesLeftCount - 1; i++)
				{
					Vector3 a = worldGrid.GetTileCenter(this.Peek(i));
					Vector3 vector = worldGrid.GetTileCenter(this.Peek(i + 1));
					a += a.normalized * d;
					vector += vector.normalized * d;
					GenDraw.DrawWorldLineBetween(a, vector);
				}
				if (pathingCaravan != null)
				{
					Vector3 a2 = pathingCaravan.DrawPos;
					Vector3 vector2 = worldGrid.GetTileCenter(this.Peek(0));
					a2 += a2.normalized * d;
					vector2 += vector2.normalized * d;
					if ((a2 - vector2).sqrMagnitude > 0.005f)
					{
						GenDraw.DrawWorldLineBetween(a2, vector2);
					}
				}
			}
		}

		// Token: 0x04004142 RID: 16706
		private List<int> nodes = new List<int>(128);

		// Token: 0x04004143 RID: 16707
		private float totalCostInt;

		// Token: 0x04004144 RID: 16708
		private int curNodeIndex;

		// Token: 0x04004145 RID: 16709
		public bool inUse;
	}
}
