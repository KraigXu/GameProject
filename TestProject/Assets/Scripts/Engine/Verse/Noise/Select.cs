using System;

namespace Verse.Noise
{
	// Token: 0x020004BB RID: 1211
	public class Select : ModuleBase
	{
		// Token: 0x060023A8 RID: 9128 RVA: 0x000D5D08 File Offset: 0x000D3F08
		public Select() : base(3)
		{
		}

		// Token: 0x060023A9 RID: 9129 RVA: 0x000D5D30 File Offset: 0x000D3F30
		public Select(ModuleBase inputA, ModuleBase inputB, ModuleBase controller) : base(3)
		{
			this.modules[0] = inputA;
			this.modules[1] = inputB;
			this.modules[2] = controller;
		}

		// Token: 0x060023AA RID: 9130 RVA: 0x000D5D7D File Offset: 0x000D3F7D
		public Select(double min, double max, double fallOff, ModuleBase inputA, ModuleBase inputB) : this(inputA, inputB, null)
		{
			this.m_min = min;
			this.m_max = max;
			this.FallOff = fallOff;
		}

		// Token: 0x17000708 RID: 1800
		// (get) Token: 0x060023AB RID: 9131 RVA: 0x000D5168 File Offset: 0x000D3368
		// (set) Token: 0x060023AC RID: 9132 RVA: 0x000D5172 File Offset: 0x000D3372
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

		// Token: 0x17000709 RID: 1801
		// (get) Token: 0x060023AD RID: 9133 RVA: 0x000D5D9F File Offset: 0x000D3F9F
		// (set) Token: 0x060023AE RID: 9134 RVA: 0x000D5DA8 File Offset: 0x000D3FA8
		public double FallOff
		{
			get
			{
				return this.m_fallOff;
			}
			set
			{
				double num = this.m_max - this.m_min;
				this.m_raw = value;
				this.m_fallOff = ((value > num / 2.0) ? (num / 2.0) : value);
			}
		}

		// Token: 0x1700070A RID: 1802
		// (get) Token: 0x060023AF RID: 9135 RVA: 0x000D5DEC File Offset: 0x000D3FEC
		// (set) Token: 0x060023B0 RID: 9136 RVA: 0x000D5DF4 File Offset: 0x000D3FF4
		public double Maximum
		{
			get
			{
				return this.m_max;
			}
			set
			{
				this.m_max = value;
				this.FallOff = this.m_raw;
			}
		}

		// Token: 0x1700070B RID: 1803
		// (get) Token: 0x060023B1 RID: 9137 RVA: 0x000D5E09 File Offset: 0x000D4009
		// (set) Token: 0x060023B2 RID: 9138 RVA: 0x000D5E11 File Offset: 0x000D4011
		public double Minimum
		{
			get
			{
				return this.m_min;
			}
			set
			{
				this.m_min = value;
				this.FallOff = this.m_raw;
			}
		}

		// Token: 0x060023B3 RID: 9139 RVA: 0x000D5E26 File Offset: 0x000D4026
		public void SetBounds(double min, double max)
		{
			this.m_min = min;
			this.m_max = max;
			this.FallOff = this.m_fallOff;
		}

		// Token: 0x060023B4 RID: 9140 RVA: 0x000D5E44 File Offset: 0x000D4044
		public override double GetValue(double x, double y, double z)
		{
			double value = this.modules[2].GetValue(x, y, z);
			if (this.m_fallOff > 0.0)
			{
				if (value < this.m_min - this.m_fallOff)
				{
					return this.modules[0].GetValue(x, y, z);
				}
				if (value < this.m_min + this.m_fallOff)
				{
					double num = this.m_min - this.m_fallOff;
					double num2 = this.m_min + this.m_fallOff;
					double position = Utils.MapCubicSCurve((value - num) / (num2 - num));
					return Utils.InterpolateLinear(this.modules[0].GetValue(x, y, z), this.modules[1].GetValue(x, y, z), position);
				}
				if (value < this.m_max - this.m_fallOff)
				{
					return this.modules[1].GetValue(x, y, z);
				}
				if (value < this.m_max + this.m_fallOff)
				{
					double num3 = this.m_max - this.m_fallOff;
					double num4 = this.m_max + this.m_fallOff;
					double position = Utils.MapCubicSCurve((value - num3) / (num4 - num3));
					return Utils.InterpolateLinear(this.modules[1].GetValue(x, y, z), this.modules[0].GetValue(x, y, z), position);
				}
				return this.modules[0].GetValue(x, y, z);
			}
			else
			{
				if (value < this.m_min || value > this.m_max)
				{
					return this.modules[0].GetValue(x, y, z);
				}
				return this.modules[1].GetValue(x, y, z);
			}
		}

		// Token: 0x0400158D RID: 5517
		private double m_fallOff;

		// Token: 0x0400158E RID: 5518
		private double m_raw;

		// Token: 0x0400158F RID: 5519
		private double m_min = -1.0;

		// Token: 0x04001590 RID: 5520
		private double m_max = 1.0;
	}
}
