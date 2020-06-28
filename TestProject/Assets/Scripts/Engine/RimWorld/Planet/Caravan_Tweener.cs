using System;
using UnityEngine;

namespace RimWorld.Planet
{
	// Token: 0x02001229 RID: 4649
	public class Caravan_Tweener
	{
		// Token: 0x17001233 RID: 4659
		// (get) Token: 0x06006C40 RID: 27712 RVA: 0x0025BAA8 File Offset: 0x00259CA8
		public Vector3 TweenedPos
		{
			get
			{
				return this.tweenedPos;
			}
		}

		// Token: 0x17001234 RID: 4660
		// (get) Token: 0x06006C41 RID: 27713 RVA: 0x0025BAB0 File Offset: 0x00259CB0
		public Vector3 LastTickTweenedVelocity
		{
			get
			{
				return this.TweenedPos - this.lastTickSpringPos;
			}
		}

		// Token: 0x17001235 RID: 4661
		// (get) Token: 0x06006C42 RID: 27714 RVA: 0x0025BAC3 File Offset: 0x00259CC3
		public Vector3 TweenedPosRoot
		{
			get
			{
				return CaravanTweenerUtility.PatherTweenedPosRoot(this.caravan) + CaravanTweenerUtility.CaravanCollisionPosOffsetFor(this.caravan);
			}
		}

		// Token: 0x06006C43 RID: 27715 RVA: 0x0025BAE0 File Offset: 0x00259CE0
		public Caravan_Tweener(Caravan caravan)
		{
			this.caravan = caravan;
		}

		// Token: 0x06006C44 RID: 27716 RVA: 0x0025BAFC File Offset: 0x00259CFC
		public void TweenerTick()
		{
			this.lastTickSpringPos = this.tweenedPos;
			Vector3 a = this.TweenedPosRoot - this.tweenedPos;
			this.tweenedPos += a * 0.09f;
		}

		// Token: 0x06006C45 RID: 27717 RVA: 0x0025BB43 File Offset: 0x00259D43
		public void ResetTweenedPosToRoot()
		{
			this.tweenedPos = this.TweenedPosRoot;
			this.lastTickSpringPos = this.tweenedPos;
		}

		// Token: 0x0400436C RID: 17260
		private Caravan caravan;

		// Token: 0x0400436D RID: 17261
		private Vector3 tweenedPos = Vector3.zero;

		// Token: 0x0400436E RID: 17262
		private Vector3 lastTickSpringPos;

		// Token: 0x0400436F RID: 17263
		private const float SpringTightness = 0.09f;
	}
}
