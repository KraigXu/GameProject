using System;

namespace Verse.Noise
{
	// Token: 0x0200049F RID: 1183
	public class Const : ModuleBase
	{
		// Token: 0x170006DC RID: 1756
		// (get) Token: 0x060022EC RID: 8940 RVA: 0x000D3BB6 File Offset: 0x000D1DB6
		// (set) Token: 0x060022ED RID: 8941 RVA: 0x000D3BBE File Offset: 0x000D1DBE
		public double Value
		{
			get
			{
				return this.val;
			}
			set
			{
				this.val = value;
			}
		}

		// Token: 0x060022EE RID: 8942 RVA: 0x000D397C File Offset: 0x000D1B7C
		public Const() : base(0)
		{
		}

		// Token: 0x060022EF RID: 8943 RVA: 0x000D3BC7 File Offset: 0x000D1DC7
		public Const(double value) : base(0)
		{
			this.Value = value;
		}

		// Token: 0x060022F0 RID: 8944 RVA: 0x000D3BB6 File Offset: 0x000D1DB6
		public override double GetValue(double x, double y, double z)
		{
			return this.val;
		}

		// Token: 0x04001542 RID: 5442
		private double val;
	}
}
