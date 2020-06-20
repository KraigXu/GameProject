using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000009 RID: 9
	public struct ColorInt : IEquatable<ColorInt>
	{
		// Token: 0x06000082 RID: 130 RVA: 0x00004183 File Offset: 0x00002383
		public ColorInt(int r, int g, int b)
		{
			this.r = r;
			this.g = g;
			this.b = b;
			this.a = 255;
		}

		// Token: 0x06000083 RID: 131 RVA: 0x000041A5 File Offset: 0x000023A5
		public ColorInt(int r, int g, int b, int a)
		{
			this.r = r;
			this.g = g;
			this.b = b;
			this.a = a;
		}

		// Token: 0x06000084 RID: 132 RVA: 0x000041C4 File Offset: 0x000023C4
		public ColorInt(Color32 col)
		{
			this.r = (int)col.r;
			this.g = (int)col.g;
			this.b = (int)col.b;
			this.a = (int)col.a;
		}

		// Token: 0x06000085 RID: 133 RVA: 0x000041F6 File Offset: 0x000023F6
		public static ColorInt operator +(ColorInt colA, ColorInt colB)
		{
			return new ColorInt(colA.r + colB.r, colA.g + colB.g, colA.b + colB.b, colA.a + colB.a);
		}

		// Token: 0x06000086 RID: 134 RVA: 0x00004231 File Offset: 0x00002431
		public static ColorInt operator +(ColorInt colA, Color32 colB)
		{
			return new ColorInt(colA.r + (int)colB.r, colA.g + (int)colB.g, colA.b + (int)colB.b, colA.a + (int)colB.a);
		}

		// Token: 0x06000087 RID: 135 RVA: 0x0000426C File Offset: 0x0000246C
		public static ColorInt operator -(ColorInt a, ColorInt b)
		{
			return new ColorInt(a.r - b.r, a.g - b.g, a.b - b.b, a.a - b.a);
		}

		// Token: 0x06000088 RID: 136 RVA: 0x000042A7 File Offset: 0x000024A7
		public static ColorInt operator *(ColorInt a, int b)
		{
			return new ColorInt(a.r * b, a.g * b, a.b * b, a.a * b);
		}

		// Token: 0x06000089 RID: 137 RVA: 0x000042CE File Offset: 0x000024CE
		public static ColorInt operator *(ColorInt a, float b)
		{
			return new ColorInt((int)((float)a.r * b), (int)((float)a.g * b), (int)((float)a.b * b), (int)((float)a.a * b));
		}

		// Token: 0x0600008A RID: 138 RVA: 0x000042FD File Offset: 0x000024FD
		public static ColorInt operator /(ColorInt a, int b)
		{
			return new ColorInt(a.r / b, a.g / b, a.b / b, a.a / b);
		}

		// Token: 0x0600008B RID: 139 RVA: 0x00004324 File Offset: 0x00002524
		public static ColorInt operator /(ColorInt a, float b)
		{
			return new ColorInt((int)((float)a.r / b), (int)((float)a.g / b), (int)((float)a.b / b), (int)((float)a.a / b));
		}

		// Token: 0x0600008C RID: 140 RVA: 0x00004353 File Offset: 0x00002553
		public static bool operator ==(ColorInt a, ColorInt b)
		{
			return a.r == b.r && a.g == b.g && a.b == b.b && a.a == b.a;
		}

		// Token: 0x0600008D RID: 141 RVA: 0x0000438F File Offset: 0x0000258F
		public static bool operator !=(ColorInt a, ColorInt b)
		{
			return a.r != b.r || a.g != b.g || a.b != b.b || a.a != b.a;
		}

		// Token: 0x0600008E RID: 142 RVA: 0x000043CE File Offset: 0x000025CE
		public override bool Equals(object o)
		{
			return o is ColorInt && this.Equals((ColorInt)o);
		}

		// Token: 0x0600008F RID: 143 RVA: 0x000043E6 File Offset: 0x000025E6
		public bool Equals(ColorInt other)
		{
			return this == other;
		}

		// Token: 0x06000090 RID: 144 RVA: 0x000043F4 File Offset: 0x000025F4
		public override int GetHashCode()
		{
			return this.r + this.g * 256 + this.b * 256 * 256 + this.a * 256 * 256 * 256;
		}

		// Token: 0x06000091 RID: 145 RVA: 0x00004440 File Offset: 0x00002640
		public void ClampToNonNegative()
		{
			if (this.r < 0)
			{
				this.r = 0;
			}
			if (this.g < 0)
			{
				this.g = 0;
			}
			if (this.b < 0)
			{
				this.b = 0;
			}
			if (this.a < 0)
			{
				this.a = 0;
			}
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000092 RID: 146 RVA: 0x00004490 File Offset: 0x00002690
		public Color ToColor
		{
			get
			{
				return new Color
				{
					r = (float)this.r / 255f,
					g = (float)this.g / 255f,
					b = (float)this.b / 255f,
					a = (float)this.a / 255f
				};
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x06000093 RID: 147 RVA: 0x000044F8 File Offset: 0x000026F8
		public Color32 ToColor32
		{
			get
			{
				Color32 result = default(Color32);
				if (this.a > 255)
				{
					result.a = byte.MaxValue;
				}
				else
				{
					result.a = (byte)this.a;
				}
				if (this.r > 255)
				{
					result.r = byte.MaxValue;
				}
				else
				{
					result.r = (byte)this.r;
				}
				if (this.g > 255)
				{
					result.g = byte.MaxValue;
				}
				else
				{
					result.g = (byte)this.g;
				}
				if (this.b > 255)
				{
					result.b = byte.MaxValue;
				}
				else
				{
					result.b = (byte)this.b;
				}
				return result;
			}
		}

		// Token: 0x04000023 RID: 35
		public int r;

		// Token: 0x04000024 RID: 36
		public int g;

		// Token: 0x04000025 RID: 37
		public int b;

		// Token: 0x04000026 RID: 38
		public int a;
	}
}
