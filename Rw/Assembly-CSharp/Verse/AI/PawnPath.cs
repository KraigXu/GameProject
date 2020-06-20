using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse.AI
{
	// Token: 0x02000577 RID: 1399
	public class PawnPath : IDisposable
	{
		// Token: 0x1700079E RID: 1950
		// (get) Token: 0x06002798 RID: 10136 RVA: 0x000E84C7 File Offset: 0x000E66C7
		public bool Found
		{
			get
			{
				return this.totalCostInt >= 0f;
			}
		}

		// Token: 0x1700079F RID: 1951
		// (get) Token: 0x06002799 RID: 10137 RVA: 0x000E84D9 File Offset: 0x000E66D9
		public float TotalCost
		{
			get
			{
				return this.totalCostInt;
			}
		}

		// Token: 0x170007A0 RID: 1952
		// (get) Token: 0x0600279A RID: 10138 RVA: 0x000E84E1 File Offset: 0x000E66E1
		public int NodesLeftCount
		{
			get
			{
				return this.curNodeIndex + 1;
			}
		}

		// Token: 0x170007A1 RID: 1953
		// (get) Token: 0x0600279B RID: 10139 RVA: 0x000E84EB File Offset: 0x000E66EB
		public int NodesConsumedCount
		{
			get
			{
				return this.nodes.Count - this.NodesLeftCount;
			}
		}

		// Token: 0x170007A2 RID: 1954
		// (get) Token: 0x0600279C RID: 10140 RVA: 0x000E84FF File Offset: 0x000E66FF
		public bool UsedRegionHeuristics
		{
			get
			{
				return this.usedRegionHeuristics;
			}
		}

		// Token: 0x170007A3 RID: 1955
		// (get) Token: 0x0600279D RID: 10141 RVA: 0x000E8507 File Offset: 0x000E6707
		public List<IntVec3> NodesReversed
		{
			get
			{
				return this.nodes;
			}
		}

		// Token: 0x170007A4 RID: 1956
		// (get) Token: 0x0600279E RID: 10142 RVA: 0x000E850F File Offset: 0x000E670F
		public IntVec3 FirstNode
		{
			get
			{
				return this.nodes[this.nodes.Count - 1];
			}
		}

		// Token: 0x170007A5 RID: 1957
		// (get) Token: 0x0600279F RID: 10143 RVA: 0x000E8529 File Offset: 0x000E6729
		public IntVec3 LastNode
		{
			get
			{
				return this.nodes[0];
			}
		}

		// Token: 0x170007A6 RID: 1958
		// (get) Token: 0x060027A0 RID: 10144 RVA: 0x000E8537 File Offset: 0x000E6737
		public static PawnPath NotFound
		{
			get
			{
				return PawnPathPool.NotFoundPath;
			}
		}

		// Token: 0x060027A1 RID: 10145 RVA: 0x000E853E File Offset: 0x000E673E
		public void AddNode(IntVec3 nodePosition)
		{
			this.nodes.Add(nodePosition);
		}

		// Token: 0x060027A2 RID: 10146 RVA: 0x000E854C File Offset: 0x000E674C
		public void SetupFound(float totalCost, bool usedRegionHeuristics)
		{
			if (this == PawnPath.NotFound)
			{
				Log.Warning("Calling SetupFound with totalCost=" + totalCost + " on PawnPath.NotFound", false);
				return;
			}
			this.totalCostInt = totalCost;
			this.usedRegionHeuristics = usedRegionHeuristics;
			this.curNodeIndex = this.nodes.Count - 1;
		}

		// Token: 0x060027A3 RID: 10147 RVA: 0x000E859E File Offset: 0x000E679E
		public void Dispose()
		{
			this.ReleaseToPool();
		}

		// Token: 0x060027A4 RID: 10148 RVA: 0x000E85A6 File Offset: 0x000E67A6
		public void ReleaseToPool()
		{
			if (this != PawnPath.NotFound)
			{
				this.totalCostInt = 0f;
				this.usedRegionHeuristics = false;
				this.nodes.Clear();
				this.inUse = false;
			}
		}

		// Token: 0x060027A5 RID: 10149 RVA: 0x000E85D4 File Offset: 0x000E67D4
		public static PawnPath NewNotFound()
		{
			return new PawnPath
			{
				totalCostInt = -1f
			};
		}

		// Token: 0x060027A6 RID: 10150 RVA: 0x000E85E6 File Offset: 0x000E67E6
		public IntVec3 ConsumeNextNode()
		{
			IntVec3 result = this.Peek(1);
			this.curNodeIndex--;
			return result;
		}

		// Token: 0x060027A7 RID: 10151 RVA: 0x000E85FD File Offset: 0x000E67FD
		public IntVec3 Peek(int nodesAhead)
		{
			return this.nodes[this.curNodeIndex - nodesAhead];
		}

		// Token: 0x060027A8 RID: 10152 RVA: 0x000E8614 File Offset: 0x000E6814
		public override string ToString()
		{
			if (!this.Found)
			{
				return "PawnPath(not found)";
			}
			if (!this.inUse)
			{
				return "PawnPath(not in use)";
			}
			return string.Concat(new object[]
			{
				"PawnPath(nodeCount= ",
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

		// Token: 0x060027A9 RID: 10153 RVA: 0x000E86D0 File Offset: 0x000E68D0
		public void DrawPath(Pawn pathingPawn)
		{
			if (!this.Found)
			{
				return;
			}
			float y = AltitudeLayer.Item.AltitudeFor();
			if (this.NodesLeftCount > 0)
			{
				for (int i = 0; i < this.NodesLeftCount - 1; i++)
				{
					Vector3 a = this.Peek(i).ToVector3Shifted();
					a.y = y;
					Vector3 b = this.Peek(i + 1).ToVector3Shifted();
					b.y = y;
					GenDraw.DrawLineBetween(a, b);
				}
				if (pathingPawn != null)
				{
					Vector3 drawPos = pathingPawn.DrawPos;
					drawPos.y = y;
					Vector3 b2 = this.Peek(0).ToVector3Shifted();
					b2.y = y;
					if ((drawPos - b2).sqrMagnitude > 0.01f)
					{
						GenDraw.DrawLineBetween(drawPos, b2);
					}
				}
			}
		}

		// Token: 0x040017A9 RID: 6057
		private List<IntVec3> nodes = new List<IntVec3>(128);

		// Token: 0x040017AA RID: 6058
		private float totalCostInt;

		// Token: 0x040017AB RID: 6059
		private int curNodeIndex;

		// Token: 0x040017AC RID: 6060
		private bool usedRegionHeuristics;

		// Token: 0x040017AD RID: 6061
		public bool inUse;
	}
}
