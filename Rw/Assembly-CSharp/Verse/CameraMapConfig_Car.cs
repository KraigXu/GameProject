using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000061 RID: 97
	public class CameraMapConfig_Car : CameraMapConfig
	{
		// Token: 0x06000414 RID: 1044 RVA: 0x000152F0 File Offset: 0x000134F0
		public CameraMapConfig_Car()
		{
			this.dollyRateKeys = 0f;
			this.dollyRateScreenEdge = 0f;
			this.camSpeedDecayFactor = 1f;
			this.moveSpeedScale = 1f;
		}

		// Token: 0x06000415 RID: 1045 RVA: 0x00015324 File Offset: 0x00013524
		public override void ConfigFixedUpdate_60(ref Vector3 velocity)
		{
			base.ConfigFixedUpdate_60(ref velocity);
			float num = 0.0166666675f;
			if (KeyBindingDefOf.MapDolly_Left.IsDown)
			{
				this.targetAngle += 0.72f * num;
			}
			if (KeyBindingDefOf.MapDolly_Right.IsDown)
			{
				this.targetAngle -= 0.72f * num;
			}
			if (KeyBindingDefOf.MapDolly_Up.IsDown)
			{
				this.speed += 1.2f * num;
			}
			if (KeyBindingDefOf.MapDolly_Down.IsDown)
			{
				this.speed -= 1.2f * num;
				if (this.speed < 0f)
				{
					this.speed = 0f;
				}
			}
			this.angle = Mathf.Lerp(this.angle, this.targetAngle, 0.02f);
			velocity.x = Mathf.Cos(this.angle) * this.speed;
			velocity.z = Mathf.Sin(this.angle) * this.speed;
		}

		// Token: 0x04000153 RID: 339
		private float targetAngle;

		// Token: 0x04000154 RID: 340
		private float angle;

		// Token: 0x04000155 RID: 341
		private float speed;

		// Token: 0x04000156 RID: 342
		private const float SpeedChangeSpeed = 1.2f;

		// Token: 0x04000157 RID: 343
		private const float AngleChangeSpeed = 0.72f;
	}
}
