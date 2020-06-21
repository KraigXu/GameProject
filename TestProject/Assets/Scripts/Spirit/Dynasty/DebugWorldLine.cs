using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020011E2 RID: 4578
	public class DebugWorldLine
	{
		// Token: 0x170011BA RID: 4538
		// (get) Token: 0x060069F7 RID: 27127 RVA: 0x0024FDD4 File Offset: 0x0024DFD4
		// (set) Token: 0x060069F8 RID: 27128 RVA: 0x0024FDDC File Offset: 0x0024DFDC
		public int TicksLeft
		{
			get
			{
				return this.ticksLeft;
			}
			set
			{
				this.ticksLeft = value;
			}
		}

		// Token: 0x060069F9 RID: 27129 RVA: 0x0024FDE5 File Offset: 0x0024DFE5
		public DebugWorldLine(Vector3 a, Vector3 b, bool onPlanetSurface)
		{
			this.a = a;
			this.b = b;
			this.onPlanetSurface = onPlanetSurface;
			this.ticksLeft = 100;
		}

		// Token: 0x060069FA RID: 27130 RVA: 0x0024FE0A File Offset: 0x0024E00A
		public DebugWorldLine(Vector3 a, Vector3 b, bool onPlanetSurface, int ticksLeft)
		{
			this.a = a;
			this.b = b;
			this.onPlanetSurface = onPlanetSurface;
			this.ticksLeft = ticksLeft;
		}

		// Token: 0x060069FB RID: 27131 RVA: 0x0024FE30 File Offset: 0x0024E030
		public void Draw()
		{
			float num = Vector3.Distance(this.a, this.b);
			if (num < 0.001f)
			{
				return;
			}
			if (this.onPlanetSurface)
			{
				float averageTileSize = Find.WorldGrid.averageTileSize;
				int num2 = Mathf.Max(Mathf.RoundToInt(num / averageTileSize), 0);
				float num3 = 0.05f;
				for (int i = 0; i < num2; i++)
				{
					Vector3 vector = Vector3.Lerp(this.a, this.b, (float)i / (float)num2);
					Vector3 vector2 = Vector3.Lerp(this.a, this.b, (float)(i + 1) / (float)num2);
					vector = vector.normalized * (100f + num3);
					vector2 = vector2.normalized * (100f + num3);
					GenDraw.DrawWorldLineBetween(vector, vector2);
				}
				return;
			}
			GenDraw.DrawWorldLineBetween(this.a, this.b);
		}

		// Token: 0x0400420A RID: 16906
		public Vector3 a;

		// Token: 0x0400420B RID: 16907
		public Vector3 b;

		// Token: 0x0400420C RID: 16908
		public int ticksLeft;

		// Token: 0x0400420D RID: 16909
		private bool onPlanetSurface;
	}
}
