using System;

namespace Verse.Noise
{
	
	public class ScaleBias : ModuleBase
	{
		
		public ScaleBias() : base(1)
		{
		}

		
		public ScaleBias(ModuleBase input) : base(1)
		{
			this.modules[0] = input;
		}

		
		public ScaleBias(double scale, double bias, ModuleBase input) : base(1)
		{
			this.modules[0] = input;
			this.Bias = bias;
			this.Scale = scale;
		}

		
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

		
		public override double GetValue(double x, double y, double z)
		{
			return this.modules[0].GetValue(x, y, z) * this.scale + this.bias;
		}

		
		private double scale = 1.0;

		
		private double bias;
	}
}
