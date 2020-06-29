using System;

namespace Verse.Noise
{
	
	public class Blend : ModuleBase
	{
		
		public Blend() : base(3)
		{
		}

		
		public Blend(ModuleBase lhs, ModuleBase rhs, ModuleBase controller) : base(3)
		{
			this.modules[0] = lhs;
			this.modules[1] = rhs;
			this.modules[2] = controller;
		}

		
		// (get) Token: 0x06002349 RID: 9033 RVA: 0x000D5168 File Offset: 0x000D3368
		// (set) Token: 0x0600234A RID: 9034 RVA: 0x000D5172 File Offset: 0x000D3372
		public ModuleBase Controller
		{
			get
			{
				return this.modules[2];
			}
			set
			{
				this.modules[2] = value;
			}
		}

		
		public override double GetValue(double x, double y, double z)
		{
			double value = this.modules[0].GetValue(x, y, z);
			double value2 = this.modules[1].GetValue(x, y, z);
			double position = (this.modules[2].GetValue(x, y, z) + 1.0) / 2.0;
			return Utils.InterpolateLinear(value, value2, position);
		}
	}
}
