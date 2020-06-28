using System;

namespace Verse.Noise
{
	// Token: 0x020004B7 RID: 1207
	public class PowerKeepSign : ModuleBase
	{
		// Token: 0x06002388 RID: 9096 RVA: 0x000D50BE File Offset: 0x000D32BE
		public PowerKeepSign() : base(2)
		{
		}

		// Token: 0x06002389 RID: 9097 RVA: 0x000D50C7 File Offset: 0x000D32C7
		public PowerKeepSign(ModuleBase lhs, ModuleBase rhs) : base(2)
		{
			this.modules[0] = lhs;
			this.modules[1] = rhs;
		}

		// Token: 0x0600238A RID: 9098 RVA: 0x000D58E8 File Offset: 0x000D3AE8
		public override double GetValue(double x, double y, double z)
		{
			double value = this.modules[0].GetValue(x, y, z);
			return (double)Math.Sign(value) * Math.Pow(Math.Abs(value), this.modules[1].GetValue(x, y, z));
		}
	}
}
