using System;
using UnityEngine;

namespace RimWorld.Planet
{
	// Token: 0x020011DA RID: 4570
	public class WorldCameraConfig_Car : WorldCameraConfig
	{
		// Token: 0x060069CC RID: 27084 RVA: 0x0024EA7F File Offset: 0x0024CC7F
		public WorldCameraConfig_Car()
		{
			this.dollyRateKeys = 0f;
			this.dollyRateScreenEdge = 0f;
			this.camRotationDecayFactor = 1f;
			this.rotationSpeedScale = 0.15f;
		}

		// Token: 0x060069CD RID: 27085 RVA: 0x0024EAB4 File Offset: 0x0024CCB4
		public override void ConfigFixedUpdate_60(ref Vector2 rotationVelocity)
		{
			base.ConfigFixedUpdate_60(ref rotationVelocity);
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
				this.speed += 1.5f * num;
			}
			if (KeyBindingDefOf.MapDolly_Down.IsDown)
			{
				this.speed -= 1.5f * num;
				if (this.speed < 0f)
				{
					this.speed = 0f;
				}
			}
			this.angle = Mathf.Lerp(this.angle, this.targetAngle, 0.02f);
			rotationVelocity.x = Mathf.Cos(this.angle) * this.speed;
			rotationVelocity.y = Mathf.Sin(this.angle) * this.speed;
		}

		// Token: 0x040041CC RID: 16844
		private float targetAngle;

		// Token: 0x040041CD RID: 16845
		private float angle;

		// Token: 0x040041CE RID: 16846
		private float speed;

		// Token: 0x040041CF RID: 16847
		private const float SpeedChangeSpeed = 1.5f;

		// Token: 0x040041D0 RID: 16848
		private const float AngleChangeSpeed = 0.72f;
	}
}
