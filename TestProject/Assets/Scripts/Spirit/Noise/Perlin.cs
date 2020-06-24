using System;
using UnityEngine;

namespace Verse.Noise
{
	// Token: 0x020004A0 RID: 1184
	public class Perlin : ModuleBase
	{
		// Token: 0x170006DD RID: 1757
		// (get) Token: 0x060022F1 RID: 8945 RVA: 0x000D3BD7 File Offset: 0x000D1DD7
		// (set) Token: 0x060022F2 RID: 8946 RVA: 0x000D3BDF File Offset: 0x000D1DDF
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

		// Token: 0x170006DE RID: 1758
		// (get) Token: 0x060022F3 RID: 8947 RVA: 0x000D3BE8 File Offset: 0x000D1DE8
		// (set) Token: 0x060022F4 RID: 8948 RVA: 0x000D3BF0 File Offset: 0x000D1DF0
		public double Lacunarity
		{
			get
			{
				return this.lacunarity;
			}
			set
			{
				this.lacunarity = value;
			}
		}

		// Token: 0x170006DF RID: 1759
		// (get) Token: 0x060022F5 RID: 8949 RVA: 0x000D3BF9 File Offset: 0x000D1DF9
		// (set) Token: 0x060022F6 RID: 8950 RVA: 0x000D3C01 File Offset: 0x000D1E01
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

		// Token: 0x170006E0 RID: 1760
		// (get) Token: 0x060022F7 RID: 8951 RVA: 0x000D3C0A File Offset: 0x000D1E0A
		// (set) Token: 0x060022F8 RID: 8952 RVA: 0x000D3C12 File Offset: 0x000D1E12
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

		// Token: 0x170006E1 RID: 1761
		// (get) Token: 0x060022F9 RID: 8953 RVA: 0x000D3C23 File Offset: 0x000D1E23
		// (set) Token: 0x060022FA RID: 8954 RVA: 0x000D3C2B File Offset: 0x000D1E2B
		public double Persistence
		{
			get
			{
				return this.persistence;
			}
			set
			{
				this.persistence = value;
			}
		}

		// Token: 0x170006E2 RID: 1762
		// (get) Token: 0x060022FB RID: 8955 RVA: 0x000D3C34 File Offset: 0x000D1E34
		// (set) Token: 0x060022FC RID: 8956 RVA: 0x000D3C3C File Offset: 0x000D1E3C
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

		// Token: 0x060022FD RID: 8957 RVA: 0x000D3C48 File Offset: 0x000D1E48
		public Perlin() : base(0)
		{
		}

		// Token: 0x060022FE RID: 8958 RVA: 0x000D3C98 File Offset: 0x000D1E98
		public Perlin(double frequency, double lacunarity, double persistence, int octaves, int seed, QualityMode quality) : base(0)
		{
			this.Frequency = frequency;
			this.Lacunarity = lacunarity;
			this.OctaveCount = octaves;
			this.Persistence = persistence;
			this.Seed = seed;
			this.Quality = quality;
		}

		// Token: 0x060022FF RID: 8959 RVA: 0x000D3D14 File Offset: 0x000D1F14
		public override double GetValue(double x, double y, double z)
		{
			double num = 0.0;
			double num2 = 1.0;
			x *= this.frequency;
			y *= this.frequency;
			z *= this.frequency;
			for (int i = 0; i < this.octaveCount; i++)
			{
				double x2 = Utils.MakeInt32Range(x);
				double y2 = Utils.MakeInt32Range(y);
				double z2 = Utils.MakeInt32Range(z);
				long num3 = (long)(this.seed + i) & (long)((ulong)-1);
				double num4 = Utils.GradientCoherentNoise3D(x2, y2, z2, num3, this.quality);
				num += num4 * num2;
				x *= this.lacunarity;
				y *= this.lacunarity;
				z *= this.lacunarity;
				num2 *= this.persistence;
			}
			return num;
		}

		// Token: 0x04001543 RID: 5443
		private double frequency = 1.0;

		// Token: 0x04001544 RID: 5444
		private double lacunarity = 2.0;

		// Token: 0x04001545 RID: 5445
		private QualityMode quality = QualityMode.Medium;

		// Token: 0x04001546 RID: 5446
		private int octaveCount = 6;

		// Token: 0x04001547 RID: 5447
		private double persistence = 0.5;

		// Token: 0x04001548 RID: 5448
		private int seed;
	}
}
