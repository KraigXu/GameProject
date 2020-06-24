using System;

namespace Verse.Noise
{
	// Token: 0x020004AB RID: 1195
	public class Clamp : ModuleBase
	{
		// Token: 0x06002351 RID: 9041 RVA: 0x000D525C File Offset: 0x000D345C
		public Clamp() : base(1)
		{
		}

		// Token: 0x06002352 RID: 9042 RVA: 0x000D5283 File Offset: 0x000D3483
		public Clamp(ModuleBase input) : base(1)
		{
			this.modules[0] = input;
		}

		// Token: 0x06002353 RID: 9043 RVA: 0x000D52B3 File Offset: 0x000D34B3
		public Clamp(double min, double max, ModuleBase input) : base(1)
		{
			this.Minimum = min;
			this.Maximum = max;
			this.modules[0] = input;
		}

		// Token: 0x170006F8 RID: 1784
		// (get) Token: 0x06002354 RID: 9044 RVA: 0x000D52F1 File Offset: 0x000D34F1
		// (set) Token: 0x06002355 RID: 9045 RVA: 0x000D52F9 File Offset: 0x000D34F9
		public double Maximum
		{
			get
			{
				return this.m_max;
			}
			set
			{
				this.m_max = value;
			}
		}

		// Token: 0x170006F9 RID: 1785
		// (get) Token: 0x06002356 RID: 9046 RVA: 0x000D5302 File Offset: 0x000D3502
		// (set) Token: 0x06002357 RID: 9047 RVA: 0x000D530A File Offset: 0x000D350A
		public double Minimum
		{
			get
			{
				return this.m_min;
			}
			set
			{
				this.m_min = value;
			}
		}

		// Token: 0x06002358 RID: 9048 RVA: 0x000D5313 File Offset: 0x000D3513
		public void SetBounds(double min, double max)
		{
			this.m_min = min;
			this.m_max = max;
		}

		// Token: 0x06002359 RID: 9049 RVA: 0x000D5324 File Offset: 0x000D3524
		public override double GetValue(double x, double y, double z)
		{
			if (this.m_min > this.m_max)
			{
				double min = this.m_min;
				this.m_min = this.m_max;
				this.m_max = min;
			}
			double value = this.modules[0].GetValue(x, y, z);
			if (value < this.m_min)
			{
				return this.m_min;
			}
			if (value > this.m_max)
			{
				return this.m_max;
			}
			return value;
		}

		// Token: 0x04001574 RID: 5492
		private double m_min = -1.0;

		// Token: 0x04001575 RID: 5493
		private double m_max = 1.0;
	}
}
