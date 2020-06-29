using System;

namespace Verse.Noise
{
	
	public class InverseLerp : ModuleBase
	{
		
		public InverseLerp() : base(1)
		{
		}

		
		public InverseLerp(ModuleBase module, float from, float to) : base(1)
		{
			this.modules[0] = module;
			this.from = from;
			this.to = to;
		}

		
		public override double GetValue(double x, double y, double z)
		{
			double num = (this.modules[0].GetValue(x, y, z) - (double)this.from) / (double)(this.to - this.from);
			if (num < 0.0)
			{
				return 0.0;
			}
			if (num > 1.0)
			{
				return 1.0;
			}
			return num;
		}

		
		private float from;

		
		private float to;
	}
}
