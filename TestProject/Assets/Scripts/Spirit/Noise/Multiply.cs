using System;

namespace Verse.Noise
{
	// Token: 0x020004B4 RID: 1204
	public class Multiply : ModuleBase
	{
		// Token: 0x0600237F RID: 9087 RVA: 0x000D50BE File Offset: 0x000D32BE
		public Multiply() : base(2)
		{
		}

		// Token: 0x06002380 RID: 9088 RVA: 0x000D50C7 File Offset: 0x000D32C7
		public Multiply(ModuleBase lhs, ModuleBase rhs) : base(2)
		{
			this.modules[0] = lhs;
			this.modules[1] = rhs;
		}

		// Token: 0x06002381 RID: 9089 RVA: 0x000D5880 File Offset: 0x000D3A80
		public override double GetValue(double x, double y, double z)
		{
			return this.modules[0].GetValue(x, y, z) * this.modules[1].GetValue(x, y, z);
		}
	}
}
