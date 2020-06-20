using System;

namespace Verse.Noise
{
	// Token: 0x020004BE RID: 1214
	public class Translate : ModuleBase
	{
		// Token: 0x060023C3 RID: 9155 RVA: 0x000D61F4 File Offset: 0x000D43F4
		public Translate() : base(1)
		{
		}

		// Token: 0x060023C4 RID: 9156 RVA: 0x000D622A File Offset: 0x000D442A
		public Translate(ModuleBase input) : base(1)
		{
			this.modules[0] = input;
		}

		// Token: 0x060023C5 RID: 9157 RVA: 0x000D626C File Offset: 0x000D446C
		public Translate(double x, double y, double z, ModuleBase input) : base(1)
		{
			this.modules[0] = input;
			this.X = x;
			this.Y = y;
			this.Z = z;
		}

		// Token: 0x1700070F RID: 1807
		// (get) Token: 0x060023C6 RID: 9158 RVA: 0x000D62CC File Offset: 0x000D44CC
		// (set) Token: 0x060023C7 RID: 9159 RVA: 0x000D62D4 File Offset: 0x000D44D4
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

		// Token: 0x17000710 RID: 1808
		// (get) Token: 0x060023C8 RID: 9160 RVA: 0x000D62DD File Offset: 0x000D44DD
		// (set) Token: 0x060023C9 RID: 9161 RVA: 0x000D62E5 File Offset: 0x000D44E5
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

		// Token: 0x17000711 RID: 1809
		// (get) Token: 0x060023CA RID: 9162 RVA: 0x000D62EE File Offset: 0x000D44EE
		// (set) Token: 0x060023CB RID: 9163 RVA: 0x000D62F6 File Offset: 0x000D44F6
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

		// Token: 0x060023CC RID: 9164 RVA: 0x000D62FF File Offset: 0x000D44FF
		public override double GetValue(double x, double y, double z)
		{
			return this.modules[0].GetValue(x + this.m_x, y + this.m_y, z + this.m_z);
		}

		// Token: 0x04001593 RID: 5523
		private double m_x = 1.0;

		// Token: 0x04001594 RID: 5524
		private double m_y = 1.0;

		// Token: 0x04001595 RID: 5525
		private double m_z = 1.0;
	}
}
