using System;

namespace Verse.Noise
{
	// Token: 0x020004BC RID: 1212
	public class Subtract : ModuleBase
	{
		// Token: 0x060023B5 RID: 9141 RVA: 0x000D50BE File Offset: 0x000D32BE
		public Subtract() : base(2)
		{
		}

		// Token: 0x060023B6 RID: 9142 RVA: 0x000D50C7 File Offset: 0x000D32C7
		public Subtract(ModuleBase lhs, ModuleBase rhs) : base(2)
		{
			this.modules[0] = lhs;
			this.modules[1] = rhs;
		}

		// Token: 0x060023B7 RID: 9143 RVA: 0x000D5FC1 File Offset: 0x000D41C1
		public override double GetValue(double x, double y, double z)
		{
			return this.modules[0].GetValue(x, y, z) - this.modules[1].GetValue(x, y, z);
		}
	}
}
