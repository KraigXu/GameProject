using System;

namespace Verse.Noise
{
	// Token: 0x020004A9 RID: 1193
	public class Blend : ModuleBase
	{
		// Token: 0x06002347 RID: 9031 RVA: 0x000D513B File Offset: 0x000D333B
		public Blend() : base(3)
		{
		}

		// Token: 0x06002348 RID: 9032 RVA: 0x000D5144 File Offset: 0x000D3344
		public Blend(ModuleBase lhs, ModuleBase rhs, ModuleBase controller) : base(3)
		{
			this.modules[0] = lhs;
			this.modules[1] = rhs;
			this.modules[2] = controller;
		}

		// Token: 0x170006F6 RID: 1782
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

		// Token: 0x0600234B RID: 9035 RVA: 0x000D5180 File Offset: 0x000D3380
		public override double GetValue(double x, double y, double z)
		{
			double value = this.modules[0].GetValue(x, y, z);
			double value2 = this.modules[1].GetValue(x, y, z);
			double position = (this.modules[2].GetValue(x, y, z) + 1.0) / 2.0;
			return Utils.InterpolateLinear(value, value2, position);
		}
	}
}
