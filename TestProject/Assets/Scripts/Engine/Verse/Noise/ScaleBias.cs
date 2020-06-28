using System;

namespace Verse.Noise
{
	// Token: 0x020004BA RID: 1210
	public class ScaleBias : ModuleBase
	{
		// Token: 0x060023A0 RID: 9120 RVA: 0x000D5C5E File Offset: 0x000D3E5E
		public ScaleBias() : base(1)
		{
		}

		// Token: 0x060023A1 RID: 9121 RVA: 0x000D5C76 File Offset: 0x000D3E76
		public ScaleBias(ModuleBase input) : base(1)
		{
			this.modules[0] = input;
		}

		// Token: 0x060023A2 RID: 9122 RVA: 0x000D5C97 File Offset: 0x000D3E97
		public ScaleBias(double scale, double bias, ModuleBase input) : base(1)
		{
			this.modules[0] = input;
			this.Bias = bias;
			this.Scale = scale;
		}

		// Token: 0x17000706 RID: 1798
		// (get) Token: 0x060023A3 RID: 9123 RVA: 0x000D5CC6 File Offset: 0x000D3EC6
		// (set) Token: 0x060023A4 RID: 9124 RVA: 0x000D5CCE File Offset: 0x000D3ECE
		public double Bias
		{
			get
			{
				return this.bias;
			}
			set
			{
				this.bias = value;
			}
		}

		// Token: 0x17000707 RID: 1799
		// (get) Token: 0x060023A5 RID: 9125 RVA: 0x000D5CD7 File Offset: 0x000D3ED7
		// (set) Token: 0x060023A6 RID: 9126 RVA: 0x000D5CDF File Offset: 0x000D3EDF
		public double Scale
		{
			get
			{
				return this.scale;
			}
			set
			{
				this.scale = value;
			}
		}

		// Token: 0x060023A7 RID: 9127 RVA: 0x000D5CE8 File Offset: 0x000D3EE8
		public override double GetValue(double x, double y, double z)
		{
			return this.modules[0].GetValue(x, y, z) * this.scale + this.bias;
		}

		// Token: 0x0400158B RID: 5515
		private double scale = 1.0;

		// Token: 0x0400158C RID: 5516
		private double bias;
	}
}
