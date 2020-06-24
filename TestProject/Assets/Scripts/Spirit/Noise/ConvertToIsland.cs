using System;
using UnityEngine;

namespace Verse.Noise
{
	// Token: 0x0200049B RID: 1179
	public class ConvertToIsland : ModuleBase
	{
		// Token: 0x060022E0 RID: 8928 RVA: 0x000D39D4 File Offset: 0x000D1BD4
		public ConvertToIsland() : base(1)
		{
		}

		// Token: 0x060022E1 RID: 8929 RVA: 0x000D39DD File Offset: 0x000D1BDD
		public ConvertToIsland(Vector3 viewCenter, float viewAngle, ModuleBase input) : base(1)
		{
			this.viewCenter = viewCenter;
			this.viewAngle = viewAngle;
			this.modules[0] = input;
		}

		// Token: 0x060022E2 RID: 8930 RVA: 0x000D3A00 File Offset: 0x000D1C00
		public override double GetValue(double x, double y, double z)
		{
			float num = Vector3.Angle(this.viewCenter, new Vector3((float)x, (float)y, (float)z));
			double value = this.modules[0].GetValue(x, y, z);
			float num2 = Mathf.Max(2.5f, this.viewAngle * 0.25f);
			float num3 = Mathf.Max(0.8f, this.viewAngle * 0.1f);
			if (num < this.viewAngle - num2)
			{
				return value;
			}
			float num4 = GenMath.LerpDouble(this.viewAngle - num2, this.viewAngle - num3, 0f, 0.62f, num);
			if (value > -0.11999999731779099)
			{
				return (value - -0.11999999731779099) * (double)(1f - num4 * 0.7f) - (double)(num4 * 0.3f) + -0.11999999731779099;
			}
			return value - (double)(num4 * 0.3f);
		}

		// Token: 0x0400153A RID: 5434
		public Vector3 viewCenter;

		// Token: 0x0400153B RID: 5435
		public float viewAngle;

		// Token: 0x0400153C RID: 5436
		private const float WaterLevel = -0.12f;
	}
}
