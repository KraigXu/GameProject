using System;

namespace Verse.Noise
{
	// Token: 0x020004B0 RID: 1200
	public class InverseLerp : ModuleBase
	{
		// Token: 0x06002373 RID: 9075 RVA: 0x000D39D4 File Offset: 0x000D1BD4
		public InverseLerp() : base(1)
		{
		}

		// Token: 0x06002374 RID: 9076 RVA: 0x000D577D File Offset: 0x000D397D
		public InverseLerp(ModuleBase module, float from, float to) : base(1)
		{
			this.modules[0] = module;
			this.from = from;
			this.to = to;
		}

		// Token: 0x06002375 RID: 9077 RVA: 0x000D57A0 File Offset: 0x000D39A0
		public override double GetValue(double x, double y, double z)
		{
			double num = (this.modules[0].GetValue(x, y, z) - (double)this.from) / (double)(this.to - this.from);
			if (num < 0.0)
			{
				return 0.0;
			}
			if (num > 1.0)
			{
				return 1.0;
			}
			return num;
		}

		// Token: 0x0400157A RID: 5498
		private float from;

		// Token: 0x0400157B RID: 5499
		private float to;
	}
}
