using System;

namespace Verse.Noise
{
	// Token: 0x020004A6 RID: 1190
	public class Abs : ModuleBase
	{
		// Token: 0x0600233E RID: 9022 RVA: 0x000D39D4 File Offset: 0x000D1BD4
		public Abs() : base(1)
		{
		}

		// Token: 0x0600233F RID: 9023 RVA: 0x000D5095 File Offset: 0x000D3295
		public Abs(ModuleBase input) : base(1)
		{
			this.modules[0] = input;
		}

		// Token: 0x06002340 RID: 9024 RVA: 0x000D50A7 File Offset: 0x000D32A7
		public override double GetValue(double x, double y, double z)
		{
			return Math.Abs(this.modules[0].GetValue(x, y, z));
		}
	}
}
