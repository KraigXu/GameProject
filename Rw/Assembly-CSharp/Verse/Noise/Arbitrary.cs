using System;

namespace Verse.Noise
{
	// Token: 0x020004A8 RID: 1192
	public class Arbitrary : ModuleBase
	{
		// Token: 0x06002344 RID: 9028 RVA: 0x000D39D4 File Offset: 0x000D1BD4
		public Arbitrary() : base(1)
		{
		}

		// Token: 0x06002345 RID: 9029 RVA: 0x000D5105 File Offset: 0x000D3305
		public Arbitrary(ModuleBase source, Func<double, double> processor) : base(1)
		{
			this.modules[0] = source;
			this.processor = processor;
		}

		// Token: 0x06002346 RID: 9030 RVA: 0x000D511E File Offset: 0x000D331E
		public override double GetValue(double x, double y, double z)
		{
			return this.processor(this.modules[0].GetValue(x, y, z));
		}

		// Token: 0x0400156E RID: 5486
		private Func<double, double> processor;
	}
}
