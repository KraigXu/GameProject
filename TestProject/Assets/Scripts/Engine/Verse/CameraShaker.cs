using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000066 RID: 102
	public class CameraShaker
	{
		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x0600041D RID: 1053 RVA: 0x00015706 File Offset: 0x00013906
		// (set) Token: 0x0600041E RID: 1054 RVA: 0x0001570E File Offset: 0x0001390E
		public float CurShakeMag
		{
			get
			{
				return this.curShakeMag;
			}
			set
			{
				this.curShakeMag = Mathf.Clamp(value, 0f, 0.2f);
			}
		}

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x0600041F RID: 1055 RVA: 0x00015728 File Offset: 0x00013928
		public Vector3 ShakeOffset
		{
			get
			{
				float x = Mathf.Sin(Time.realtimeSinceStartup * 24f) * this.curShakeMag;
				float y = Mathf.Sin(Time.realtimeSinceStartup * 24f * 1.05f) * this.curShakeMag;
				float z = Mathf.Sin(Time.realtimeSinceStartup * 24f * 1.1f) * this.curShakeMag;
				return new Vector3(x, y, z);
			}
		}

		// Token: 0x06000420 RID: 1056 RVA: 0x0001578F File Offset: 0x0001398F
		public void DoShake(float mag)
		{
			if (mag <= 0f)
			{
				return;
			}
			this.CurShakeMag += mag;
		}

		// Token: 0x06000421 RID: 1057 RVA: 0x000157A8 File Offset: 0x000139A8
		public void SetMinShake(float mag)
		{
			this.CurShakeMag = Mathf.Max(this.CurShakeMag, mag);
		}

		// Token: 0x06000422 RID: 1058 RVA: 0x000157BC File Offset: 0x000139BC
		public void Update()
		{
			this.curShakeMag -= 0.5f * RealTime.realDeltaTime;
			if (this.curShakeMag < 0f)
			{
				this.curShakeMag = 0f;
			}
		}

		// Token: 0x04000159 RID: 345
		private float curShakeMag;

		// Token: 0x0400015A RID: 346
		private const float ShakeDecayRate = 0.5f;

		// Token: 0x0400015B RID: 347
		private const float ShakeFrequency = 24f;

		// Token: 0x0400015C RID: 348
		private const float MaxShakeMag = 0.2f;
	}
}
