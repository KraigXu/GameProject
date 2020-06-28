using System;
using UnityEngine;

namespace Verse.Noise
{
	// Token: 0x02000498 RID: 1176
	public class AbsLatitudeCurve : ModuleBase
	{
		// Token: 0x060022D9 RID: 8921 RVA: 0x000D397C File Offset: 0x000D1B7C
		public AbsLatitudeCurve() : base(0)
		{
		}

		// Token: 0x060022DA RID: 8922 RVA: 0x000D3985 File Offset: 0x000D1B85
		public AbsLatitudeCurve(SimpleCurve curve, float planetRadius) : base(0)
		{
			this.curve = curve;
			this.planetRadius = planetRadius;
		}

		// Token: 0x060022DB RID: 8923 RVA: 0x000D399C File Offset: 0x000D1B9C
		public override double GetValue(double x, double y, double z)
		{
			float f = Mathf.Asin((float)(y / (double)this.planetRadius)) * 57.29578f;
			return (double)this.curve.Evaluate(Mathf.Abs(f));
		}

		// Token: 0x04001538 RID: 5432
		public SimpleCurve curve;

		// Token: 0x04001539 RID: 5433
		public float planetRadius;
	}
}
