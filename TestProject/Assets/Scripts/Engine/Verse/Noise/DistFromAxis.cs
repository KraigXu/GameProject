using System;

namespace Verse.Noise
{
	// Token: 0x0200049D RID: 1181
	public class DistFromAxis : ModuleBase
	{
		// Token: 0x060022E7 RID: 8935 RVA: 0x000D397C File Offset: 0x000D1B7C
		public DistFromAxis() : base(0)
		{
		}

		// Token: 0x060022E8 RID: 8936 RVA: 0x000D3B5E File Offset: 0x000D1D5E
		public DistFromAxis(float span) : base(0)
		{
			this.span = span;
		}

		// Token: 0x060022E9 RID: 8937 RVA: 0x000D3B6E File Offset: 0x000D1D6E
		public override double GetValue(double x, double y, double z)
		{
			return Math.Abs(x) / (double)this.span;
		}

		// Token: 0x04001540 RID: 5440
		public float span;
	}
}
