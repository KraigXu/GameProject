using System;

namespace Verse.Noise
{
	// Token: 0x020004B8 RID: 1208
	public class Rotate : ModuleBase
	{
		// Token: 0x0600238B RID: 9099 RVA: 0x000D5929 File Offset: 0x000D3B29
		public Rotate() : base(1)
		{
			this.SetAngles(0.0, 0.0, 0.0);
		}

		// Token: 0x0600238C RID: 9100 RVA: 0x000D5095 File Offset: 0x000D3295
		public Rotate(ModuleBase input) : base(1)
		{
			this.modules[0] = input;
		}

		// Token: 0x0600238D RID: 9101 RVA: 0x000D5953 File Offset: 0x000D3B53
		public Rotate(double x, double y, double z, ModuleBase input) : base(1)
		{
			this.modules[0] = input;
			this.SetAngles(x, y, z);
		}

		// Token: 0x17000700 RID: 1792
		// (get) Token: 0x0600238E RID: 9102 RVA: 0x000D596F File Offset: 0x000D3B6F
		// (set) Token: 0x0600238F RID: 9103 RVA: 0x000D5977 File Offset: 0x000D3B77
		public double X
		{
			get
			{
				return this.m_x;
			}
			set
			{
				this.SetAngles(value, this.m_y, this.m_z);
			}
		}

		// Token: 0x17000701 RID: 1793
		// (get) Token: 0x06002390 RID: 9104 RVA: 0x000D598C File Offset: 0x000D3B8C
		// (set) Token: 0x06002391 RID: 9105 RVA: 0x000D5994 File Offset: 0x000D3B94
		public double Y
		{
			get
			{
				return this.m_y;
			}
			set
			{
				this.SetAngles(this.m_x, value, this.m_z);
			}
		}

		// Token: 0x17000702 RID: 1794
		// (get) Token: 0x06002392 RID: 9106 RVA: 0x000D596F File Offset: 0x000D3B6F
		// (set) Token: 0x06002393 RID: 9107 RVA: 0x000D59A9 File Offset: 0x000D3BA9
		public double Z
		{
			get
			{
				return this.m_x;
			}
			set
			{
				this.SetAngles(this.m_x, this.m_y, value);
			}
		}

		// Token: 0x06002394 RID: 9108 RVA: 0x000D59C0 File Offset: 0x000D3BC0
		private void SetAngles(double x, double y, double z)
		{
			double num = Math.Cos(x * 0.017453292519943295);
			double num2 = Math.Cos(y * 0.017453292519943295);
			double num3 = Math.Cos(z * 0.017453292519943295);
			double num4 = Math.Sin(x * 0.017453292519943295);
			double num5 = Math.Sin(y * 0.017453292519943295);
			double num6 = Math.Sin(z * 0.017453292519943295);
			this.m_x1Matrix = num5 * num4 * num6 + num2 * num3;
			this.m_y1Matrix = num * num6;
			this.m_z1Matrix = num5 * num3 - num2 * num4 * num6;
			this.m_x2Matrix = num5 * num4 * num3 - num2 * num6;
			this.m_y2Matrix = num * num3;
			this.m_z2Matrix = -num2 * num4 * num3 - num5 * num6;
			this.m_x3Matrix = -num5 * num;
			this.m_y3Matrix = num4;
			this.m_z3Matrix = num2 * num;
			this.m_x = x;
			this.m_y = y;
			this.m_z = z;
		}

		// Token: 0x06002395 RID: 9109 RVA: 0x000D5AC0 File Offset: 0x000D3CC0
		public override double GetValue(double x, double y, double z)
		{
			double x2 = this.m_x1Matrix * x + this.m_y1Matrix * y + this.m_z1Matrix * z;
			double y2 = this.m_x2Matrix * x + this.m_y2Matrix * y + this.m_z2Matrix * z;
			double z2 = this.m_x3Matrix * x + this.m_y3Matrix * y + this.m_z3Matrix * z;
			return this.modules[0].GetValue(x2, y2, z2);
		}

		// Token: 0x0400157C RID: 5500
		private double m_x;

		// Token: 0x0400157D RID: 5501
		private double m_x1Matrix;

		// Token: 0x0400157E RID: 5502
		private double m_x2Matrix;

		// Token: 0x0400157F RID: 5503
		private double m_x3Matrix;

		// Token: 0x04001580 RID: 5504
		private double m_y;

		// Token: 0x04001581 RID: 5505
		private double m_y1Matrix;

		// Token: 0x04001582 RID: 5506
		private double m_y2Matrix;

		// Token: 0x04001583 RID: 5507
		private double m_y3Matrix;

		// Token: 0x04001584 RID: 5508
		private double m_z;

		// Token: 0x04001585 RID: 5509
		private double m_z1Matrix;

		// Token: 0x04001586 RID: 5510
		private double m_z2Matrix;

		// Token: 0x04001587 RID: 5511
		private double m_z3Matrix;
	}
}
