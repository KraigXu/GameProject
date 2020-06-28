using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000068 RID: 104
	public class CameraSwooper
	{
		// Token: 0x06000428 RID: 1064 RVA: 0x000157EE File Offset: 0x000139EE
		public void StartSwoopFromRoot(Vector3 FinalOffset, float FinalOrthoSizeOffset, float TotalSwoopTime, SwoopCallbackMethod SwoopFinishedCallback)
		{
			this.Swooping = true;
			this.TimeSinceSwoopStart = 0f;
			this.FinalOffset = FinalOffset;
			this.FinalOrthoSizeOffset = FinalOrthoSizeOffset;
			this.TotalSwoopTime = TotalSwoopTime;
			this.SwoopFinishedCallback = SwoopFinishedCallback;
			this.SwoopingTo = false;
		}

		// Token: 0x06000429 RID: 1065 RVA: 0x00015826 File Offset: 0x00013A26
		public void StartSwoopToRoot(Vector3 FinalOffset, float FinalOrthoSizeOffset, float TotalSwoopTime, SwoopCallbackMethod SwoopFinishedCallback)
		{
			this.StartSwoopFromRoot(FinalOffset, FinalOrthoSizeOffset, TotalSwoopTime, SwoopFinishedCallback);
			this.SwoopingTo = true;
		}

		// Token: 0x0600042A RID: 1066 RVA: 0x0001583C File Offset: 0x00013A3C
		public void Update()
		{
			if (this.Swooping)
			{
				this.TimeSinceSwoopStart += Time.deltaTime;
				if (this.TimeSinceSwoopStart >= this.TotalSwoopTime)
				{
					this.Swooping = false;
					if (this.SwoopFinishedCallback != null)
					{
						this.SwoopFinishedCallback();
					}
				}
			}
		}

		// Token: 0x0600042B RID: 1067 RVA: 0x0001588C File Offset: 0x00013A8C
		public void OffsetCameraFrom(GameObject camObj, Vector3 basePos, float baseSize)
		{
			float num = this.TimeSinceSwoopStart / this.TotalSwoopTime;
			if (!this.Swooping)
			{
				num = 0f;
			}
			else
			{
				num = this.TimeSinceSwoopStart / this.TotalSwoopTime;
				if (this.SwoopingTo)
				{
					num = 1f - num;
				}
				num = (float)Math.Pow((double)num, 1.7000000476837158);
			}
			camObj.transform.position = basePos + this.FinalOffset * num;
			Find.Camera.orthographicSize = baseSize + this.FinalOrthoSizeOffset * num;
		}

		// Token: 0x0400015D RID: 349
		public bool Swooping;

		// Token: 0x0400015E RID: 350
		private bool SwoopingTo;

		// Token: 0x0400015F RID: 351
		private float TimeSinceSwoopStart;

		// Token: 0x04000160 RID: 352
		private Vector3 FinalOffset;

		// Token: 0x04000161 RID: 353
		private float FinalOrthoSizeOffset;

		// Token: 0x04000162 RID: 354
		private float TotalSwoopTime;

		// Token: 0x04000163 RID: 355
		private SwoopCallbackMethod SwoopFinishedCallback;
	}
}
