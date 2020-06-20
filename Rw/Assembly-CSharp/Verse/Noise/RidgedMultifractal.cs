using System;
using UnityEngine;

namespace Verse.Noise
{
	// Token: 0x020004A1 RID: 1185
	public class RidgedMultifractal : ModuleBase
	{
		// Token: 0x170006E3 RID: 1763
		// (get) Token: 0x06002300 RID: 8960 RVA: 0x000D3DD5 File Offset: 0x000D1FD5
		// (set) Token: 0x06002301 RID: 8961 RVA: 0x000D3DDD File Offset: 0x000D1FDD
		public double Frequency
		{
			get
			{
				return this.frequency;
			}
			set
			{
				this.frequency = value;
			}
		}

		// Token: 0x170006E4 RID: 1764
		// (get) Token: 0x06002302 RID: 8962 RVA: 0x000D3DE6 File Offset: 0x000D1FE6
		// (set) Token: 0x06002303 RID: 8963 RVA: 0x000D3DEE File Offset: 0x000D1FEE
		public double Lacunarity
		{
			get
			{
				return this.lacunarity;
			}
			set
			{
				this.lacunarity = value;
				this.UpdateWeights();
			}
		}

		// Token: 0x170006E5 RID: 1765
		// (get) Token: 0x06002304 RID: 8964 RVA: 0x000D3DFD File Offset: 0x000D1FFD
		// (set) Token: 0x06002305 RID: 8965 RVA: 0x000D3E05 File Offset: 0x000D2005
		public QualityMode Quality
		{
			get
			{
				return this.quality;
			}
			set
			{
				this.quality = value;
			}
		}

		// Token: 0x170006E6 RID: 1766
		// (get) Token: 0x06002306 RID: 8966 RVA: 0x000D3E0E File Offset: 0x000D200E
		// (set) Token: 0x06002307 RID: 8967 RVA: 0x000D3E16 File Offset: 0x000D2016
		public int OctaveCount
		{
			get
			{
				return this.octaveCount;
			}
			set
			{
				this.octaveCount = Mathf.Clamp(value, 1, 30);
			}
		}

		// Token: 0x170006E7 RID: 1767
		// (get) Token: 0x06002308 RID: 8968 RVA: 0x000D3E27 File Offset: 0x000D2027
		// (set) Token: 0x06002309 RID: 8969 RVA: 0x000D3E2F File Offset: 0x000D202F
		public int Seed
		{
			get
			{
				return this.seed;
			}
			set
			{
				this.seed = value;
			}
		}

		// Token: 0x0600230A RID: 8970 RVA: 0x000D3E38 File Offset: 0x000D2038
		public RidgedMultifractal() : base(0)
		{
			this.UpdateWeights();
		}

		// Token: 0x0600230B RID: 8971 RVA: 0x000D3E8C File Offset: 0x000D208C
		public RidgedMultifractal(double frequency, double lacunarity, int octaves, int seed, QualityMode quality) : base(0)
		{
			this.Frequency = frequency;
			this.Lacunarity = lacunarity;
			this.OctaveCount = octaves;
			this.Seed = seed;
			this.Quality = quality;
		}

		// Token: 0x0600230C RID: 8972 RVA: 0x000D3F00 File Offset: 0x000D2100
		private void UpdateWeights()
		{
			double num = 1.0;
			for (int i = 0; i < 30; i++)
			{
				this.weights[i] = Math.Pow(num, -1.0);
				num *= this.lacunarity;
			}
		}

		// Token: 0x0600230D RID: 8973 RVA: 0x000D3F44 File Offset: 0x000D2144
		public override double GetValue(double x, double y, double z)
		{
			x *= this.frequency;
			y *= this.frequency;
			z *= this.frequency;
			double num = 0.0;
			double num2 = 1.0;
			double num3 = 1.0;
			double num4 = 2.0;
			for (int i = 0; i < this.octaveCount; i++)
			{
				double x2 = Utils.MakeInt32Range(x);
				double y2 = Utils.MakeInt32Range(y);
				double z2 = Utils.MakeInt32Range(z);
				long num5 = (long)(this.seed + i & int.MaxValue);
				double num6 = Utils.GradientCoherentNoise3D(x2, y2, z2, num5, this.quality);
				num6 = Math.Abs(num6);
				num6 = num3 - num6;
				num6 *= num6;
				num6 *= num2;
				num2 = num6 * num4;
				if (num2 > 1.0)
				{
					num2 = 1.0;
				}
				if (num2 < 0.0)
				{
					num2 = 0.0;
				}
				num += num6 * this.weights[i];
				x *= this.lacunarity;
				y *= this.lacunarity;
				z *= this.lacunarity;
			}
			return num * 1.25 - 1.0;
		}

		// Token: 0x04001549 RID: 5449
		private double frequency = 1.0;

		// Token: 0x0400154A RID: 5450
		private double lacunarity = 2.0;

		// Token: 0x0400154B RID: 5451
		private QualityMode quality = QualityMode.Medium;

		// Token: 0x0400154C RID: 5452
		private int octaveCount = 6;

		// Token: 0x0400154D RID: 5453
		private int seed;

		// Token: 0x0400154E RID: 5454
		private double[] weights = new double[30];
	}
}
