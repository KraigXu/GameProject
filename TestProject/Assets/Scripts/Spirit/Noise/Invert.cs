using System;

namespace Verse.Noise
{
	// Token: 0x020004B1 RID: 1201
	public class Invert : ModuleBase
	{
		// Token: 0x06002376 RID: 9078 RVA: 0x000D39D4 File Offset: 0x000D1BD4
		public Invert() : base(1)
		{
		}

		// Token: 0x06002377 RID: 9079 RVA: 0x000D5095 File Offset: 0x000D3295
		public Invert(ModuleBase input) : base(1)
		{
			this.modules[0] = input;
		}

		// Token: 0x06002378 RID: 9080 RVA: 0x000D5802 File Offset: 0x000D3A02
		public override double GetValue(double x, double y, double z)
		{
			return -this.modules[0].GetValue(x, y, z);
		}
	}
}
