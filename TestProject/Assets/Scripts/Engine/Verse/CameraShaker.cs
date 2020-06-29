using System;
using UnityEngine;

namespace Verse
{
	
	public class CameraShaker
	{
		
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

		
		public void DoShake(float mag)
		{
			if (mag <= 0f)
			{
				return;
			}
			this.CurShakeMag += mag;
		}

		
		public void SetMinShake(float mag)
		{
			this.CurShakeMag = Mathf.Max(this.CurShakeMag, mag);
		}

		
		public void Update()
		{
			this.curShakeMag -= 0.5f * RealTime.realDeltaTime;
			if (this.curShakeMag < 0f)
			{
				this.curShakeMag = 0f;
			}
		}

		
		private float curShakeMag;

		
		private const float ShakeDecayRate = 0.5f;

		
		private const float ShakeFrequency = 24f;

		
		private const float MaxShakeMag = 0.2f;
	}
}
