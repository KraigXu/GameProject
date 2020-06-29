using System;

namespace Verse.Noise
{
	
	public class Select : ModuleBase
	{
		
		public Select() : base(3)
		{
		}

		
		public Select(ModuleBase inputA, ModuleBase inputB, ModuleBase controller) : base(3)
		{
			this.modules[0] = inputA;
			this.modules[1] = inputB;
			this.modules[2] = controller;
		}

		
		public Select(double min, double max, double fallOff, ModuleBase inputA, ModuleBase inputB) : this(inputA, inputB, null)
		{
			this.m_min = min;
			this.m_max = max;
			this.FallOff = fallOff;
		}

		
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

		
		public void SetBounds(double min, double max)
		{
			this.m_min = min;
			this.m_max = max;
			this.FallOff = this.m_fallOff;
		}

		
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

		
		private double m_fallOff;

		
		private double m_raw;

		
		private double m_min = -1.0;

		
		private double m_max = 1.0;
	}
}
