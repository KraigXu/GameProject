using System;

namespace Verse.Noise
{
	
	public class Displace : ModuleBase
	{
		
		public Displace() : base(4)
		{
		}

		
		public Displace(ModuleBase input, ModuleBase x, ModuleBase y, ModuleBase z) : base(4)
		{
			this.modules[0] = input;
			this.modules[1] = x;
			this.modules[2] = y;
			this.modules[3] = z;
		}

		
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

		
		public override double GetValue(double x, double y, double z)
		{
			double x2 = x + this.modules[1].GetValue(x, y, z);
			double y2 = y + this.modules[2].GetValue(x, y, z);
			double z2 = z + this.modules[3].GetValue(x, y, z);
			return this.modules[0].GetValue(x2, y2, z2);
		}
	}
}
