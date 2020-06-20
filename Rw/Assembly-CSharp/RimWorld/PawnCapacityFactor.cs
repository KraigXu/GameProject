using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000907 RID: 2311
	public class PawnCapacityFactor
	{
		// Token: 0x06003709 RID: 14089 RVA: 0x00128C44 File Offset: 0x00126E44
		public float GetFactor(float capacityEfficiency)
		{
			float num = capacityEfficiency;
			if (this.allowedDefect != 0f && num < 1f)
			{
				num = Mathf.InverseLerp(0f, 1f - this.allowedDefect, num);
			}
			if (num > this.max)
			{
				num = this.max;
			}
			if (this.useReciprocal)
			{
				if (Mathf.Abs(num) < 0.001f)
				{
					num = 5f;
				}
				else
				{
					num = Mathf.Min(1f / num, 5f);
				}
			}
			return num;
		}

		// Token: 0x04001FFA RID: 8186
		public PawnCapacityDef capacity;

		// Token: 0x04001FFB RID: 8187
		public float weight = 1f;

		// Token: 0x04001FFC RID: 8188
		public float max = 9999f;

		// Token: 0x04001FFD RID: 8189
		public bool useReciprocal;

		// Token: 0x04001FFE RID: 8190
		public float allowedDefect;

		// Token: 0x04001FFF RID: 8191
		private const float MaxReciprocalFactor = 5f;
	}
}
