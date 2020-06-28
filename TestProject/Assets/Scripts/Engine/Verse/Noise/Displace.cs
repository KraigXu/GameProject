using System;

namespace Verse.Noise
{
	// Token: 0x020004AD RID: 1197
	public class Displace : ModuleBase
	{
		// Token: 0x06002361 RID: 9057 RVA: 0x000D5597 File Offset: 0x000D3797
		public Displace() : base(4)
		{
		}

		// Token: 0x06002362 RID: 9058 RVA: 0x000D55A0 File Offset: 0x000D37A0
		public Displace(ModuleBase input, ModuleBase x, ModuleBase y, ModuleBase z) : base(4)
		{
			this.modules[0] = input;
			this.modules[1] = x;
			this.modules[2] = y;
			this.modules[3] = z;
		}

		// Token: 0x170006FC RID: 1788
		// (get) Token: 0x06002363 RID: 9059 RVA: 0x000D55CE File Offset: 0x000D37CE
		// (set) Token: 0x06002364 RID: 9060 RVA: 0x000D55D8 File Offset: 0x000D37D8
		public ModuleBase X
		{
			get
			{
				return this.modules[1];
			}
			set
			{
				this.modules[1] = value;
			}
		}

		// Token: 0x170006FD RID: 1789
		// (get) Token: 0x06002365 RID: 9061 RVA: 0x000D5168 File Offset: 0x000D3368
		// (set) Token: 0x06002366 RID: 9062 RVA: 0x000D5172 File Offset: 0x000D3372
		public ModuleBase Y
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

		// Token: 0x170006FE RID: 1790
		// (get) Token: 0x06002367 RID: 9063 RVA: 0x000D55E3 File Offset: 0x000D37E3
		// (set) Token: 0x06002368 RID: 9064 RVA: 0x000D55ED File Offset: 0x000D37ED
		public ModuleBase Z
		{
			get
			{
				return this.modules[3];
			}
			set
			{
				this.modules[3] = value;
			}
		}

		// Token: 0x06002369 RID: 9065 RVA: 0x000D55F8 File Offset: 0x000D37F8
		public override double GetValue(double x, double y, double z)
		{
			double x2 = x + this.modules[1].GetValue(x, y, z);
			double y2 = y + this.modules[2].GetValue(x, y, z);
			double z2 = z + this.modules[3].GetValue(x, y, z);
			return this.modules[0].GetValue(x2, y2, z2);
		}
	}
}
