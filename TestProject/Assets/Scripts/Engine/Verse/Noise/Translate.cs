using System;

namespace Verse.Noise
{
	
	public class Translate : ModuleBase
	{
		
		public Translate() : base(1)
		{
		}

		
		public Translate(ModuleBase input) : base(1)
		{
			this.modules[0] = input;
		}

		
		public Translate(double x, double y, double z, ModuleBase input) : base(1)
		{
			this.modules[0] = input;
			this.X = x;
			this.Y = y;
			this.Z = z;
		}

		
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

		
		public override double GetValue(double x, double y, double z)
		{
			return this.modules[0].GetValue(x + this.m_x, y + this.m_y, z + this.m_z);
		}

		
		private double m_x = 1.0;

		
		private double m_y = 1.0;

		
		private double m_z = 1.0;
	}
}
