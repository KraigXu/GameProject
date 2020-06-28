using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200008C RID: 140
	public class DamageGraphicData
	{
		// Token: 0x060004C7 RID: 1223 RVA: 0x00017E85 File Offset: 0x00016085
		public void ResolveReferencesSpecial()
		{
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				if (this.scratches != null)
				{
					this.scratchMats = new List<Material>();
					for (int i = 0; i < this.scratches.Count; i++)
					{
						this.scratchMats[i] = MaterialPool.MatFrom(this.scratches[i], ShaderDatabase.Transparent);
					}
				}
				if (this.cornerTL != null)
				{
					this.cornerTLMat = MaterialPool.MatFrom(this.cornerTL, ShaderDatabase.Transparent);
				}
				if (this.cornerTR != null)
				{
					this.cornerTRMat = MaterialPool.MatFrom(this.cornerTR, ShaderDatabase.Transparent);
				}
				if (this.cornerBL != null)
				{
					this.cornerBLMat = MaterialPool.MatFrom(this.cornerBL, ShaderDatabase.Transparent);
				}
				if (this.cornerBR != null)
				{
					this.cornerBRMat = MaterialPool.MatFrom(this.cornerBR, ShaderDatabase.Transparent);
				}
				if (this.edgeTop != null)
				{
					this.edgeTopMat = MaterialPool.MatFrom(this.edgeTop, ShaderDatabase.Transparent);
				}
				if (this.edgeBot != null)
				{
					this.edgeBotMat = MaterialPool.MatFrom(this.edgeBot, ShaderDatabase.Transparent);
				}
				if (this.edgeLeft != null)
				{
					this.edgeLeftMat = MaterialPool.MatFrom(this.edgeLeft, ShaderDatabase.Transparent);
				}
				if (this.edgeRight != null)
				{
					this.edgeRightMat = MaterialPool.MatFrom(this.edgeRight, ShaderDatabase.Transparent);
				}
			});
		}

		// Token: 0x04000226 RID: 550
		public bool enabled = true;

		// Token: 0x04000227 RID: 551
		public Rect rectN;

		// Token: 0x04000228 RID: 552
		public Rect rectE;

		// Token: 0x04000229 RID: 553
		public Rect rectS;

		// Token: 0x0400022A RID: 554
		public Rect rectW;

		// Token: 0x0400022B RID: 555
		public Rect rect;

		// Token: 0x0400022C RID: 556
		[NoTranslate]
		public List<string> scratches;

		// Token: 0x0400022D RID: 557
		[NoTranslate]
		public string cornerTL;

		// Token: 0x0400022E RID: 558
		[NoTranslate]
		public string cornerTR;

		// Token: 0x0400022F RID: 559
		[NoTranslate]
		public string cornerBL;

		// Token: 0x04000230 RID: 560
		[NoTranslate]
		public string cornerBR;

		// Token: 0x04000231 RID: 561
		[NoTranslate]
		public string edgeLeft;

		// Token: 0x04000232 RID: 562
		[NoTranslate]
		public string edgeRight;

		// Token: 0x04000233 RID: 563
		[NoTranslate]
		public string edgeTop;

		// Token: 0x04000234 RID: 564
		[NoTranslate]
		public string edgeBot;

		// Token: 0x04000235 RID: 565
		[Unsaved(false)]
		public List<Material> scratchMats;

		// Token: 0x04000236 RID: 566
		[Unsaved(false)]
		public Material cornerTLMat;

		// Token: 0x04000237 RID: 567
		[Unsaved(false)]
		public Material cornerTRMat;

		// Token: 0x04000238 RID: 568
		[Unsaved(false)]
		public Material cornerBLMat;

		// Token: 0x04000239 RID: 569
		[Unsaved(false)]
		public Material cornerBRMat;

		// Token: 0x0400023A RID: 570
		[Unsaved(false)]
		public Material edgeLeftMat;

		// Token: 0x0400023B RID: 571
		[Unsaved(false)]
		public Material edgeRightMat;

		// Token: 0x0400023C RID: 572
		[Unsaved(false)]
		public Material edgeTopMat;

		// Token: 0x0400023D RID: 573
		[Unsaved(false)]
		public Material edgeBotMat;
	}
}
