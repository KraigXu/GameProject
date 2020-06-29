using System;

namespace Verse.Noise
{
	
	public class Exponent : ModuleBase
	{
		
		public Exponent() : base(1)
		{
		}

		
		public Exponent(ModuleBase input) : base(1)
		{
			this.modules[0] = input;
		}

		
		public Exponent(double exponent, ModuleBase input) : base(1)
		{
			this.modules[0] = input;
			this.Value = exponent;
		}

		
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

		
		public override double GetValue(double x, double y, double z)
		{
			return Math.Pow(Math.Abs((this.modules[0].GetValue(x, y, z) + 1.0) / 2.0), this.m_exponent) * 2.0 - 1.0;
		}

		
		private double m_exponent = 1.0;
	}
}
