using System;

namespace Verse.Noise
{
	// Token: 0x020004AE RID: 1198
	public class Exponent : ModuleBase
	{
		// Token: 0x0600236A RID: 9066 RVA: 0x000D564E File Offset: 0x000D384E
		public Exponent() : base(1)
		{
		}

		// Token: 0x0600236B RID: 9067 RVA: 0x000D5666 File Offset: 0x000D3866
		public Exponent(ModuleBase input) : base(1)
		{
			this.modules[0] = input;
		}

		// Token: 0x0600236C RID: 9068 RVA: 0x000D5687 File Offset: 0x000D3887
		public Exponent(double exponent, ModuleBase input) : base(1)
		{
			this.modules[0] = input;
			this.Value = exponent;
		}

		// Token: 0x170006FF RID: 1791
		// (get) Token: 0x0600236D RID: 9069 RVA: 0x000D56AF File Offset: 0x000D38AF
		// (set) Token: 0x0600236E RID: 9070 RVA: 0x000D56B7 File Offset: 0x000D38B7
		public double Value
		{
			get
			{
				return this.m_exponent;
			}
			set
			{
				this.m_exponent = value;
			}
		}

		// Token: 0x0600236F RID: 9071 RVA: 0x000D56C0 File Offset: 0x000D38C0
		public override double GetValue(double x, double y, double z)
		{
			return Math.Pow(Math.Abs((this.modules[0].GetValue(x, y, z) + 1.0) / 2.0), this.m_exponent) * 2.0 - 1.0;
		}

		// Token: 0x04001577 RID: 5495
		private double m_exponent = 1.0;
	}
}
