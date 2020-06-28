using System;

namespace Verse.Noise
{
	// Token: 0x020004AA RID: 1194
	public class Cache : ModuleBase
	{
		// Token: 0x0600234C RID: 9036 RVA: 0x000D39D4 File Offset: 0x000D1BD4
		public Cache() : base(1)
		{
		}

		// Token: 0x0600234D RID: 9037 RVA: 0x000D5095 File Offset: 0x000D3295
		public Cache(ModuleBase input) : base(1)
		{
			this.modules[0] = input;
		}

		// Token: 0x170006F7 RID: 1783
		public override ModuleBase this[int index]
		{
			get
			{
				return base[index];
			}
			set
			{
				base[index] = value;
				this.m_cached = false;
			}
		}

		// Token: 0x06002350 RID: 9040 RVA: 0x000D51F4 File Offset: 0x000D33F4
		public override double GetValue(double x, double y, double z)
		{
			if (!this.m_cached || this.m_x != x || this.m_y != y || this.m_z != z)
			{
				this.m_value = this.modules[0].GetValue(x, y, z);
				this.m_x = x;
				this.m_y = y;
				this.m_z = z;
			}
			this.m_cached = true;
			return this.m_value;
		}

		// Token: 0x0400156F RID: 5487
		private double m_value;

		// Token: 0x04001570 RID: 5488
		private bool m_cached;

		// Token: 0x04001571 RID: 5489
		private double m_x;

		// Token: 0x04001572 RID: 5490
		private double m_y;

		// Token: 0x04001573 RID: 5491
		private double m_z;
	}
}
