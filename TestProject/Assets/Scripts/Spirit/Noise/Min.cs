using System;

namespace Verse.Noise
{
	// Token: 0x020004B3 RID: 1203
	public class Min : ModuleBase
	{
		// Token: 0x0600237C RID: 9084 RVA: 0x000D50BE File Offset: 0x000D32BE
		public Min() : base(2)
		{
		}

		// Token: 0x0600237D RID: 9085 RVA: 0x000D50C7 File Offset: 0x000D32C7
		public Min(ModuleBase lhs, ModuleBase rhs) : base(2)
		{
			this.modules[0] = lhs;
			this.modules[1] = rhs;
		}

		// Token: 0x0600237E RID: 9086 RVA: 0x000D584C File Offset: 0x000D3A4C
		public override double GetValue(double x, double y, double z)
		{
			double value = this.modules[0].GetValue(x, y, z);
			double value2 = this.modules[1].GetValue(x, y, z);
			return Math.Min(value, value2);
		}
	}
}
