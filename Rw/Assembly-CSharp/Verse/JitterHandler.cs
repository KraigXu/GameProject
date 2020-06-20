using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020002FF RID: 767
	public class JitterHandler
	{
		// Token: 0x17000462 RID: 1122
		// (get) Token: 0x06001597 RID: 5527 RVA: 0x0007E120 File Offset: 0x0007C320
		public Vector3 CurrentOffset
		{
			get
			{
				return this.curOffset;
			}
		}

		// Token: 0x06001598 RID: 5528 RVA: 0x0007E128 File Offset: 0x0007C328
		public void JitterHandlerTick()
		{
			if (this.curOffset.sqrMagnitude < this.JitterDropPerTick * this.JitterDropPerTick)
			{
				this.curOffset = new Vector3(0f, 0f, 0f);
				return;
			}
			this.curOffset -= this.curOffset.normalized * this.JitterDropPerTick;
		}

		// Token: 0x06001599 RID: 5529 RVA: 0x0007E191 File Offset: 0x0007C391
		public void Notify_DamageApplied(DamageInfo dinfo)
		{
			if (dinfo.Def.hasForcefulImpact)
			{
				this.AddOffset(this.DamageJitterDistance, dinfo.Angle);
			}
		}

		// Token: 0x0600159A RID: 5530 RVA: 0x0007E1B4 File Offset: 0x0007C3B4
		public void Notify_DamageDeflected(DamageInfo dinfo)
		{
			if (dinfo.Def.hasForcefulImpact)
			{
				this.AddOffset(this.DeflectJitterDistance, dinfo.Angle);
			}
		}

		// Token: 0x0600159B RID: 5531 RVA: 0x0007E1D8 File Offset: 0x0007C3D8
		public void AddOffset(float dist, float dir)
		{
			this.curOffset += Quaternion.AngleAxis(dir, Vector3.up) * Vector3.forward * dist;
			if (this.curOffset.sqrMagnitude > this.JitterMax * this.JitterMax)
			{
				this.curOffset *= this.JitterMax / this.curOffset.magnitude;
			}
		}

		// Token: 0x04000E22 RID: 3618
		private Vector3 curOffset = new Vector3(0f, 0f, 0f);

		// Token: 0x04000E23 RID: 3619
		private float DamageJitterDistance = 0.17f;

		// Token: 0x04000E24 RID: 3620
		private float DeflectJitterDistance = 0.1f;

		// Token: 0x04000E25 RID: 3621
		private float JitterDropPerTick = 0.018f;

		// Token: 0x04000E26 RID: 3622
		private float JitterMax = 0.35f;
	}
}
