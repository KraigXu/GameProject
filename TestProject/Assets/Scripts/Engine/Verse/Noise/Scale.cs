using System;

namespace Verse.Noise
{
	
	public class Scale : ModuleBase
	{
		
		public Scale() : base(1)
		{
		}

		
		public Scale(ModuleBase input) : base(1)
		{
			this.modules[0] = input;
		}

		
		public Scale(double x, double y, double z, ModuleBase input) : base(1)
		{
			this.modules[0] = input;
			this.X = x;
			this.Y = y;
			this.Z = z;
		}

		
		// (get) Token: 0x06002399 RID: 9113 RVA: 0x000D5C04 File Offset: 0x000D3E04
		// (set) Token: 0x0600239A RID: 9114 RVA: 0x000D5C0C File Offset: 0x000D3E0C
		public double X
		{
			get
			{
				return this.m_x;
			}
			set
			{
				this.m_x = value;
			}
		}

		
		// (get) Token: 0x0600239B RID: 9115 RVA: 0x000D5C15 File Offset: 0x000D3E15
		// (set) Token: 0x0600239C RID: 9116 RVA: 0x000D5C1D File Offset: 0x000D3E1D
		public double Y
		{
			get
			{
				return this.m_y;
			}
			set
			{
				this.m_y = value;
			}
		}

		
		// (get) Token: 0x0600239D RID: 9117 RVA: 0x000D5C26 File Offset: 0x000D3E26
		// (set) Token: 0x0600239E RID: 9118 RVA: 0x000D5C2E File Offset: 0x000D3E2E
		public double Z
		{
			get
			{
				return this.m_z;
			}
			set
			{
				this.m_z = value;
			}
		}

		
		public override double GetValue(double x, double y, double z)
		{
			return this.modules[0].GetValue(x * this.m_x, y * this.m_y, z * this.m_z);
		}

		
		private double m_x = 1.0;

		
		private double m_y = 1.0;

		
		private double m_z = 1.0;
	}
}
