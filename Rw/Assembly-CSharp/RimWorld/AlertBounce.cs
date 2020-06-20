using System;
using UnityEngine;

namespace RimWorld
{
	// Token: 0x02000DD2 RID: 3538
	internal class AlertBounce
	{
		// Token: 0x060055EA RID: 21994 RVA: 0x001C8024 File Offset: 0x001C6224
		public void DoAlertStartEffect()
		{
			this.position = 300f;
			this.velocity = -200f;
			this.lastTime = Time.time;
			this.idle = false;
		}

		// Token: 0x060055EB RID: 21995 RVA: 0x001C8050 File Offset: 0x001C6250
		public float CalculateHorizontalOffset()
		{
			if (this.idle)
			{
				return this.position;
			}
			float num = Mathf.Min(Time.time - this.lastTime, 0.05f);
			this.lastTime = Time.time;
			this.velocity -= 1200f * num;
			this.position += this.velocity * num;
			if (this.position < 0f)
			{
				this.position = 0f;
				this.velocity = Mathf.Max(-this.velocity / 3f - 1f, 0f);
			}
			if (Mathf.Abs(this.velocity) < 0.0001f && this.position < 1f)
			{
				this.velocity = 0f;
				this.position = 0f;
				this.idle = true;
			}
			return this.position;
		}

		// Token: 0x04002EFB RID: 12027
		private float position;

		// Token: 0x04002EFC RID: 12028
		private float velocity;

		// Token: 0x04002EFD RID: 12029
		private float lastTime = Time.time;

		// Token: 0x04002EFE RID: 12030
		private bool idle;

		// Token: 0x04002EFF RID: 12031
		private const float StartPosition = 300f;

		// Token: 0x04002F00 RID: 12032
		private const float StartVelocity = -200f;

		// Token: 0x04002F01 RID: 12033
		private const float Acceleration = 1200f;

		// Token: 0x04002F02 RID: 12034
		private const float DampingRatio = 3f;

		// Token: 0x04002F03 RID: 12035
		private const float DampingConstant = 1f;

		// Token: 0x04002F04 RID: 12036
		private const float MaxDelta = 0.05f;
	}
}
