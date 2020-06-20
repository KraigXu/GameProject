using System;

namespace Verse.Noise
{
	// Token: 0x020004AF RID: 1199
	public class Filter : ModuleBase
	{
		// Token: 0x06002370 RID: 9072 RVA: 0x000D39D4 File Offset: 0x000D1BD4
		public Filter() : base(1)
		{
		}

		// Token: 0x06002371 RID: 9073 RVA: 0x000D5715 File Offset: 0x000D3915
		public Filter(ModuleBase module, float from, float to) : base(1)
		{
			this.modules[0] = module;
			this.from = from;
			this.to = to;
		}

		// Token: 0x06002372 RID: 9074 RVA: 0x000D5738 File Offset: 0x000D3938
		public override double GetValue(double x, double y, double z)
		{
			double value = this.modules[0].GetValue(x, y, z);
			if (value >= (double)this.from && value <= (double)this.to)
			{
				return 1.0;
			}
			return 0.0;
		}

		// Token: 0x04001578 RID: 5496
		private float from;

		// Token: 0x04001579 RID: 5497
		private float to;
	}
}
