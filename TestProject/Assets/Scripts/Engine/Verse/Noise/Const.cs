using System;

namespace Verse.Noise
{
	
	public class Const : ModuleBase
	{
		
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

		
		public Const() : base(0)
		{
		}

		
		public Const(double value) : base(0)
		{
			this.Value = value;
		}

		
		public override double GetValue(double x, double y, double z)
		{
			return this.val;
		}

		
		private double val;
	}
}
