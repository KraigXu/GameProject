using System;

namespace Verse.Noise
{
	// Token: 0x020004A7 RID: 1191
	public class Add : ModuleBase
	{
		// Token: 0x06002341 RID: 9025 RVA: 0x000D50BE File Offset: 0x000D32BE
		public Add() : base(2)
		{
		}

		// Token: 0x06002342 RID: 9026 RVA: 0x000D50C7 File Offset: 0x000D32C7
		public Add(ModuleBase lhs, ModuleBase rhs) : base(2)
		{
			this.modules[0] = lhs;
			this.modules[1] = rhs;
		}

		// Token: 0x06002343 RID: 9027 RVA: 0x000D50E2 File Offset: 0x000D32E2
		public override double GetValue(double x, double y, double z)
		{
			return this.modules[0].GetValue(x, y, z) + this.modules[1].GetValue(x, y, z);
		}
	}
}
