using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020008F8 RID: 2296
	public class RoadDef : Def
	{
		// Token: 0x060036D4 RID: 14036 RVA: 0x00128488 File Offset: 0x00126688
		public float GetLayerWidth(RoadWorldLayerDef def)
		{
			if (this.cachedLayerWidth == null)
			{
				this.cachedLayerWidth = new float[DefDatabase<RoadWorldLayerDef>.DefCount];
				for (int i = 0; i < DefDatabase<RoadWorldLayerDef>.DefCount; i++)
				{
					RoadWorldLayerDef roadWorldLayerDef = DefDatabase<RoadWorldLayerDef>.AllDefsListForReading[i];
					if (this.worldRenderSteps != null)
					{
						foreach (RoadDef.WorldRenderStep worldRenderStep in this.worldRenderSteps)
						{
							if (worldRenderStep.layer == roadWorldLayerDef)
							{
								this.cachedLayerWidth[(int)roadWorldLayerDef.index] = worldRenderStep.width;
							}
						}
					}
				}
			}
			return this.cachedLayerWidth[(int)def.index];
		}

		// Token: 0x060036D5 RID: 14037 RVA: 0x0012853C File Offset: 0x0012673C
		public override void ClearCachedData()
		{
			base.ClearCachedData();
			this.cachedLayerWidth = null;
		}

		// Token: 0x04001F78 RID: 8056
		public int priority;

		// Token: 0x04001F79 RID: 8057
		public bool ancientOnly;

		// Token: 0x04001F7A RID: 8058
		public float movementCostMultiplier = 1f;

		// Token: 0x04001F7B RID: 8059
		public int tilesPerSegment = 15;

		// Token: 0x04001F7C RID: 8060
		public RoadPathingDef pathingMode;

		// Token: 0x04001F7D RID: 8061
		public List<RoadDefGenStep> roadGenSteps;

		// Token: 0x04001F7E RID: 8062
		public List<RoadDef.WorldRenderStep> worldRenderSteps;

		// Token: 0x04001F7F RID: 8063
		[NoTranslate]
		public string worldTransitionGroup = "";

		// Token: 0x04001F80 RID: 8064
		public float distortionFrequency = 1f;

		// Token: 0x04001F81 RID: 8065
		public float distortionIntensity;

		// Token: 0x04001F82 RID: 8066
		[Unsaved(false)]
		private float[] cachedLayerWidth;

		// Token: 0x0200192B RID: 6443
		public class WorldRenderStep
		{
			// Token: 0x04005FD1 RID: 24529
			public RoadWorldLayerDef layer;

			// Token: 0x04005FD2 RID: 24530
			public float width;
		}
	}
}
