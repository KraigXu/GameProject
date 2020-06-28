using System;

namespace Verse.Noise
{
	// Token: 0x020004B6 RID: 1206
	public class Power : ModuleBase
	{
		// Token: 0x06002385 RID: 9093 RVA: 0x000D50BE File Offset: 0x000D32BE
		public Power() : base(2)
		{
		}

		// Token: 0x06002386 RID: 9094 RVA: 0x000D50C7 File Offset: 0x000D32C7
		public Power(ModuleBase lhs, ModuleBase rhs) : base(2)
		{
			this.modules[0] = lhs;
			this.modules[1] = rhs;
		}

		// Token: 0x06002387 RID: 9095 RVA: 0x000D58BF File Offset: 0x000D3ABF
		public override double GetValue(double x, double y, double z)
		{
			return Math.Pow(this.modules[0].GetValue(x, y, z), this.modules[1].GetValue(x, y, z));
		}
	}
}
