﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse.AI
{
	
	public class PawnPath : IDisposable
	{
		
		// (get) Token: 0x06002798 RID: 10136 RVA: 0x000E84C7 File Offset: 0x000E66C7
		public bool Found
		{
			get
			{
				return this.totalCostInt >= 0f;
			}
		}

		
		// (get) Token: 0x06002799 RID: 10137 RVA: 0x000E84D9 File Offset: 0x000E66D9
		public float TotalCost
		{
			get
			{
				return this.totalCostInt;
			}
		}

		
		// (get) Token: 0x0600279A RID: 10138 RVA: 0x000E84E1 File Offset: 0x000E66E1
		public int NodesLeftCount
		{
			get
			{
				return this.curNodeIndex + 1;
			}
		}

		
		// (get) Token: 0x0600279B RID: 10139 RVA: 0x000E84EB File Offset: 0x000E66EB
		public int NodesConsumedCount
		{
			get
			{
				return this.nodes.Count - this.NodesLeftCount;
			}
		}

		
		// (get) Token: 0x0600279C RID: 10140 RVA: 0x000E84FF File Offset: 0x000E66FF
		public bool UsedRegionHeuristics
		{
			get
			{
				return this.usedRegionHeuristics;
			}
		}

		
		// (get) Token: 0x0600279D RID: 10141 RVA: 0x000E8507 File Offset: 0x000E6707
		public List<IntVec3> NodesReversed
		{
			get
			{
				return this.nodes;
			}
		}

		
		// (get) Token: 0x0600279E RID: 10142 RVA: 0x000E850F File Offset: 0x000E670F
		public IntVec3 FirstNode
		{
			get
			{
				return this.nodes[this.nodes.Count - 1];
			}
		}

		
		// (get) Token: 0x0600279F RID: 10143 RVA: 0x000E8529 File Offset: 0x000E6729
		public IntVec3 LastNode
		{
			get
			{
				return this.nodes[0];
			}
		}

		
		// (get) Token: 0x060027A0 RID: 10144 RVA: 0x000E8537 File Offset: 0x000E6737
		public static PawnPath NotFound
		{
			get
			{
				return PawnPathPool.NotFoundPath;
			}
		}

		
		public void AddNode(IntVec3 nodePosition)
		{
			this.nodes.Add(nodePosition);
		}

		
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

		
		public void Dispose()
		{
			this.ReleaseToPool();
		}

		
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

		
		public static PawnPath NewNotFound()
		{
			return new PawnPath
			{
				totalCostInt = -1f
			};
		}

		
		public IntVec3 ConsumeNextNode()
		{
			IntVec3 result = this.Peek(1);
			this.curNodeIndex--;
			return result;
		}

		
		public IntVec3 Peek(int nodesAhead)
		{
			return this.nodes[this.curNodeIndex - nodesAhead];
		}

		
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

		
		private List<IntVec3> nodes = new List<IntVec3>(128);

		
		private float totalCostInt;

		
		private int curNodeIndex;

		
		private bool usedRegionHeuristics;

		
		public bool inUse;
	}
}
