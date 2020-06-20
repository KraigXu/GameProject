using System;
using System.Globalization;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200000D RID: 13
	public struct IntRange : IEquatable<IntRange>
	{
		// Token: 0x17000037 RID: 55
		// (get) Token: 0x060000B6 RID: 182 RVA: 0x00004888 File Offset: 0x00002A88
		public static IntRange zero
		{
			get
			{
				return new IntRange(0, 0);
			}
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x060000B7 RID: 183 RVA: 0x00004891 File Offset: 0x00002A91
		public static IntRange one
		{
			get
			{
				return new IntRange(1, 1);
			}
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x060000B8 RID: 184 RVA: 0x0000489A File Offset: 0x00002A9A
		public int TrueMin
		{
			get
			{
				return Mathf.Min(this.min, this.max);
			}
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x060000B9 RID: 185 RVA: 0x000048AD File Offset: 0x00002AAD
		public int TrueMax
		{
			get
			{
				return Mathf.Max(this.min, this.max);
			}
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x060000BA RID: 186 RVA: 0x000048C0 File Offset: 0x00002AC0
		public float Average
		{
			get
			{
				return ((float)this.min + (float)this.max) / 2f;
			}
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x060000BB RID: 187 RVA: 0x000048D7 File Offset: 0x00002AD7
		public int RandomInRange
		{
			get
			{
				return Rand.RangeInclusive(this.min, this.max);
			}
		}

		// Token: 0x060000BC RID: 188 RVA: 0x000048EA File Offset: 0x00002AEA
		public IntRange(int min, int max)
		{
			this.min = min;
			this.max = max;
		}

		// Token: 0x060000BD RID: 189 RVA: 0x000048FA File Offset: 0x00002AFA
		public int Lerped(float lerpFactor)
		{
			return this.min + Mathf.RoundToInt(lerpFactor * (float)(this.max - this.min));
		}

		// Token: 0x060000BE RID: 190 RVA: 0x00004918 File Offset: 0x00002B18
		public static IntRange FromString(string s)
		{
			CultureInfo invariantCulture = CultureInfo.InvariantCulture;
			string[] array = s.Split(new char[]
			{
				'~'
			});
			if (array.Length == 1)
			{
				int num = Convert.ToInt32(array[0], invariantCulture);
				return new IntRange(num, num);
			}
			return new IntRange(Convert.ToInt32(array[0], invariantCulture), Convert.ToInt32(array[1], invariantCulture));
		}

		// Token: 0x060000BF RID: 191 RVA: 0x0000496A File Offset: 0x00002B6A
		public override string ToString()
		{
			return this.min.ToString() + "~" + this.max.ToString();
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x0000498C File Offset: 0x00002B8C
		public override int GetHashCode()
		{
			return Gen.HashCombineInt(this.min, this.max);
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x0000499F File Offset: 0x00002B9F
		public override bool Equals(object obj)
		{
			return obj is IntRange && this.Equals((IntRange)obj);
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x000049B7 File Offset: 0x00002BB7
		public bool Equals(IntRange other)
		{
			return this.min == other.min && this.max == other.max;
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x000049D7 File Offset: 0x00002BD7
		public static bool operator ==(IntRange lhs, IntRange rhs)
		{
			return lhs.Equals(rhs);
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x000049E1 File Offset: 0x00002BE1
		public static bool operator !=(IntRange lhs, IntRange rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x000049ED File Offset: 0x00002BED
		internal bool Includes(int val)
		{
			return val >= this.min && val <= this.max;
		}

		// Token: 0x0400002A RID: 42
		public int min;

		// Token: 0x0400002B RID: 43
		public int max;
	}
}
