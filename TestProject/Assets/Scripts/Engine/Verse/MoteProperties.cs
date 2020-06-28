using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000093 RID: 147
	public class MoteProperties
	{
		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x060004E0 RID: 1248 RVA: 0x00018BF0 File Offset: 0x00016DF0
		public float Lifespan
		{
			get
			{
				return this.fadeInTime + this.solidTime + this.fadeOutTime;
			}
		}

		// Token: 0x04000277 RID: 631
		public bool realTime;

		// Token: 0x04000278 RID: 632
		public float fadeInTime;

		// Token: 0x04000279 RID: 633
		public float solidTime = 1f;

		// Token: 0x0400027A RID: 634
		public float fadeOutTime;

		// Token: 0x0400027B RID: 635
		public Vector3 acceleration = Vector3.zero;

		// Token: 0x0400027C RID: 636
		public float speedPerTime;

		// Token: 0x0400027D RID: 637
		public float growthRate;

		// Token: 0x0400027E RID: 638
		public bool collide;

		// Token: 0x0400027F RID: 639
		public SoundDef landSound;

		// Token: 0x04000280 RID: 640
		public Vector3 attachedDrawOffset;

		// Token: 0x04000281 RID: 641
		public bool needsMaintenance;

		// Token: 0x04000282 RID: 642
		public bool rotateTowardsTarget;

		// Token: 0x04000283 RID: 643
		public bool rotateTowardsMoveDirection;

		// Token: 0x04000284 RID: 644
		public bool scaleToConnectTargets;

		// Token: 0x04000285 RID: 645
		public bool attachedToHead;
	}
}
