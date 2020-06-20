using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000307 RID: 775
	public class MoteSplash : Mote
	{
		// Token: 0x17000470 RID: 1136
		// (get) Token: 0x060015CB RID: 5579 RVA: 0x0007EDA9 File Offset: 0x0007CFA9
		protected override bool EndOfLife
		{
			get
			{
				return base.AgeSecs >= this.targetSize / this.velocity;
			}
		}

		// Token: 0x17000471 RID: 1137
		// (get) Token: 0x060015CC RID: 5580 RVA: 0x0007EDC4 File Offset: 0x0007CFC4
		public override float Alpha
		{
			get
			{
				Mathf.Clamp01(base.AgeSecs * 10f);
				float num = 1f;
				float num2 = Mathf.Clamp01(1f - base.AgeSecs / (this.targetSize / this.velocity));
				return num * num2 * this.CalculatedIntensity();
			}
		}

		// Token: 0x060015CD RID: 5581 RVA: 0x0007EE11 File Offset: 0x0007D011
		public void Initialize(Vector3 position, float size, float velocity)
		{
			this.exactPosition = position;
			this.targetSize = size;
			this.velocity = velocity;
			base.Scale = 0f;
		}

		// Token: 0x060015CE RID: 5582 RVA: 0x0007EE34 File Offset: 0x0007D034
		protected override void TimeInterval(float deltaTime)
		{
			base.TimeInterval(deltaTime);
			if (base.Destroyed)
			{
				return;
			}
			float scale = base.AgeSecs * this.velocity;
			base.Scale = scale;
			this.exactPosition += base.Map.waterInfo.GetWaterMovement(this.exactPosition) * deltaTime;
		}

		// Token: 0x060015CF RID: 5583 RVA: 0x0007EE93 File Offset: 0x0007D093
		public float CalculatedIntensity()
		{
			return Mathf.Sqrt(this.targetSize) / 10f;
		}

		// Token: 0x060015D0 RID: 5584 RVA: 0x0007EEA6 File Offset: 0x0007D0A6
		public float CalculatedShockwaveSpan()
		{
			return Mathf.Min(Mathf.Sqrt(this.targetSize) * 0.8f, this.exactScale.x) / this.exactScale.x;
		}

		// Token: 0x04000E3F RID: 3647
		public const float VelocityFootstep = 1.5f;

		// Token: 0x04000E40 RID: 3648
		public const float SizeFootstep = 2f;

		// Token: 0x04000E41 RID: 3649
		public const float VelocityGunfire = 4f;

		// Token: 0x04000E42 RID: 3650
		public const float SizeGunfire = 1f;

		// Token: 0x04000E43 RID: 3651
		public const float VelocityExplosion = 20f;

		// Token: 0x04000E44 RID: 3652
		public const float SizeExplosion = 6f;

		// Token: 0x04000E45 RID: 3653
		private float targetSize;

		// Token: 0x04000E46 RID: 3654
		private float velocity;
	}
}
