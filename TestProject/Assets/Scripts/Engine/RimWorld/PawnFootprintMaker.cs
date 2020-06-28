using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000AEB RID: 2795
	public class PawnFootprintMaker
	{
		// Token: 0x0600420B RID: 16907 RVA: 0x00160E0F File Offset: 0x0015F00F
		public PawnFootprintMaker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x0600420C RID: 16908 RVA: 0x00160E20 File Offset: 0x0015F020
		public void FootprintMakerTick()
		{
			if (!this.pawn.RaceProps.makesFootprints)
			{
				TerrainDef terrain = this.pawn.Position.GetTerrain(this.pawn.Map);
				if (terrain == null || !terrain.takeSplashes)
				{
					return;
				}
			}
			if ((this.pawn.Drawer.DrawPos - this.lastFootprintPlacePos).MagnitudeHorizontalSquared() > 0.399424046f)
			{
				this.TryPlaceFootprint();
			}
		}

		// Token: 0x0600420D RID: 16909 RVA: 0x00160E94 File Offset: 0x0015F094
		private void TryPlaceFootprint()
		{
			Vector3 drawPos = this.pawn.Drawer.DrawPos;
			Vector3 normalized = (drawPos - this.lastFootprintPlacePos).normalized;
			float rot = normalized.AngleFlat();
			float angle = (float)(this.lastFootprintRight ? 90 : -90);
			Vector3 b = normalized.RotatedBy(angle) * 0.17f * Mathf.Sqrt(this.pawn.BodySize);
			Vector3 vector = drawPos + PawnFootprintMaker.FootprintOffset + b;
			IntVec3 c = vector.ToIntVec3();
			if (c.InBounds(this.pawn.Map))
			{
				TerrainDef terrain = c.GetTerrain(this.pawn.Map);
				if (terrain != null)
				{
					if (terrain.takeSplashes)
					{
						MoteMaker.MakeWaterSplash(vector, this.pawn.Map, Mathf.Sqrt(this.pawn.BodySize) * 2f, 1.5f);
					}
					if (this.pawn.RaceProps.makesFootprints && terrain.takeFootprints && this.pawn.Map.snowGrid.GetDepth(this.pawn.Position) >= 0.4f)
					{
						MoteMaker.PlaceFootprint(vector, this.pawn.Map, rot);
					}
				}
			}
			this.lastFootprintPlacePos = drawPos;
			this.lastFootprintRight = !this.lastFootprintRight;
		}

		// Token: 0x04002628 RID: 9768
		private Pawn pawn;

		// Token: 0x04002629 RID: 9769
		private Vector3 lastFootprintPlacePos;

		// Token: 0x0400262A RID: 9770
		private bool lastFootprintRight;

		// Token: 0x0400262B RID: 9771
		private const float FootprintIntervalDist = 0.632f;

		// Token: 0x0400262C RID: 9772
		private static readonly Vector3 FootprintOffset = new Vector3(0f, 0f, -0.3f);

		// Token: 0x0400262D RID: 9773
		private const float LeftRightOffsetDist = 0.17f;

		// Token: 0x0400262E RID: 9774
		private const float FootprintSplashSize = 2f;
	}
}
