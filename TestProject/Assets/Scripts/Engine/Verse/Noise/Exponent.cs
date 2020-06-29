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
