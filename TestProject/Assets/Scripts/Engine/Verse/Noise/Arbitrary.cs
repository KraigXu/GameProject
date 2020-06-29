using System;

namespace Verse.Noise
{
	
	public class Arbitrary : ModuleBase
	{
		
		public Arbitrary() : base(1)
		{
		}

		
		public Arbitrary(ModuleBase source, Func<double, double> processor) : base(1)
		{
			this.modules[0] = source;
			this.processor = processor;
		}

		
		public override double GetValue(double x, double y, double z)
		{
			return this.processor(this.modules[0].GetValue(x, y, z));
		}

		
		private Func<double, double> processor;
	}
}
