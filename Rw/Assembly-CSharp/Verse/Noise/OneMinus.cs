using System;

namespace Verse.Noise
{
	// Token: 0x020004B5 RID: 1205
	public class OneMinus : ModuleBase
	{
		// Token: 0x06002382 RID: 9090 RVA: 0x000D39D4 File Offset: 0x000D1BD4
		public OneMinus() : base(1)
		{
		}

		// Token: 0x06002383 RID: 9091 RVA: 0x000D5095 File Offset: 0x000D3295
		public OneMinus(ModuleBase module) : base(1)
		{
			this.modules[0] = module;
		}

		// Token: 0x06002384 RID: 9092 RVA: 0x000D58A3 File Offset: 0x000D3AA3
		public override double GetValue(double x, double y, double z)
		{
			return 1.0 - this.modules[0].GetValue(x, y, z);
		}
	}
}
