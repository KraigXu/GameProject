using System;
using UnityEngine;

namespace Verse.Noise
{
	// Token: 0x0200049C RID: 1180
	public class DistanceFromPlanetViewCenter : ModuleBase
	{
		// Token: 0x060022E3 RID: 8931 RVA: 0x000D397C File Offset: 0x000D1B7C
		public DistanceFromPlanetViewCenter() : base(0)
		{
		}

		// Token: 0x060022E4 RID: 8932 RVA: 0x000D3AD9 File Offset: 0x000D1CD9
		public DistanceFromPlanetViewCenter(Vector3 viewCenter, float viewAngle, bool invert = false) : base(0)
		{
			this.viewCenter = viewCenter;
			this.viewAngle = viewAngle;
			this.invert = invert;
		}

		// Token: 0x060022E5 RID: 8933 RVA: 0x000D3AF8 File Offset: 0x000D1CF8
		public override double GetValue(double x, double y, double z)
		{
			float valueInt = this.GetValueInt(x, y, z);
			if (this.invert)
			{
				return (double)(1f - valueInt);
			}
			return (double)valueInt;
		}

		// Token: 0x060022E6 RID: 8934 RVA: 0x000D3B22 File Offset: 0x000D1D22
		private float GetValueInt(double x, double y, double z)
		{
			if (this.viewAngle >= 180f)
			{
				return 0f;
			}
			return Mathf.Min(Vector3.Angle(this.viewCenter, new Vector3((float)x, (float)y, (float)z)) / this.viewAngle, 1f);
		}

		// Token: 0x0400153D RID: 5437
		public Vector3 viewCenter;

		// Token: 0x0400153E RID: 5438
		public float viewAngle;

		// Token: 0x0400153F RID: 5439
		public bool invert;
	}
}
