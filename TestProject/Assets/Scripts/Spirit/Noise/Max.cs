using System;

namespace Verse.Noise
{
	// Token: 0x020004B2 RID: 1202
	public class Max : ModuleBase
	{
		// Token: 0x06002379 RID: 9081 RVA: 0x000D50BE File Offset: 0x000D32BE
		public Max() : base(2)
		{
		}

		// Token: 0x0600237A RID: 9082 RVA: 0x000D50C7 File Offset: 0x000D32C7
		public Max(ModuleBase lhs, ModuleBase rhs) : base(2)
		{
			this.modules[0] = lhs;
			this.modules[1] = rhs;
		}

		// Token: 0x0600237B RID: 9083 RVA: 0x000D5818 File Offset: 0x000D3A18
		public override double GetValue(double x, double y, double z)
		{
			double value = this.modules[0].GetValue(x, y, z);
			double value2 = this.modules[1].GetValue(x, y, z);
			return Math.Max(value, value2);
		}
	}
}
