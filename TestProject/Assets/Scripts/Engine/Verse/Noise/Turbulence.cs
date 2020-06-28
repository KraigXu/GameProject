using System;

namespace Verse.Noise
{
	// Token: 0x020004BF RID: 1215
	public class Turbulence : ModuleBase
	{
		// Token: 0x060023CD RID: 9165 RVA: 0x000D6326 File Offset: 0x000D4526
		public Turbulence() : base(1)
		{
			this.m_xDistort = new Perlin();
			this.m_yDistort = new Perlin();
			this.m_zDistort = new Perlin();
		}

		// Token: 0x060023CE RID: 9166 RVA: 0x000D6360 File Offset: 0x000D4560
		public Turbulence(ModuleBase input) : base(1)
		{
			this.m_xDistort = new Perlin();
			this.m_yDistort = new Perlin();
			this.m_zDistort = new Perlin();
			this.modules[0] = input;
		}

		// Token: 0x060023CF RID: 9167 RVA: 0x000D63AD File Offset: 0x000D45AD
		public Turbulence(double power, ModuleBase input) : this(new Perlin(), new Perlin(), new Perlin(), power, input)
		{
		}

		// Token: 0x060023D0 RID: 9168 RVA: 0x000D63C6 File Offset: 0x000D45C6
		public Turbulence(Perlin x, Perlin y, Perlin z, double power, ModuleBase input) : base(1)
		{
			this.m_xDistort = x;
			this.m_yDistort = y;
			this.m_zDistort = z;
			this.modules[0] = input;
			this.Power = power;
		}

		// Token: 0x17000712 RID: 1810
		// (get) Token: 0x060023D1 RID: 9169 RVA: 0x000D6405 File Offset: 0x000D4605
		// (set) Token: 0x060023D2 RID: 9170 RVA: 0x000D6412 File Offset: 0x000D4612
		public double Frequency
		{
			get
			{
				return this.m_xDistort.Frequency;
			}
			set
			{
				this.m_xDistort.Frequency = value;
				this.m_yDistort.Frequency = value;
				this.m_zDistort.Frequency = value;
			}
		}

		// Token: 0x17000713 RID: 1811
		// (get) Token: 0x060023D3 RID: 9171 RVA: 0x000D6438 File Offset: 0x000D4638
		// (set) Token: 0x060023D4 RID: 9172 RVA: 0x000D6440 File Offset: 0x000D4640
		public double Power
		{
			get
			{
				return this.m_power;
			}
			set
			{
				this.m_power = value;
			}
		}

		// Token: 0x17000714 RID: 1812
		// (get) Token: 0x060023D5 RID: 9173 RVA: 0x000D6449 File Offset: 0x000D4649
		// (set) Token: 0x060023D6 RID: 9174 RVA: 0x000D6456 File Offset: 0x000D4656
		public int Roughness
		{
			get
			{
				return this.m_xDistort.OctaveCount;
			}
			set
			{
				this.m_xDistort.OctaveCount = value;
				this.m_yDistort.OctaveCount = value;
				this.m_zDistort.OctaveCount = value;
			}
		}

		// Token: 0x17000715 RID: 1813
		// (get) Token: 0x060023D7 RID: 9175 RVA: 0x000D647C File Offset: 0x000D467C
		// (set) Token: 0x060023D8 RID: 9176 RVA: 0x000D6489 File Offset: 0x000D4689
		public int Seed
		{
			get
			{
				return this.m_xDistort.Seed;
			}
			set
			{
				this.m_xDistort.Seed = value;
				this.m_yDistort.Seed = value + 1;
				this.m_zDistort.Seed = value + 2;
			}
		}

		// Token: 0x060023D9 RID: 9177 RVA: 0x000D64B4 File Offset: 0x000D46B4
		public override double GetValue(double x, double y, double z)
		{
			double x2 = x + this.m_xDistort.GetValue(x + 0.189422607421875, y + 0.99371337890625, z + 0.4781646728515625) * this.m_power;
			double y2 = y + this.m_yDistort.GetValue(x + 0.4046478271484375, y + 0.276611328125, z + 0.9230499267578125) * this.m_power;
			double z2 = z + this.m_zDistort.GetValue(x + 0.82122802734375, y + 0.1710968017578125, z + 0.6842803955078125) * this.m_power;
			return this.modules[0].GetValue(x2, y2, z2);
		}

		// Token: 0x04001596 RID: 5526
		private const double X0 = 0.189422607421875;

		// Token: 0x04001597 RID: 5527
		private const double Y0 = 0.99371337890625;

		// Token: 0x04001598 RID: 5528
		private const double Z0 = 0.4781646728515625;

		// Token: 0x04001599 RID: 5529
		private const double X1 = 0.4046478271484375;

		// Token: 0x0400159A RID: 5530
		private const double Y1 = 0.276611328125;

		// Token: 0x0400159B RID: 5531
		private const double Z1 = 0.9230499267578125;

		// Token: 0x0400159C RID: 5532
		private const double X2 = 0.82122802734375;

		// Token: 0x0400159D RID: 5533
		private const double Y2 = 0.1710968017578125;

		// Token: 0x0400159E RID: 5534
		private const double Z2 = 0.6842803955078125;

		// Token: 0x0400159F RID: 5535
		private double m_power = 1.0;

		// Token: 0x040015A0 RID: 5536
		private Perlin m_xDistort;

		// Token: 0x040015A1 RID: 5537
		private Perlin m_yDistort;

		// Token: 0x040015A2 RID: 5538
		private Perlin m_zDistort;
	}
}
